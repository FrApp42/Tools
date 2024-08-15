using FrApp42.System.Net;
using System.Net;

namespace System.Test
{
	[TestClass]
	public class IsOnlineTest
	{
		private readonly string _googleHostname = "google.com";
		private readonly IPAddress _googleIpAddress = IPAddress.Parse("8.8.8.8");

		[TestMethod]
		public void ConstructorWithHostnameAndDefaultTimeout()
		{
			IsOnline isOnline = new(_googleHostname);
			Assert.AreEqual(5, isOnline.TimeOut, "Default timeout should be 5.");
		}

		[TestMethod]
		public void ConstructorWithHostnameAndCustomTimeout()
		{
			int customTimeout = 10;

			IsOnline isOnline = new(_googleHostname, customTimeout);
			Assert.AreEqual(customTimeout, isOnline.TimeOut, $"Timeout should be {customTimeout}.");
		}

		[TestMethod]
		public void ConstructorWithIpAddressAndDefaultTimeout()
		{
			IsOnline isOnline = new(_googleIpAddress);
			Assert.AreEqual(5, isOnline.TimeOut, "Default timeout should be 5.");
		}

		[TestMethod]
		public void ConstructorWithIpAddressAndCustomTimeout()
		{
			int customTimeout = 10;

			IsOnline isOnline = new(_googleIpAddress, customTimeout);
			Assert.AreEqual(customTimeout, isOnline.TimeOut, $"Timeout should be {customTimeout}.");
		}

		/*[TestMethod]
		public void CheckWithHostname()
		{
			IsOnline isOnline = new(_googleHostname);

			bool result = isOnline.Check();

			Assert.IsTrue(result, "Google hostname should be reachable.");
		}

		[TestMethod]
		public void CheckWithIpAddress()
		{
			IsOnline isOnline = new(_googleIpAddress);

			bool result = isOnline.Check();

			Assert.IsTrue(result, "Google IP address should be reachable.");
		}*/
	}
}
