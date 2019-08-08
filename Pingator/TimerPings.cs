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
		Timer stateTimer;
		long Interval;
		private static List<TPT> alllist = new List<TPT>();
		public void EventStateTimer(Object objtpt) {
			Cycle();
		} // //////////////////////////////////////////////////////////////////////////////////////////
		public TimerPings(int msec, List<PingControlAsync> s) {
			Interval = msec * 10000;
			foreach (PingControlAsync p in s)
				alllist.Add(new TPT(p.Adress, 1000));
			stateTimer = new Timer(EventStateTimer, null, 1000, 6000);
		} // /////////////////////////////////////////////////////////////////////
		~TimerPings() {
			stateTimer.Dispose();
		} // //////////////////////////////////////////////////////////////////////
		public void Cycle() {
			int i = 1;
			TPT tpt = alllist[i];
			bool ready = tpt.IsReadySendPing();
			bool replaycompleted = tpt.IsReplayComplete();
			Console.WriteLine("Cycle():ready=" + ready + " replaycompl=" + replaycompleted+"; tpt:" + tpt.ToString());
			if (ready) {
				//Console.WriteLine("Cycle()->tpt.ReadyStartPing=true(before Pinger) " + tpt.ToString());
				Pinger(tpt);
				Console.WriteLine("Cycle()->after Pinger; " + tpt.ToString());
			} else if (replaycompleted) {
				//Console.WriteLine("Cycle()->tpt.FinishPing=true(before setBrush)" + tpt.ToString());
				SaveReplay(tpt, i); // setBrush(tpt.brush, i) inside
				Console.WriteLine("Cycle()->after setBrush; " + tpt.ToString());
			} else {
				Console.WriteLine("Cycle(): ELSE " + tpt.ToString());
			}
		} // ///////////////////////////////////////////////////////////////////////////////////////////////
		public void CycleA() {
			Action<TPT> asyn = new Action<TPT>(PingerA);

			for (int i = 0; i < alllist.Count; i++) {
				TPT tpt = alllist[i];
				if (tpt.Reply == null) {
					long interval = DateTime.Now.Ticks - 0; // tpt.Time;
					if (interval > Interval) {
						asyn.Invoke(tpt);
					}
				} else {
					setBrush(tpt.brush, i);
					//tpt.Reply = null;
					//tpt.Time = DateTime.Now.Ticks;
				}
			}
		} // ///////////////////////////////////////////////////////////////////////////////////////////////
		private void Pinger(TPT tpt) {
			Ping png = new Ping();
			//try {
			tpt.PrepareSendReplay();
			tpt.Reply = png.Send(tpt.Adress);
			tpt.AfterSendReplay();
			//Console.WriteLine("After Send-> " + tpt.ToString());
			//tpt.Check();
			//tpt.Time = DateTime.Now.Ticks;
			//} catch {
			//Console.WriteLine("Возникла ошибка! " + tpt.Adress);
			//tpt.Time = 0;
			//}
		} // /////////////////////////////////////////////////////////////////////////////////////////////
		async private static void PingerA(TPT tpt) {
			Ping png = new Ping();
			//try {
			tpt.PrepareSendReplay();
			tpt.Reply = await png.SendPingAsync(tpt.Adress);
			tpt.AfterSendReplay();
			//Console.WriteLine(string.Format("Status for {0} = {1}, ip-адрес: {2}", tpt.Adress, tpt.Reply.Status, tpt.Reply.Address));
			//tpt.Check();
			//tpt.Time = DateTime.Now.Ticks;
			//} catch {
			//Console.WriteLine("Возникла ошибка! " + tpt.Adress);
			//tpt.Time = 0;
			//}
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
		private void  SaveReplay(TPT tpt, int i) {
			setBrush(tpt.brush, i);
		} // /////////////////////////////////////////////////////////////////////////////////////////
		private void setBrush(Brush brush, int i) {
			int bnd = 0;
			if (i == (bnd++))
				Brush00 = brush;
			else if (i == (bnd++))
				Brush01 = brush;
			else if (i == (bnd++))
				Brush02 = brush;
			else if (i == (bnd++))
				Brush03 = brush;
			else if (i == (bnd++))
				Brush04 = brush;
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
