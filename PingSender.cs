using System.Net.NetworkInformation;

namespace Pinger
{
    /// <summary>
    /// Send ICMP ping to target.
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

        public PingResult Send(string target)
        {
            if (string.IsNullOrWhiteSpace(target))
            {
                throw new ArgumentNullException(nameof(target));
            }

            using var ping = new Ping();
            var options = new PingOptions
            {
                DontFragment = true
            };
            var buf = new byte[buffer];

            try
            {
                var reply = ping.Send(target, timeout, buf, options);
                return new PingResult(reply);
            }
            catch (PingException pex)
            {
                return new PingResult(pex.Message);
            }
            catch (Exception ex)
            {
                return new PingResult(ex.Message);
            }
        }
    }
}
