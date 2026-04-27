using CompanyLookup.Api.Services.Companies;
using Microsoft.AspNetCore.Mvc;

namespace CompanyLookup.Api.Endpoints.Companies
{
    public static class GetCompanyByOrgnr
    {
        public static async Task<IResult> Handle(
            string orgnr,
            [FromServices] ICompanyService service,
            CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(orgnr)) {
                    return Results.BadRequest("Missing orgnr.");
                }

                if (orgnr.Length != 9)
                {
                    return Results.BadRequest("Orgnr must be 9 digits long.");
                }

                var company = await service.GetAsync(orgnr, cancellationToken);
                if (company is null)
                {
                    return Results.NotFound($"Company with orgnr {orgnr} not found.");
                }

                return Results.Ok(company);
            }
            catch (Exception ex)
            {
                var errorMsg = $"Unknown error occured when fetching company with orgnr: {orgnr} - {ex.Message}";
                Console.WriteLine(errorMsg); // TODO: Log to a logging framework

                return Results.InternalServerError();
            }
        }
    }
}