using APPLICATION.Handlers;
using APPLICATION.Use_cases.Claims_case;
using DOMAIN.Entities;
using DOMAIN.Interfaces;
using Moq;

namespace PLECSYS_API_TESTS
{
    [TestFixture]
    public class ClaimsTest
    {
        private Mock<IClaimRepository> _claimRepo;
        private Mock<IInvoiceRepository> _invoiceRepo;
        private Mock<IInvoiceHistoryRepository> _historyRepo;
        private ClaimHandler _claimHandler;

        [SetUp]
        public void Setup()
        {
            _claimRepo = new Mock<IClaimRepository>();
            _invoiceRepo = new Mock<IInvoiceRepository>();
            _historyRepo = new Mock<IInvoiceHistoryRepository>();
            _claimHandler = new ClaimHandler(_claimRepo.Object, _invoiceRepo.Object, _historyRepo.Object);

            // Setups por defecto
            _invoiceRepo.Setup(r => r.GetInvoiceById(1)).ReturnsAsync(BuildInvoice());
            _claimRepo.Setup(r => r.CreateClaim(It.IsAny<Claim>())).ReturnsAsync(BuildClaim());
            _invoiceRepo.Setup(r => r.UpdateInvoice(It.IsAny<Invoice>())).ReturnsAsync(BuildInvoice());
            _historyRepo.Setup(r => r.CreateHistory(It.IsAny<InvoiceHistory>())).ReturnsAsync(BuildHistory());
        }
        // ── Helpers ──

        private static Invoice BuildInvoice(string status = "Pendiente") =>
            new Invoice
            {
                Invoice_id = 1,
                Status = status,
                Total_voucher = 1000,
                Pending_balance = 1000,
                User_creator_id = "user-1",
                Sell_company_id = 1,
                Charged_company_id = 1,
                Currency_id = 1
            };

        private static Claim BuildClaim() =>
            new Claim
            {
                Claim_id = 1,
                Invoice_id = 1,
                User_id = "user-1",
                Description = "Reclamo test",
                Claim_amount = 500,
                Record_date = DateTime.Now
            };

        private static ClaimRequest BuildRequest(
            int invoiceId = 1,
            decimal claimAmount = 500,
            string userId = "user-1") =>
            new ClaimRequest
            {
                Invoice_id = invoiceId,
                User_id = userId,
                Description = "Reclamo test",
                Claim_amount = claimAmount,
                Record_date = DateTime.Now
            };

        private static InvoiceHistory BuildHistory() =>
            new InvoiceHistory
            {
                Invoice_id = 1,
                Action = "Reclamo registrado",
                Description = "Test history"
            };

        // ─────────────────────────────────────────
        // Casos de éxito
        // ─────────────────────────────────────────

        [Test]
        public async Task CreateClaim_WhenValid_ReturnsSuccess()
        {
            var result = await _claimHandler.CreateClaim(BuildRequest());

            Assert.That(result.Success, Is.True);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Message, Does.Contain("exitosamente"));
        }

        [Test]
        public async Task CreateClaim_WhenValid_InvoiceStatusUpdatesToConReclamo()
        {
            await _claimHandler.CreateClaim(BuildRequest());

            _invoiceRepo.Verify(r => r.UpdateInvoice(It.Is<Invoice>(i =>
                i.Status == "Con reclamo"
            )), Times.Once);
        }

        [Test]
        public async Task CreateClaim_WhenValid_HistoryIsRegistered()
        {
            await _claimHandler.CreateClaim(BuildRequest());

            _historyRepo.Verify(r => r.CreateHistory(It.Is<InvoiceHistory>(h =>
                h.Action == "Reclamo registrado" &&
                h.New_status == "Con reclamo"
            )), Times.Once);
        }

        // ─────────────────────────────────────────
        // Validaciones / Fallos
        // ─────────────────────────────────────────

        [Test]
        public async Task CreateClaim_WhenInvoiceNotFound_ReturnsFailure()
        {
            _invoiceRepo.Setup(r => r.GetInvoiceById(It.IsAny<int>())).ReturnsAsync((Invoice?)null);

            var result = await _claimHandler.CreateClaim(BuildRequest());

            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Does.Contain("No se encontró la factura"));
            _claimRepo.Verify(r => r.CreateClaim(It.IsAny<Claim>()), Times.Never);
        }

        [Test]
        public async Task CreateClaim_WhenInvoiceHasActiveClaim_ReturnsFailure()
        {
            _invoiceRepo.Setup(r => r.GetInvoiceById(1)).ReturnsAsync(BuildInvoice(status: "Con reclamo"));

            var result = await _claimHandler.CreateClaim(BuildRequest());

            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Does.Contain("reclamo activo"));
            _claimRepo.Verify(r => r.CreateClaim(It.IsAny<Claim>()), Times.Never);
        }

        [Test]
        public async Task CreateClaim_WhenRepositoryReturnsNull_ReturnsFailure()
        {
            _claimRepo.Setup(r => r.CreateClaim(It.IsAny<Claim>())).ReturnsAsync((Claim?)null);

            var result = await _claimHandler.CreateClaim(BuildRequest());

            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Does.Contain("error al registrar el reclamo"));
            _invoiceRepo.Verify(r => r.UpdateInvoice(It.IsAny<Invoice>()), Times.Never);
        }

        [Test]
        public async Task CreateClaim_WhenExceptionIsThrown_ReturnsFailure()
        {
            _invoiceRepo.Setup(r => r.GetInvoiceById(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Timeout de BD"));

            var result = await _claimHandler.CreateClaim(BuildRequest());

            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Does.Contain("Timeout de BD"));
        }
    }
}
