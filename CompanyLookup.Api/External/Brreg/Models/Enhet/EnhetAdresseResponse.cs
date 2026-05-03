namespace CompanyLookup.Api.External.Brreg.Models.Enhet
{
    public class EnhetAdresseResponse
    {
        public string? Kommune { get; set; }
        public string? Landkode { get; set; }
        public string? Postnummer { get; set; }
        public IEnumerable<string> Adresse { get; set; } = [];
        public string? Land { get; set; }
        public string? Kommunenummer { get; set; }
        public string? Poststed { get; set; }
    }
}
