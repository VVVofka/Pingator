using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
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
		private static Timer aTimer;
		public MainWindow() {
			InitializeComponent();
			mv = new MainViewModel();
			this.DataContext = mv;
			//this.DataContext = new MainViewModel();
			PingSimplInit();
			SetTimer();
		} // //////////////////////////////////////////////////////////////////////////////
		private void Btn1_Click(object sender, RoutedEventArgs e) {
			Action<PingControlAsync> asyn = new Action<PingControlAsync>(Pinger);
			// Для каждой рабочей станции запускаем Pinger'а

			foreach (PingControlAsync s in pingas)
				asyn.Invoke(s);
			Console.WriteLine("complete");
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
		private static void SetTimer() {
			// Create a timer with a two second interval.
			aTimer = new Timer(6000);
			// Hook up the Elapsed event for the timer. 
			aTimer.Elapsed += OnTimedEvent;
			aTimer.AutoReset = true;
			aTimer.Enabled = true;
		} // ///////////////////////////////////////////////////////////////////////////////////////////////
		private static void OnTimedEvent(Object source, ElapsedEventArgs e) {
			Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",
							  e.SignalTime);
		} // ///////////////////////////////////////////////////////////////////////////////////////////////
		private void Window_Closed(object sender, EventArgs e) {
			aTimer.Stop();
			aTimer.Dispose();
		} // //////////////////////////////////////////////////////////////////////////////////////////////////
	} // -------------------------------------------------------------------------------------
}
