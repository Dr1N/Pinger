using Microsoft.Extensions.Configuration;

namespace Pinger
{
    /// <summary>
    /// Main app class. Collect all logic
    /// </summary>
    internal class PingApp
    {
        private const string ConfigurationFileName = "appsettings.json";

        private readonly IConfiguration configuration;
        private bool isRunning = false;

        public PingApp(string[] args)
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile(ConfigurationFileName);
            builder.AddCommandLine(args);

            configuration = builder.Build();
        }

        public void Run()
        {
            isRunning = true;

            var timeoutInSeconds = int.Parse(configuration["timeout"]);
            var bufferInBytes = int.Parse(configuration["buffer"]);
            var sender = new PingSender(timeoutInSeconds, bufferInBytes);

            // TODO: use batch ping

            var reply = sender.Send(configuration["target"]);
            
            // TODO: prepare result
            // TODO: pring result
        }

        public void Stop()
        {
            isRunning = false;
        }
    }
}
