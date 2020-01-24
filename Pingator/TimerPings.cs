using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace Pingator
{
	public partial class TimerPings : INotifyPropertyChanged
	{
		public string ProtocolFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"\Data", @"protocol.csv");
		private List<AdressIndex> alllist = new List<AdressIndex>();
		private const string separator = "\t";
		public TimerPings(string f_name)
		{
			SaveHeader(ProtocolFileName, "DateTime" + separator +
										"Adress" + separator +
										"Status");

			LoadFromFile(f_name);
			List<Task> tasks = new List<Task>();
			foreach (AdressIndex ai in alllist)
			{
				setBrush(ai.brush, ai.index); // color for non initializing items (magenta)
				tasks.Add(Task.Factory.StartNew(() => TaskFunc(ai))); // MAIN
			}
		} // /////////////////////////////////////////////////////////////////////
		void TaskFunc(AdressIndex indata)
		{
			Ping png = new Ping();
			bool first = true;
			IPStatus prevStatus = IPStatus.Unknown;
			PingTimeSaver pingTimeSaver = new PingTimeSaver(indata.adress);
			while (true)
			{
				long start = DateTime.Now.Ticks;
				PingReply reply = png.Send(indata.adress, indata.timeout);
				Brush brush = GetBrush(reply);
				setBrush(brush, indata.index);
				if (first || (prevStatus == IPStatus.Success ^
								reply.Status == IPStatus.Success))
				{
					first = false;
					SaveReplay(indata.adress, reply);
					prevStatus = reply.Status;
				}
				if (reply.Status == IPStatus.Success)
					pingTimeSaver.Add(reply.RoundtripTime, reply.Status);
				int timeInWork = (int)((DateTime.Now.Ticks - start) / 10000);
				if (timeInWork < indata.timecycle)
					Thread.Sleep(indata.timecycle - timeInWork);
			}
		} // ////////////////////////////////////////////////////////////////////////

		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName]string prop = "")
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
		} // /////////////////////////////////////////////////////////////////////////////////////////
		private void SaveReplay(string adress, PingReply reply)
		{
			// save status
			string s = String.Format("{0}{3}{1}{3}{2}",
				DateTime.Now.ToString("s").Replace('T', ' '),
				adress,
				reply.Status, separator);
			//Console.WriteLine("~~" + s);
			SaveLineM(ProtocolFileName, s);
		} // /////////////////////////////////////////////////////////////////////////////////////////
		private void setBrush(Brush brush, int i)
		{
			if (alllist.Count > 0)
			{
				int bnd = 0;
				if (i == (bnd++)) Brush00 = brush;
				else if (i == (bnd++)) Brush01 = brush;
				else if (i == (bnd++)) Brush02 = brush;
				else if (i == bnd) Brush03 = brush;
			}
		} // ///////////////////////////////////////////////////////////////////////////////////////////
		public Brush Brush00 {
			get { 
				return alllist.Count > 0? alllist[0].brush : null; 
			}
			set { alllist[0].brush = value; OnPropertyChanged("Brush00"); }
		} // /-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/
		public Brush Brush01 {
			get { return alllist.Count > 1 ? alllist[1].brush : null; }
			set { alllist[1].brush = value; OnPropertyChanged("Brush01"); }
		} // /-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/
		public Brush Brush02 {
			get { return alllist.Count > 2 ? alllist[2].brush : null; }
			set { alllist[2].brush = value; OnPropertyChanged("Brush02"); }
		} // /-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/
		public Brush Brush03 {
			get { return alllist.Count > 3 ? alllist[3].brush : null; }
			set { alllist[3].brush = value; OnPropertyChanged("Brush03"); }
		} // /-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/
		private void SaveHeader(string fname, string s)
		{
			FileInfo fileInf = new FileInfo(ProtocolFileName);
			if (!fileInf.Exists)
			{
				SaveLine(fname, s, false);
			}
		} // ////////////////////////////////////////////////////////////////////////////////////////
		private void SaveLine(string fname, string s, bool attach = true)
		{
			try
			{
				using (StreamWriter sw = new StreamWriter(fname, attach, System.Text.Encoding.Default))
				{
					sw.WriteLine(s);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		} // //////////////////////////////////////////////////////////////////////////////////////
		private void SaveLineM(string fname, string s)
		{
			using (var mutex = new Mutex(false, "UNIQUE_NAME"))
			{
				mutex.WaitOne();
				using (var aFile = new FileStream(fname, FileMode.Append, FileAccess.Write, FileShare.Write))
				using (StreamWriter writer = new StreamWriter(aFile))
				{
					writer.WriteLine(s);
					writer.Flush();
					writer.BaseStream.Flush();
				}
				mutex.ReleaseMutex();
			}
		} // //////////////////////////////////////////////////////////////////////////////////////
		public static Brush GetBrush(PingReply reply)
		{
			if (reply == null)
				return Brushes.Magenta;
			switch (reply.Status)
			{
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
		public static void DoEvents()
		{
			if (Application.Current != null)
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
		} // /////////////////////////////////////////////////////////////////////////////////////////
		public void LoadFromFile(string f_name)
		{
			int index = 0;
			try
			{
				using (StreamReader sr = new StreamReader(f_name, Encoding.Default))
				{
					while (sr.Peek() >= 0)
					{
						string line = sr.ReadLine();
						string[] words = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
						if (words.Count() == 3)
						{
							int i = 0;
							string adress = words[i++];
							int timeout = Convert.ToInt32(words[i++]);
							int period = Convert.ToInt32(words[i++]);
							alllist.Add(new AdressIndex(adress, index++, timeout, period));
						}
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(f_name + "->" + e.Message);
			}
		} // ////////////////////////////////////////////////////////////////////////////////////
	} // -----------------------------------------------------------------------------
}

