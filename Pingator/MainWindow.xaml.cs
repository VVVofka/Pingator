using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pingator {
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		private List<PingControl> pings = new List<PingControl>();
		private List<PingControlAsync> pingas = new List<PingControlAsync>();
		MainViewModel mv;
		public MainWindow() {
			InitializeComponent();
			mv = new MainViewModel();
			this.DataContext = mv;
			//this.DataContext = new MainViewModel();
			PingSimplInit();
		} // //////////////////////////////////////////////////////////////////////////////
		private void Btn1_Click(object sender, RoutedEventArgs e) {
			Action<PingControlAsync> asyn = new Action<PingControlAsync>(Pinger);
			// Для каждой рабочей станции запускаем Pinger'а
			foreach (PingControlAsync s in pingas)
				asyn.Invoke(s);

			//foreach (PingControl p in pings) 
				//p.Check();
		} // /////////////////////////////////////////////////////////////////////////////////
		private void PingSimplInit() {
			pings.Add(new PingControl("192.168.1.1", rtSecr));
			pings.Add(new PingControl("192.168.1.199", swSecr));
			pings.Add(new PingControl("google.com", pcSecr));
			pings.Add(new PingControl("192.168.2.199", pcDirect));
			pingas.Add(new PingControlAsync("192.168.1.1", rtSecr));
			pingas.Add(new PingControlAsync("192.168.1.199", swSecr));
			pingas.Add(new PingControlAsync("google.com", pcSecr));
			pingas.Add(new PingControlAsync("192.168.2.199", pcDirect));
		} // //////////////////////////////////////////////////////////////////////////////////
		async private static void Pinger(PingControlAsync pgcntl) {
			Ping png = new Ping();
			string hostAdress = pgcntl.Adress;
			try {
				PingReply pr = await png.SendPingAsync(hostAdress);
				pgcntl.Check(pr);
				Console.WriteLine(string.Format("Status for {0} = {1}, ip-адрес: {2}", hostAdress, pr.Status, pr.Address));
			} catch {
				Console.WriteLine("Возникла ошибка! " + hostAdress);
			}
		} // /////////////////////////////////////////////////////////////////////////////////////////////
	} // -------------------------------------------------------------------------------------
}
