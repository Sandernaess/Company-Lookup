namespace CompanyLookup.Api.Common
{
    public static class ResultExtensions
    {
        public static IResult ToHttpResult(this Result result)
        {
            if (result.IsSuccess)
            {
                return TypedResults.Ok();
            }

            return result.ErrorType switch
            {
                ErrorType.Validation => TypedResults.BadRequest(result.Error),
                ErrorType.NotFound => TypedResults.NotFound(result.Error),
                ErrorType.Conflict => TypedResults.Conflict(result.Error),
                ErrorType.Unauthorized => TypedResults.Problem(result.Error, statusCode: 401),
                _ => throw new InvalidOperationException($"Unhandled ErrorType '{result.ErrorType}' encountered in HTTP mapping.")
            };
        }

        public static IResult ToHttpResult<T>(this Result<T> result, Func<T, IResult>? onSuccess = null)
        {
            if (result.IsSuccess)
            {
                return onSuccess is not null
                    ? onSuccess(result.Value!)
                    : TypedResults.Ok(result.Value);
            }

            return result.ErrorType switch
            {
                ErrorType.Validation => TypedResults.BadRequest(result.Error),
                ErrorType.NotFound => TypedResults.NotFound(result.Error),
                ErrorType.Conflict => TypedResults.Conflict(result.Error),
                ErrorType.Unauthorized => TypedResults.Problem(result.Error, statusCode: 401),
                _ => throw new InvalidOperationException($"Unhandled ErrorType '{result.ErrorType}' encountered in HTTP mapping.")
            };
        }
    }
}
