using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calibration
{
	public class LensControl {
		private Camera theCamera;
		private byte mval = 0x00;

		//diopter = a * val + b
		private double a = 0.061;
		private double b = -12.144;

		private double maxval = 255.0;
		private double minval = 100.0;

		public LensControl(Camera cam) {
			theCamera = cam;
		}

		public  bool SetVal(byte val) {
			if ( val == 0x00 ) {
				val = 0x01; //increase with 1 to prevent standby
			}

			bool result = theCamera.SetSupertex(val); //TODO: error handling

			if ( true ) {
				mval = val;
				return true;
			} else {
				return false;
			}
		}

		public bool SetDiopter(double diopter, ref double diopterset) {
			double dval = ( diopter - b ) / a;
			byte bval = ( byte )Math.Round(Math.Min(Math.Max(minval, dval), maxval));
			bool result = SetVal(bval);
			diopterset = GetDiopter();
			return result;
		}

		private double Val2Diopter(int val) {
			return a * ( double )val + b;
		}

		public double GetDiopter() {
			return Val2Diopter(mval);
		}

		public double MinDiopter {
			get {
				return Val2Diopter(( int )minval);
			}
		}

		public double MaxDiopter {
			get {
				return Val2Diopter(( int )maxval);
			}
		}

		public bool LiquidLensStandby(byte val) {
			mval = 0x00;
			return theCamera.SetSupertex(0x00);
		}

	}
}
