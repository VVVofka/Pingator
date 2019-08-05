using System.Windows.Media;
using System.Windows.Shapes;
using System.Net.NetworkInformation;

namespace Pingator {
	public class PingControlAsync {
		public readonly Shape control;
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
			switch (reply.Status) {
				case IPStatus.Success:
					return Brushes.Green;
				case IPStatus.TimedOut:
					return Brushes.Yellow;
				default:
					return Brushes.Red;
			}
		} // //////////////////////////////////////////////////////////////////////////////////
	} // ----------------------------------------------------------------------------------------
}
