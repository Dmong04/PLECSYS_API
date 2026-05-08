using APPLICATION.Handlers;
using DOMAIN.Entities;
using DOMAIN.Interfaces;
using Moq;

namespace PLECSYS_API_TESTS
{
    [TestFixture]
    public class CompaniesTests
    {

        private Mock<ICompanyRepository> _repo;
        private CompanyHandler _handler;

        [SetUp]
        public void Setup()
        {
            _repo = new Mock<ICompanyRepository>();
            _handler = new CompanyHandler(_repo.Object);
        }

        /*============================================
         Desplegar todas las compañías
         ===========================================*/
        [Test]
        public async Task GetAllCompanies_Return_Empty()
        {
            _repo.Setup(r => r.GetAllCompanies())
                .ReturnsAsync(new List<Company>());

            var result = await _handler.GetAllCompanies();

            Assert.That(result.Success, Is.False);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.Message, Does.Contain("No se encuentran compañías registradas en el servidor"));
        }

        [Test]
        public async Task GetAllCompanies_Return_Charged()
        {

            var completed = new List<Company> {
                new() {
                    Company_id = 1,
                    Company_name = "PLECSYS",
                    Address = "San josé",
                    Phone = "12345678",
                    Email= "contacto@plecsys.net"
                }
            };
            _repo.Setup(r => r.GetAllCompanies())
                .ReturnsAsync(completed);

            var result = await _handler.GetAllCompanies();

            Assert.That(result.Success, Is.True);
            Assert.That(result.Data, Is.Not.Empty);
            Assert.That(result.Message, Does.Contain("Se obtuvo el registro de compañías de manera exitosa"));
        }
    }
}
