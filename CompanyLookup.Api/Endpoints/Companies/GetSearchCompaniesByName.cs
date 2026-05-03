using CompanyLookup.Api.Services.Companies;
using Microsoft.AspNetCore.Mvc;

namespace CompanyLookup.Api.Endpoints.Companies
{
    public static class GetSearchCompaniesByName
    {
        public static async Task<IResult> Handle(
            string name,
            [FromServices] ICompanyService service,
            CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return Results.BadRequest("Missing name.");
                }

                var companies = await service.SearchAsync(name, cancellationToken);

                return Results.Ok(companies);
            }
            catch (Exception ex)
            {
                var errorMsg = $"Unknown error occured when searching for companies with name: {name} - {ex.Message}";
                Console.WriteLine(errorMsg); // TODO: Log to a logging framework

                return Results.InternalServerError();
            }
        }
    }
}
