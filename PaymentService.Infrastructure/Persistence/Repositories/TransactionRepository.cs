using Microsoft.EntityFrameworkCore;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Interfaces;

namespace PaymentService.Infrastructure.Persistence.Repositories;

public class TransactionRepository(PaymentDbContext dbContext) : ITransactionRepository
{
    public async Task Add(Transaction transaction)
    {
        await dbContext.AddAsync(transaction);
        await dbContext.SaveChangesAsync();
    }
}