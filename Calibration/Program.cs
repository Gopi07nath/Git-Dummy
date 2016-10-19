using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace Calibration
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        public static calibrationUI _currentInstance;

        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += Current_UnhandleException;
            System.Windows.Forms.Application.ThreadException += Application_ThreadException;
            //try
            //{
            bool createdNew = true;
            using (Mutex mutex = new Mutex(true, "Calibration", out createdNew))
            {
                if (createdNew)
                {
                    Application.EnableVisualStyles();
#if !DEBUG
                        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);
#endif

                    Application.SetCompatibleTextRenderingDefault(false);
                    try
                    {
                        _currentInstance = new calibrationUI();
                        Application.Run(_currentInstance);
                    }
                    catch (Exception ex)
                    {
                        //STRING_Program_061
                        System.IO.StreamWriter sw = new System.IO.StreamWriter("Calibration.log", true);

                        string exception = DateTime.Now.ToString() + Environment.NewLine;
                        exception += ex.Message + Environment.NewLine;
                        exception += ex.StackTrace + Environment.NewLine;
                        sw.WriteLine(exception);
                        sw.Close();
                        MessageBox.Show(System.IO.Directory.GetCurrentDirectory());
                        MessageBox.Show("Program.cs: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Application is Already Running", "CalibrationTool", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        public static void Current_UnhandleException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            File.AppendAllText(Globals.LogFilePath, ex.StackTrace);
        }

        public static void Application_ThreadException(object sender, EventArgs e)
        {

        }
        
    }
}
