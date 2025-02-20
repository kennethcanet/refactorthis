
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RefactorThis.Domain;
using RefactorThis.Domain.Invoices;
using RefactorThis.Domain.Payments;
using RefactorThis.Shared.Shared;
using System.Text.Json.Serialization;

namespace RefactorThis.Application.Services
{
    public class InvoiceService: IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly decimal _taxRate = 0.14m;
        private readonly ILogger<InvoiceService> _logger;
        public InvoiceService(IInvoiceRepository invoiceRepository, ILogger<InvoiceService> logger)
        {
            _invoiceRepository = invoiceRepository;
            _logger = logger;
        }

        public Result ProcessPayment(Payment payment)
        {
            _logger.LogInformation($"ProcessPayment | Request={JsonConvert.SerializeObject(payment)}");

            var inv = _invoiceRepository.GetInvoice(payment.Reference);

            if (inv == null)
            {
                _logger.LogError($"ProcessPayment | Error={InvoiceErrors.NoInvoiceFound}");
                return Result.Failure(InvoiceErrors.NoInvoiceFound);
            }

            var validationResult = InvoicePaymentValidator.Validate(inv, payment);

            if (!validationResult.IsSuccess)
            {
                _logger.LogError($"ProcessPayment | Error={validationResult.Message}");
                return validationResult;
            }

            var result = ProcessInvoicePayment(inv,payment);

            _invoiceRepository.SaveInvoice(inv);

            _logger.LogInformation($"ProcessPayment | Success | {result.Message}");

            return result;
        }

        private Result ProcessInvoicePayment(Invoice inv,Payment payment)
        {
            
            Result result;

            if (inv.HasPartialPayment())
            {
                result = GenerateResultMessageForHasPayment(inv, payment);

                inv.AmountPaid += payment.Amount;
                if (inv.Type == InvoiceType.Commercial)
                {
                    inv.TaxAmount = inv.TaxAmount * _taxRate;
                }
            }
            else
            {
               result = GenerateResultMessageForFirstTimePayment(inv, payment);

                inv.AmountPaid = payment.Amount;
                inv.TaxAmount = inv.TaxAmount * _taxRate;
            }

            inv.Payments?.Add(payment);

            return result;

        }

        private Result GenerateResultMessageForHasPayment(Invoice inv, Payment payment)
        {
            if ((inv.Amount - inv.AmountPaid) == payment.Amount)
            {
                return Result.Success(App.PartialInvoiceFullyPaid);
            }
            else
            {
                return Result.Success(App.PartialInvoiceNotFullyPaid);
            }
        }

        private Result GenerateResultMessageForFirstTimePayment(Invoice inv, Payment payment)
        {
            if (inv.Amount == payment.Amount)
            {
                return Result.Success(App.FullyPaid);
            }
            else
            {
               return Result.Success(App.PartialInvoicePaid);
            }
        }
    }
}