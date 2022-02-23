using ConsoleTables;

namespace Pinger
{
    /// <summary>
    /// Result printer in console
    /// </summary>
    internal class ConsolePrinter
    {
        private readonly string delimiter = new('-', 85);

        public void PrintResult(PingSettings settings, PingBatchResult result, List<PingResult> errors)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            if (errors == null || errors.Any(e => e == null))
            {
                throw new ArgumentNullException(nameof(errors));
            }

            Console.Clear();
            PrintSettings(settings);
            PrintResult(result);
            PrintErrors(errors);
        }

        private void PrintSettings(PingSettings settings)
        {
            PrintHeader("PARAMETERS");

            var table = new ConsoleTable("Name", "Value");
            table.Configure(e => e.EnableCount = false)
                .Configure(e => e.NumberAlignment = Alignment.Right);
            table.AddRow("Target", settings.Target)
                .AddRow("Delay", $"{settings.Delay} ms")
                .AddRow("Buffer", $"{settings.Buffer} bytes")
                .AddRow("Batch Size", settings.BatchSize)
                .AddRow("Timeout", $"{settings.Timeout} ms");
            table.Write(Format.Minimal);
        }

        private void PrintResult(PingBatchResult result)
        {
            PrintHeader("CURRENT STATE");

            var table = new ConsoleTable("Time", "Ping", "Count", "Fails", "Max", "Min", "Avg", "Errors");
            table.Configure(e => e.EnableCount = false)
                .Configure(e => e.NumberAlignment = Alignment.Right);
            var currentAvg = Math.Round(result.CurrentAvgPing.TotalMilliseconds, 2).ToString("0.00");
            var maxPing = Math.Round(result.MaxPing, 2);
            var minPing = Math.Round(result.MinPing, 2);
            var avg = Math.Round(result.AvgPing, 2);

            table.AddRow(
                result.CurrentTime.ToLongTimeString(),
                $"{currentAvg} ms",
                $"{result.CurrentCount} req",
                result.CurrentErrors,
                maxPing.ToString("0.00") + " ms",
                minPing.ToString("0.00") + " ms",
                avg.ToString("0.00") + " ms",
                result.Errors);

            table.Write(Format.Default);
        }

        private void PrintErrors(List<PingResult> errors)
        {
            PrintHeader($"LAST ERRORS ({errors.Count})");

            var table = new ConsoleTable("Time", "Error");
            table.Configure(e => e.NumberAlignment = Alignment.Right)
                .Configure(e => e.EnableCount = true);
            foreach (var error in errors)
            {
                table.AddRow(error.Time.ToLongTimeString(), error.Error);
            }

            table.Write(Format.Minimal);
        }

        private void PrintHeader(string headerText)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(delimiter);
            Console.WriteLine("{0, -40}", headerText);
            Console.WriteLine(delimiter);
            Console.ResetColor();
        }
    }
}
