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
			stateTimer = new Timer(EventStateTimer, null, 1000, 1000);
		} // /////////////////////////////////////////////////////////////////////
		~TimerPings() {
			stateTimer.Dispose();
		} // //////////////////////////////////////////////////////////////////////
		public TimerPings() {
			//Brush01 = Brushes.Magenta;
		} // //////////////////////////////////////////////////////////////////////
		public void Cycle() {
			for (int i = 0; i < alllist.Count; i++) {
				TPT tpt = alllist[i];
				switch (tpt.ready) {
					case TPT.Ready.WaitTimer:
						bool checktimer = tpt.CheckTimer();
						if (checktimer) {
							Pinger(tpt); // ready=WaitReplay; Reply=Send(Adress); ready=ReadReplay;
							Console.WriteLine("Cycle()->after Pinger; " + tpt.ToString());
						}
						break;
					case TPT.Ready.WaitReplay:
						break;
					case TPT.Ready.ReadReplay:
						Console.WriteLine("Cycle() case:ReadReplay tpt:" + tpt.ToString());
						SaveReplay(tpt, i); // setBrush(tpt.brush, i) inside
						if (tpt.ChangeStatus())
							setBrush(tpt.brush, i);
						tpt.SetTimer();
						Console.WriteLine("Cycle()->after setBrush; " + tpt.ToString());
						break;
					case TPT.Ready.First:
						tpt.Init();
						setBrush(tpt.brush, i);
						break;
				} // ----------- switch(tpt.ready) 
			} // --------------for
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
			tpt.ready = TPT.Ready.WaitReplay;
			tpt.Reply = png.Send(tpt.Adress);
			tpt.ready = TPT.Ready.ReadReplay;
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
			tpt.ready = TPT.Ready.WaitReplay;
			tpt.Reply = await png.SendPingAsync(tpt.Adress);
			tpt.ready = TPT.Ready.ReadReplay;
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
		private void SaveReplay(TPT tpt, int i) {
			// TODO
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
			else if (i == (bnd++))
				Brush05 = brush;
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
		public Brush Brush05 {
			get { return alllist[5].brush; }
			set {
				alllist[5].brush = value;
				OnPropertyChanged("Brush05");
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
