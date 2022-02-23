using System.Diagnostics;
using System.Net.NetworkInformation;

namespace Pinger
{
    /// <summary>
    /// Send ICMP ping to host.
    /// Wrapper on <see cref="System.Net.NetworkInformation.Ping"/>
    /// </summary>
    internal class PingSender
    {
        private readonly int timeout;
        private readonly int buffer;

        public PingSender(int timeout, int buffer)
        {
            if (timeout <= 0)
            {
                throw new ArgumentException("Invalid timeout value", nameof(timeout));
            }

            if (buffer <= 0 || 1024 < buffer)
            {
                throw new ArgumentException("Invalid buffer size", nameof(buffer));
            }

            this.timeout = timeout;
            this.buffer = buffer;
        }

        public PingResult Send(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                throw new ArgumentNullException(nameof(address));
            }

            var ping = new Ping();
            var options = new PingOptions
            {
                DontFragment = true
            };
            var buf = new byte[buffer];

            var result = new PingResult
            {
                Ping = TimeSpan.Zero,
                Status = PingStatus.Error,
            };
            try
            {
                var reply = ping.Send(address, timeout, buf, options);
                if (reply.Status == IPStatus.Success)
                {
                    result.Status = PingStatus.Success;
                    result.Ping = TimeSpan.FromMilliseconds(reply.RoundtripTime);
                }
            }
            catch (PingException pex)
            {
                Debug.WriteLine($"Ping error: {pex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Common error: {ex.Message}");
            }

            return result;
        }
    }
}
