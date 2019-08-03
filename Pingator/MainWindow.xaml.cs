using System;
using System.Collections.Generic;
using System.Linq;
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
		MainViewModel mv;
		public MainWindow() {
			InitializeComponent();
			mv = new MainViewModel();
			this.DataContext = mv;
			//this.DataContext = new MainViewModel();
		} // //////////////////////////////////////////////////////////////////////////////

		private void Btn1_Click(object sender, RoutedEventArgs e) {
			//mv.Del();
			//ModelPingSync.PingsSync();
			//this.Close();
			foreach(PingControl p in pings) {
				p.Check();
			}
		} // /////////////////////////////////////////////////////////////////////////////////
		private void PingSimpl() {
			pings.Add(new PingControl("192.168.1.1", rtSecr));
			pings.Add(new PingControl("192.168.1.199", swSecr));
			pings.Add(new PingControl("google.com", pcSecr));
			pings.Add(new PingControl("192.168.2.199", pcDirect));
		} // //////////////////////////////////////////////////////////////////////////////////
	} // -------------------------------------------------------------------------------------
}
