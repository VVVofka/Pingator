using System;
using System.Net.NetworkInformation;

namespace Pingator {
	class PingSimpleAsync {
		public readonly string server;
		private IPStatus prevStatus;    // enum
		public PingSimpleAsync(string server) {
			this.server = server;
		} // //////////////////////////////////////////////////////////////////////////////
		public PingReply Reply { get; private set; } = null; //pr
		// /////////////////////////////////////////////////////////////////////////////
		public IPStatus Status {
			get { return Reply.Status; }
		} // /////////////////////////////////////////////////////////////////////////////
	} // --------------------------------------------------------------------------------
}
