using System;
using System.IO;
using System.Net.NetworkInformation;

namespace Pingator {
	class PingTimeSaver {
		private readonly string Fname;
		private DateTime prevdt = new DateTime(0);
		private long sum = 0;
		private int cnt = 0;
		private bool success = false;
		public PingTimeSaver(string adress) {
			Fname = adress.Replace('.', '_') + @".csv";
			SaveHeader(Fname, "DateTime;\tPing");
			prevdt = DateTime.Now;
		} // /////////////////////////////////////////////////////////////////
		public void Add(long ms, IPStatus status) {
			DateTime now = DateTime.Now;
			if ((cnt != 0) && (	// if new minute
					now.Minute != prevdt.Minute || 
					now.Hour != prevdt.Hour || 
					now.Day != prevdt.Day || 
					now.Month != prevdt.Month || 
					now.Year != prevdt.Year)
					) {
				long avg = sum / cnt;
				string s = prevdt.ToString("yyyy-MM-dd hh:mm") + 
					";\t" + avg.ToString() + 
					";\t" + success.ToString();
				SaveLine(Fname, s);
				cnt = 1;
				sum = ms;
				success = (status == IPStatus.Success);
				prevdt = now;
			} else {
				cnt++;
				sum += ms;
				success = success && (status == IPStatus.Success);
			}
		} // /////////////////////////////////////////////////////////////////////////////////////
		private void SaveHeader(string f_name, string s) {
			FileInfo fileInf = new FileInfo(f_name);
			if (!fileInf.Exists) {
				SaveLine(f_name, s, false);
			}
		} // ////////////////////////////////////////////////////////////////////////////////////////
		private void SaveLine(string f_name, string s, bool attach = true) {
			try {
				using (StreamWriter sw = new StreamWriter(f_name, attach, System.Text.Encoding.Default)) {
					sw.WriteLine(s);
				}
			} catch (Exception e) {
				Console.WriteLine(f_name + "->" + e.Message);
			}
		} // //////////////////////////////////////////////////////////////////////////////////////
	} // ----------------------------------------------------------------
}
