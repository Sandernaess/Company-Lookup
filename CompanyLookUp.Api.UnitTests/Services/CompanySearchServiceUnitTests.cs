using CompanyLookup.Api.Common;
using CompanyLookup.Api.External.Brreg.Models.Enhet;
using CompanyLookup.Api.External.Brreg.Services.Enhet;
using CompanyLookup.Api.Models.Companies;
using CompanyLookup.Api.Services.Companies;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace CompanyLookUp.Api.UnitTests.Services
{
    [TestClass]
    public sealed class CompanySearchServiceUnitTests
    {
        private IEnhetRepository _repository = null!;
        private CompanySearchService _service = null!;
        private CancellationToken _ct;

        [TestInitialize]
        public void Setup()
        {
            _repository = Substitute.For<IEnhetRepository>();
            _service = new CompanySearchService(_repository);
            _ct = CancellationToken.None;
        }

        [TestMethod]
        public async Task SearchAsync_When_Query_Is_Null_Throws_ArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsExactlyAsync<ArgumentNullException>(
                () => _service.SearchAsync(null!, _ct));
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("   ")]
        public async Task SearchAsync_With_Invalid_Name_Returns_ValidationFailure(string? name)
        {
            // Arrange
            var query = new CompanySearchQuery(name!, 1, 10);

            // Act
            var result = await _service.SearchAsync(query, _ct);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(ErrorType.Validation, result.ErrorType);
        }

        [TestMethod]
        public async Task SearchAsync_With_Valid_Query_Calls_Service_With_Correct_Parameters()
        {
            // Arrange
            var query = new CompanySearchQuery("TestCompany", 2, 15);

            _repository
                .SearchEnheterByNameAsync(Arg.Any<EnhetSearchQuery>(), _ct)
                .Returns([]);

            // Act
            await _service.SearchAsync(query, _ct);

            // Assert
            await _repository.Received(1).SearchEnheterByNameAsync(
                Arg.Is<EnhetSearchQuery>(q => 
                    q.Name == query.Name && 
                    q.Page == query.Page && 
                    q.Size == query.Size),
                _ct);
        }

        [TestMethod]
        public async Task SearchAsync_When_Service_Returns_Empty_Collection_Returns_Empty_Collection()
        {
            // Arrange
            var query = new CompanySearchQuery("Test", 1, 10);
            
            _repository
                .SearchEnheterByNameAsync(Arg.Any<EnhetSearchQuery>(), _ct)
                .Returns([]);

            // Act
            var result = await _service.SearchAsync(query, _ct);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
            Assert.IsEmpty(result.Value);
        }

        [TestMethod]
        public async Task SearchAsync_When_Service_Returns_Companies_Maps_And_Returns_Them()
        {
            // Arrange
            var query = new CompanySearchQuery("Test", 1, 10);

            var enhetResponses = new List<EnhetResponse>
            {
                new() { Organisasjonsnummer = "1", Navn = "Company A", HarRegistrertAntallAnsatte = false },
                new() { Organisasjonsnummer = "2", Navn = "Company B", HarRegistrertAntallAnsatte = true }
            };

            _repository
                .SearchEnheterByNameAsync(Arg.Any<EnhetSearchQuery>(), _ct)
                .Returns(enhetResponses);

            // Act
            var result = await _service.SearchAsync(query, _ct);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
            Assert.HasCount(2, result.Value);
        }

        [TestMethod]
        public async Task SearchAsync_With_Valid_Query_Passes_CancellationToken_To_Service()
        {
            // Arrange
            var query = new CompanySearchQuery("Test", 1, 10);
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            _repository
                .SearchEnheterByNameAsync(Arg.Any<EnhetSearchQuery>(), token)
                .Returns([]);

            // Act
            await _service.SearchAsync(query, token);

            // Assert
            await _repository.Received(1)
                .SearchEnheterByNameAsync(
                    Arg.Any<EnhetSearchQuery>(), 
                    token);
        }

        [TestMethod]
        public async Task SearchAsync_When_Service_Throws_Exception_Exception_Is_Propagated()
        {
            // Arrange
            var query = new CompanySearchQuery("Test", 1, 10);

            _repository
                .SearchEnheterByNameAsync(Arg.Any<EnhetSearchQuery>(), _ct)
                .ThrowsAsync(new Exception("Service error"));

            // Act & Assert
            var ex = await Assert.ThrowsExactlyAsync<Exception>(
                () => _service.SearchAsync(query, _ct));

            Assert.AreEqual("Service error", ex.Message);
        }

        [TestMethod]
        public async Task SearchAsync_Maps_Enhet_Properties_To_Company_Properties()
        {
            // Arrange
            var query = new CompanySearchQuery("Test", 1, 10);
            var enhetResponse = new EnhetResponse 
            { 
                Organisasjonsnummer = "123456789", 
                Navn = "Test Company",
                HarRegistrertAntallAnsatte = false
            };
            var enhetResponses = new List<EnhetResponse> { enhetResponse };

            _repository
                .SearchEnheterByNameAsync(Arg.Any<EnhetSearchQuery>(), _ct)
                .Returns(enhetResponses);

            // Act
            var result = await _service.SearchAsync(query, _ct);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);

            var companies = result.Value.ToList();
            Assert.HasCount(1, companies);
            Assert.AreEqual(enhetResponse.Organisasjonsnummer, companies[0].OrganizationNumber);
            Assert.AreEqual(enhetResponse.Navn, companies[0].Name);
        }
    }
}