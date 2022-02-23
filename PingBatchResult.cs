namespace Pinger
{
    /// <summary>
    /// Batch ping result. Calculated values
    /// </summary>
    internal class PingBatchResult
    {
        public DateTime Time { get; set; }

        public int Errors { get; set; }

        public TimeSpan AvgPing { get; set; }

        public int Count { get; set; }

        public override string ToString()
        {
            return $"{Time.ToLongTimeString()} {AvgPing.TotalMilliseconds} ms Count: {Count} Errors: {Errors}";
        }
    }
}
