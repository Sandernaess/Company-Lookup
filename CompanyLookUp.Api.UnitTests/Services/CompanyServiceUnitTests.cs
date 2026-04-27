using CompanyLookup.Api.External.Brreg.Enhet;
using CompanyLookup.Api.Services.Companies;
using NSubstitute;

namespace CompanyLookUp.Api.UnitTests.Services
{
    [TestClass]
    public sealed class CompanyServiceUnitTests
    {
        private IEnhetService _enhetService = null!;
        private CompanyService _companyService = null!;
        private CancellationToken _ct;

        [TestInitialize]
        public void Setup()
        {
            _enhetService = Substitute.For<IEnhetService>();
            _companyService = new CompanyService(_enhetService);
            _ct = CancellationToken.None;
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("   ")]
        public async Task GetAsync_With_Invalid_Orgnr_Throws_ArgumentNullException(string invalidOrgnr)
        {
            await Assert.ThrowsExactlyAsync<ArgumentNullException>(
                async () => await _companyService.GetAsync(invalidOrgnr, _ct)
            );
        }

        [TestMethod]
        public async Task GetAsync_With_Null_Response_Throws_Exception()
        {
            var orgnr = "123456789";

            _enhetService
                .GetEnhet(orgnr, _ct)
                .Returns((EnhetResponse?)null);

            var exception = await Assert.ThrowsExactlyAsync<Exception>(
                async () => await _companyService.GetAsync(orgnr, _ct)
            );

            Assert.AreEqual("Enhet not found in brreg", exception.Message);
        }

        [TestMethod]
        public async Task GetAsync_Should_Fetch_Enhet()
        {
            var orgnr = "123456789";
         
            var enhetResponse = CreateSampleEnhetResponse(orgnr);

            _enhetService
                .GetEnhet(orgnr, _ct)
                .Returns(enhetResponse);

            await _companyService.GetAsync(orgnr, _ct);

            await _enhetService
                .Received(1)
                .GetEnhet(orgnr, _ct);
        }

        [TestMethod]
        public async Task GetAsync_With_Valid_Response_Returns_CompanyResponse()
        {
            var orgnr = "123456789";
            var enhetResponse = CreateSampleEnhetResponse(orgnr);

            _enhetService
                .GetEnhet(orgnr, CancellationToken.None)
                .Returns(enhetResponse);

            var result = await _companyService.GetAsync(orgnr, CancellationToken.None);

            Assert.IsNotNull(result);
            Assert.AreEqual(orgnr, result.OrganizationNumber);
            Assert.AreEqual("Test Company", result.Name);
            Assert.AreEqual(10, result.EmployeeCount);
        }

        private static EnhetResponse CreateSampleEnhetResponse(string orgnr = "123456789")
        {
            return new EnhetResponse
            {
                Organisasjonsnummer = orgnr,
                Navn = "Test Company",
                HarRegistrertAntallAnsatte = true,
                AntallAnsatte = 10,
                Hjemmeside = "www.testcompany.com",
            };
        }
    }
}