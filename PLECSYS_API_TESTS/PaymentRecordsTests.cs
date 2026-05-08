using APPLICATION.Handlers;
using APPLICATION.Use_cases.PaymentRecords_case;
using DOMAIN.Entities;
using DOMAIN.Interfaces;
using Moq;

namespace PLECSYS_API_TESTS
{
    [TestFixture]
    public class PaymentRecordsTests
    {
        private Mock<IPaymentRecordRepository> _paymentRepo;
        private Mock<IInvoiceRepository> _invoiceRepo;
        private Mock<IInvoiceHistoryRepository> _invoiceHistoryRepo;
        private PaymentRecordHandler _handler;

        [SetUp]
        public void Setup()
        {
            _paymentRepo = new Mock<IPaymentRecordRepository>();
            _invoiceRepo = new Mock<IInvoiceRepository>();
            _invoiceHistoryRepo = new Mock<IInvoiceHistoryRepository>();
            _handler = new PaymentRecordHandler(_paymentRepo.Object, _invoiceRepo.Object, _invoiceHistoryRepo.Object);

            // Setups por defecto
            _invoiceRepo.Setup(r => r.GetInvoiceById(1)).ReturnsAsync(BuildInvoice());
            _paymentRepo.Setup(r => r.CreatePaymentRecord(It.IsAny<PaymentRecord>())).ReturnsAsync(BuildPaymentRecord());
            _invoiceRepo.Setup(r => r.UpdateInvoice(It.IsAny<Invoice>())).ReturnsAsync(BuildInvoice());
            _invoiceHistoryRepo.Setup(r => r.CreateHistory(It.IsAny<InvoiceHistory>())).ReturnsAsync(BuildHistory());
        }

        private static Invoice BuildInvoice(string status = "Pendiente", decimal total = 1000, decimal? pending = 1000) =>
            new Invoice
            {
                Invoice_id = 1,
                Status = status,
                Total_voucher = total,
                Pending_balance = pending,
                User_creator_id = "user-1",
                Sell_company_id = 1,
                Charged_company_id = 1,
                Currency_id = 1
            };

        private static PaymentRecord BuildPaymentRecord(decimal paidAmount = 1000) =>
            new PaymentRecord
            {
                Payment_record_id = 1,
                Source_id = 1,
                Currency_id = 1,
                Payment_method_id = 1,
                Paid_amount = paidAmount,
                Payment_date = DateTime.Now,
                Payment_detail = "Pago test",
                Third_party_transaction_id = "TX-001"
            };

        private static PaymentRecordRequest BuildRequest(
            decimal paidAmount = 1000,
            int paymentMethodId = 1,
            string? detail = null) =>
            new PaymentRecordRequest
            {
                Source_id = 1,
                Currency_id = 1,
                Payment_method_id = paymentMethodId,
                Paid_amount = paidAmount,
                Payment_date = DateTime.Now,
                Payment_detail = "Pago test",
                Third_party_transaction_id = "TX-001",
                Detail_payment_method = detail
            };

        private static InvoiceHistory BuildHistory() =>
            new InvoiceHistory
            {
                Invoice_id = 1,
                Action = "Pago registrado",
                Description = "Test history"
            };

        // ─────────────────────────────────────────
        // Casos de éxito
        // ─────────────────────────────────────────

        [Test]
        public async Task CreatePaymentRecord_WhenValid_ReturnsSuccess()
        {
            var result = await _handler.CreatePaymentRecord(BuildRequest());

            Assert.That(result.Success, Is.True);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Message, Does.Contain("éxito"));
        }

        [Test]
        public async Task CreatePaymentRecord_WhenPaidAmountIsPartial_InvoiceStatusIsPartial()
        {
            _invoiceRepo.Setup(r => r.GetInvoiceById(1)).ReturnsAsync(BuildInvoice(pending: 1000));

            await _handler.CreatePaymentRecord(BuildRequest(paidAmount: 500));

            _invoiceRepo.Verify(r => r.UpdateInvoice(It.Is<Invoice>(i =>
                i.Status == "Parcial" &&
                i.Pending_balance == 500
            )), Times.Once);
        }

        [Test]
        public async Task CreatePaymentRecord_WhenFullyPaid_InvoiceStatusIsPaid()
        {
            await _handler.CreatePaymentRecord(BuildRequest(paidAmount: 1000));

            _invoiceRepo.Verify(r => r.UpdateInvoice(It.Is<Invoice>(i =>
                i.Status == "Pagado" &&
                i.Pending_balance == 0
            )), Times.Once);
        }

        // ─────────────────────────────────────────
        // Validaciones / Fallos
        // ─────────────────────────────────────────

        [Test]
        public async Task CreatePaymentRecord_WhenInvoiceNotFound_ReturnsFailure()
        {
            _invoiceRepo.Setup(r => r.GetInvoiceById(It.IsAny<int>())).ReturnsAsync((Invoice?)null);

            var result = await _handler.CreatePaymentRecord(BuildRequest());

            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Does.Contain("No se encontró la factura"));
            _paymentRepo.Verify(r => r.CreatePaymentRecord(It.IsAny<PaymentRecord>()), Times.Never);
        }

        [Test]
        public async Task CreatePaymentRecord_WhenInvoiceHasActiveClaim_ReturnsFailure()
        {
            _invoiceRepo.Setup(r => r.GetInvoiceById(1)).ReturnsAsync(BuildInvoice(status: "Con reclamo"));

            var result = await _handler.CreatePaymentRecord(BuildRequest());

            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Does.Contain("reclamo activo"));
            _paymentRepo.Verify(r => r.CreatePaymentRecord(It.IsAny<PaymentRecord>()), Times.Never);
        }

        [Test]
        public async Task CreatePaymentRecord_WhenPaymentMethodIsOtroWithoutDetail_ReturnsFailure()
        {
            var result = await _handler.CreatePaymentRecord(BuildRequest(paymentMethodId: 99, detail: ""));

            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Does.Contain("OTRO"));
            _paymentRepo.Verify(r => r.CreatePaymentRecord(It.IsAny<PaymentRecord>()), Times.Never);
        }

        [Test]
        public async Task CreatePaymentRecord_WhenRepositoryReturnsNull_ReturnsFailure()
        {
            _paymentRepo.Setup(r => r.CreatePaymentRecord(It.IsAny<PaymentRecord>()))
                .ReturnsAsync((PaymentRecord?)null);

            var result = await _handler.CreatePaymentRecord(BuildRequest());

            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Does.Contain("error al registrar el pago"));
        }

        [Test]
        public async Task CreatePaymentRecord_WhenExceptionIsThrown_ReturnsFailure()
        {
            _invoiceRepo.Setup(r => r.GetInvoiceById(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Timeout de BD"));

            var result = await _handler.CreatePaymentRecord(BuildRequest());

            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Does.Contain("Timeout de BD"));
        }
    }
}