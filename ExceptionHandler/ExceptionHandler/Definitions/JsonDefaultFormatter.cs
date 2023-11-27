using ExceptionHandler.Extensions;
using Serilog.Events;
using Serilog.Formatting;
using System.Text.Json;

namespace ExceptionHandler.Definitions;

internal class JsonDefaultFormatter : ITextFormatter
{
    readonly JsonSerializerOptions options = new() { WriteIndented = true };
    public void Format(LogEvent logEvent, TextWriter output)
    {
        LogFormat exceptionFormat = new($"{logEvent.Timestamp.UtcDateTime} ", $"{logEvent.MessageTemplate}");
        output.Write(JsonSerializer.Serialize(exceptionFormat, options) + Environment.NewLine);
    }
}
