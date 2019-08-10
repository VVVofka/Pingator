using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace Pingator {
	public class TimerPings : INotifyPropertyChanged {
		private class AdressIndex {
			public AdressIndex(string adress, int index) {
				this.adress = adress;
				this.index = index;
			}
			public string adress;
			public int index;
		}
		Timer stateTimer;
		public string ProtocolFileName = @"protocol.csv";
		private static List<TPT> alllist = new List<TPT>();
		public void EventStateTimer(Object objtpt) {
			bool isasync = (bool)objtpt;
			if (isasync)
				CycleA();
			else
				Cycle();
		} // //////////////////////////////////////////////////////////////////////////////////////////
		public TimerPings(int ping_time_out, int timer_interval, List<ControlAdress> list, bool is_async) {
			if (ping_time_out > timer_interval * 0.8) {
				ping_time_out = (int)(0.5 + timer_interval * 0.8);
				Console.WriteLine("ping_time_out > timer_interval*0.8");
			}
			int delay = (int)(0.5 + (timer_interval - ping_time_out) * 0.75);
			if (delay < 0)
				delay = 0;
			foreach (ControlAdress p in list)
				alllist.Add(new TPT(p.Adress, delay, ping_time_out));
			SaveHeader(ProtocolFileName, "DateTime; Adress; Status; Delay");
			//stateTimer = new Timer(EventStateTimer, is_async, 100, timer_interval);
			Run();
		} // /////////////////////////////////////////////////////////////////////
		~TimerPings() {
			if (stateTimer != null)
				stateTimer.Dispose();
		} // //////////////////////////////////////////////////////////////////////
		public void Run() {
			Task task1 = new Task(() => Factorial(new AdressIndex("192.168.1.122", 3)));
			task1.Start();
			Task task2 = Task.Factory.StartNew(() => Factorial(new AdressIndex("192.168.1.123", 2)));
		} // /////////////////////////////////////////////////////////////
		void Factorial(AdressIndex indata) {
			Ping png = new Ping();
			for (int i = 0; i < 999999999; i++) {
				PingReply reply = png.Send(indata.adress, 1000);
				Brush brush = TPT.GetBrush(reply);
				setBrush(brush, indata.index);
			}
		} // ////////////////////////////////////////////////////////////////////////

		public void Cycle() {
			for (int i = 0; i < alllist.Count; i++) {
				TPT tpt = alllist[i];
				switch (tpt.ready) {
					case TPT.Ready.WaitTimer:
						bool checktimer = tpt.CheckTimer();
						if (checktimer) {
							Pinger(tpt); // ready=WaitReplay; Reply=Send(Adress); ready=ReadReplay;
						}
						break;
					case TPT.Ready.WaitReplay:
						break;
					case TPT.Ready.ReadReplay:
						//Console.WriteLine("Cycle() case:ReadReplay tpt:" + tpt.ToString());
						if (tpt.ChangeStatus())
							SaveReplay(tpt);
						if (tpt.ChangeStatus())
							setBrush(tpt.brush, i);
						tpt.SetTimer();
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
				switch (tpt.ready) {
					case TPT.Ready.WaitTimer:
						bool checktimer = tpt.CheckTimer();
						if (checktimer) {
							asyn.Invoke(tpt); // ready=WaitReplay; Reply=Send(Adress); ready=ReadReplay;
						}
						break;
					case TPT.Ready.WaitReplay:
						break;
					case TPT.Ready.ReadReplay:
						//Console.WriteLine("Cycle() case:ReadReplay tpt:" + tpt.ToString());
						if (tpt.ChangeStatus())
							SaveReplay(tpt); // setBrush(tpt.brush, i) inside
						if (tpt.ChangeStatus())
							setBrush(tpt.brush, i);
						tpt.SetTimer();
						//Console.WriteLine("Cycle()->after setBrush; " + tpt.ToString());
						break;
					case TPT.Ready.First:
						tpt.Init();
						setBrush(tpt.brush, i);
						break;
				} // ----------- switch(tpt.ready) 
			} // --------------for
		} // ///////////////////////////////////////////////////////////////////////////////////////////////
		private void Pinger(TPT tpt) {
			Ping png = new Ping();
			try {
				tpt.ready = TPT.Ready.WaitReplay;
				tpt.Reply = png.Send(tpt.Adress, tpt.pingTimeOut);
				tpt.ready = TPT.Ready.ReadReplay;
			} catch {
				Console.WriteLine("Возникла ошибка ping! " + tpt.Adress);
			}
		} // /////////////////////////////////////////////////////////////////////////////////////////////
		async private static void PingerA(TPT tpt) {
			Ping png = new Ping();
			try {
				tpt.ready = TPT.Ready.WaitReplay;
				tpt.Reply = await png.SendPingAsync(tpt.Adress, tpt.pingTimeOut);
				tpt.ready = TPT.Ready.ReadReplay;
			} catch {
				Console.WriteLine("Возникла ошибка aping! " + tpt.Adress);
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
		private void SaveReplay(TPT tpt) {
			// save status
			string s = String.Format("{0};{1};{2};{3}",
				DateTime.Now.ToString("s").Replace('T', ' '),
				tpt.Adress,
				tpt.Reply.Status,
				tpt.Reply.RoundtripTime);
			//Console.WriteLine("~~" + s);
			SaveLine(ProtocolFileName, s);
		} // /////////////////////////////////////////////////////////////////////////////////////////
		private void setBrush(Brush brush, int i) {
			int bnd = 0;
			if (i == (bnd++)) Brush00 = brush;
			else if (i == (bnd++)) Brush01 = brush;
			else if (i == (bnd++)) Brush02 = brush;
			else if (i == (bnd++)) Brush03 = brush;
			else if (i == (bnd++)) Brush04 = brush;
			else if (i == (bnd++)) Brush05 = brush;
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
			get {
				return alllist[2].brush;
			}
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
		public Brush Brush06 {
			get { return alllist[6].brush; }
			set {
				alllist[6].brush = value;
				OnPropertyChanged("Brush06");
			}
		} // /-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/
		private void SaveHeader(string fname, string s) {
			FileInfo fileInf = new FileInfo(ProtocolFileName);
			if (!fileInf.Exists) {
				SaveLine(fname, s, false);
			}
		} // ////////////////////////////////////////////////////////////////////////////////////////
		private void SaveLine(string fname, string s, bool attach = true) {
			try {
				using (StreamWriter sw = new StreamWriter(fname, attach, System.Text.Encoding.Default)) {
					sw.WriteLine(s);
				}
			} catch (Exception e) {
				Console.WriteLine(e.Message);
			}
		} // //////////////////////////////////////////////////////////////////////////////////////
	} // -----------------------------------------------------------------------------
}

