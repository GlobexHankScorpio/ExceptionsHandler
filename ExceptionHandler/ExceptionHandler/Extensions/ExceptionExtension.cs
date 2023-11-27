
namespace ExceptionHandler.Extensions;

internal static class ExceptionsExtension
{
    /// <summary>
    /// Gets all the inner exception levels.
    /// </summary>
    /// <param name="exception"></param>
    /// <returns></returns>
    internal static IEnumerable<Exception> DescendantsAndSelf(this Exception exception)
    {
        do
        {
            yield return exception;
            exception = exception.InnerException;

        } while (exception is not null);
    }

    /// <summary>
    /// Unifies all the inner exception messages retrieved from DescendantsAndSelf()
    /// </summary>
    /// <param name="exceptions"></param>
    /// <returns></returns>
    internal static string StringifyMessages(this IEnumerable<Exception> exceptions)
    {
        IEnumerable<string> messages = exceptions.Select((ex, index) => BuildMessage(ex, index += 1));

        return string.Join(Environment.NewLine, messages);
    }

    private static string BuildMessage(Exception ex, int index)
    {
        var lines = ex.StackTrace?.Split("\r\n").Select(e => e.TrimStart());

        string allLines = string.Join(Environment.NewLine, lines);

        return "Inner exception info level " + index + ": [" + ex.Message + "]" +
                "\r\nStack trace from level " + index + ": [" + allLines + "]";
    }
}


