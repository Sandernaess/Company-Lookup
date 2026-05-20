using CompanyLookup.Api.Common;
using CompanyLookup.Api.Endpoints.Companies;
using CompanyLookup.Api.Models.Companies;
using CompanyLookup.Api.Services.Companies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System.Net;

namespace CompanyLookUp.Api.UnitTests.Endpoints
{
    [TestClass]
    public sealed class GetCompanyByOrgnrUnitTests
    {
        private ICompanyService _companyService = null!;
        private CancellationToken _ct;

        [TestInitialize]
        public void Setup()
        {
            _companyService = Substitute.For<ICompanyService>();
            _ct = CancellationToken.None;
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("   ")]
        public async Task Handle_With_Invalid_Orgnr_Returns_BadRequest(string invalidOrgnr)
        {
            // Act
            IResult result = await GetCompanyByOrgnr
                .Handle(invalidOrgnr, _companyService, _ct);

            // Assert
            var statusCodeResult = result as IStatusCodeHttpResult;

            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, statusCodeResult.StatusCode);
        }

        [TestMethod]
        [DataRow("12345678")]
        [DataRow("1234567890")]
        public async Task Handle_With_Invalid_Orgnr_Length_Returns_BadRequest(string invalidLength)
        {
            // Act
            IResult result = await GetCompanyByOrgnr
                .Handle(invalidLength, _companyService, _ct);

            // Assert
            var statusCodeResult = result as IStatusCodeHttpResult;
            Assert.IsNotNull(statusCodeResult);

            Assert.AreEqual((int)HttpStatusCode.BadRequest, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public async Task Handle_With_Valid_Orgnr_But_None_Company_Given_Returns_NotFound()
        {
            var orgnr = "123456789";

            var failedResult = Result.Failure<CompanyResponse>(
                $"Company with orgnr {orgnr} not found.",
                ErrorType.NotFound);

            _companyService
                .GetAsync(orgnr, _ct)
                .Returns(failedResult);

            var result = await GetCompanyByOrgnr
                .Handle(orgnr, _companyService, _ct);

            var statusCodeResult = result as IStatusCodeHttpResult;
            Assert.IsNotNull(statusCodeResult);

            Assert.AreEqual((int)HttpStatusCode.NotFound, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public async Task Handle_With_Valid_Orgnr_And_Valid_Company_Returns_Ok()
        {
            var orgnr = "123456789";
            var company = CreateSampleCompanyResponse(orgnr);

            _companyService
                .GetAsync(orgnr, _ct)
                .Returns(Result.Success(company));

            var result = await GetCompanyByOrgnr.Handle(orgnr, _companyService, _ct);

            var okResult = result as Ok<CompanyResponse>;
            Assert.IsNotNull(okResult);

            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(company.OrganizationNumber, okResult.Value?.OrganizationNumber);
            Assert.AreEqual(company.Name, okResult.Value?.Name);
        }

        [TestMethod]
        public async Task Handle_When_Service_Throws_Exception_Propagates()
        {
            var orgnr = "123456789";

            _companyService
                .GetAsync(orgnr, _ct)
                .Throws(new HttpRequestException("Database connection failed"));

            await Assert.ThrowsExactlyAsync<HttpRequestException>(
                () => GetCompanyByOrgnr.Handle(orgnr, _companyService, _ct));
        }

        [TestMethod]
        public async Task Handle_With_Valid_Orgnr_Calls_Service_GetAsync()
        {
            var orgnr = "123456789";
            var company = CreateSampleCompanyResponse(orgnr);

            _companyService
                .GetAsync(orgnr, _ct)
                .Returns(Result.Success(company));

            await GetCompanyByOrgnr.Handle(orgnr, _companyService, _ct);

            await _companyService
                .Received(1)
                .GetAsync(orgnr, _ct);
        }

        private static CompanyResponse CreateSampleCompanyResponse(string orgnr = "123456789")
        {
            return new CompanyResponse
            {
                OrganizationNumber = orgnr,
                Name = "Test Company",
                HasRegisteredEmployeeCount = false
            };
        }
    }
}