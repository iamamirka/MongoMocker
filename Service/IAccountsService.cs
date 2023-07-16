using Service.Entities;

namespace Service;

public interface IAccountsService
{
    public Task<Balance> GetAccountBalanceAsync(int account);
    public Task DepositToAccountAsync(int account, decimal amount);
    public Task WithdrawFromAccountAsync(int account, decimal amount);
}