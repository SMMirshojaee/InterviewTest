using Autofac;
using PaymentService.Infrastructure.Persistence.Repositories;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using PaymentService.Domain.Interfaces;
using PaymentService.Infrastructure.Persistence;
using Module = Autofac.Module;

namespace PaymentService.Api.Common;

public class AutofacDi : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var assembly = Assembly.Load("PaymentService.Infrastructure");
        var types = assembly.GetTypes().Where(a => a.Name.EndsWith("Repository"));

        //foreach (Type type in types)
        //    builder.RegisterType(type).AsSelf();
        builder.RegisterType<TransactionRepository>().As<ITransactionRepository>();
    }

}