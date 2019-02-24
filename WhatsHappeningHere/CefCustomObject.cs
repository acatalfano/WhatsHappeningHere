using System.Diagnostics;

using CefSharp;
using CefSharp.WinForms;

namespace WhatsHappeningHere
{
    class CefCustomObject
    {
        private static ChromiumWebBrowser _instanceBrowser = null;
        private static Form1 _instanceMainForm = null;


        public CefCustomObject(ChromiumWebBrowser originalBrowser, Form1 mainForm)
        {
            _instanceBrowser = originalBrowser;
            _instanceMainForm = mainForm;
        }

        public void ShowDevTools() => _instanceBrowser.ShowDevTools();

        public void Opencmd()
        {
            ProcessStartInfo start = new ProcessStartInfo("cmd.exe", "/c pause");
            Process.Start(start);
        }

        public void Refresh() => _instanceBrowser.Reload(true);
    }
}
