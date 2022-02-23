using Microsoft.Extensions.Configuration;

namespace Pinger
{
    /// <summary>
    /// Main app class. Collect all logic
    /// </summary>
    internal class PingApp
    {
        private const string ConfigurationFileName = "appsettings.json";
        private const int ErrorLimit = 5;

        private readonly IConfiguration configuration;
        private readonly ConsolePrinter consolePrinter;
        private bool isRunning = false;

        public PingBatchResult Result { get; set; }

        public PingApp(string[] args)
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile(ConfigurationFileName);
            builder.AddCommandLine(args);

            configuration = builder.Build();
            consolePrinter = new ConsolePrinter();
            Result = new PingBatchResult();
        }

        public async void Run()
        {
            isRunning = true;

            var settings = PrepareSettings(configuration);
            var sender = new PingSender(settings.Timeout, settings.Buffer);

            // Save all errors
            var errors = new List<PingResult>(10);
            while (isRunning)
            {
                try
                {
                    // Current ping result
                    var replies = new List<PingResult>(settings.BatchSize);
                    for (int i = 0; i < settings.BatchSize; i++)
                    {
                        var reply = sender.Send(settings.Target);
                        replies.Add(reply);

                        // Save last errors
                        if (reply.Status != PingStatus.Success)
                        {
                            if (errors.Count > ErrorLimit)
                            {
                                errors.RemoveAt(0);
                            }
                            errors.Add(reply);
                        }
                    }

                    // Prepare and print result to Console
                    Result.Update(replies);
                    consolePrinter.PrintResult(settings, Result, errors);
                    await Task.Delay(settings.Delay);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"App error: {ex.Message}");
                }
            }
        }

        public void Stop()
        {
            isRunning = false;
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
