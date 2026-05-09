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
    public sealed class GetSearchCompaniesUnitTests
    {
        private ICompanySearchService _searchService = null!;
        private CancellationToken _ct;

        [TestInitialize]
        public void Setup()
        {
            _searchService = Substitute.For<ICompanySearchService>();
            _ct = CancellationToken.None;
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("   ")]
        public async Task Handle_With_Invalid_Name_Returns_BadRequest(string? name)
        {
            var request = new CompanySearchRequest { Name = name, Page = 1, Size = 10 };

            var result = await GetSearchCompanies.Handle(request, _searchService, _ct);

            var statusCodeResult = result as IStatusCodeHttpResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public async Task Handle_With_Short_Name_Returns_BadRequest()
        {
            var request = new CompanySearchRequest { Name = "A", Page = 1, Size = 10 };

            var result = await GetSearchCompanies.Handle(request, _searchService, _ct);

            var statusCodeResult = result as IStatusCodeHttpResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public async Task Handle_With_Valid_Name_Returns_Ok()
        {
            var request = new CompanySearchRequest { Name = "Test", Page = 1, Size = 10 };
            var companies = new List<CompanyResponse>
            {
                new CompanyResponse { OrganizationNumber = "123", Name = "Test", HasRegisteredEmployeeCount = false }
            };

            _searchService.SearchAsync(Arg.Any<CompanySearchQuery>(), _ct).Returns(companies);

            var result = await GetSearchCompanies.Handle(request, _searchService, _ct);

            var okResult = result as Ok<IEnumerable<CompanyResponse>>;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(companies, okResult.Value);
        }

        [TestMethod]
        public async Task Handle_When_Service_Throws_Exception_Returns_InternalServerError()
        {
            var request = new CompanySearchRequest { Name = "Test", Page = 1, Size = 10 };
            _searchService.SearchAsync(Arg.Any<CompanySearchQuery>(), _ct)
                .ThrowsAsync(new Exception("Service error"));

            var result = await GetSearchCompanies.Handle(request, _searchService, _ct);

            var statusCodeResult = result as IStatusCodeHttpResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public async Task Handle_With_Valid_Request_Passes_Correct_Page_And_Size_To_Service()
        {
            // Arrange
            var request = new CompanySearchRequest { Name = "Test", Page = 3, Size = 25 };
            var companies = new List<CompanyResponse>();
            CompanySearchQuery? capturedQuery = null;

            _searchService
                .SearchAsync(Arg.Do<CompanySearchQuery>(q => capturedQuery = q), _ct)
                .Returns(companies);

            // Act
            await GetSearchCompanies.Handle(request, _searchService, _ct);

            // Assert
            Assert.IsNotNull(capturedQuery);
            Assert.AreEqual(request.Page, capturedQuery.Page);
            Assert.AreEqual(request.Size, capturedQuery.Size);
            Assert.AreEqual(request.Name.Trim(), capturedQuery.Name);
        }

        [TestMethod]
        public async Task Handle_With_Multiple_Companies_Returns_All_Companies()
        {
            var request = new CompanySearchRequest { Name = "Test", Page = 1, Size = 10 };

            var companies = new List<CompanyResponse>
            {
                new CompanyResponse { OrganizationNumber = "1", Name = "A", HasRegisteredEmployeeCount = false },
                new CompanyResponse { OrganizationNumber = "2", Name = "B", HasRegisteredEmployeeCount = true }
            };

            _searchService.SearchAsync(Arg.Any<CompanySearchQuery>(), _ct).Returns(companies);

            var result = await GetSearchCompanies.Handle(request, _searchService, _ct);

            var okResult = result as Ok<IEnumerable<CompanyResponse>>;
            
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNotNull(okResult.Value);
            CollectionAssert.AreEqual(companies, okResult.Value.ToList());
        }

        [TestMethod]
        public async Task Handle_Passes_CancellationToken_To_Service()
        {
            var request = new CompanySearchRequest { Name = "Test", Page = 1, Size = 10 };
            var companies = new List<CompanyResponse>();
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            _searchService.SearchAsync(Arg.Any<CompanySearchQuery>(), token).Returns(companies);

            await GetSearchCompanies.Handle(request, _searchService, token);

            await _searchService.Received(1).SearchAsync(Arg.Any<CompanySearchQuery>(), token);
        }

        [TestMethod]
        public async Task Handle_When_Service_Returns_Empty_Result_Returns_Ok_With_Empty_Collection()
        {
            var request = new CompanySearchRequest { Name = "Test", Page = 1, Size = 10 };
            _searchService.SearchAsync(Arg.Any<CompanySearchQuery>(), _ct).Returns(Array.Empty<CompanyResponse>());

            var result = await GetSearchCompanies.Handle(request, _searchService, _ct);

            var okResult = result as Ok<IEnumerable<CompanyResponse>>;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNotNull(okResult.Value);
            Assert.IsFalse(okResult.Value.Any());
        }
    }
}