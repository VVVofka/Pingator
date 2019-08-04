using System;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Net.NetworkInformation;

namespace Pingator {
	class TPT {
		public readonly int Interval = 1000;	// милисек.
		public readonly string Adress;
		public readonly Shape Control;
		public int Time;
		private IPStatus prevStatus;    // enum

		public TPT(string adress, Shape control, int time = 0) {
			this.Adress = adress;
			this.Control = control;
			this.Time = time;
		} // ////////////////////////////////////////////////////////////
		public void Check(PingReply reply) {
			if (prevStatus != reply.Status) {
				prevStatus = reply.Status;
				Control.Fill = GetBrush(reply);
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
