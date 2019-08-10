using System.Windows.Shapes;

namespace Pingator {
	public class ControlAdress {
		public readonly Shape Control;
		public readonly string Adress;
		public ControlAdress(string adress, Shape control) {
			this.Control = control;
			this.Adress = adress;
		} // ///////////////////////////////////////////////////////////////////////////////////
	} // ----------------------------------------------------------------------------------------
}
