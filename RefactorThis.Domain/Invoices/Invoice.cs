using RefactorThis.Domain.Payments;

namespace RefactorThis.Domain.Invoices
{
    public class Invoice
	{
		public decimal Amount { get; set; }
		public decimal AmountPaid { get; set; }
		public decimal TaxAmount { get; set; }
		public List<Payment> Payments { get; set; }
		
		public InvoiceType Type { get; set; }


        public decimal GetTotalPaidAmount() => Payments.Sum(x => x.Amount);
        public bool HasPartialPayment() => Payments is not null && Payments.Any();
        public bool IsFullyPaid() => GetTotalPaidAmount() >= Amount;
        public decimal GetRemainingBalance() => Amount - GetTotalPaidAmount();
    }
}