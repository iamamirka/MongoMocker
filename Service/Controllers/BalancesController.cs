using Microsoft.AspNetCore.Mvc;

namespace Service.Controllers;

[ApiController]
[Route("account")]
public class BalancesController : ControllerBase
{
    private readonly IAccountService accountService;
    private readonly ILogger<BalancesController> logger;

    public BalancesController(IAccountService accountService,
        ILogger<BalancesController> logger)
    {
        this.accountService = accountService;
        this.logger = logger;
    }

    [HttpGet("")]
    public async Task<decimal?> GetAccountBalance(int account)
    {
        return (await accountService.GetAccountBalanceAsync(account).ConfigureAwait(false))?.Amount;
    }

    [HttpPost("{amount}")]
    public Task DepositToAccount(int account, decimal amount)
    {
        return accountService.DepositToAccountAsync(account, amount);
    }

    [HttpDelete("{amount}")]
    public Task WithdrawFromAccount(int account, decimal amount)
    {
        return accountService.WithdrawFromAccountAsync(account, amount);
    }
}