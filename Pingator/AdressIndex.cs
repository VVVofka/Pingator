using System.Windows.Media;

namespace Pingator {
	public class AdressIndex {
		public string adress;
		public int index;
		public int timeout;
		public int timecycle;
		public Brush brush;
		public AdressIndex(string adress="", int index=-1, int timeout=0, int timecycle=0) {
			this.adress = adress;
			this.index = index;
			this.timeout = timeout;
			this.timecycle = timecycle;
			brush = Brushes.Magenta;
		} // ///////////////////////////////////////////////////////////////////////////////////////////
	} // -------------------------------------------------------------------------------------
}

