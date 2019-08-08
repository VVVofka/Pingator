using System.Windows.Media;
using System.Windows.Shapes;
using System.Net.NetworkInformation;
using System.Windows.Data;
using System;

namespace Pingator {
	class TPT {
		public readonly int Interval;    // милисек.
		public readonly string Adress;
		public Brush brush;
		private IPStatus prevStatus = IPStatus.DestinationUnreachable;    // enum
		private bool first = true;
		private PingReply reply;
		private long time = 0;  // tick
		private bool isreply = false;
		public TPT(string adress, int interval) {
			this.Adress = adress;
			this.Interval = interval;
			brush = GetBrush();
		} // ////////////////////////////////////////////////////////////
		public bool IsReadySendPing() {
			long now = DateTime.Now.Ticks;
			long interval = now - time;
			if (interval < Interval * 10000)
				return false;
			return true;
		} // ////////////////////////////////////////////////////////////////
		public void PrepareSendReplay() {
			isreply = false;
			reply = null;   // TODO
			time = DateTime.Now.Ticks;  // TODO
		} // /////////////////////////////////////////////////////////////////
		public void AfterSendReplay() {
			isreply = true;
			time = DateTime.Now.Ticks;  // TODO
		} // /////////////////////////////////////////////////////////////////
		public bool IsReplayComplete() {
			return isreply;
		} // ///////////////////////////////////////////////////////////////
		  /// <summary>
		  /// for static use
		  /// </summary>
		public PingReply Reply {
			set {
				if (value == null) {
					Console.WriteLine("Adress={0} prevStatus={1} reply.Status={2} newreplay=null",
						Adress, prevStatus.ToString(), reply.Status.ToString());
					//reply = null;
					prevStatus = IPStatus.Success;
				} else {
					if (first) {
						//Console.WriteLine("Adress={0} reply.Status={1}", Adress, value.Status.ToString());
						first = false;
					} else if (prevStatus != value.Status) {
						Console.WriteLine("Adress={0} prevStatus={1} reply.Status={2}",
							Adress, prevStatus.ToString(), value.Status.ToString());
						prevStatus = value.Status;
						brush = GetBrush();
					}
				}
				reply = value;
				time = DateTime.Now.Ticks;
				isreply = true;
			}
			get { return reply; }
		} // ///////////////////////////////////////////////////////////////////
		private Brush GetBrush() {
			if(reply == null)
				return Brushes.LightGray;
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
			string s = "`" + Adress + "` " + brush.ToString() + " prevStatus:" + prevStatus;
			if (reply == null)
				s += " replyStatus:null";
			else
				s += " replyStatus:" + reply.Status;
			string sisreply;
			if (isreply) sisreply = "1";
			else sisreply = "0";
			string sfirst;
			if (first) sfirst = "1";
			else sfirst = "0";
			s += " time:" + time / 10000000 + "c isreply:" + sisreply + " first:" + sfirst;
			return s;
		} // //////////////////////////////////////////////////////////
	} // ----------------------------------------------------------------------------------------
}
