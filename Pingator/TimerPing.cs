using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Timers;
using System.Windows;
using System.Windows.Threading;

namespace Pingator {
	public static class TimerPing {
		public static readonly int Interval = 1000;
		private static List<TPT> alllist = new List<TPT>();
		private static Timer tmr = new Timer();
		public static void Init(int msec, List<PingControlAsync> s) {
			tmr = new Timer(msec);
			tmr.Elapsed += OnTimedEvent;
			tmr.AutoReset = true;
			tmr.Enabled = true;
			foreach (PingControlAsync p in s)
				alllist.Add(new TPT(p.Adress, p.control));
		} // /////////////////////////////////////////////////////////////////////
		private static void OnTimedEvent(Object source, ElapsedEventArgs e) {
			/*
			int now = DateTime.Now.Millisecond;
			foreach (TPT p in alllist) {
				int interval = now - p.Time;
				if (interval >= Interval)
					worklist.Add(p);
			}*/
			Cycle();
		} // /////////////////////////////////////////////////////////////////////////////
		private static void Cycle() {
			Action<TPT> asyn = new Action<TPT>(Pinger);
			long now = DateTime.Now.Ticks / 10000;
			foreach (TPT tpt in alllist) {
				long interval = now - tpt.Time;
				if (interval >= Interval) {
					asyn.Invoke(tpt);
					DoEvents();
				}
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
		public static void Close() {
			tmr.Stop();
			tmr.Dispose();

		} // ///////////////////////////////////////////////////////////////////////////////////
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
