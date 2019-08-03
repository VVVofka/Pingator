using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
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
			// Для каждой рабочей станции запускаем Pinger'а
			for (int i=0; i<3 ; i++) {
				Cycle();
				//Thread.Sleep(7000);
				Console.WriteLine("complete");
			}
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
			pingas.Add(new PingControlAsync("192.168.1.198", pcResurs));
		} // //////////////////////////////////////////////////////////////////////////////////
		async private static void Pinger(PingControlAsync pgcntl) {
			if (pgcntl.inwork)
				return;
			pgcntl.inwork = true;
			Ping png = new Ping();
			try {
				PingReply pr = await png.SendPingAsync(pgcntl.Adress);
				Console.WriteLine(string.Format("Status for {0} = {1}, ip-адрес: {2}", pgcntl.Adress, pr.Status, pr.Address));
				pgcntl.Check(pr);
				pgcntl.inwork = false;
			} catch {
				Console.WriteLine("Возникла ошибка! " + pgcntl.Adress);
				pgcntl.inwork = false;
			}
		} // /////////////////////////////////////////////////////////////////////////////////////////////
		private static void OnTimedEvent(Object source, ElapsedEventArgs e) {
			Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",
							  e.SignalTime);
		} // ///////////////////////////////////////////////////////////////////////////////////////////////
		public void Cycle() {
			Action<PingControlAsync> asyn = new Action<PingControlAsync>(Pinger);
			foreach (PingControlAsync s in pingas)
				asyn.Invoke(s);
		} // ///////////////////////////////////////////////////////////////////////////////////////////////
	} // -------------------------------------------------------------------------------------
}
