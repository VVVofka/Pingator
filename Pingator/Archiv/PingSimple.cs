using System.Net.NetworkInformation;

namespace Pingator {
	class PingSimple {
		private readonly string adress;
		private Ping ping = new Ping();
		private IPStatus prevStatus;    // enum
		public PingSimple(string adress) {
			this.adress = adress;
		} // //////////////////////////////////////////////////////////////////////////////
		public PingReply Reply { get; private set; } = null;
		// /////////////////////////////////////////////////////////////////////////////
		public IPStatus Status {
			get { return Reply.Status; }
		} // /////////////////////////////////////////////////////////////////////////////
		public bool Check() {
			bool first = (Reply == null);
			Reply = ping.Send(adress);
			if (first || prevStatus != Reply.Status) {
				prevStatus = Reply.Status;
				return true;
			}
			return false;
		} // //////////////////////////////////////////////////////////////////////////////
	} // ----------------------------------------------------------------------------------
}
