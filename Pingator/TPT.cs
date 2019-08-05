using System;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Net.NetworkInformation;

namespace Pingator {
	class TPT {
		public readonly int Interval = 1000;	// милисек.
		public readonly string Adress;
		public readonly Shape Control;
		public long Time;
		private IPStatus prevStatus;    // enum
		private bool first = true;

		public TPT(string adress, Shape control, int time = 0) {
			this.Adress = adress;
			this.Control = control;
			this.Time = time;
		} // ////////////////////////////////////////////////////////////
		public void Check(PingReply reply) {
			if (first || prevStatus != reply.Status) {
				prevStatus = reply.Status;
				Control.Fill = GetBrush(reply);
				//Control.InvalidateVisual();
				//Control.UpdateLayout();
			}
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
