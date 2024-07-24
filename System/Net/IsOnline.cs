using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace FrApps42.System.Net
{
    public class IsOnline
    {
        private string _hostname;
        private IPAddress _address;

        public int TimeOut { get; set; } = 5;

        public IsOnline(string hostname)
        {
            _hostname = hostname;
        }

        public IsOnline(string hostname, int timeout): this(hostname)
        {
            TimeOut = timeout;
        }

        public IsOnline(IPAddress address)
        {
            _address = address;
        }

        public IsOnline(IPAddress address, int timeout): this(address)
        {
            TimeOut = timeout;
        }

        public bool Check()
        {
            string target = !(string.IsNullOrEmpty(_hostname)) ? _hostname : _address.ToString();

            Ping sender = new();
            PingOptions options = new()
            {
                DontFragment = true,
            };

            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);

            PingReply reply = sender.Send(target, TimeOut, buffer, options);

            if (reply.Status != IPStatus.Success)
                return false;

            return true;
        }
    }
}
