namespace Pinger
{
    /// <summary>
    /// Batch ping result. Calculated values
    /// </summary>
    internal class PingBatchResult
    {
        public DateTime CurrentTime { get; private set; }

        public TimeSpan CurrentAvgPing { get; private set; }

        public int CurrentCount { get; private set; }

        public int CurrentErrors { get; private set; }

        public int Errors { get; private set; }

        public double MaxPing { get; private set; } = double.MinValue;

        public double MinPing { get; private set; } = double.MaxValue;

        public double AvgPing { get; private set; }

        public void Update(IEnumerable<PingResult> source)
        {
            // Current values
            var avg = TimeSpan.FromMilliseconds(
                Math.Round(
                    source.Select(e => e.Ping.TotalMilliseconds)
                        .Average(),
                    2));
            CurrentTime = DateTime.Now;
            CurrentAvgPing = avg;
            CurrentCount = source.Count();
            CurrentErrors = source.Count(e => e.Status != PingStatus.Success);

            // Calculated values
            Errors += CurrentErrors;
            MaxPing = Math.Max(MaxPing, source.Max(e => e.Ping.TotalMilliseconds));
            MinPing = Math.Min(MinPing, source.Min(e => e.Ping.TotalMilliseconds));
            AvgPing = AvgPing > 0
                ? (AvgPing + CurrentAvgPing.TotalMilliseconds) / 2.0
                : CurrentAvgPing.TotalMilliseconds;
        }

        public override string ToString()
        {
            return $"{CurrentTime.ToLongTimeString()} {AvgPing} ms Count: {CurrentCount} Errors: {CurrentErrors}";
        }
    }
}
