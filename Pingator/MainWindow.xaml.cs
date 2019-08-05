using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
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
		TimerPings tp;
//		AutoResetEvent autoEvent = new AutoResetEvent(false);
		int autoEvent = 0;
		Timer stateTimer;
		public MainWindow() {
			InitializeComponent();
			mv = new MainViewModel();
			this.DataContext = mv;
			//this.DataContext = new MainViewModel();
			stateTimer = new Timer(func, autoEvent, 3000, 7000000);
			PingSimplInit();
			tp = new TimerPings(7000, pingas);
		} // //////////////////////////////////////////////////////////////////////////////
		public void func(Object i) {
			Btn1_Click(btn1, null);
		} // ///////////////////////////////////////////////////////////////////////////////
		private void Btn1_Click(object sender, RoutedEventArgs e) {
			//Thread.Sleep(7000);
			Console.WriteLine("complete");
			tp.Cycle();
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
		private void Window_Closed(object sender, EventArgs e) {
			//this.RaiseEvent()
			//Invalidate
		} // /////////////////////////////////////////////////////////////////////////////////////////
	} // -------------------------------------------------------------------------------------
}
