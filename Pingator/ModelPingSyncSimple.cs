using System.Net.NetworkInformation;

namespace Pingator {
	class ModelPingSyncSimple {
		private string server;
		private Ping ping = new Ping();
		private PingReply pingReply = null;
		private IPStatus prevStatus;    // enum
		public ModelPingSyncSimple(string server) {
			this.server = server;
		} // //////////////////////////////////////////////////////////////////////////////
		public PingReply Reply {
			get { return pingReply; }
		} // /////////////////////////////////////////////////////////////////////////////
		public IPStatus Status {
			get { return pingReply.Status; }
		} // /////////////////////////////////////////////////////////////////////////////
		public bool Check() {
			bool first = (pingReply == null);
			pingReply = ping.Send(server);
			if (first || prevStatus != pingReply.Status) {
				prevStatus = pingReply.Status;
				return true;
			}
			return false;
		} // //////////////////////////////////////////////////////////////////////////////
	} // ----------------------------------------------------------------------------------
}
