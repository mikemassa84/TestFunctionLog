using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;
using NLog.Targets.Wrappers;
using System.Reflection;
using TestFunctionLog;

[assembly: FunctionsStartup(typeof(Startup))]
namespace TestFunctionLog
{
    public class Startup : FunctionsStartup
    {
        private readonly Logger _logger;

        public Startup()
        {
            ConfigurationItemFactory.Default = new ConfigurationItemFactory(typeof(ILogger).GetTypeInfo().Assembly);

            var logConsole = new ConsoleTarget("logconsole");
            var asyncConsole = new AsyncTargetWrapper(logConsole) { Name = "logconsole" };

            var config = new LoggingConfiguration();
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, asyncConsole);
            LogManager.Configuration = config;

            _logger = LogManager.GetCurrentClassLogger();
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddLogging((loggingBuilder) =>
            {
                loggingBuilder.AddNLog(new NLogProviderOptions() { AutoShutdown = true, ShutdownOnDispose = true, RemoveLoggerFactoryFilter = true });
            });
        }
    }
}
