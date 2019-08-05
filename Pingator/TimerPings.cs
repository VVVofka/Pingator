using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace Pingator {
	public class TimerPings : INotifyPropertyChanged {
		long Interval;
		Timer stateTimer;
		private static List<TPT> alllist = new List<TPT>();
		public void CheckStatus(Object objtpt) {
			Cycle();
		} // //////////////////////////////////////////////////////////////////////////////////////////
		public TimerPings(int msec, List<PingControlAsync> s) {
			Interval = msec * 10000;
			foreach (PingControlAsync p in s)
				alllist.Add(new TPT(p.Adress, p.control));
			stateTimer = new Timer(CheckStatus, null, 1000, 1000);
		} // /////////////////////////////////////////////////////////////////////
		~TimerPings() {
			stateTimer.Dispose();
		} // //////////////////////////////////////////////////////////////////////
		public void Cycle() {
			Action<TPT> asyn = new Action<TPT>(Pinger);

			for (int i = 0; i < alllist.Count; i++) {
				TPT tpt = alllist[i];
				if (tpt.reply == null) {
					long interval = DateTime.Now.Ticks - tpt.Time;
					if (interval > Interval) {
						asyn.Invoke(tpt);
					}
				} else {
					setBrush(tpt.brush, i);
					tpt.reply = null;
					tpt.Time = DateTime.Now.Ticks;
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
		public static void DoEvents() {
			if (Application.Current != null)
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
		} // /////////////////////////////////////////////////////////////////////////////////////////

		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName]string prop = "") {
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
		} // /////////////////////////////////////////////////////////////////////////////////////////

		private void setBrush(Brush brush, int i) {
			int bnd = 0;
			if (i == (bnd++))
				Brush00 = brush;
			else if (i == (bnd++))
				Brush01 = brush;
		} // ///////////////////////////////////////////////////////////////////////////////////////////
		public Brush Brush00 {
			get { return alllist[0].brush; }
			set {
				alllist[0].brush = value;
				OnPropertyChanged("Brush00");
			}
		} // /-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/
		public Brush Brush01 {
			get { return alllist[1].brush; }
			set {
				alllist[1].brush = value;
				OnPropertyChanged("Brush01");
			}
		} // /-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/
		public Brush Brush02 {
			get { return alllist[2].brush; }
			set {
				alllist[2].brush = value;
				OnPropertyChanged("Brush02");
			}
		} // /-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/
		public Brush Brush03 {
			get { return alllist[3].brush; }
			set {
				alllist[3].brush = value;
				OnPropertyChanged("Brush03");
			}
		} // /-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/
		public Brush Brush04 {
			get { return alllist[4].brush; }
			set {
				alllist[4].brush = value;
				OnPropertyChanged("Brush04");
			}
		} // /-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/
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
