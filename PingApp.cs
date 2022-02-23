using System.Diagnostics;
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
        private readonly ConsolePrinter consolePrinter;
        private bool isRunning = false;

        public PingApp(string[] args)
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile(ConfigurationFileName);
            builder.AddCommandLine(args);

            configuration = builder.Build();
            consolePrinter = new ConsolePrinter();
        }

        public async void Run()
        {
            isRunning = true;

            var settings = PrepareSettings(configuration);
            var sender = new PingSender(settings.Timeout, settings.Buffer);

            // Save all errors
            var errors = new List<PingResult>();
            while (isRunning)
            {
                // Current ping result
                var replies = new List<PingResult>(5);
                for (int i = 0; i < settings.Batch; i++)
                {
                    var reply = sender.Send(settings.Target);
                    replies.Add(reply);
                    if (reply.Status != PingStatus.Success)
                    {
                        errors.Add(reply);
                    }
                }
                
                // Print result to Console
                var batchResult = PrepareResult(replies);
                consolePrinter.PrintResult(settings, batchResult, errors);
                await Task.Delay(settings.Delay);
            }
        }

        public void Stop()
        {
            isRunning = false;
        }

        private static PingBatchResult PrepareResult(IEnumerable<PingResult> source)
        {
            var avg = TimeSpan.FromMilliseconds(
                Math.Round(
                    source.Select(e => e.Ping.TotalMilliseconds)
                        .Average(),
                    2));
            return new PingBatchResult
            {
                Count = source.Count(),
                Time = DateTime.Now,
                AvgPing = avg,
                Errors = source.Count(e => e.Status != PingStatus.Success),
            };
        }

        private static PingSettings PrepareSettings(IConfiguration configuration)
        {
            try
            {
                return new PingSettings(
                   int.Parse(configuration["timeout"]),
                   int.Parse(configuration["buffer"]),
                   int.Parse(configuration["batch"]),
                   configuration["target"],
                   int.Parse(configuration["delay"]));
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Invalid configuration format: {ex.Message}");
                Console.ResetColor();
                throw;
            }
        }
    }
}
