using CompanyLookup.Api.External.Brreg.Enhet;
using CompanyLookup.Api.Models.Companies;

namespace CompanyLookup.Api.Mapping.Companies
{
    public static class CompanyMapper
    {
        public static CompanyResponse ToCompany(this EnhetResponse response)
        {
            return new CompanyResponse
            {
                OrganizationNumber = response.Organisasjonsnummer,
                Name = response.Navn,

                Website = response.Hjemmeside,
                Email = response.Epostadresse,
                Phone = response.Mobil ?? response.Telefon,

                EmployeeCount = response.AntallAnsatte,
                HasRegisteredEmployeeCount = response.HarRegistrertAntallAnsatte,

                Address = response.Forretningsadresse is not null ? MapToAddress(response.Forretningsadresse) : null
            };
        }

        private static string? MapToAddress(EnhetAdresseResponse enhetAdresse)
        {
            var firstRegisteredAdresse = enhetAdresse.Adresse?.FirstOrDefault();
            if (firstRegisteredAdresse is null)
            {
                return null;
            }

            var addressParts = new List<string> { firstRegisteredAdresse };

            if (!string.IsNullOrWhiteSpace(enhetAdresse.Postnummer) && !string.IsNullOrWhiteSpace(enhetAdresse.Poststed))
            {
                addressParts.Add($"{enhetAdresse.Postnummer} {enhetAdresse.Poststed}");
            }

            if (!string.IsNullOrWhiteSpace(enhetAdresse.Land))
            {
                addressParts.Add(enhetAdresse.Land);
            }

            return string.Join(", ", addressParts);
        }
    }
}

