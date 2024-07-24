using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;
using System.Text.RegularExpressions;

namespace FrApps42.System.Net
{
    public partial class WakeOnLan
    {
        /// <summary>
        /// Sends a Wake-on-LAN magic packet to a specified MAC address.
        /// </summary>
        /// <param name="macAddress">The MAC address of the device to wake up.</param>
        public static async Task WakeUp(string macAddress)
        {
            byte[] magicPacket = BuildMagicPacket(macAddress);

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
                            await SendWakeOnLan(unicastIPAddressInformation.Address, multicastIpAddress, magicPacket);
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
                            await SendWakeOnLan(unicastIPAddressInformation.Address, multicastIpAddress, magicPacket);
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Builds a Wake-on-LAN magic packet from a given MAC address.
        /// </summary>
        /// <param name="macAddress">The MAC address to include in the magic packet.</param>
        /// <returns>A byte array representing the magic packet.</returns>
        public static byte[] BuildMagicPacket(string macAddress)
        {
            macAddress = MacFormatter().Replace(macAddress, "");
            byte[] macBytes = Convert.FromHexString(macAddress);

            IEnumerable<byte> header = Enumerable.Repeat((byte)0xff, 6);
            IEnumerable<byte> data = Enumerable.Repeat(macBytes, 16).SelectMany(m => m);

            return header.Concat(data).ToArray();
        }

        /// <summary>
        /// Sends a Wake-on-LAN magic packet to a specified multicast IP address.
        /// </summary>
        /// <param name="localIpAddress">The local IP address to use for sending the packet.</param>
        /// <param name="multicastIpAddress">The multicast IP address to send the packet to.</param>
        /// <param name="magicPacket">The magic packet to send.</param>
        public static async Task SendWakeOnLan(IPAddress localIpAddress, IPAddress multicastIpAddress, byte[] magicPacket)
        {
            UdpClient client = new(new IPEndPoint(localIpAddress, 0));
            await client.SendAsync(magicPacket, magicPacket.Length, new IPEndPoint(multicastIpAddress, 9));
        }

        /// <summary>
        /// Creates a regular expression to format MAC addresses by removing colons, hyphens, and spaces.
        /// </summary>
        /// <returns>A Regex object for formatting MAC addresses.</returns>
        [GeneratedRegex("[: -]")]
        private static partial Regex MacFormatter();
    }
}
