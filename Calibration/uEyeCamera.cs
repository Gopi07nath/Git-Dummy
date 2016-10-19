using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Calibration

{
    public class uEyeCamera
    {
        //uEye stuff
        private uEye m_uEye;
        private bool m_bLive = false;
        private bool m_bIsConnected = false;
        private bool m_bDrawing;
        private int m_RenderMode;
        public static bool IsCameraConnected = true;

        private bool bAutoConnect = false;
        
        // uEye images
        private const int IMAGE_COUNT = 1;
        private struct UEYEIMAGE
        {
            public IntPtr pMemory;
            public int MemID;
            public int nSeqNum;
        }
        private UEYEIMAGE[] m_UeyeImages;
        private IntPtr m_pCurMem;
        private IntPtr hWindow;
        private IntPtr hWndProc;
        //private System.Windows.Forms.Form cForm;

        private int local_bin_mode = 0; // used by auto_connect

        public event EventHandler StatusChanged;

        public uEyeCamera(IntPtr wndProcHandle, IntPtr windowHandle)
        {
            //cForm = creatorForm;//TODO: make this neat
            hWndProc = wndProcHandle;
            hWindow = windowHandle;
            uEyeInit();
        }

        private void uEyeInit()
        {
            // init variables
            m_bLive = false;
            m_bDrawing = false;
            m_bIsConnected = false;
            // set render mode
            //m_RenderMode = uEye.IS_RENDER_NORMAL;
            m_RenderMode = uEye.IS_RENDER_FIT_TO_WINDOW;

            // init our ueye object
            m_uEye = new uEye();
            // enable static messages ( no open camera is needed )		
            m_uEye.EnableMessage(uEye.IS_NEW_DEVICE, hWndProc.ToInt32());
            m_uEye.EnableMessage(uEye.IS_DEVICE_REMOVAL, hWndProc.ToInt32());

            // init our image struct and alloc marshall pointers for the uEye memory
            m_UeyeImages = new UEYEIMAGE[IMAGE_COUNT];
            int nLoop = 0;
            for (nLoop = 0; nLoop < IMAGE_COUNT; nLoop++)
            {
                m_UeyeImages[nLoop].pMemory = Marshal.AllocCoTaskMem(4);	// create marshal object pointers
                m_UeyeImages[nLoop].MemID = 0;
                m_UeyeImages[nLoop].nSeqNum = 0;
            }
        }

        // ------------------------  GetImageID -------------------------------
        //
        int GetImageID(IntPtr pBuffer)
        {
            // get image id for a given memory
            if (!m_uEye.IsOpen())
                return 0;

            int i = 0;
            for (i = 0; i < IMAGE_COUNT; i++)
                if (m_UeyeImages[i].pMemory == pBuffer)
                    return m_UeyeImages[i].MemID;
            return 0;
        }

        // ------------------------  GetImageNum -------------------------------
        //
        int GetImageNum(IntPtr pBuffer)
        {
            // get number of sequence for a given memory
            if (!m_uEye.IsOpen())
                return 0;

            int i = 0;
            for (i = 0; i < IMAGE_COUNT; i++)
                if (m_UeyeImages[i].pMemory == pBuffer)
                    return m_UeyeImages[i].nSeqNum;

            return 0;
        }

        // -----------------  DrawImage  -------------------------
        //
        void DrawImage()
        {
            m_bDrawing = true;
            // draw current memory if a camera is opened
            if (m_uEye.IsOpen())
            {
                int num = 0;
                IntPtr pMem = new IntPtr();
                IntPtr pLast = new IntPtr();
                m_uEye.GetActSeqBuf(ref num, ref pMem, ref pLast);
                if (pLast.ToInt32() == 0)
                {
                    m_bDrawing = false;
                    return;
                }

                int nLastID = GetImageID(pLast);
                int nLastNum = GetImageNum(pLast);
                m_uEye.LockSeqBuf(nLastNum, pLast);

                m_pCurMem = pLast;		// remember current buffer for our tootip ctrl

                m_uEye.RenderBitmap(nLastID, hWindow.ToInt32(), m_RenderMode);

                /*
                IntPtr ppMem = IntPtr.Zero;
                if (m_uEye.GetImageMem(ref ppMem) == uEye.IS_SUCCESS)
                {
                    double cval = 0;
                    GetContrast(ppMem, out cval);
                    ((MainForm)cForm).SetContrastStatus(cval);
                }
                */
                m_uEye.UnlockSeqBuf(nLastNum, pLast);
            }
            m_bDrawing = false;
        }

        public Size GetSensorSize()
        {
            return new Size(m_uEye.GetDisplayWidth(), m_uEye.GetDisplayHeight());
        }


        // ---------------------  Read I2C  ----------------------------------------
        // 
        public int ReadI2C(int nDeviceAddr, int nRegisterAddr, byte[] pbData, int nLen)
        {
            return m_uEye.ReadI2C(nDeviceAddr, nRegisterAddr, pbData, nLen);
        }

        // ---------------------  WriteI2C ---------------------------
        //
        public int WriteI2C(int nDeviceAddr, int nRegisterAddr, byte[] pbData, int nLen)
        {
            return m_uEye.WriteI2C(nDeviceAddr, nRegisterAddr, pbData, nLen);
        }

        public unsafe void GetContrast(IntPtr pmem, out double cval)
        {
            //Calculate contrast based on Sobel operataor

            byte* ptr = (byte*)pmem;
            int w = m_uEye.GetDisplayWidth();
            int h = m_uEye.GetDisplayHeight();

            //define region of interest
            int xmarg = (int)Math.Round((decimal)w * 1 / 3);
            int ymarg = (int)Math.Round((decimal)h * 1 / 3);
            int xmin = xmarg;
            int xmax = w - xmarg;
            int ymin = ymarg;
            int ymax = h - ymarg;

            //Sobel filter kernels:
            //      |+1 +2 +1|       |+1 0 -1|
            // Gy = | 0  0  0|, Gx = |+2 0 -2|
            //      |-1 -2 -1|       |+1 0 -1|
            double[] S = new double[3] {1, 2, 1};

            //total size of filtered image
            int fimlen = (ymax - ymin) * (xmax - xmin);
            double fsum = 0.0;
            double fx = 0.0;
            double fy = 0.0;

            for (int y = ymin; y < ymax; y++)
            {
                for (int x = xmin; x < xmax; x++)
                {
                    fx = 0.0;
                    fy = 0.0;
                    int si = 0;
                    for (int fi1 = -1; fi1 <= 1; fi1++) //filter in x-direction
                    {
                        fx +=  ((double)(*(ptr + (y + fi1) * (w << 2) + ((x -   1) << 2))) 
                               -(double)(*(ptr + (y + fi1) * (w << 2) + ((x +   1) << 2)))) * S[si];
                        fy +=  ((double)(*(ptr + (y -   1) * (w << 2) + ((x + fi1) << 2)))
                               -(double)(*(ptr + (y +   1) * (w << 2) + ((x + fi1) << 2)))) * S[si];
                        si++;
                    }
                    fsum += Math.Sqrt(Math.Pow(fx, 2) + Math.Pow(fy, 2));
                }
            }

            //Mark border around region of interest by setting red pixels to max
            int tmax = 5;
            for (int y = ymin - tmax; y < ymax + tmax; y++)
            {
                for (int t = 1; t < tmax + 1; t++)
                {
                    *(ptr + y * (w << 2) + ((xmin - t) << 2) + 2) = 255;
                    *(ptr + y * (w << 2) + ((xmax + t) << 2) + 2) = 255;
                }
            }

            for (int x = xmin - tmax; x < xmax + tmax; x++)
            {
                for (int t = 1; t < tmax + 1; t++)
                {
                    *(ptr + (ymin - t) * (w << 2) + (x << 2) + 2) = 255;
                    *(ptr + (ymax + t) * (w << 2) + (x << 2) + 2) = 255;
                }
            }

            cval = fsum / fimlen;

        }


        unsafe void GetContrast1(IntPtr pmem, out double cval)
        {
            //DateTime start = DateTime.Now;

            byte* ptr = (byte*)pmem;
            int w = m_uEye.GetDisplayWidth();
            int h = m_uEye.GetDisplayHeight();
            
            int xmarg = (int)Math.Round((decimal)w*3/7);
            int ymarg = (int)Math.Round((decimal)h*3/7);
            int xmin = xmarg;
            int xmax = w-xmarg;
            int ymin = ymarg;
            int ymax = h-ymarg;

            int fw = 3;

            //first create a LP filter:
            int N = 2 * fw + 2;
            double a = 2.5;
            double[] fc_lp = new double[N];
            for (int n = 0; n < N; n++)
            {
                fc_lp[n] = Math.Exp(-0.5 * Math.Pow(a * 2 * (n-((N-1)/2)) / N, 2));
            }

            //Create HP-filter
            double [] fc = new double[N-1];
            fc[fw] = 0;
            for (int n = 0; n < N-1; n++)
            {
                fc[n] =  fc_lp[n+1] - fc_lp[n];
            }

            double fsum = 0.0;
            for (int c = 0; c < fc.Length; c++)
            {
                fsum += Math.Abs(fc[c]);
            }
            for (int c = 0; c < fc.Length; c++)
            {
                fc[c] = fc[c]/fsum;
            }

            int fimlen = (ymax - ymin) * (xmax - xmin);
            int fi = 0;
            double[] fimx = new double[fimlen];
            double[] fimy = new double[fimlen];
            double fimsum = 0.0;
            double fimmax = 0.0;


            for (int y = ymin; y < ymax; y++)
            {
                for (int x = xmin; x < xmax; x++)
                {
                    fimx[fi] = 0.0;
                    fimy[fi] = 0.0;
                    for (int fci = 0; fci < fc.Length; fci++) //filter in x-direction
                    {
                        fimx[fi] += (double)((*(ptr + y * (w << 2) + ((x + fci - fw) << 2))) * fc[fci]);
                        fimy[fi] += (double)((*(ptr + (y + fci - fw) * (w << 2) + (x << 2))) * fc[fci]);
                    }
                    double fimscore = Math.Pow(fimx[fi], 2) + Math.Pow(fimy[fi], 2);
                    fimsum += fimscore;
                    fimmax = Math.Max(fimscore, fimmax);
                    fi++;
                }
            }

            int tmax = 5;
            for (int y = ymin-tmax; y < ymax+tmax; y++)
            {
                for (int t = 1; t < tmax + 1; t++)
                {
                    *(ptr + y * (w << 2) + ((xmin-t) << 2) + 2) = 255;
                    *(ptr + y * (w << 2) + ((xmax+t) << 2) + 2) = 255;
                }
            }

            for (int x = xmin-tmax; x < xmax+tmax; x++)
            {
                for (int t = 1; t < tmax + 1; t++)
                {
                    *(ptr + (ymin-t) * (w << 2) + (x << 2) + 2) = 255;
                    *(ptr + (ymax+t) * (w << 2) + (x << 2) + 2) = 255;
                }
            }

            bool debug = false;

            if (debug)
            {
                Bitmap test = new Bitmap(xmax-xmin, ymax-ymin, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                for (int y = 0; y < ymax - ymin; y++)
                {
                    for (int x = 0; x < xmax - xmin; x++)
                    {
                        byte color = (byte)(255*fimx[y*(xmax-xmin)+x]/fimmax);
                        test.SetPixel(x, y, Color.FromArgb(color, color, color));
                    }
                }
                test.Save("f:/test.png");
            }




            /*
            byte* yptr = (byte*)pmem + ymin * (w << 2);//goto row offset
            for (int y = ymin; y < ymax; y++)
            {
                ptr = yptr + (xmin<<2);
                for (int x = xmin; x < xmax; x++)
                {
                    sum += *(ptr += 4); //read only first of four bytes (red channel)
                }
                yptr += (w << 2);
            }

            double mn = sum/((xmax-xmin)*(ymax-ymin));
            double tmp = 0.0;

            yptr = (byte*)pmem + ymin * (w << 2);//goto row offset
            for (int y = ymin; y < ymax; y++)
            {
                ptr = yptr + (xmin << 2);// goto column offset within row
                for (int x = xmin; x < xmax; x++)
                {
                    tmp = (*(ptr += 4)) - mn; //read only first of four bytes (red channel)
                    std += tmp * tmp;
                }
                yptr += (w << 2); //increase row offset
            }

            cval = std / ((xmax - xmin) * (ymax - ymin));
            */
            cval = fimsum/fimlen;
            //double dur = (DateTime.Now - start).Milliseconds;

        }

        public bool Open(int bin_mode) // binning parameter as input
        {
            // if opened before, close now
            if (m_uEye.IsOpen())
            {
                m_uEye.ExitCamera();
                m_bIsConnected = false;
            }

            //if (bin_mode == (uEye.IS_BINNING_2X_HORIZONTAL | uEye.IS_BINNING_2X_VERTICAL))
            //{
            //    local_bin_mode = bin_mode;
              m_uEye.SetBinning(bin_mode);
            //}
            // open a camera
            int nRet = m_uEye.InitCamera(0, hWindow.ToInt32());
            if (nRet == uEye.IS_STARTER_FW_UPLOAD_NEEDED)
            {
                /************************************************************************************************/
                /*                                                                                              */
                /*  If the camera returns with "IS_STARTER_FW_UPLOAD_NEEDED", an upload of a new firmware       */
                /*  is necessary. This upload can take several seconds. We recommend to check the required      */
                /*  time with the function is_GetDuration().                                                    */
                /*                                                                                              */
                /*  In this case, the camera can only be opened if the flag "IS_ALLOW_STARTER_FW_UPLOAD"        */
                /*  is "OR"-ed to m_hCam. This flag allows an automatic upload of the firmware.                 */
                /*                                                                                              */
                /************************************************************************************************/

                uint nUploadTime = 25000;

                m_uEye.GetDuration(uEye.IS_SE_STARTER_FW_UPLOAD, ref nUploadTime);

                String Str;
                Str = "This camera requires a new firmware. The upload will take about " + nUploadTime / 1000 + " seconds. Please wait ...";
                //STRING_uEYE_075
                //System.Windows.Forms.MessageBox.Show(Str, Globals.languageResource.GetString("STRING_3nethra"));

                nRet = m_uEye.InitCamera(0 | uEye.IS_ALLOW_STARTER_FW_UPLOAD, hWindow.ToInt32());
            }

            if (nRet != uEye.IS_SUCCESS)
            {
                //System.Windows.Forms.MessageBox.Show("Camera initialization failed", "3nethra");
                //return false;
              //  System.Windows.Forms.MessageBox.Show("Camera not connected, connect camera and relaunch the application", "3nethra");

                IsCameraConnected = false;
                return false;
            }

            // check for image size
            uEye.SENSORINFO sensorInfo = new uEye.SENSORINFO();

            //m_uEye.SetTopoFlashMode();
            m_uEye.GetSensorInfo(ref sensorInfo);

           
            m_uEye.SetBinning(bin_mode);
            AllocateMemory();

            m_uEye.EnableMessage(uEye.IS_FRAME, hWndProc.ToInt32());
            m_uEye.EnableMessage(uEye.IS_NEW_DEVICE, hWndProc.ToInt32());
            m_uEye.EnableMessage(uEye.IS_DEVICE_REMOVED, hWndProc.ToInt32());
            m_uEye.EnableMessage(uEye.IS_UEYE_MESSAGE, hWndProc.ToInt32());
           // m_uEye.
            // free image
            //if (liveDisplayWindow.Image != null)
            //{
            //    liveDisplayWindow.Image.Dispose();
            //    liveDisplayWindow.Image = null;
            //}
            // capture a single image
            m_uEye.FreezeVideo(uEye.IS_WAIT);

            m_bIsConnected = true;
            //Send event in case handlers have been hooked
            if (StatusChanged != null)
                StatusChanged(this, new EventArgs());

            return true;
        }
        
        public int GetDisplayWidth()
        {
            return m_uEye.GetDisplayWidth();
        }

        public int GetDisplayHeight()
        {
            return m_uEye.GetDisplayHeight();
        }


        public void AllocateMemory()
        {
            int x, y;
            x = m_uEye.GetDisplayWidth();//sensorInfo.nMaxWidth ;
            y = m_uEye.GetDisplayHeight();//sensorInfo.nMaxHeight ;

            m_uEye.SetImageSize(x, y);
            m_uEye.SetColorMode(uEye.IS_SET_CM_RGB24); //IS_SET_CM_RGB32
		
			int pnNum = 0;
			IntPtr pPc = IntPtr.Zero;
			IntPtr pPpcLast = IntPtr.Zero;

			//m_uEye.GetActSeqBuf(ref pnNum, ref pPc, ref pPpcLast);

			int nLoop = 0;
			for ( nLoop = 0; nLoop < IMAGE_COUNT; nLoop++ ) {
				// de allocate memory
				m_uEye.FreeImageMem(m_UeyeImages[nLoop].pMemory, m_UeyeImages[nLoop].MemID);
			}

            // alloc images
            m_uEye.ClearSequence();
            //int nLoop = 0;
            for (nLoop = 0; nLoop < IMAGE_COUNT; nLoop++)
            {
                // alloc memory
                m_uEye.AllocImageMem(x, y, 24, ref m_UeyeImages[nLoop].pMemory, ref m_UeyeImages[nLoop].MemID);
                // m_uEye.AllocImageMem(x, y, 32, ref m_UeyeImages[nLoop].pMemory, ref m_UeyeImages[nLoop].MemID);
                // add our memory to the sequence
                m_uEye.AddToSequence(m_UeyeImages[nLoop].pMemory, m_UeyeImages[nLoop].MemID);
                // set sequence number
                m_UeyeImages[nLoop].nSeqNum = nLoop + 1;
            }
        }
        //public bool Binning2x()
        //{
        //    if (m_uEye.IsOpen())
        //    {
        //        int x = m_uEye.GetDisplayWidth();//sensorInfo.nMaxWidth ;
        //        int y = m_uEye.GetDisplayHeight();//sensorInfo.nMaxHeight ;
        //        m_uEye.SetImageSize(x, y);
        //        m_uEye.SetColorMode(uEye.IS_SET_CM_RGB32);

        //        // alloc images
        //        m_uEye.ClearSequence();
        //        int nLoop = 0;
        //        for (nLoop = 0; nLoop < IMAGE_COUNT; nLoop++)
        //        {
        //            Marshal.FreeCoTaskMem(m_UeyeImages[nLoop].pMemory);
        //        }

        //        for (nLoop = 0; nLoop < IMAGE_COUNT; nLoop++)
        //        {
        //            m_UeyeImages[nLoop].pMemory = Marshal.AllocCoTaskMem(4);	// create marshal object pointers
        //            m_UeyeImages[nLoop].MemID = 0;
        //            m_UeyeImages[nLoop].nSeqNum = 0; 
        //        }
        //        for (nLoop = 0; nLoop < IMAGE_COUNT; nLoop++)
        //        {
        //            // alloc memory
        //            m_uEye.AllocImageMem(x, y, 32, ref m_UeyeImages[nLoop].pMemory, ref m_UeyeImages[nLoop].MemID);
        //            // add our memory to the sequence
        //            m_uEye.AddToSequence(m_UeyeImages[nLoop].pMemory, m_UeyeImages[nLoop].MemID);
        //            // set sequence number
        //            m_UeyeImages[nLoop].nSeqNum = nLoop + 1;
        //        }
        //    }
        //    return true;
        //}

                                                                                                                  
        public bool Close()
        {
            m_bLive = false;

            // release marshal object pointers
            int nLoop = 0;
            for (nLoop = 0; nLoop < IMAGE_COUNT; nLoop++)

                m_uEye.FreeImageMem(m_UeyeImages[nLoop].pMemory, m_UeyeImages[nLoop].MemID);
               // Marshal.FreeCoTaskMem(m_UeyeImages[nLoop].pMemory);
                Array.Clear(m_UeyeImages.ToArray(), 0, m_UeyeImages.Length);


            bool bSuccess = false;
            if (m_uEye.IsOpen())
            {
                // Nagendra 25-july-2011 for 64-bit runtime proble
                  if (m_uEye.ExitCamera() == uEye.IS_SUCCESS)
                  
                bSuccess = true;
            }

            m_bIsConnected = false;
            //Send event in case handlers have been hooked
            if (StatusChanged != null)
                StatusChanged(this, new EventArgs());

            return bSuccess;
        }
       
        public bool GetCameraInfo(ref uEye.CAMINFO camInfo)
        {
            return m_uEye.GetCameraInfo(ref camInfo) == uEye.IS_SUCCESS;
        }
        // ------------------------  Start Live Video -------------------------------
        //
        public bool LiveMode(bool en)
        {
            bool bSuccess = false;
            if (en)
            {
                if (m_uEye.CaptureVideo(uEye.IS_WAIT) == uEye.IS_SUCCESS)
                {
                    m_bLive = true;
                    bSuccess = true;
                }
                else
                    return bSuccess;
                   // System.Windows.Forms.MessageBox.Show("Capture Video failed!","3nethra");
            }
            else
            {
                if (m_uEye.StopLiveVideo(uEye.IS_WAIT) == uEye.IS_SUCCESS)
                {
                    m_bLive = false;
                    bSuccess = true;
                }
                else
                    return bSuccess;
                   // System.Windows.Forms.MessageBox.Show("Capture Video failed!","3nethra");
            }
            //Send event in case handlers have been hooked
            if (StatusChanged != null)
                StatusChanged(this, new EventArgs());

            return bSuccess;
        }

        public bool SetLedIR(bool bVal, out bool bPrevVal)
        {
            int nPrevVal = 0;
            bool bSuccess = false;

            if (bVal)
                bSuccess = m_uEye.SetIO(0x01, 0x01, out nPrevVal);
            else
                bSuccess = m_uEye.SetIO(0x01, 0x00, out nPrevVal);

            bPrevVal = (nPrevVal & 0x01) == 0x01;
            return bSuccess;
        }

        public bool SetLedRing(bool bVal, out bool bPrevVal)
        {
            int nPrevVal = 0;
            bool bSuccess = false;

            if (bVal)
                bSuccess = m_uEye.SetIO(0x02, 0x02, out nPrevVal);
            else
                bSuccess = m_uEye.SetIO(0x02, 0x00, out nPrevVal);

            bPrevVal = (nPrevVal & 0x02) == 0x02;
            return bSuccess;
        }

        public bool SetLedWhite(bool bVal, out bool bPrevVal)
        {
            return m_uEye.SetFlashContinuous(bVal, out bPrevVal) == uEye.IS_SUCCESS;
        }

        public int SetExternalTrigger(int trig)
        {
            return m_uEye.SetExternalTrigger(trig);
        }

        public int GetTrigger()
        {
            return m_uEye.SetExternalTrigger(uEye.IS_GET_TRIGGER_STATUS);
        }

        public int SetFlashDelay(int ulDelay, int ulDuration)
        {
            m_uEye.SetFlashDelay(ulDelay, ulDuration);
            return 1;
        }

        public int SetFlashStrobe(int nMode, int nField)
        {
            m_uEye.SetFlashStrobe(nMode, nField);
            return 1;
        }

        public bool FlashOn(bool IsGlobalShutter,int flashDelay,int flashDuration)
        {
            m_uEye.Set3NethraFlashMode(IsGlobalShutter,flashDelay,flashDuration);
            return m_uEye.SetFlashEnabled() == uEye.IS_SUCCESS;
        }

        public bool FlashOff()
        {
            //commented by Ajith to improve performance
            //m_uEye.Set3NethraFlashModeOff();
            return m_uEye.SetFlashOff() == uEye.IS_SUCCESS;
        }

        public int SetPixelClock(int Clock)
        {
            return m_uEye.SetPixelClock(Clock);
        }

        public int GetPixelClockRange(ref int pnMin, ref int pnMax)
        {
            return m_uEye.GetPixelClockRange(ref  pnMin, ref  pnMax);
        }

        public int SetBinning(int mode)
        {

            if (m_uEye.SetBinning(mode) == uEye.IS_SUCCESS)
            {
                AllocateMemory();
                return uEye.IS_SUCCESS;
            }
            else
            {
                return 0;
            }
            
        }

        // ------------------------  Single Capture  -------------------------------
        //
        public bool CaptureSingle()
        {
            // capture a single image
            if (m_uEye.FreezeVideo(uEye.IS_WAIT) != uEye.IS_SUCCESS)
            {
               
                //System.Windows.Forms.MessageBox.Show("Camera Failed to Campture Image,Please retry!", "3nethra", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                Globals.isCaptureFailure = true;
                return false;
            }
            Globals.isCaptureFailure = false;

            return true;
        }

        // ------------------------  Single Capture  -------------------------------
        //
        public bool CaptureSingle(out double ContrastVal)
        {
            ContrastVal = 0.0;

            // capture a single image
            if (m_uEye.FreezeVideo(uEye.IS_WAIT) != uEye.IS_SUCCESS)
            {
               // System.Windows.Forms.MessageBox.Show("Error freeze image","3nethra");
                return false;
            }

            m_bDrawing = true;
            if (m_uEye.IsOpen())
            {
                int num = 0;
                IntPtr pMem = new IntPtr();
                IntPtr pLast = new IntPtr();

                m_uEye.GetActSeqBuf(ref num, ref pMem, ref pLast);
                if (pLast.ToInt32() == 0)
                {
                    m_bDrawing = false;
                    return false;
                }

                int nLastID = GetImageID(pLast);
                int nLastNum = GetImageNum(pLast);
                m_uEye.LockSeqBuf(nLastNum, pLast);

                IntPtr ppMem = IntPtr.Zero;
                if (m_uEye.GetImageMem(ref ppMem) == uEye.IS_SUCCESS)
                {
                    GetContrast(ppMem, out ContrastVal);
                    //TODO: ((MainForm)cForm).SetContrastStatus(ContrastVal);
                }

                m_uEye.RenderBitmap(nLastID, hWindow.ToInt32(), m_RenderMode);

                m_uEye.UnlockSeqBuf(nLastNum, pLast);
                m_bDrawing = false;
                return true;
            }
            else
            {
                m_bDrawing = false;
                return false;
            }

        }

        // ------------------------  MainForm_Closing  -------------------------------
        //
        /*
        private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (m_uEye.IsOpen())
            {
                // release marshal object pointers
                int nLoop = 0;
                for (nLoop = 0; nLoop < IMAGE_COUNT; nLoop++)
                    Marshal.FreeCoTaskMem(m_UeyeImages[nLoop].pMemory);
                m_uEye.ExitCamera();
            }
        }
        */

        // ------------------------  DisplayWindow_Paint  -------------------------------
        //
        public void Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (!m_bDrawing)
                DrawImage();
        }


        // ------------------------  MainForm_Paint  -------------------------------
        //
        private void MainForm_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
        }

        /*
                // ------------------------  WndProc  -------------------------------
                //
                [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
                protected override void WndProc(ref System.Windows.Forms.Message m)
                {
                    // Listen for operating system messages
                    switch (m.Msg)
                    {
                        // Ueye Message
                        case uEye.IS_UEYE_MESSAGE:
                            HandleUeyeMessage(m.WParam.ToInt32(), m.LParam.ToInt32());
                            break;
                    }
                    base.WndProc(ref m);
                }
        */

        // ------------------------  HandleUeyeMessage  -------------------------------
        //
        public void HandleUeyeMessage(int wParam, int lParam)
        {
            switch (wParam)
            {
                case uEye.IS_FRAME:
                    if (!m_bDrawing)
                        DrawImage();
                    break;

                case uEye.IS_DEVICE_REMOVAL:
                    Close();
                    break;
                case uEye.IS_NEW_DEVICE:
                    //UpdateInfos();
                    if (bAutoConnect)
                        AutoConnect();
                    break;
            }
        }

        public bool AutoConnect()
        {
            Open(local_bin_mode);
            return true;
        }


        // ------------------------  HandleUeyeMessage  -------------------------------
        //
        private void radioNormal_CheckedChanged(object sender, System.EventArgs e)
        {
            // set render mode
            m_RenderMode = uEye.IS_RENDER_NORMAL;
        }

        public double GetContrast(int numpix)
        {
            return 0;
        }

        public bool SetAutoGain(bool bEnable)
        {
            if (bEnable)
            {
                return m_uEye.SetGain(uEye.IS_SET_ENABLE_AUTO_GAIN) == uEye.IS_SUCCESS;
            }
            else
            {
                return m_uEye.SetGain(m_uEye.SetGain(uEye.IS_GET_MASTER_GAIN)) == uEye.IS_SUCCESS;
            }
        }

        public bool SetGain(int val)
        {
            if (val >= 0 && val <= 100)
            {
                return (m_uEye.SetGain(val) == uEye.IS_SUCCESS);
            }
            else
            {
                //invalid gain value... TODO
                return false;
            }
        }

        public bool GetGain(ref int val)
        {
            val = m_uEye.SetGain(uEye.IS_GET_MASTER_GAIN);
            if (val >= 0 && val <= 100)
            {
                return true;
            }
            else
            {
                //invalid gain value... TODO
                return false;
            }
        }

        public bool SetRGBGain(int nRed, int nGreen, int nBlue)
        {
            return m_uEye.SetRGBGain(nRed, nGreen, nBlue);
        }

        public bool GetRGBGain(ref int nRed, ref int nGreen, ref int nBlue)
        {
            return m_uEye.GetRGBGain(ref nRed, ref nGreen, ref nBlue);
        }

        public bool SetFrameRate(double val, ref double newval)
        {
            return m_uEye.SetFrameRate(val, ref newval) == uEye.IS_SUCCESS;
        }

        public bool SetExposure(double val, ref double newval)
        {
            double exp_min = 0, exp_max = 0, exp_iv = 0;

            if (m_uEye.GetExposureRange(ref exp_min, ref exp_max, ref exp_iv) == uEye.IS_SUCCESS)
            {
                if (val > exp_min && val < exp_max)
                {
                    return m_uEye.SetExposureTime(val, ref newval) == uEye.IS_SUCCESS;
                }
            }

            return false;
        }

        public int SetColorCorrection(int nEnable)
        {
            double [] factors = new double[9]{0,0,0,0,0,0,0,0,0};
            return m_uEye.SetColorCorrection(nEnable, factors);
        }

        public int SetEdgeEnhancement(int nEnable)
        {
            return m_uEye.SetEdgeEnhancement(nEnable);
        }

        public int SetGamma(int gamma)
        {
            return m_uEye.SetGamma(gamma);
        }

        public int SetColorConverter(int ColorMode, int ConvertMode)
        {
            return m_uEye.SetColorConverter(ColorMode, ConvertMode);
        }

        public int SetGainBoost(int mode)
        {
            return m_uEye.SetGainBoost(mode);
        }

        /* Legacy
        public bool GetExposure(ref int newval)
        {
            double min = 0, max = 0, intval = 0, newexp = 0;

            m_uEye.GetExposureRange(ref min, ref max, ref intval);

            bool bSuccess = (m_uEye.SetExposureTime(uEye.IS_GET_EXPOSURE_TIME, ref newexp) == uEye.IS_SUCCESS);
            if (bSuccess)
            {
                if (max - min > 0)
                    newval = (int)(100 * (newexp - min) / (max - min));
                else
                    newval = 0;
                return true;
            }
            else return false;
        }*/

        public bool GetFramerate(ref double fps_min, ref double fps_max, ref double fps_iv, ref double fps_cur)
        {
            double ftr_min = 0, ftr_max = 0, ftr_iv = 0;
            if (m_uEye.GetFrameTimeRange(ref ftr_min, ref ftr_max, ref ftr_iv) == uEye.IS_SUCCESS)
            {
                fps_min = 1 / ftr_max;
                fps_max = 1 / ftr_min;
                fps_iv = 1 / ftr_iv;
                return (m_uEye.GetFramesPerSecond(ref fps_cur) == uEye.IS_SUCCESS) ;
            }
            return false;
        }

        public bool GetExposure(ref double exp_min, ref double exp_max, ref double exp_iv, ref double exp_cur)
        {
            if (m_uEye.GetExposureRange(ref exp_min, ref exp_max, ref exp_iv) == uEye.IS_SUCCESS)
            {
                return (m_uEye.SetExposureTime(uEye.IS_GET_EXPOSURE_TIME, ref exp_cur) == uEye.IS_SUCCESS);
            }
            return false;
        }

        public bool GetGlobalExposure(ref long exp_delay, ref long exp_duration)
        {
            return m_uEye.GetGlobalFlashDelay(ref exp_delay, ref exp_duration) == uEye.IS_SUCCESS;
        }

        public bool SetAutoExposure(bool bEnable)
        {
            if (bEnable)
            {
                double dVal2 = 0;
                return m_uEye.SetExposureTime(uEye.IS_SET_ENABLE_AUTO_SHUTTER, ref dVal2) == uEye.IS_SUCCESS;
            }
            else
            {
                double dVal2 = 0;
                m_uEye.SetExposureTime(uEye.IS_GET_EXPOSURE_TIME, ref dVal2);
                return m_uEye.SetExposureTime(dVal2, ref dVal2) == uEye.IS_SUCCESS;
            }
        }

        public bool SetAutoParameters(double brightnessRef,double maxGain)
        {
                //Enable auto gain:
                double enable = 1;
                double d = 0;
                m_uEye.SetAutoParameter(uEye.IS_SET_AUTO_GAIN_MAX, ref maxGain, ref d);
                m_uEye.SetAutoParameter(uEye.IS_SET_ENABLE_AUTO_GAIN, ref enable, ref d);
                m_uEye.SetAutoParameter(uEye.IS_SET_ENABLE_AUTO_SENSOR_GAIN, ref enable, ref d);
                enable = 1;
                m_uEye.SetAutoParameter(uEye.IS_SET_AUTO_REFERENCE, ref brightnessRef, ref d);
                return true;
        }

        public bool IsLiveMode()
        {
            return m_bLive;
        }
        public bool IsOpen()
        {
            return m_bIsConnected;
        }

        public bool SaveImage(String fname)
        {
            byte[] b1 = System.Text.Encoding.UTF8.GetBytes(fname + ".bmp");
            if (m_uEye.SaveImage(b1) == uEye.IS_SUCCESS)
            {
                // hari save only bmp
                return true;

                /*Bitmap bmResult = new Bitmap(fname + ".bmp");
                try
                {
                    bmResult.Save(fname + ".png", System.Drawing.Imaging.ImageFormat.Png);
                }
                catch
                {
                    return false;
                }
                bmResult.Dispose();
                System.IO.File.Delete(fname + ".bmp");
                return true;*/
            }
            return false;
        }

        public bool GetSensorInfo(ref string sensorname)
        {
            uEye.SENSORINFO pSensorInfo = new uEye.SENSORINFO();
            if (m_uEye.GetSensorInfo(ref pSensorInfo) == uEye.IS_SUCCESS)
            {
                sensorname = pSensorInfo.strSensorName;
                return true;
            }
            return false;
        }

        public bool Rotate180(bool bEnable)
        {
            if (bEnable)
            {
                return m_uEye.SetRopEffect(uEye.IS_SET_ROP_MIRROR_LEFTRIGHT | uEye.IS_SET_ROP_MIRROR_UPDOWN, 1, 0) == uEye.IS_SUCCESS;
            }
            else
            {
                return m_uEye.SetRopEffect(uEye.IS_SET_ROP_MIRROR_LEFTRIGHT | uEye.IS_SET_ROP_MIRROR_UPDOWN, 0, 0) == uEye.IS_SUCCESS;
            }
        }

        public int GetImageMem(ref IntPtr ppMem)
        {
            return m_uEye.GetImageMem(ref ppMem);
        }

        public int WriteEEPROM(int Adr, byte[] pcString, int count)
        {
            return m_uEye.WriteEEPROM(Adr, pcString, count);
        }

        // ---------------------  ReadEEPROM  -----------------------
        //
        public int ReadEEPROM(int Adr, byte[] pcString, int count)
        {
            return m_uEye.ReadEEPROM(Adr, pcString, count);
        }
        //For specifying Region of interest 
        public int SetImageAOI(int pXPos, int pYPos, int pWidth, int pHeight)
        {
            return m_uEye.SetImageAOI(ref pXPos, ref pYPos, ref pWidth, ref pHeight);
        }

		public bool SetSupertex(byte val) {
			return m_uEye.SetSupertex(val);
		}

        public bool GetSupertex(byte val)
        {
            return m_uEye.GetSupertex(val);
        }

		public void setLiquidLensLight(Boolean val) {
			m_uEye.SetIOExpanderConfig(0xF0);
			if ( val == true ) {
				m_uEye.SetIOExpanderOutput(0x01, 0x01);
			} else {
				m_uEye.SetIOExpanderOutput(0x00, 0x01);
			}
        }


        // Added by sriram 10 april 2013 to fix TS1X-1107 this is the api for enabling dead pixel correction from the camera.cs.
        public bool setBadPixelCorrection(bool enableCorrection)
        {
            unsafe
            {
                uint size = 4;
                int modeVal = 0;
                IntPtr modePtr = new IntPtr(modeVal);
                int retValue;
                if (enableCorrection)
                    retValue = m_uEye.HotPixel(uEye.IS_HOTPIXEL_ENABLE_CAMERA_CORRECTION, null, sizeof(int));
                // gch.Free();
                else
                    retValue = m_uEye.HotPixel(uEye.IS_HOTPIXEL_DISABLE_CORRECTION, null, sizeof(int));

                if (retValue == uEye.IS_SUCCESS)
                    return true;
                else
                    return false;
            }
        }

        //end uEye stuff
    }
}
