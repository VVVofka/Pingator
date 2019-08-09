using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pingator {
	class PingTimeSaver {
		private readonly string Fname;
		private DateTime prevdt = new DateTime(0);
		private long sum = 0;
		private int cnt = 0;
		public PingTimeSaver(string adress) {
			Fname = adress.Replace('.', '_') + @".csv";
			SaveHeader(Fname, "DateTime;Ping");
		} // /////////////////////////////////////////////////////////////////
		public void Add(long ms) {
			DateTime now = DateTime.Now;

			if ((cnt != 0) && (	// if new minute
					now.Minute != prevdt.Minute || 
					now.Hour != prevdt.Hour || 
					now.Day != prevdt.Day || 
					now.Month != prevdt.Month || 
					now.Year != prevdt.Year)
					) {
				long avg = sum / cnt;
				string s = prevdt.ToString("s").Replace('T', ' ') + ";" + avg.ToString();
				SaveLine(Fname, s);
				cnt = 0;
				sum = 0;
				prevdt = now;
			} else {
				cnt++;
				sum += ms;
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
