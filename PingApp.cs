using Microsoft.Extensions.Configuration;

namespace Pinger
{
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
            var reply = sender.Send(configuration["address"]);
            Console.WriteLine(reply.Status);
            Console.WriteLine(reply.Ping.TotalMilliseconds);
        }

        public void Stop()
        {
            isRunning = false;
        }
    }
}
