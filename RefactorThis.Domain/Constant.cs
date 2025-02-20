namespace RefactorThis.Domain
{
    public static class App
    {
        public const string PartialInvoiceFullyPaid = "final partial payment received, invoice is now fully paid";
        public const string PartialInvoiceNotFullyPaid = "another partial payment received, still not fully paid";
        public const string FullyPaid = "invoice is now fully paid";
        public const string PartialInvoicePaid = "invoice is now partially paid";
    }
}
