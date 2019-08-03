using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Pingator {
	public class StatusIP : INotifyPropertyChanged {
		public enum StatusType {
			UNDEF,
			LINK,
			NOLINK
		}
		private string ip = "0.0.0.0";
		private StatusType status = StatusType.UNDEF;

		public string IP {
			get { return ip; }
			set { ip = value; }
		} // ///////////////////////////////////////////////////////////////////////////
		public StatusType Status {
			get { return status; }
			set {
				status = value;
				OnPropertyChanged("Status");
			}
		} // ///////////////////////////////////////////////////////////////////////////
		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string prop = "") {
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
		} // /////////////////////////////////////////////////////////////////////////
	} // -------------------------------------------------------------------------------
}
