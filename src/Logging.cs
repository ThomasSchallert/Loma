using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog.Core;

namespace LomaPro
{
    public class Logging
    {
        static public Logger logger = new LoggerConfiguration()
            .WriteTo.File("log-.txt", rollingInterval:
            RollingInterval.Day, retainedFileCountLimit: 15).CreateLogger();

    }

}
