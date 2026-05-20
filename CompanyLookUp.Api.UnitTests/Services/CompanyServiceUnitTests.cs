using CompanyLookup.Api.Common;
using CompanyLookup.Api.External.Brreg.Models.Enhet;
using CompanyLookup.Api.External.Brreg.Services.Enhet;
using CompanyLookup.Api.Services.Companies;
using NSubstitute;

namespace CompanyLookUp.Api.UnitTests.Services
{
    [TestClass]
    public sealed class CompanyServiceUnitTests
    {
        private IEnhetRepository _repository = null!;
        private CompanyService _companyService = null!;
        private CancellationToken _ct;

        [TestInitialize]
        public void Setup()
        {
            _repository = Substitute.For<IEnhetRepository>();
            _companyService = new CompanyService(_repository);
            _ct = CancellationToken.None;
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("   ")]
        public async Task GetAsync_With_Invalid_Orgnr_Returns_ValidationFailure(string invalidOrgnr)
        {
            var result = await _companyService.GetAsync(invalidOrgnr, _ct);

            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(ErrorType.Validation, result.ErrorType);
        }

        [TestMethod]
        public async Task GetAsync_When_Enhet_Is_Null_Returns_NotFound()
        {
            var orgnr = "123456789";

            _repository
                .GetEnhetAsync(orgnr, _ct)
                .Returns((EnhetResponse?)null);

            var result = await _companyService.GetAsync(orgnr, _ct);

            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(ErrorType.NotFound, result.ErrorType);
        }

        [TestMethod]
        public async Task GetAsync_Should_Fetch_Enhet()
        {
            var orgnr = "123456789";
         
            var enhetResponse = CreateSampleEnhetResponse(orgnr);

            _repository
                .GetEnhetAsync(orgnr, _ct)
                .Returns(enhetResponse);

            await _companyService.GetAsync(orgnr, _ct);

            await _repository
                .Received(1)
                .GetEnhetAsync(orgnr, _ct);
        }

        [TestMethod]
        public async Task GetAsync_With_Valid_Response_Returns_CompanyResponse()
        {
            var orgnr = "123456789";
            var enhetResponse = CreateSampleEnhetResponse(orgnr);

            _repository
                .GetEnhetAsync(orgnr, CancellationToken.None)
                .Returns(enhetResponse);

            var result = await _companyService.GetAsync(orgnr, CancellationToken.None);

            Assert.IsNotNull(result);
            Assert.AreEqual(orgnr, result.Value?.OrganizationNumber);
            Assert.AreEqual("Test Company", result.Value?.Name);
            Assert.AreEqual(10, result.Value?.EmployeeCount);
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