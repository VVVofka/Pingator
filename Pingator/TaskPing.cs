using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Pingator {
	class TaskPing {
		public class InData {
			public InData(string adress, Brush brush) {
				this.adress = adress;
				this.brush = brush;
			}
			public string adress;
			public Brush brush;
		} // ////////////////////////////////////////////////////////////
		public void Run(Brush brush) {
			Task<Brush> task1 = new Task<Brush>(() => Factorial(new InData("192.168.1.122", brush)));
			task1.Start();
		} // /////////////////////////////////////////////////////////////
		Brush Factorial(InData indata) {
			Ping png = new Ping();
			PingReply reply;
			for (int i=0; i<999999999 ;i++ ) {
				reply = png.Send(indata.adress, 1000);
				indata.brush = GetBrush(reply);
			}
			return Brushes.Black;
		} // ////////////////////////////////////////////////////////////////////////
		static public Brush GetBrush(PingReply reply) {
			if (reply == null)
				return Brushes.Magenta;
			switch (reply.Status) {
				case IPStatus.Success:
					return Brushes.Green;
				case IPStatus.TimedOut:
					return Brushes.Orange;
				case IPStatus.DestinationHostUnreachable:
					return Brushes.DarkGray;
				case IPStatus.DestinationNetworkUnreachable:
					return Brushes.Gray;
				case IPStatus.DestinationPortUnreachable:
					return Brushes.Silver;
				case IPStatus.DestinationProtocolUnreachable:
					return Brushes.LightGray;
				default:
					return Brushes.Red;
			}
		} // //////////////////////////////////////////////////////////////////////////////////
	} // ----------------------------------------------------------------------------------------
}
