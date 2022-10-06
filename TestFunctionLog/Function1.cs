using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using NLog;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TestFunctionLog
{
    public class Function1
    {
        private static readonly Logger _nLogger = LogManager.GetCurrentClassLogger();

        private readonly ILogger<Function1> _msLogger;

        public Function1(ILogger<Function1> log)
        {
            _msLogger = log;
        }

        [FunctionName("Function1")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            var stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < 1000; i++)
            {
                _nLogger.Info($"{i}");
            }

            stopwatch.Stop();
            var nlogElapsed = stopwatch.Elapsed;

            stopwatch.Restart();
            for (int i = 0; i < 1000; i++)
            {
                _msLogger.LogInformation($"{i}");
            }
            stopwatch.Stop();

            return new OkObjectResult($"NLog: {nlogElapsed}. ILogger: {stopwatch.Elapsed}");
        }

        [FunctionName("Function2")]
        public async Task<IActionResult> Run2([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            var stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < 1000; i++)
            {
                _nLogger.Info($"{i}");
            }

            stopwatch.Stop();
            var nlogElapsed = stopwatch.Elapsed;

            return new OkObjectResult($"NLog: {nlogElapsed}");
        }

        [FunctionName("Function3")]
        public async Task<IActionResult> Run3([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            var stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < 1000; i++)
            {
                _msLogger.LogInformation($"{i}");
            }

            stopwatch.Stop();

            return new OkObjectResult($"ILogger: {stopwatch.Elapsed}");
        }
    }
}
