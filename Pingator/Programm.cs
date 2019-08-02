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
	class ModelPingSync { //: INotifyPropertyChanged {
					// данная коллекция будет содержать имена рабочих станций
		private static ObservableCollection<string> hosts = new ObservableCollection<string>();
		//public event PropertyChangedEventHandler PropertyChanged;
		public static void Run() {
			// В переменную hosts записываем все рабочие станции из файла
			hosts = getComputersListFromTxtFile("C:\\Temp\\computersList.txt");
			// Создаём Action типизированный string, данный Action будет запускать функцию Pinger
			Action<string> asyn = new Action<string>(Pinger);
			// Для каждой рабочей станции запускаем Pinger'а
			foreach (string s in hosts)
				asyn.Invoke(s);
			//hosts.ForEach(e => { asyn.Invoke(e);});
			//Console.Read();
		} // /////////////////////////////////////////////////////////////////////////////////////////////
		  // Функция получения списка рабочих станций из файла
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
		  // Функция записи проблемных рабочих станций в файл
		private static void writeProblemComputersToFile(string fullPathToFile, List<string> problemComputersList) {
			using (StreamWriter sw = new StreamWriter(fullPathToFile, true, Encoding.Default)) {
				problemComputersList.ForEach(e => { sw.WriteLine(e); });
			}
		} // /////////////////////////////////////////////////////////////////////////////////////////////
		  // Проверяем доступна ли папка admin$
		private static bool checkAdminShare(string computerName) {
			if (Directory.Exists("\\\\" + computerName + "\\admin$")) {
				return true;
			} else {
				return false;
			}
		} // /////////////////////////////////////////////////////////////////////////////////////////////
		  // Наш основной класс, который будет отправлять команду ping
		async private static void Pinger(string hostAdress) {
			// Создаём экземпляр класса Ping
			Ping png = new Ping();
			try {
				// Пингуем рабочую станцию hostAdress
				PingReply pr = await png.SendPingAsync(hostAdress);
				List<string> problemComputersList = new List<string>();
				Console.WriteLine(string.Format("Status for {0} = {1}, ip-адрес: {2}", hostAdress, pr.Status, pr.Address));
				// Если рабочая станция пингуется и папка admin$ недоступна,
				// то такую машину заносим в список
				//if (pr.Status == IPStatus.Success && !checkAdminShare(hostAdress)) {
				//	problemComputersList.Add(hostAdress);
				//}
				// Записываем в файл все проблемные машины
				writeProblemComputersToFile("C:\\Temp\\problemsWithAdminShare.txt", problemComputersList);
			} catch {
				Console.WriteLine("Возникла ошибка! " + hostAdress);
			}
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
