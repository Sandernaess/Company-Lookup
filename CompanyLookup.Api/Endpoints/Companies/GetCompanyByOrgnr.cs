using CompanyLookup.Api.Common;
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
            if (string.IsNullOrWhiteSpace(orgnr))
            {
                return TypedResults.BadRequest("Orgnr is required.");
            }

            if (orgnr.Length != 9)
            {
                return TypedResults.BadRequest("Orgnr must be 9 digits long.");
            }

            var result = await service.GetAsync(orgnr, cancellationToken);

            return result.ToHttpResult();
        }
    }
}