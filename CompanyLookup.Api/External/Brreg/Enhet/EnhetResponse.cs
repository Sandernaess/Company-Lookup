namespace CompanyLookup.Api.External.Brreg.Enhet
{
    public class EnhetResponse
    {
        public required string Organisasjonsnummer { get; set; }
        public required string Navn { get; set; }
        public EnhetAdresse? Postadresse { get; set; }
        public EnhetAdresse? Forretningsadresse { get; set; }

        public bool Konkurs { get; set; }
        public string? Konkursdato { get; set; }

        public string? Hjemmeside { get; set; }
        public int AntallAnsatte { get; set; }
        public required bool HarRegistrertAntallAnsatte { get; set; }

        public string? Epostadresse { get; set; }
        public string? Telefon { get; set; }
        public string? Mobil { get; set; }

        public Kapital? Kapital { get; set; }
    }

    public class EnhetAdresse
    {
        public string? Kommune { get; set; }
        public string? Landkode { get; set; }
        public string? Postnummer { get; set; }
        public string[]? Adresse { get; set; }
        public string? Land { get; set; }
        public string? Kommunenummer { get; set; }
        public string? Poststed { get; set; }
    }

    public class Kapital
    {
        public int AntallAksjer { get; set; }
        public string? Type { get; set; }
        public int Bundet { get; set; }
        public string? Valuta { get; set; }
        public int Innbetalt { get; set; }
        public bool FulltInnbetalt { get; set; }
        public string? InnfortDato { get; set; }
    }
}

