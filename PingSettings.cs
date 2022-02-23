using System.Net;

namespace Pinger
{
    internal class PingSettings
    {
        public int Timeout { get; }

        public int Buffer { get; }

        public int Batch { get;  }

        public string Target { get; }

        public int Delay { get; }

        public PingSettings(
            int timeout,
            int buffer,
            int batch,
            string target,
            int delay)
        {
            if (timeout <= 0)
            {
                throw new ArgumentException("Invalid timeout value in configuration. Should be > 0" , nameof(timeout));
            }

            if (buffer <= 0 || 1024 < buffer)
            {
                throw new ArgumentException("Invalid buffer size in configuration. Should be in (0, 1024]", nameof(buffer));
            }

            if (batch <= 0 || batch > 100)
            {
                throw new ArgumentException("Invalid bacth size in configuration. Shoule be in (0, 100]", nameof(buffer));
            }

            if (delay <= 500)
            {
                throw new ArgumentException("Invalid delay size in configuration. Shoule be > 500 (ms)", nameof(buffer));
            }

            if (!Uri.TryCreate(target, UriKind.Absolute, out var _)
                || !IPAddress.TryParse(target, out var _))
            {
                throw new ArgumentException($"Invalid target for ping. Value: {target}");
            }

            Timeout = timeout;
            Buffer = buffer;
            Batch = batch;
            Target = target;
            Delay = delay;
        }
    }
}
