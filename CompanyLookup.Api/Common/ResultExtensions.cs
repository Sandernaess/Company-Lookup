namespace CompanyLookup.Api.Common
{
    public static class ResultExtensions
    {
        public static IResult ToHttpResult(this Result result)
        {
            if (result.IsSuccess)
            {
                return Results.Ok();
            }

            return result.ErrorType switch
            {
                ErrorType.Validation => Results.BadRequest(result.Error),
                ErrorType.NotFound => Results.NotFound(result.Error),
                ErrorType.Conflict => Results.Conflict(result.Error),
                ErrorType.Unauthorized => Results.Unauthorized(),
                _ => Results.InternalServerError()
            };
        }

        public static IResult ToHttpResult<T>(this Result<T> result)
        {
            if (result.IsSuccess)
            {
                return Results.Ok(result.Value);
            }

            return result.ErrorType switch
            {
                ErrorType.Validation => Results.BadRequest(result.Error),
                ErrorType.NotFound => Results.NotFound(result.Error),
                ErrorType.Conflict => Results.Conflict(result.Error),
                ErrorType.Unauthorized => Results.Unauthorized(),
                _ => Results.InternalServerError()
            };
        }
    }
}
