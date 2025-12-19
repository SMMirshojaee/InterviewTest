using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PaymentService.Domain.Enums;
using PaymentService.Domain.Interfaces;
using PaymentService.Infrastructure.Persistence.Configurations;

namespace PaymentService.Infrastructure.BackgroundJobs;

public class TransactionsManagement(ITransactionRepository repository)
{
    public async Task ExpirationCheckJob()
    {
        try
        {
            string updateCommand =
                $"UPDATE dbo.Transactions SET Status='{(int)PaymentStatus.Expired}' ,UpdatedAt=GETDATE()  WHERE Status='{(int)PaymentStatus.Pending}'  AND CreatedAt<DATEADD(MINUTE, -{2}, GETDATE())";
            await repository.Context.Database.ExecuteSqlRawAsync(updateCommand);
        }
        catch (Exception e)
        {
            throw new DatabaseException("خطا در آپدیت منقضی شده ها", e);
        }
    }
}