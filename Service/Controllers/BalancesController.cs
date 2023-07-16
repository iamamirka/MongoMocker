using Microsoft.AspNetCore.Mvc;

namespace Service.Controllers;

[ApiController]
[Route("account")]
public class BalancesController : ControllerBase
{
    private readonly IAccountsService accountsService;
    private readonly ILogger<BalancesController> logger;

    public BalancesController(IAccountsService accountsService,
        ILogger<BalancesController> logger)
    {
        this.accountsService = accountsService;
        this.logger = logger;
    }

    [HttpGet("")]
    public async Task<decimal?> GetAccountBalance(int account)
    {
        return (await accountsService.GetAccountBalanceAsync(account).ConfigureAwait(false))?.Amount;
    }

    [HttpPost("{amount}")]
    public Task DepositToAccount(int account, decimal amount)
    {
        return accountsService.DepositToAccountAsync(account, amount);
    }

    [HttpDelete("{amount}")]
    public Task WithdrawFromAccount(int account, decimal amount)
    {
        return accountsService.WithdrawFromAccountAsync(account, amount);
    }
}