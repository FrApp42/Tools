using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;
using System.Text.RegularExpressions;

namespace FrenchyApps42.System.WakeOnLan
{
    public partial class WakeOnLan
    {
        public static async Task WakeUp(string macAddress)
        {
            byte[] magicPacket = WakeOnLan.BuildMagicPacket(macAddress);

            IEnumerable<NetworkInterface> networkInterfaces = NetworkInterface
                .GetAllNetworkInterfaces()
                .Where((n) => n.NetworkInterfaceType != NetworkInterfaceType.Loopback && n.OperationalStatus == OperationalStatus.Up);

            foreach (NetworkInterface networkInterface in networkInterfaces)
            {
                IPInterfaceProperties iPInterfaceProperties = networkInterface.GetIPProperties();

                foreach (MulticastIPAddressInformation multicastIPAddressInformation in iPInterfaceProperties.MulticastAddresses)
                {
                    IPAddress multicastIpAddress = multicastIPAddressInformation.Address;

                    if (multicastIpAddress.ToString().StartsWith("ff02::1%", StringComparison.OrdinalIgnoreCase))
                    {
                        UnicastIPAddressInformation? unicastIPAddressInformation = iPInterfaceProperties
                            .UnicastAddresses
                            .Where((u) => u.Address.AddressFamily == AddressFamily.InterNetworkV6 && !u.Address.IsIPv6LinkLocal)
                            .FirstOrDefault();

                        if (unicastIPAddressInformation != null)
                        {
                            await WakeOnLan.SendWakeOnLan(unicastIPAddressInformation.Address, multicastIpAddress, magicPacket);
                        }
                    }
                    else if (multicastIpAddress.ToString().Equals("224.0.0.1"))
                    {
                        UnicastIPAddressInformation? unicastIPAddressInformation = iPInterfaceProperties
                            .UnicastAddresses
                            .Where((u) => u.Address.AddressFamily == AddressFamily.InterNetwork)
                            .Where((u) => !iPInterfaceProperties.GetIPv4Properties().IsAutomaticPrivateAddressingActive)
                            .FirstOrDefault();

                        if (unicastIPAddressInformation != null)
                        {
                            await WakeOnLan.SendWakeOnLan(unicastIPAddressInformation.Address, multicastIpAddress, magicPacket);
                        }
                    }
                }
            }

        }

        public static byte[] BuildMagicPacket(string macAddress)
        {
            macAddress = MacFormatter().Replace(macAddress, "");
            byte[] macBytes = Convert.FromHexString(macAddress);

            IEnumerable<byte> header = Enumerable.Repeat((byte)0xff, 6);
            IEnumerable<byte> data = Enumerable.Repeat(macBytes, 16).SelectMany(m => m);

            return header.Concat(data).ToArray();
        }

        public static async Task SendWakeOnLan(IPAddress localIpAddress, IPAddress multicastIpAddress, byte[] magicPacket)
        {
            UdpClient client = new(new IPEndPoint(localIpAddress, 0));
            await client.SendAsync(magicPacket, magicPacket.Length, new IPEndPoint(multicastIpAddress, 9));
        }

        [GeneratedRegex("[: -]")]
        private static partial Regex MacFormatter();
    }
}
