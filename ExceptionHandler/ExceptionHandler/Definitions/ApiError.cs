namespace ExceptionHandler.Definitions;

internal record ApiError(string Id, string Request, string Utc, string Reason, int? ResponseStatusCode, string[] Exception);