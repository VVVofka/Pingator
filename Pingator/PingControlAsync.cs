using System.Windows.Media;
using System.Windows.Shapes;
using System.Net.NetworkInformation;

namespace Pingator {
	public class PingControlAsync {
		private Shape control;
		private IPStatus prevStatus;    // enum
		private bool first = true;
		public readonly string Adress;
		public bool inwork = false;
		public PingControlAsync(string adress, Shape control) {
			this.control = control;
			Adress = adress;
		} // ///////////////////////////////////////////////////////////////////////////////////
		public bool Check(PingReply reply) {
			inwork = false;
			if (first || prevStatus != reply.Status) {
				prevStatus = reply.Status;
				control.Fill = GetBrush(reply);
				return true;
			}
			return false;
		} // ///////////////////////////////////////////////////////////////////////////////////
		private Brush GetBrush(PingReply reply) {
			Brush brush;
			switch (reply.Status) {
				case IPStatus.Success:
					brush = Brushes.Green;
					break;
				case IPStatus.TimedOut:
					brush = Brushes.Yellow;
					break;
				default:
					brush = Brushes.Red;
					break;
			}
			return brush;
		} // //////////////////////////////////////////////////////////////////////////////////
	} // ----------------------------------------------------------------------------------------
}
