﻿namespace RefactorThis.Domain.Invoices
{
    public interface IInvoiceRepository
    {
        void Add(Invoice invoice);
        Invoice GetInvoice(string reference);
        void SaveInvoice(Invoice invoice);
    }
}