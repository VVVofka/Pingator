using System.Windows.Media;

namespace Pingator {
	public class AdressIndex {
		public string adress;
		public int index;
		public int timeout;
		public int timecycle;
		public Brush brush;
		public AdressIndex(string adress, int index, int timeout, int timecycle) {
			this.adress = adress;
			this.index = index;
			this.timeout = timeout;
			this.timecycle = timecycle;
			brush = Brushes.Magenta;
		} // ///////////////////////////////////////////////////////////////////////////////////////////
	} // -------------------------------------------------------------------------------------
}

