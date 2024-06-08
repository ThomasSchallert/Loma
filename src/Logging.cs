using Serilog;
using System;
using System.IO;
using Serilog.Core;
using System.Reflection;

namespace LomaPro
{
    public class Logging
    {
        static public Logger logger = new LoggerConfiguration()
            .WriteTo.File(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "log-.txt"),
                          rollingInterval: RollingInterval.Day,
                          retainedFileCountLimit: 15)
            .CreateLogger();
    }
}