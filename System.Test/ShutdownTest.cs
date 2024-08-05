using FrApp42.System.Computer.Shutdown;

namespace System.Test
{
	[TestClass]
	public class ShutdownTest
	{
		private readonly string _hostname = "machine.local";

		[TestMethod]
		public void ClassicShutdown()
		{
			Shutdown shutdown = new();
			shutdown
				.SetMachine(_hostname)
				.ShutdownComputer();

			string command = shutdown.GetCommand();
			string expectedCommand = $" /m {_hostname} /s";

			Assert.AreEqual(expectedCommand, command, "The generated command does not match the expected command.");
		}

		[TestMethod]
		public void LogOffUser()
		{
			Shutdown shutdown = new();
			shutdown
				.LogoutUser();

			string command = shutdown.GetCommand();
			string expectedCommand = $" /l";

			Assert.AreEqual(expectedCommand, command, "The generated command does not match the expected command.");
		}

		[TestMethod]
		public void ShutdownAndSignOnAuto()
		{
			Shutdown shutdown = new();
			shutdown
				.ShutdownAndSignOn();

			string command = shutdown.GetCommand();
			string expectedCommand = $" /sg";

			Assert.AreEqual(expectedCommand, command, "The generated command does not match the expected command.");
		}

		[TestMethod]
		public void TimeOutAndComment()
		{
			Shutdown shutdown = new();
			shutdown
				.SetMachine(_hostname)
				.SetTimeOut(60)
				.SetComment("Scheduled maintenance")
				.ShutdownComputer();

			string command = shutdown.GetCommand();
			string expectedCommand = $" /m {_hostname} /t 60 /c \"Scheduled maintenance\" /s";

			Assert.AreEqual(expectedCommand, command, "The generated command does not match the expected command.");
		}

		[TestMethod]
		public void RebootComputer()
		{
			Shutdown shutdown = new();
			shutdown
				.SetMachine(_hostname)
				.Reboot();

			string command = shutdown.GetCommand();
			string expectedCommand = $" /m {_hostname} /r";

			Assert.AreEqual(expectedCommand, command, "The generated command does not match the expected command.");
		}

		[TestMethod]
		public void HibernateComputer()
		{
			Shutdown shutdown = new();
			shutdown
				.SetMachine(_hostname)
				.Hibernate();

			string command = shutdown.GetCommand();
			string expectedCommand = $" /m {_hostname} /h";

			Assert.AreEqual(expectedCommand, command, "The generated command does not match the expected command.");
		}

		[TestMethod]
		public void SoftShutdown()
		{
			Shutdown shutdown = new();
			shutdown
				.SetMachine(_hostname)
				.ShutdownComputer()
				.Soft();

			string command = shutdown.GetCommand();
			string expectedCommand = $" /m {_hostname} /s /soft";

			Assert.AreEqual(expectedCommand, command, "The generated command does not match the expected command.");
		}

		[TestMethod]
		public void OpenBootOptionsTest()
		{
			Shutdown shutdown = new();
			shutdown
				.SetMachine(_hostname)
				.Reboot()
				.OpenBootOptions();

			string command = shutdown.GetCommand();
			string expectedCommand = $" /m {_hostname} /r /o";

			Assert.AreEqual(expectedCommand, command, "The generated command does not match the expected command.");
		}

		[TestMethod]
		public void ForceShutdownComputer()
		{
			Shutdown shutdown = new();
			shutdown
				.SetMachine(_hostname)
				.ShutdownComputer()
				.ForceShutdown();

			string command = shutdown.GetCommand();
			string expectedCommand = $" /m {_hostname} /s /f";

			Assert.AreEqual(expectedCommand, command, "The generated command does not match the expected command.");
		}

		[TestMethod]
		public void ShutdownWithComment()
		{
			Shutdown shutdown = new();
			shutdown
				.SetMachine(_hostname)
				.ShutdownComputer()
				.SetComment("End of day shutdown");

			string command = shutdown.GetCommand();
			string expectedCommand = $" /m {_hostname} /s /c \"End of day shutdown\"";

			Assert.AreEqual(expectedCommand, command, "The generated command does not match the expected command.");
		}

		[TestMethod]
		public void LogoutNotWithMachine()
		{
			Shutdown shutdown = new();
			shutdown
				.SetMachine(_hostname)
				.LogoutUser();

			string command = shutdown.GetCommand();
			string expectedCommand = $" /m {_hostname}";

			Assert.AreEqual(expectedCommand, command, "The command should not contain /l when /m is present.");
		}

		[TestMethod]
		public void LogoutNotWithTimeout()
		{
			Shutdown shutdown = new();
			shutdown
				.SetTimeOut(60)
				.LogoutUser();

			string command = shutdown.GetCommand();
			string expectedCommand = $" /t 60";

			Assert.AreEqual(expectedCommand, command, "The command should not contain /l when /t is present.");
		}

		[TestMethod]
		public void MachineNotWithLogout()
		{
			Shutdown shutdown = new();
			shutdown
				.LogoutUser()
				.SetMachine(_hostname);

			string command = shutdown.GetCommand();
			string expectedCommand = $" /l";

			Assert.AreEqual(expectedCommand, command, "The command should not contain /m when /l is present.");
		}

		[TestMethod]
		public void TimeoutNotWithLogout()
		{
			Shutdown shutdown = new();
			shutdown
				.LogoutUser()
				.SetTimeOut(60);

			string command = shutdown.GetCommand();
			string expectedCommand = $" /l";

			Assert.AreEqual(expectedCommand, command, "The command should not contain /t when /l is present.");
		}

		[TestMethod]
		public void NoDuplicateArguments()
		{
			Shutdown shutdown = new();
			shutdown
				.SetMachine(_hostname)
				.SetMachine(_hostname);

			string command = shutdown.GetCommand();
			string expectedCommand = $" /m {_hostname}";

			Assert.AreEqual(expectedCommand, command, "The command should not contain duplicated /m arguments.");

			shutdown = new Shutdown();
			shutdown
				.SetTimeOut(60)
				.SetTimeOut(60);

			command = shutdown.GetCommand();
			expectedCommand = $" /t 60";

			Assert.AreEqual(expectedCommand, command, "The command should not contain duplicated /t arguments.");

			shutdown = new Shutdown();
			shutdown
				.ShutdownComputer()
				.ShutdownComputer();

			command = shutdown.GetCommand();
			expectedCommand = $" /s";

			Assert.AreEqual(expectedCommand, command, "The command should not contain duplicated /s arguments.");
		}
	}
}
