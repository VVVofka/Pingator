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
		private List<AdressIndex> pingas = new List<AdressIndex>();

		public MainWindow() {
			InitializeComponent();
			PingSimplInit();
			DataContext = new TimerPings(pingas);
		} // //////////////////////////////////////////////////////////////////////////////
		private void Btn1_Click(object sender, RoutedEventArgs e) {
			//Thread.Sleep(7000);
		} // /////////////////////////////////////////////////////////////////////////////////
		private void PingSimplInit() {
			int index = 0;
			pingas.Add(new AdressIndex("192.168.1.1", index++, 300, 1000));
			pingas.Add(new AdressIndex("google.com", index++, 1000, 5000));
			pingas.Add(new AdressIndex("192.168.1.100", index++, 300, 1000));
			pingas.Add(new AdressIndex("192.168.1.122", index++, 300, 1000));
			pingas.Add(new AdressIndex("192.168.1.123", index++, 300, 1000));
			pingas.Add(new AdressIndex("www.microsoft.com", index++, 1100, 5100));
		} // //////////////////////////////////////////////////////////////////////////////////
	} // -------------------------------------------------------------------------------------
}
