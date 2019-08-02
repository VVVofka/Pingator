using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.NetworkInformation;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Pingator {
	class ModelPingSyncSimple { //: INotifyPropertyChanged {
		private static ObservableCollection<string> hosts = new ObservableCollection<string>();
		//public event PropertyChangedEventHandler PropertyChanged;
		private static ObservableCollection<string> getComputersListFromTxtFile(string pathToFile) {
			ObservableCollection<string> computersList = new ObservableCollection<string>();
			using (StreamReader sr = new StreamReader(pathToFile, Encoding.Default)) {
				string line = "";
				while ((line = sr.ReadLine()) != null) {
					if (line.Length >= 7)
						computersList.Add(line);
				}
			}
			return computersList;
		} // /////////////////////////////////////////////////////////////////////////////////////////////
		public static void PingsSync() {
			List<string> serversList = new List<string>();
			serversList.Add("www.microsoft.com");
			serversList.Add("google.com");
			serversList.Add("192.168.1.1");
			serversList.Add("192.168.1.112");
			serversList.Add("192.168.1.87");
			serversList.Add("192.168.1.133");
			serversList.Add("192.168.1.199");
			serversList.Add("192.168.2.9");


			using (TextWriter tw = new StreamWriter("C:\\Temp\\MyLog.txt")) {
				Ping ping = new System.Net.NetworkInformation.Ping();
				PingReply pingReply = null;

				foreach (string server in serversList) {
					pingReply = ping.Send(server);
					Console.WriteLine("");
					if (pingReply.Status != IPStatus.TimedOut) {
						tw.WriteLine(server); //server
						tw.WriteLine(pingReply.Address); //IP
						tw.WriteLine(pingReply.Status); //Статус
						tw.WriteLine(pingReply.RoundtripTime); //Время ответа
						if (pingReply.Options != null) {
							tw.WriteLine(pingReply.Options.Ttl); //TTL
							tw.WriteLine(pingReply.Options.DontFragment); //Фрагментирование
						} else {
							tw.WriteLine(server); //server
							tw.WriteLine(pingReply.Status);
						}
						
						Console.WriteLine("server " + server); //server
						Console.WriteLine("IP " + pingReply.Address); //IP
						Console.WriteLine("Status " + pingReply.Status); //Статус
						Console.WriteLine("RoundtripTime " + pingReply.RoundtripTime); //Время ответа
						if (pingReply.Options != null) {
							Console.WriteLine("TTL " + pingReply.Options.Ttl); //TTL
							Console.WriteLine("DontFragment " + pingReply.Options.DontFragment); //Фрагментирование
						}
						Console.WriteLine("Buffer.Length " + pingReply.Buffer.Length); //Размер буфера
					} else {
						Console.WriteLine("server err " + server); //server
						Console.WriteLine(pingReply.Status);
					}
				}
			}
			//Console.ReadKey();
			//try {
			//	IPHostEntry hostInfo = Dns.Resolve(server);
			//} catch (Exception ee) {
			//	tw.WriteLine("опаньки...");
			//	tw.WriteLine(server); //server
			//	tw.WriteLine(ee.Message);
			//	tw.WriteLine();
			//	continue;
			//}
		} // ///////////////////////////////////////////////////////////////////////////////////////////////////////
	}
}
