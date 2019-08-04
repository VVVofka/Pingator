using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Threading;

namespace Pingator {
	public static class TimerPing {
		private static Timer tmr;
		private static List<PingControlAsync> pingas;
		public static void Init(int msec, List<PingControlAsync> s) {
			tmr = new Timer(msec);
			tmr.Elapsed += OnTimedEvent;
			tmr.AutoReset = true;
			tmr.Enabled = true;
		} // /////////////////////////////////////////////////////////////////////
		private static void OnTimedEvent(Object source, ElapsedEventArgs e) {
			Cycle();
		} // /////////////////////////////////////////////////////////////////////////////
		private static void Cycle() {
			Action<PingControlAsync> asyn = new Action<PingControlAsync>(Pinger);
			foreach (PingControlAsync s in pingas)
				asyn.Invoke(s);
		} // ///////////////////////////////////////////////////////////////////////////////////////////////
		async private static void Pinger(PingControlAsync pgcntl) {
			if (pgcntl.inwork)
				return;
			pgcntl.inwork = true;
			Ping png = new Ping();
			try {
				PingReply pr = await png.SendPingAsync(pgcntl.Adress);
				Console.WriteLine(string.Format("Status for {0} = {1}, ip-адрес: {2}", pgcntl.Adress, pr.Status, pr.Address));
				pgcntl.Check(pr);
				pgcntl.inwork = false;
			} catch {
				Console.WriteLine("Возникла ошибка! " + pgcntl.Adress);
				pgcntl.inwork = false;
			}
		} // /////////////////////////////////////////////////////////////////////////////////////////////
		public static void DoEvents() {
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
