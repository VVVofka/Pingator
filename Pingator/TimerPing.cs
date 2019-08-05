using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Timers;
using System.Windows;
using System.Windows.Threading;

namespace Pingator {
	public class TimerPing {
		private static List<TPT> alllist = new List<TPT>();
		public void Init(int msec, List<PingControlAsync> s) {
			foreach (PingControlAsync p in s)
				alllist.Add(new TPT(p.Adress, p.control));
		} // /////////////////////////////////////////////////////////////////////
		public void Cycle() {
			Action<TPT> asyn = new Action<TPT>(Pinger);
			foreach (TPT tpt in alllist) {
				asyn.Invoke(tpt);
			}
		} // ///////////////////////////////////////////////////////////////////////////////////////////////
		async private static void Pinger(TPT tpt) {
			Ping png = new Ping();
			try {
				PingReply pr = await png.SendPingAsync(tpt.Adress);
				Console.WriteLine(string.Format("Status for {0} = {1}, ip-адрес: {2}", tpt.Adress, pr.Status, pr.Address));
				tpt.Check(pr);
				tpt.Time = DateTime.Now.Millisecond;
			} catch {
				Console.WriteLine("Возникла ошибка! " + tpt.Adress);
				tpt.Time = 0;
			}
		} // /////////////////////////////////////////////////////////////////////////////////////////////
		public static void DoEvents() {
			if (Application.Current != null)
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
		} // /////////////////////////////////////////////////////////////////////////////////////////
	} // -----------------------------------------------------------------------------
}
/*
 void DoEvents()
{ MSG msg;
  while(PeekMessage(&msg, NULL, NULL, NULL, PM_REMOVE))
	{ TranslateMessage(&msg);
    DispatchMessage(&msg);
  }
}////////////////////////////////////////////////////////////////////////////////
 */
