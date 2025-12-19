using Microsoft.EntityFrameworkCore;
using PaymentService.Domain.Entities;

namespace PaymentService.Domain.Interfaces;

public interface ITransactionRepository
{
    DbContext Context { get; }
    Task Add(Transaction transaction);
    Task<Transaction?> GetByToken(Guid token);
    Task SetAppCode(Transaction transaction, string appCode);
    Task SaveChanges();
}