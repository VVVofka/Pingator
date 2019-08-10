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
		private List<ControlAdress> pingas = new List<ControlAdress>();

		public MainWindow() {
			InitializeComponent();
			PingSimplInit();
			DataContext = new TimerPings(500, 3000, pingas, true);
		} // //////////////////////////////////////////////////////////////////////////////
		private void Btn1_Click(object sender, RoutedEventArgs e) {
			//Thread.Sleep(7000);
		} // /////////////////////////////////////////////////////////////////////////////////
		private void PingSimplInit() {
			pingas.Add(new ControlAdress("192.168.1.1", rtSecr));
			pingas.Add(new ControlAdress("google.com", swSecr));
			pingas.Add(new ControlAdress("192.168.1.100", pcSecr));
			pingas.Add(new ControlAdress("192.168.1.122", pcDirect));
			pingas.Add(new ControlAdress("192.168.1.123", pcResurs));
			pingas.Add(new ControlAdress("www.microsoft.com", Earth));
		} // //////////////////////////////////////////////////////////////////////////////////
	} // -------------------------------------------------------------------------------------
}
