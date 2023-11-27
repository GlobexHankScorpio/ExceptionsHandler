using Serilog.Events;
using Serilog.Formatting;
using System.Text.Json;

namespace ExceptionHandler.Definitions;

internal class JsonErrorFormatter : ITextFormatter
{
    JsonSerializerOptions options = new() { WriteIndented = true };
    public void Format(LogEvent logEvent, TextWriter output)
    {
        if (logEvent.Exception is null)
        {
            output.Write(logEvent.MessageTemplate + Environment.NewLine);
            return;
        }
        return;
    }
}
