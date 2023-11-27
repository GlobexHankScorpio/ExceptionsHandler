using Serilog.Events;

namespace ExceptionHandler.Definitions;
internal class LogConfiguration : Dictionary<LogEventLevel, string>
{
    public void Add(LogLevelPath conf)
    {
        Add(conf.level, conf.path);
    }

    public new string this[LogEventLevel title]
    {
        get { return base[title]; }
        set { base[title] = value; }
    }
}