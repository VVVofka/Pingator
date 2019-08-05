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
		private List<PingControlAsync> pingas = new List<PingControlAsync>();
		
		public MainWindow() {
			InitializeComponent();
			PingSimplInit();
			DataContext = new TimerPings(7000, pingas);
		} // //////////////////////////////////////////////////////////////////////////////
		private void Btn1_Click(object sender, RoutedEventArgs e) {
			//Thread.Sleep(7000);
			Console.WriteLine("complete");
		} // /////////////////////////////////////////////////////////////////////////////////
		private void PingSimplInit() {
			pingas.Add(new PingControlAsync("192.168.1.1", rtSecr));
			pingas.Add(new PingControlAsync("192.168.1.199", swSecr));
			pingas.Add(new PingControlAsync("google.com", pcSecr));
			pingas.Add(new PingControlAsync("192.168.2.199", pcDirect));
			pingas.Add(new PingControlAsync("192.168.1.198", pcResurs));
		} // //////////////////////////////////////////////////////////////////////////////////
	} // -------------------------------------------------------------------------------------
}
