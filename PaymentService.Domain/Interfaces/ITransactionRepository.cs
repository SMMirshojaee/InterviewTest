using PaymentService.Domain.Entities;

namespace PaymentService.Domain.Interfaces;

public interface ITransactionRepository
{
    Task Add(Transaction transaction);
}