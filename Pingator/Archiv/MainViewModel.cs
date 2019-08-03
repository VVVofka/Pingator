using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace Pingator {
	class MainViewModel{
		private StatusIP changedIP;
		public ObservableCollection<StatusIP> ListStatusIP {get; set;}
		public StatusIP ChangedStatusIP {
			get { return changedIP; }
			set {
				changedIP = value;
				OnPropertyChanged("ChangedStatusIP");
			}
		}
		public MainViewModel() {
			ListStatusIP = new ObservableCollection<StatusIP>();

			Add("192.168.1.1");
			Add("192.168.1.208");
			Add("192.168.1.108");
			Add("192.168.2.208");
			Add("google.com");
		} // ///////////////////////////////////////////////////////////
		public void Add(string ip) {
			StatusIP st = new StatusIP();
			st.IP = ip;
			ListStatusIP.Add(st);
		} // ///////////////////////////////////////////////////////////////////
		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName]string prop = "") {
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
		} // /////////////////////////////////////////////////////////////////////
	} // -------------------------------------------------------------------------
}
