using System.Windows.Media;
using System.Windows.Shapes;
using System.Net.NetworkInformation;
using System.Windows.Data;
using System;

namespace Pingator {
	class TPT {
		public enum Ready {
			First,
			WaitReplay,
			ReadReplay,
			WaitTimer
		}
		public Ready ready = Ready.First;
		public readonly int Interval;    // милисек.
		public readonly string Adress;
		public Brush brush;
		private PingReply reply;
		private PingReply prevreply;
		private long time = 0;  // tick
								//private bool isreply = false;
		public TPT(string adress, int interval) {
			this.Adress = adress;
			this.Interval = interval;
			brush = GetBrush();
		} // ////////////////////////////////////////////////////////////
		public void Init() {
			reply = null;
			brush = GetBrush();
			SetTimer();
			Console.WriteLine("Init: " + ToString());
		} // ///////////////////////////////////////////////////////////////
		public void SetTimer() {
			time = DateTime.Now.Ticks;
			ready = Ready.WaitTimer;
		} // //////////////////////////////////////////////////////////////
		public bool CheckTimer() {
			long now = DateTime.Now.Ticks;
			long interval = now - time;
			if (interval < Interval * 10000)
				return false;
			return true;
		} // ////////////////////////////////////////////////////////////////
		  /// <summary>
		  /// for static use
		  /// </summary>
		public PingReply Reply {
			set {
				//if (reply == null || prevStatus != value.Status) {
				//	Console.WriteLine("Adress={0} prevStatus={1} reply.Status={2}",
				//		Adress, prevStatus.ToString(), value.Status.ToString());
				//	prevStatus = value.Status;
				prevreply = reply;
				reply = value;
				if (ChangeStatus())
					brush = GetBrush();
				//}
			}
			get { return reply; }
		} // ///////////////////////////////////////////////////////////////////
		public bool ChangeStatus() {
			if (prevreply == null && reply != null)
				return true;
			if (prevreply != null && reply == null)
				return true;
			return prevreply.Status != reply.Status;
		} // ////////////////////////////////////////////////////////////////////
		private Brush GetBrush() {
			if (reply == null)
				return Brushes.Magenta;
			switch (reply.Status) {
				case IPStatus.Success:
					return Brushes.Green;
				case IPStatus.TimedOut:
					return Brushes.Yellow;
				case IPStatus.DestinationHostUnreachable:
				case IPStatus.DestinationNetworkUnreachable:
					return Brushes.Gray;
				case IPStatus.DestinationPortUnreachable:
				case IPStatus.DestinationProtocolUnreachable:
					return Brushes.LightGray;
				default:
					return Brushes.Red;
			}
		} // //////////////////////////////////////////////////////////////////////////////////
		public override string ToString() {
			string s = "`" + Adress + "` " + brush.ToString();
			if (reply == null)
				s += " reply:null";
			else
				s += " replyStatus:" + reply.Status;
			DateTime dt = new DateTime(time);
			string dts = dt.ToString("s");
			s += " time:" + dts + " ready:" + ready.ToString();
			return s;
		} // //////////////////////////////////////////////////////////
	} // ----------------------------------------------------------------------------------------
}
