using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calibration
{
   public abstract class CameraAbstract
    {
       public abstract void stopLiveMode();
       public abstract void startLiveMode();
       public abstract Boolean open_camera(object sender,EventArgs e);
      // public abstract void Close_camera(string s , Args arg);
       public abstract bool isLiveMode();
       public abstract void resumeLiveMode();
       public abstract void initLL();
       public abstract void getFrame();
       public abstract void TestCameraMemory();
       public abstract void TestPcbMemory2();
       public abstract void TestPcbMemory1();
       public abstract void exitCamera(string s , Args arg);
       public abstract void ResetNVRAMStruct(string s , Args arg);
       public abstract void ReadNVRAM(string s , Args arg);
       public abstract void WriteNVRAM(string s , Args arg);
       public abstract void SetBaloonLight(bool enable);
       public abstract bool SetCorneaIR(bool enable);
       public abstract bool SetCorneaWhiteRing(bool enable);
       public abstract bool SetRefractoIRRing(bool enable);
       public abstract bool SetIR(bool enable);
       public abstract bool SetFundusFlash(bool enable);
       public abstract void GetPowerStatus(string s , Args arg);
       public abstract char GetLaterality();
       public abstract void WhiteLightCapture();
       public abstract void IRCapture();
       public abstract void setExposure();
       public abstract void setDigitalGain();
       public abstract void setRGBGain();
       public abstract void CameraDisconnected();
       public abstract void NewCameraConnected();
    }
}
