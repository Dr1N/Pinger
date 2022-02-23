using System.Net.NetworkInformation;

namespace Pinger
{
    /// <summary>
    /// One ping result.
    /// Wrapper for <see cref="PingReply"/>
    /// </summary>
    internal class PingResult
    {
        public DateTime Time { get; }

        private string ErrorMessage { get; }

        private PingReply Reply { get; set; }

        public PingStatus Status => Reply?.Status == IPStatus.Success
            ? PingStatus.Success
            : PingStatus.Error;

        public TimeSpan Ping => TimeSpan.FromMilliseconds(Reply != null ? Reply.RoundtripTime : 0);

        public string Error => Reply != null
            ? Reply.Status.ToString()
            : ErrorMessage;

        public PingResult(PingReply reply)
        {
            Reply = reply ?? throw new ArgumentNullException(nameof(reply));
            Time = DateTime.Now;
            ErrorMessage = string.Empty;
        }

        public PingResult(string error)
        {
            Time = DateTime.Now;
            ErrorMessage = error;
        }

        public override string ToString()
        {
            return Status == PingStatus.Success
                ? $"{Time.ToLongTimeString()} : {Ping.TotalMilliseconds} ms"
                : $"Error: {Error}";
        }
    }
}
