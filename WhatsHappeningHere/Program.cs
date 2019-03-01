using System;
using System.IO;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;

namespace WhatsHappeningHere
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            CefSharpSettings.SubprocessExitIfParentProcessClosed = true;

            Cef.EnableHighDPISupport();

            var settings = new CefSettings()
            {
                CachePath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "CefSharp\\Cache"),
                BrowserSubprocessPath = @"x86\CefSharp.BrowserSubprocess.exe"
            };

            Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
