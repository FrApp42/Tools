using FrApp42.System.Net;

namespace System.Test
{
	[TestClass]
	public class WakeOnLanTest
	{
		private readonly string _macAddressColon = "FF:FF:FF:FF:FF:FF";
		private readonly string _macAddressDash = "FF-FF-FF-FF-FF-FF";
		private readonly string _expectedFormattedMacAddress = "FFFFFFFFFFFF";

		[TestMethod]
		public void BuildMagicPacketWithColon()
		{
			BuildMagicPacketTest(_macAddressColon);
		}

		[TestMethod]
		public void BuildMagicPacketWithDash()
		{
			BuildMagicPacketTest(_macAddressDash);
		}

		[TestMethod]
		public void MacFormatterWithColon()
		{
			string actualFormattedMac = WakeOnLan.MacFormatter().Replace(_macAddressColon, "");

			Assert.AreEqual(_expectedFormattedMacAddress, actualFormattedMac, "The MAC address was not formatted correctly.");
		}

		[TestMethod]
		public void MacFormatterWithDash()
		{
			string actualFormattedMac = WakeOnLan.MacFormatter().Replace(_macAddressDash, "");

			Assert.AreEqual(_expectedFormattedMacAddress, actualFormattedMac, "The MAC address was not formatted correctly.");
		}

		private void BuildMagicPacketTest(string macAddress)
		{
			byte[] expectedMagicPacket = new byte[102];

			for (int i = 0; i < 6; i++)
				expectedMagicPacket[i] = 0xFF;

			byte[] macBytes = { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };

			for (int i = 0; i < 16; i++)
				Array.Copy(macBytes, 0, expectedMagicPacket, 6 + i * 6, 6);

			byte[] actualMagicPacket = WakeOnLan.BuildMagicPacket(macAddress);

			CollectionAssert.AreEqual(expectedMagicPacket, actualMagicPacket, "The magic packet is not built correctly.");
		}
	}
}
