using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FileWatcherService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private FileWatcherSystem _fileWatcher;
        private IConfiguration _configuration;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            buildConfiguration();
        }
        private void buildConfiguration()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            _configuration = builder.Build();
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {

            _fileWatcher = new FileWatcherSystem(_logger, _configuration.GetValue<string>("Configuration:Path_To_Monitor"));
            return base.StartAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _fileWatcher.MonitorDirectory();
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("The service has been terminated");
            return base.StopAsync(cancellationToken);
        }
    }
}
