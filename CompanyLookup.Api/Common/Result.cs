namespace CompanyLookup.Api.Common
{
    public abstract class Result(
        bool isSuccess,
        string? error,
        ErrorType? errorType = null)
    {
        public bool IsSuccess { get; } = isSuccess;
        public string? Error { get; } = error;
        public ErrorType? ErrorType { get; } = errorType;

        public static Result Success() => new SuccessResult();

        public static Result Failure(
            string error,
            ErrorType errorType) =>
            new FailureResult(error, errorType);

        public static Result<T> Success<T>(T value) =>
            new Result<T>.SuccessResult(value);

        public static Result<T> Failure<T>(
            string error,
            ErrorType errorType) =>
            new Result<T>.FailureResult(error, errorType);

        private sealed class SuccessResult() : Result(true, null);

        private sealed class FailureResult(
            string error,
            ErrorType errorType)
            : Result(false, error, errorType);
    }

    public abstract class Result<T>(
        bool isSuccess,
        string? error,
        T? value,
        ErrorType? errorType = null) : Result(isSuccess, error, errorType)
    {
        public T? Value { get; } = value;

        internal sealed class SuccessResult(T value)
            : Result<T>(true, null, value);

        internal sealed class FailureResult(
            string error,
            ErrorType errorType) : Result<T>(false, error, default, errorType);
    }
}
