using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RecruitmentAbi.Context;
using RecruitmentAbi.Services;

namespace Tests
{
    [TestFixture]
    public class Tests
    {
        private ServiceProvider _provider;
        private EmpContext _context;
        private CompanyService _companyService;

        [SetUp]
        public void Setup()
        {
            // Configure in-memory EF Core context for testing
            var services = new ServiceCollection();
            services.AddDbContext<EmpContext>(options =>
                options.UseInMemoryDatabase(databaseName: "test_db"));

            _provider = services.BuildServiceProvider();

            // Create a scoped context
            var scope = _provider.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<EmpContext>();

            // Initialize service
            _companyService = new CompanyService(_context);
            _companyService.ImportSeedData(_context, null);
        }

        [TearDown]
        public void TearDown()
        {
            // Clean up the in-memory database after each test
            _context.Database.EnsureDeleted();
            _context.Dispose();
            _provider.Dispose();
        }

        [Test]
        public void employee_can_request_vacation()
        {
            // Arrange
            var employee = _context.Employees.FirstOrDefault(e => e.Id == 2);

            // Act
            bool result = _companyService.IfEmployeeCanRequestVacation(employee);

            // Assert
            Assert.That(result, "Pracownik ma dostępne dni.");
        }

        [Test]
        public void employee_cant_request_vacation()
        {
            // Arrange
            var employee = _context.Employees.FirstOrDefault();

            // Act
            bool result = _companyService.IfEmployeeCanRequestVacation(employee);

            // Assert
            Assert.That(!result, "Pracownik wykorzystał wszystkie dni.");
        }
    }
}
