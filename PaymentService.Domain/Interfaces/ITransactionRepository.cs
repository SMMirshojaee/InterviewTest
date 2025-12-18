using PaymentService.Domain.Common;
using PaymentService.Domain.Entities;

namespace PaymentService.Domain.Interfaces;

public interface ITransactionRepository
{
    Task Add(Transaction transaction);
    Task<Transaction?> GetByToken(Guid token);
    Task SetAppCode(Transaction transaction, string appCode);
}