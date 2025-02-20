using RefactorThis.Domain.Payments;
using RefactorThis.Shared.Shared;

namespace RefactorThis.Domain.Invoices;

public static class InvoicePaymentValidator
{
    public static Result Validate(Invoice invoice, Payment payment)
    {
        if (IsNoPaymentNeeded(invoice))
        {
            return Result.Failure(InvoiceErrors.NoPaymentNeeded);
        }

        if (IsInvoiceInvalidState(invoice))
        {
            return Result.Failure(InvoiceErrors.InvoiceInvalidState);
        }

        if (invoice.HasPartialPayment())
        {
            if (invoice.IsFullyPaid())
            {
                return Result.Failure(InvoiceErrors.InvoiceAlreadyFullPaid);
            }

            if (IsPaymentExceedingRemainingAmount(invoice, payment))
            {
                return Result.Failure(InvoiceErrors.PaymentIsGreaterthanPartialAmountRemaining);
            }
        }

        if (payment.Amount > invoice.Amount)
        {
            return Result.Failure(InvoiceErrors.PaymentIsGreaterthanInvoiceAmount);
        }

        return Result.Success();
    }

    private static bool IsNoPaymentNeeded(Invoice invoice) => invoice.Amount == 0 && !invoice.HasPartialPayment();
    private static bool IsInvoiceInvalidState(Invoice invoice) => invoice.Amount == 0 && invoice.HasPartialPayment();
    private static bool IsPaymentExceedingRemainingAmount(Invoice invoice, Payment payment) => payment.Amount > invoice.GetRemainingBalance();
}
