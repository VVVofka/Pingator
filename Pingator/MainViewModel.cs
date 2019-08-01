using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace Pingator {
	class MainViewModel{ // : INotifyPropertyChanged {
		public string Adress { get; set; }
		public ObservableCollection<Point> PointList { get; private set; }
		public MainViewModel() {
			PointList = new ObservableCollection<Point>();

			// Some example data:
			AddPoint(new Point(10, 10));
			AddPoint(new Point(200, 200));
			AddPoint(new Point(300, 300));
		} // ///////////////////////////////////////////////////////////

		//public event PropertyChangedEventHandler PropertyChanged;

		public void AddPoint(Point p) {
			// 3 at most, please!
			PointList.Add(p);
			if (PointList.Count == 3) {
				PointList.RemoveAt(0);
			}
		} // ///////////////////////////////////////////////////////////////////
		public void Del() {
			if (PointList.Count > 0)
				PointList.RemoveAt(0);
		} // ////////////////////////////////////////////////////////////////////////
	}
}
