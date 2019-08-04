using System.Windows.Media;
using System.Windows.Shapes;
using System.Net.NetworkInformation;

namespace Pingator {
	class PingControl {
		private Shape control;
		private PingSimple ping;
		public PingControl(string server, Shape control) {
			this.control = control;
			ping = new PingSimple(server);
		} // ///////////////////////////////////////////////////////////////////////////////////
		public void Check() {
			if(ping.Check() == true) {
				control.Fill = GetBrush();
			}
		} // ///////////////////////////////////////////////////////////////////////////////////
		private Brush GetBrush() {
			Brush brush;
			switch (ping.Status) {
				case IPStatus.Success:
					brush = Brushes.Green;
					break;
				default:
					brush = Brushes.Red;
					break;
			}
			return brush;
		} // //////////////////////////////////////////////////////////////////////////////////
	} // ----------------------------------------------------------------------------------------
}
