using System.Windows.Media;
using System.Windows.Shapes;
using System.Net.NetworkInformation;
using System.Windows.Data;
using System;

namespace Pingator {
	class TPT {
		public readonly int Interval = 1000;    // милисек.
		public readonly string Adress;
		public Brush brush = Brushes.LightGray;
		private IPStatus prevStatus;    // enum
		private bool first = true;
		private PingReply reply = null;
		private long time = 0;  // msec
		private bool finishping = false;
		public TPT(string Adress, int time = 0) {
			this.Adress = Adress;
			this.time = time;
		} // ////////////////////////////////////////////////////////////
		public bool ReadyStartPing {
			get {
				long now = DateTime.Now.Ticks;
				long interval = now - time;
				if (interval < Interval * 10000)
					return false;
				FinishPing = false;
				return true;
			}
		} // ////////////////////////////////////////////////////////////////
		public bool FinishPing {
			get {
				return finishping;
			}
			set {
				finishping = value;
			}
		}

		/// <summary>
		/// for static use
		/// </summary>
		public PingReply Reply {
			set {
				if (value == null) {
					reply = null;
					time = 0;
					FinishPing = false;
				} else if (first || prevStatus != reply.Status) {
					reply = value;
					first = false;
					prevStatus = reply.Status;
					brush = GetBrush();
					time = DateTime.Now.Ticks;
					FinishPing = true;
				}
			}
			get { return reply; }
		} // ///////////////////////////////////////////////////////////////////
		private Brush GetBrush() {
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
