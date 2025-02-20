using RefactorThis.Domain.Payments;
using RefactorThis.Shared.Shared;

namespace RefactorThis.Domain.Invoices
{
    public interface IInvoiceService
    {
        Result ProcessPayment(Payment payment);
    }
}