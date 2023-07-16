using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Service;
using Service.Controllers;
using Service.Entities;
using Xunit;

namespace Tests;

public class BalanceControllerTests
{
    private readonly Mock<IAccountsService> accountServiceMock = new();
    private readonly Mock<ILogger<BalancesController>> loggerMock = new();

    [Fact]
    public async Task GetAccountBalance_Should_ReturnCurrentAmount_WhenProperAccountNumberPassed()
    {
        //Arrange
        var balance = new Balance { Account = 1, Amount = 0 };
        accountServiceMock
            .Setup(db => db.GetAccountBalanceAsync(It.IsAny<int>()))
            .ReturnsAsync(balance);
        var sut = new BalancesController(accountServiceMock.Object, loggerMock.Object);

        //Act
        var accountBalance = await sut.GetAccountBalance(1).ConfigureAwait(false);

        //Assert
        accountBalance.Should().Be(0);
    }

    [Theory]
    [InlineData(1, 10)]
    [InlineData(2, 30)]
    public async Task DepositToAccount_Should_IncreaseBalanceAmount_OnAmountValue(int account, decimal amount)
    {
        //Arrange
        const decimal initialBalanceAmount = 30m;
        var balance = new Balance { Account = account, Amount = initialBalanceAmount + amount };
        accountServiceMock
            .Setup(db => db.GetAccountBalanceAsync(It.IsAny<int>()))
            .ReturnsAsync(balance);
        var sut = new BalancesController(accountServiceMock.Object, loggerMock.Object);
        //In case of real db it's better to seed data in db, and then get it in variable
        //that's how we can understand that value was really updated
        decimal? accountBalance = initialBalanceAmount;

        //Act
        await sut.DepositToAccount(account, amount).ConfigureAwait(false);
        accountBalance = await sut.GetAccountBalance(account).ConfigureAwait(false);

        //Assert
        accountBalance.Should().Be(initialBalanceAmount + amount);
    }

    [Fact]
    public async Task DepositToAccount_Should_UpdateOnlyOneRecord()
    {
        //Arrange
        const decimal initialBalanceAmount = 30m;
        var balance = new Balance { Account = 1, Amount = initialBalanceAmount + 30 };
        var secondBalance = new Balance { Account = 2, Amount = initialBalanceAmount };
        accountServiceMock
            .Setup(db => db.GetAccountBalanceAsync(1))
            .ReturnsAsync(balance);
        accountServiceMock
            .Setup(db => db.GetAccountBalanceAsync(2))
            .ReturnsAsync(secondBalance);
        var sut = new BalancesController(accountServiceMock.Object, loggerMock.Object);
        //In case of real db it's better to seed data in db, and then get it in variable
        //that's how we can understand that value was really updated
        decimal? firstAccountBalance = initialBalanceAmount;
        decimal? secondAccountBalance = initialBalanceAmount;

        //Act
        await sut.DepositToAccount(1, 30).ConfigureAwait(false);
        firstAccountBalance = await sut.GetAccountBalance(1).ConfigureAwait(false);
        secondAccountBalance = await sut.GetAccountBalance(2).ConfigureAwait(false);

        //Assert
        firstAccountBalance.Should().Be(initialBalanceAmount + 30);
        secondAccountBalance.Should().Be(initialBalanceAmount);
    }

    [Theory]
    [InlineData(1, 10)]
    [InlineData(2, 30)]
    public async Task WithdrawFromAccount_Should_DecreaseBalanceAmount_OnAmountValue(int account, decimal amount)
    {
        //Arrange
        const decimal initialBalanceAmount = 30m;
        var balance = new Balance { Account = account, Amount = initialBalanceAmount - amount };
        accountServiceMock
            .Setup(db => db.GetAccountBalanceAsync(It.IsAny<int>()))
            .ReturnsAsync(balance);
        var sut = new BalancesController(accountServiceMock.Object, loggerMock.Object);
        //In case of real db it's better to seed data in db, and then get it in variable,
        //that's how we can understand that value was really updated
        decimal? accountBalance = initialBalanceAmount;

        //Act
        await sut.WithdrawFromAccount(account, amount).ConfigureAwait(false);
        accountBalance = await sut.GetAccountBalance(account).ConfigureAwait(false);

        //Assert
        accountBalance.Should().Be(initialBalanceAmount - amount);
    }

    [Fact]
    public async Task WithdrawFromAccount_Should_UpdateOnlyOneRecord()
    {
        //Arrange
        const decimal initialBalanceAmount = 30m;
        var balance = new Balance { Account = 1, Amount = initialBalanceAmount - 5 };
        var secondBalance = new Balance { Account = 2, Amount = initialBalanceAmount };
        accountServiceMock
            .Setup(db => db.GetAccountBalanceAsync(1))
            .ReturnsAsync(balance);
        accountServiceMock
            .Setup(db => db.GetAccountBalanceAsync(2))
            .ReturnsAsync(secondBalance);
        var sut = new BalancesController(accountServiceMock.Object, loggerMock.Object);
        //In case of real db it's better to seed data in db, and then get it in variable
        //that's how we can understand that value was really updated
        decimal? firstAccountBalance = initialBalanceAmount;
        decimal? secondAccountBalance = initialBalanceAmount;

        //Act
        await sut.DepositToAccount(1, 30).ConfigureAwait(false);
        firstAccountBalance = await sut.GetAccountBalance(1).ConfigureAwait(false);
        secondAccountBalance = await sut.GetAccountBalance(2).ConfigureAwait(false);

        //Assert
        firstAccountBalance.Should().Be(initialBalanceAmount - 5);
        secondAccountBalance.Should().Be(initialBalanceAmount);
    }
}