using Serilog.Events;

namespace ExceptionHandler.Definitions;

internal record LogLevelPath(LogEventLevel level, string path);
