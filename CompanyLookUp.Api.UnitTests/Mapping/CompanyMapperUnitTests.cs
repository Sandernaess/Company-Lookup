using CompanyLookup.Api.External.Brreg.Models.Enhet;
using CompanyLookup.Api.Mapping.Companies;

namespace CompanyLookUp.Api.UnitTests.Mapping
{
    [TestClass]
    public sealed class CompanyMapperUnitTests
    {
        [TestMethod]
        public void ToCompany_With_Valid_Response_Maps_All_Fields()
        {
            var enhetResponse = CreateSampleEnhetResponse();

            var result = enhetResponse.ToCompany();

            Assert.IsNotNull(result);
            Assert.AreEqual("123456789", result.OrganizationNumber);
            Assert.AreEqual("Test Company", result.Name);
            Assert.AreEqual("www.testcompany.com", result.Website);
            Assert.AreEqual("test@company.com", result.Email);
            Assert.AreEqual("12345678", result.Phone);
            Assert.AreEqual(10, result.EmployeeCount);
            Assert.IsTrue(result.HasRegisteredEmployeeCount);
            Assert.IsNotNull(result.Address);
        }

        [TestMethod]
        public void ToCompany_With_Null_Address_Returns_Null_Address()
        {
            var enhetResponse = CreateSampleEnhetResponse();
            enhetResponse.Forretningsadresse = null;

            var result = enhetResponse.ToCompany();

            Assert.IsNotNull(result);
            Assert.IsNull(result.Address);
        }

        [TestMethod]
        public void ToCompany_With_Missing_Optional_Fields()
        {
            var enhetResponse = new EnhetResponse
            {
                Organisasjonsnummer = "123456789",
                Navn = "Test Company",
                HarRegistrertAntallAnsatte = false,
                Forretningsadresse = null
            };

            var result = enhetResponse.ToCompany();

            Assert.IsNotNull(result);
            Assert.AreEqual("123456789", result.OrganizationNumber);
            Assert.AreEqual("Test Company", result.Name);
            Assert.IsNull(result.Website);
            Assert.IsNull(result.Email);
            Assert.IsNull(result.Phone);
            Assert.AreEqual(0, result.EmployeeCount);
            Assert.IsFalse(result.HasRegisteredEmployeeCount);
            Assert.IsNull(result.Address);
        }

        [TestMethod]
        public void ToCompany_Phone_Prefers_Mobil_Over_Telefon()
        {
            var enhetResponse = CreateSampleEnhetResponse();
            enhetResponse.Mobil = "98765432";
            enhetResponse.Telefon = "11111111";

            var result = enhetResponse.ToCompany();

            Assert.AreEqual("98765432", result.Phone);
        }

        [TestMethod]
        public void ToCompany_Phone_Falls_Back_To_Telefon_When_Mobil_Null()
        {
            var enhetResponse = CreateSampleEnhetResponse();
            enhetResponse.Mobil = null;
            enhetResponse.Telefon = "11111111";

            var result = enhetResponse.ToCompany();

            Assert.AreEqual("11111111", result.Phone);
        }

        [TestMethod]
        public void ToCompany_Address_Includes_Postal_Code_And_City()
        {
            var enhetResponse = CreateSampleEnhetResponseWithAddress(
                adresse: "Main Street 1",
                postnummer: "4012",
                poststed: "Stavanger",
                land: "Norway"
            );

            var result = enhetResponse.ToCompany();

            Assert.IsNotNull(result.Address);
            Assert.Contains("Main Street 1", result.Address);
            Assert.Contains("4012 Stavanger", result.Address);
            Assert.Contains("Norway", result.Address);
        }

        [TestMethod]
        public void ToCompany_Address_Without_Postal_Code_Excludes_City()
        {
            var enhetResponse = CreateSampleEnhetResponseWithAddress(
                adresse: "Main Street 1",
                postnummer: null,
                poststed: "Stavanger",
                land: "Norway"
            );

            var result = enhetResponse.ToCompany();

            Assert.IsNotNull(result.Address);
            Assert.Contains("Main Street 1", result.Address);
            Assert.DoesNotContain("Stavanger", result.Address);
            Assert.Contains("Norway", result.Address);
        }

        [TestMethod]
        public void ToCompany_Address_Empty_Adresse_List_Returns_Null()
        {
            var enhetResponse = CreateSampleEnhetResponse();
            enhetResponse.Forretningsadresse!.Adresse = [];

            var result = enhetResponse.ToCompany();

            Assert.IsNull(result.Address);
        }

        private static EnhetResponse CreateSampleEnhetResponse()
        {
            return new EnhetResponse
            {
                Organisasjonsnummer = "123456789",
                Navn = "Test Company",
                HarRegistrertAntallAnsatte = true,
                AntallAnsatte = 10,
                Hjemmeside = "www.testcompany.com",
                Epostadresse = "test@company.com",
                Mobil = "12345678",
                Forretningsadresse = new()
                {
                    Adresse = ["Main Street 1"],
                    Postnummer = "4012",
                    Poststed = "Stavanger",
                    Land = "Norway"
                }
            };
        }

        private static EnhetResponse CreateSampleEnhetResponseWithAddress(
            string adresse,
            string? postnummer,
            string? poststed,
            string? land)
        {
            return new EnhetResponse
            {
                Organisasjonsnummer = "123456789",
                Navn = "Test Company",
                HarRegistrertAntallAnsatte = true,
                AntallAnsatte = 10,
                Forretningsadresse = new()
                {
                    Adresse = [adresse],
                    Postnummer = postnummer,
                    Poststed = poststed,
                    Land = land
                }
            };
        }
    }
}