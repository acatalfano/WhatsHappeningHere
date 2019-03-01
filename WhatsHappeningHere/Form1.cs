using System;
using System.Windows.Forms;
using System.IO;

using CefSharp;
using CefSharp.WinForms;

namespace WhatsHappeningHere
{
    public partial class Form1 : Form
    {
        internal ChromiumWebBrowser browser;
        
        public Form1()
        {
            InitializeComponent();

            FormBorderStyle = FormBorderStyle.FixedToolWindow;

            InitializeChromium();

            // When Javascript requests a promise object named jsBoundObject
            // register it as an instance of JavascriptBoundClass
            browser.JavascriptObjectRepository.ResolveObject += (sender, e) =>
            {
                var repo = e.ObjectRepository;
                if (e.ObjectName == "jsBoundObject")
                {
                    repo.Register(
                        "jsBoundObject",
                        new JavascriptBoundClass(browser, this),
                        isAsync: true,
                        options: BindingOptions.DefaultBinder   );
                }
            };
        }
        

        public void InitializeChromium()
        {
            string directory = AppDomain.CurrentDomain.BaseDirectory;
            string page = string.Format(@"{0}\html-resources\html\mapbox.html", directory);

            
            if(!File.Exists(page))
            {
                MessageBox.Show("Error The html file doesn't exist : " + page);
            }
            
            // Create a browser component
            browser = new ChromiumWebBrowser(page);

            // Add it to the form and fill it to the form window
            Controls.Add(browser);
            browser.Dock = DockStyle.Fill;

            // Allow the use of local resources in the browser
            BrowserSettings browserSettings = new BrowserSettings
            {
                FileAccessFromFileUrls = CefState.Enabled,
                UniversalAccessFromFileUrls = CefState.Enabled,
                WebSecurity = CefState.Disabled
            };
            browser.BrowserSettings = browserSettings;
        }

        private void Form1_FormClosing(object sender, FormClosedEventArgs e)
        {
            Cef.Shutdown();
        }
    }
}
