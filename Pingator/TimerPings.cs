using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace Pingator {
	public class TimerPings : INotifyPropertyChanged {
		Timer stateTimer;
		public void CheckStatus(Object objtpt) {
			TPT tpt = (TPT)objtpt;
			tpt.Check();

			if (phone == null)
				Console.WriteLine("{0}", DateTime.Now.ToString("h:mm:ss.fff"));
			else {
				Console.WriteLine("{0} comp:{1}", DateTime.Now.ToString("h:mm:ss.fff"), phone.Company);
				SelectedPhone.Company = DateTime.Now.ToString("h:mm:ss.fff");
			}
		}

		private static List<TPT> alllist = new List<TPT>();

		private Brush brush_rtSecr;
		public Brush BrushRtSecr {
			get { return brush_rtSecr; }
			set {
				brush_rtSecr = value;
				OnPropertyChanged("BrushRtSecr");
			}
		} // /-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/

		private Brush brush_;
		private Brush brush_;
		private Brush brush_;
		private Brush brush_;
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

		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName]string prop = "") {
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
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
