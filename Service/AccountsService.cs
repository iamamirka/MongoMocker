using MongoDB.Driver;
using Service.Entities;

namespace Service;

public interface IAccountService
{
    public Task<Balance> GetAccountBalanceAsync(int account);
    public Task DepositToAccountAsync(int account, decimal amount);
    public Task WithdrawFromAccountAsync(int account, decimal amount);
}

/// <summary>
///     Тип для выполнения зпросов к базе данных.
/// </summary>
/// <remarks>
///     Данный тип не может работать так как не имеет подключения к реальной базе данных.
/// </remarks>
public class AccountsService : IAccountService
{
    private readonly MongoClient client;
    private ILogger<AccountsService> logger;

    public AccountsService(MongoClient client,
        ILogger<AccountsService> logger)
    {
        this.client = client;
        this.logger = logger;
    }

    private IMongoCollection<Balance> Accounts => client
        .GetDatabase("Accounts")
        .GetCollection<Balance>("Balance");


    /// <summary>
    ///     Получение остатка на счёте.
    /// </summary>
    /// <param name="account">Номер счета</param>
    /// <returns>Остаток на счёте</returns>
    public Task<Balance> GetAccountBalanceAsync(int account)
    {
        return Accounts
            .Find(x => x.Account == account)
            .SingleAsync();
    }

    /// <summary>
    ///     Пополнение остатка на счёте на заданную сумму.
    /// </summary>
    /// <param name="account">Номер счета</param>
    /// <param name="amount">Сумма пополнения</param>
    /// <returns></returns>
    public Task DepositToAccountAsync(int account, decimal amount) => UpdateAccountAmountAsync(account, amount);

    /// <summary>
    ///     Уменьшение остатка на счёте на заданную сумму.
    /// </summary>
    /// <param name="account">Номер счета</param>
    /// <param name="amount">Сумма снятия</param>
    /// <returns></returns>
    public Task WithdrawFromAccountAsync(int account, decimal amount) => UpdateAccountAmountAsync(account, -amount);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="account">Номер счета</param>
    /// <param name="amount">Величина изменения суммы средств</param>
    /// <returns></returns>
    private Task UpdateAccountAmountAsync(int account, decimal amount)
    {
        return Accounts
            .FindOneAndUpdateAsync(x => x.Account == account,
                Builders<Balance>.Update.Inc(x => x.Amount, amount));
    }
}