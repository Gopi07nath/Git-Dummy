using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Calibration
{
	public delegate void NotificationHandler(String n, Args args);
	public class FacadeException : Exception {
		public String error;
		public FacadeException(String s) {
			error = s;
		}
	}
	public class Facade
	{

		#region LOGIN ---------------------------------------
        public String LOGIN_SUCCESS = "LOGIN_SUCCESS";
		#endregion

        private class Handlers : List<NotificationHandler> {};
		private class Registry : Dictionary<String, Handlers> {};
		private Registry reg = new Registry();

		#region SINGLETON -----------------------
		private static Facade _instance = null;
		public static Facade getInstance() {
			if (_instance == null)
				_instance = new Facade();
			return _instance;
		}
		#endregion

        public String SET_DISPLAY_AREA = "SET_DISPLAY_AREA";
        public String SET_Trigger = "SET_Trigger";
        public String TRIGGER_RISING_EDGE = "TRIGGER_RISING_EDGE";
        public string SET_LATERALITY = "SET_LATERALITY";
        public String SetDeviceIDPage = "SetDeviceIDPage";
        public string SetMemoryTestMode = "SetMemoryTestMode";
        public String CAMERA_CONNECTED_TEST = "CAMERA_CONNECTED_TEST";
        public String CAMERA_DISCONNECTED = "CAMERA_DISCONNECTED";
        public String CAMERA_CONNECTED = "CAMERA_CONNECTED";
        public String EXIT_CAMERA = "EXIT_CAMERA";
        public string Save_Image = "Save_Image";
        public String Set_ClassicRoyalPage = "Set_ClassicRoyalPage";
        public String Set_MainPage = "Set_MainPage";
        public string Close_Camera = "Close_Camera";
        public string DeviceID_Page = "DeviceID_Page";
        public string PowerFailure = "PowerFailure";
        public string ApplicationMode = "ApplicationMode";
        public string SetCurrentMode = "SetCurrentMode";
        public string GetFrameWidth = "GetFrameWidth";

        //Basic Test
        public string ManageLightOnOff = "ManageLightOnOff";

        // Alignment 
        public String Alignment_Image_Metrics = "Alignment_Image_Metrics";
        public String FundusAlignmentMode = "FundusAlignmentMode";
        public string ApplyMask = "ApplyMask";
        public string Update_Image_Metrics_UI = "Update_Image_Metrics_UI";
        public string Update_IlluminationGrid = "Update_IlluminationGrid";
        public String CameraControls = "CameraControls";
        public string setControls = "setControls";
        public string Refracto_Init = "Refracto_Init";
        public string displayImage = "displayImage";
        public string AlignmentSaveReport = "AlignmentSaveReport";

        //Fundus Alignment
        public string RefreshGridLabels = "RefreshGridLabels";
        public string DisableBrowseBtn = "DisableBrowseBtn";
        public string ManageIlluminationGrid = "ManageIlluminationGrid";
        public string ManageControlsAfterSaveReport = "ManageControlsAfterSaveReport";

        //Camera Alignment
        public string CameraAlignmentMode = "CameraAlignmentMode";
        public string CameraAlignmentSaveReport = "CameraAlignmentSaveReport";

        //Lens-Artifact Measurement
        public String LensArtifactMeasurementMode = "LensArtifactMeasurementMode";
        public string GetSegmentedImage = "GetSegmentedImage";
        public string SetAoiLensArtifactMeasurement = "SetAoiLensArtifactMeasurement";
        public string DisplayCorrectedImage = "DisplayCorrectedImage";
        public string SET_LACoordinates = "SET_LACoordinates";
        public string GradingLive = "GradingLive";
        public string GradingStill = "GradingStill";
        public string LASaveReport = "LASaveReport";
        public string LAchangeCoordinates = "LAchangeCoordinates";

        //NVRAM
        public string Read_Nvram = "Read_Nvram";
        public string Write_Nvram = "Write_Nvram";
        public string Reset_Nvram = "Reset_Nvram";
        //refracto calibration

        public String REFRACTO_CAPTURE_START = "REFRACTO_CAPTURE_START";
        public String REFRACTO_CAPTURE_COMPLETE = "REFRACTO_CAPTURE_COMPLETE";
        public string REFRACTO_CAPTURE = "REFRACTO_CAPTURE";
        public String REFRACTO_CALCULATIONS_COMPLETE = "REFRACTO_CALCULATIONS_COMPLETE";
        public String REFRACTO_FOCUSKNOB_STATUS = "REFRACTO_FOCUSKNOB_STATUS";
        public String GO_STANDBY = "GO_STANDBY";
        public String REFRACTO_NO_OF_SPOTS = "REFRACTO_NO_OF_SPOTS";
        public String REFRACTO_SPOTS_DETECTED = "REFRACTO_SPOTS_DETECTED";
        public String REFRACTO_CAPTURE_COMPLETE_INLIVE = "REFRACTO_CAPTURE_COMPLETE_INLIVE";
        public String LIQUIDLENS_COMPLETE = "LIQUIDLENS_COMPLETE";
        public string Refracto_Retake = "Refracto_Retake";
        public string SetRefractoCalibrationMode = "SetRefractoCalibrationMode";


		public Facade() {
		}
		public void Subscribe(String n, NotificationHandler handler) {
			Handlers hs;
			if (reg.ContainsKey(n)) {
				hs = reg[n];
			} else {
				hs = new Handlers();
				reg[n] = hs;
			}
			hs.Add(handler);
		}
		public void UnSubscribe(String n, NotificationHandler handler) {
			Handlers hs;
			if (reg.ContainsKey(n)) {
				hs = reg[n];
				if (hs.Contains(handler))
					hs.Remove(handler);
			}
		}
		public void Publish(String n) {
			Publish(n, new Args());
		}
		public void Publish(String n, Args args) {
			if ( ! reg.ContainsKey(n)) {
				//return;
				FacadeException e = new FacadeException("Can't find handlers for " + n);
				throw e;
			}
			Handlers handlers = reg[n];
			if (args == null) args = new Args();
			foreach (NotificationHandler h in handlers) {
				h(n, args);
			}
		}
	}
}
