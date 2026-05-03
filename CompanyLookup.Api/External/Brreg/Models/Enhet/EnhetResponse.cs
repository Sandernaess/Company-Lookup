namespace CompanyLookup.Api.External.Brreg.Models.Enhet
{
    public class EnhetResponse
    {
        public required string Organisasjonsnummer { get; set; }
        public required string Navn { get; set; }
        public EnhetAdresseResponse? Postadresse { get; set; }
        public EnhetAdresseResponse? Forretningsadresse { get; set; }

        public bool Konkurs { get; set; }
        public string? Konkursdato { get; set; }

        public string? Hjemmeside { get; set; }
        public int AntallAnsatte { get; set; }
        public required bool HarRegistrertAntallAnsatte { get; set; }

        public string? Epostadresse { get; set; }
        public string? Telefon { get; set; }
        public string? Mobil { get; set; }
    }
}

