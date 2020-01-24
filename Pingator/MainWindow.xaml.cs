using System;
using System.Collections.Generic;
using System.IO;
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
		public string ConfigFileName = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"\Data", @"config.txt");
		public MainWindow() {
			InitializeComponent();
			DataContext = new TimerPings(ConfigFileName);
		} // //////////////////////////////////////////////////////////////////////////////
	} // -------------------------------------------------------------------------------------
}
