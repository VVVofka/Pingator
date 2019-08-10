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
	public partial class TimerPings : INotifyPropertyChanged {
		public string ProtocolFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"protocol.csv");
		List<AdressIndex> alllist;
		public TimerPings(List<AdressIndex> list) {
			SaveHeader(ProtocolFileName, "DateTime;\tAdress;\tStatus");
			alllist = list;
			Run();
		} // /////////////////////////////////////////////////////////////////////
		public void Run() {
			List<Task> tasks = new List<Task>();
			foreach (AdressIndex ai in alllist) {
				setBrush(ai.brush, ai.index); // color for non initializing items (magenta)
				tasks.Add(Task.Factory.StartNew(() => TaskFunc(ai))); // MAIN
			}
		} // /////////////////////////////////////////////////////////////
		void TaskFunc(AdressIndex indata) {
			Ping png = new Ping();
			bool first = true;
			IPStatus prevStatus = IPStatus.Unknown;
			PingTimeSaver pingTimeSaver = new PingTimeSaver(indata.adress);
			while (true) {
				long start = DateTime.Now.Ticks;
				PingReply reply = png.Send(indata.adress, indata.timeout);
				Brush brush = GetBrush(reply);
				setBrush(brush, indata.index);
				if (first ||// (
						//prevStatus != reply.Status &&
						(prevStatus == IPStatus.Success ^ reply.Status == IPStatus.Success)
					//)
					) {
					first = false;
					SaveReplay(indata.adress, reply);
					prevStatus = reply.Status;
				}
				if (reply.Status == IPStatus.Success)
					pingTimeSaver.Add(reply.RoundtripTime);
				int timeInWork = (int)((DateTime.Now.Ticks - start) / 10000);
				if (timeInWork < indata.timecycle)
					Thread.Sleep(indata.timecycle - timeInWork);
			}
		} // ////////////////////////////////////////////////////////////////////////
		public static void DoEvents() {
			if (Application.Current != null)
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
		} // /////////////////////////////////////////////////////////////////////////////////////////

		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName]string prop = "") {
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
		} // /////////////////////////////////////////////////////////////////////////////////////////
		private void SaveReplay(string adress, PingReply reply) {
			// save status
			string s = String.Format("{0};\t{1};\t{2}",
				DateTime.Now.ToString("s").Replace('T', ' '),
				adress,
				reply.Status);
			//Console.WriteLine("~~" + s);
			SaveLineM(ProtocolFileName, s);
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
			set { alllist[0].brush = value; OnPropertyChanged("Brush00"); }
		} // /-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/
		public Brush Brush01 {
			get { return alllist[1].brush; }
			set { alllist[1].brush = value; OnPropertyChanged("Brush01"); }
		} // /-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/
		public Brush Brush02 {
			get { return alllist[2].brush; }
			set { alllist[2].brush = value; OnPropertyChanged("Brush02"); }
		} // /-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/
		public Brush Brush03 {
			get { return alllist[3].brush; }
			set { alllist[3].brush = value; OnPropertyChanged("Brush03"); }
		} // /-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/
		public Brush Brush04 {
			get { return alllist[4].brush; }
			set { alllist[4].brush = value; OnPropertyChanged("Brush04"); }
		} // /-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/
		public Brush Brush05 {
			get { return alllist[5].brush; }
			set { alllist[5].brush = value;  OnPropertyChanged("Brush05"); }
		} // /-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/
		public Brush Brush06 {
			get { return alllist[6].brush; }
			set { alllist[6].brush = value; OnPropertyChanged("Brush06"); }
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
		private void SaveLineM(string fname, string s) {
			using (var mutex = new Mutex(false, "UNIQUE_NAME")) {
				mutex.WaitOne();
				using (var aFile = new FileStream(fname, FileMode.Append, FileAccess.Write, FileShare.Write))
				using (StreamWriter writer = new StreamWriter(aFile)) {
					writer.WriteLine(s);
					writer.Flush();
					writer.BaseStream.Flush();
				}
				mutex.ReleaseMutex();
			}
		} // //////////////////////////////////////////////////////////////////////////////////////
		public static Brush GetBrush(PingReply reply) {
			if (reply == null)
				return Brushes.Magenta;
			switch (reply.Status) {
				case IPStatus.Success:
					return Brushes.Green;
				case IPStatus.TimedOut:
					return Brushes.Orange;
				case IPStatus.DestinationHostUnreachable:
					return Brushes.DarkGray;
				case IPStatus.DestinationNetworkUnreachable:
					return Brushes.Gray;
				case IPStatus.DestinationPortUnreachable:
					return Brushes.Silver;
				case IPStatus.DestinationProtocolUnreachable:
					return Brushes.LightGray;
				default:
					return Brushes.Red;
			}
		} // //////////////////////////////////////////////////////////////////////////////////
	} // -----------------------------------------------------------------------------
}

