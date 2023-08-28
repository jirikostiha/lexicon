namespace Lexicon.WebApi.HostedServices
{
    using Lexicon.WebApi.Controllers;
    using Microsoft.Extensions.Hosting;
    using System.Configuration;
    using System.Threading;
    using System.Threading.Tasks;

    public class SourcesInitializer : IHostedService
    {
        private readonly Configuration _configuration;
        private readonly ILogger<SourcesInitializer> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"> configuration </param>
        /// <param name="logger"> logger </param>
        public SourcesInitializer(ILogger<SourcesInitializer> logger)
        //public SourcesInitializer(Configuration configuration, ILogger<WordsController> logger)
        {
            //_configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Initialize data sources if they do not exist.
        /// </summary>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting..");

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}