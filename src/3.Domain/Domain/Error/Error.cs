namespace Domain.Error
{
    public enum CategoryErrorType
    {
        Hub,
        Provider
    }
    public enum ErrorType
    {
        Unauthorized,
        Setup,
        Parse,
        Timeout,        
        Error,
        TooManyRequest,
        NoResults
    }

    public class Error
    {
        public Error(string? code, string? message, ErrorType errorType, CategoryErrorType categoryType)
        {
            Code = code;
            Message = message;
            Type = errorType;
            Category = categoryType;
        }
        public string? Code { get; set; } = null;
        public string? Message { get; set; } = null;
        public ErrorType? Type { get; set; }
        public CategoryErrorType? Category { get; set; }
    }
}
