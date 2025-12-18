using Microsoft.EntityFrameworkCore;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Interfaces;
using PaymentService.Infrastructure.Persistence.Configurations;

namespace PaymentService.Infrastructure.Persistence.Repositories;

public class TransactionRepository(PaymentDbContext dbContext) : ITransactionRepository
{
    public async Task Add(Transaction transaction)
    {
        try
        {
            await dbContext.AddAsync(transaction);
            await dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new DatabaseException("خطا در ثبت توکن", e);
        }
    }

    public Task<Transaction?> GetByToken(Guid token)
    => dbContext.Transactions.FirstOrDefaultAsync(e => e.Token == token.ToString());

    public async Task SetAppCode(Transaction transaction, string appCode)
    {
        try
        {
            transaction.AppCode = appCode;
            await dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new DatabaseException("خطا در ثبت app code", e);
        }
    }

    public async Task SaveChanges()
    {
        try
        {
            await dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new DatabaseException("خطا در اعمال تغییرات", e);
        }
    }
}