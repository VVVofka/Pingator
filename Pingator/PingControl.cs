using System.Windows.Media;
using System.Windows.Shapes;

namespace Pingator {
	class PingControl {
		private Shape control;
		private ModelPingSyncSimple ping;
		public PingControl(string server, Shape control) {
			this.control = control;
			ping = new ModelPingSyncSimple(server);
		} // ///////////////////////////////////////////////////////////////////////////////////
		public void Check() {
			if(ping.Check() == true) {
				control.Fill = GetBrush();
			}
		} // ///////////////////////////////////////////////////////////////////////////////////
		private Brush GetBrush() {
			Brush brush;
			switch (ping.Status) {
				case System.Net.NetworkInformation.IPStatus.Success:
					brush = Brushes.Green;
					break;
				default:
					brush = Brushes.Black;
					break;
			}
			return brush;
		} // //////////////////////////////////////////////////////////////////////////////////
	} // ----------------------------------------------------------------------------------------
}
