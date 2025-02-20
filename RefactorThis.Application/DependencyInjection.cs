using Microsoft.Extensions.DependencyInjection;
using RefactorThis.Application.Services;
using RefactorThis.Domain.Invoices;

namespace RefactorThis.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IInvoiceService, InvoiceService>();
        return services;
	}
}
