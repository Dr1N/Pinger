namespace Pinger
{
    /// <summary>
    /// One ping result
    /// </summary>
    internal class PingResult
    {
        public PingStatus Status { get; set; }

        public TimeSpan Ping { get; set; }

        public override string ToString()
        {
            return Status == PingStatus.Success
                ? $"{Ping.TotalMilliseconds} ms"
                : "Error";
        }
    }
}
