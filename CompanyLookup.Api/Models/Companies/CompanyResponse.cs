namespace CompanyLookup.Api.Models.Companies
{
    public class CompanyResponse
    {
        public required string OrganizationNumber { get; set; }
        public required string Name { get; set; }

        public string? Website { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }

        public int EmployeeCount { get; set; }
        public bool HasRegisteredEmployeeCount { get; set; }

        public string? Address { get; set; }
    }
}
