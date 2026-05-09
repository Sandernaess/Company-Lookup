namespace CompanyLookup.Api.Models.Companies
{
    public class CompanySearchRequest
    {
        public string Name { get; set; } = string.Empty;
        public int? Page { get; set; }
        public int? Size { get; set; }
    }
}
