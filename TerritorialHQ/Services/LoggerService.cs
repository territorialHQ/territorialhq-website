using Serilog;
using Serilog.Core;

namespace TerritorialHQ.Services
{
    public class LoggerService
    {
        public Logger Log { get; set; }

        public LoggerService(IWebHostEnvironment env)
        {
            Log = new LoggerConfiguration()
               .MinimumLevel.Information()
#if DEBUG
               .WriteTo.File(env.WebRootPath + "/Data/Logs/" + "debug-log-.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: null)
#else
               .WriteTo.File(env.WebRootPath + "/Data/Logs/" + "log-.txt", rollingInterval: RollingInterval.Day)
#endif
               .CreateLogger();
        }
    }
}
