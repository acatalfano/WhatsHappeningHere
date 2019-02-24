using System;
using System.IO;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using System.Xml.Linq;
using System.Linq;

namespace WhatsHappeningHere
{
    static class Program
    {
        /*
        class LonLat
        {
            public string Latitude { get; set; }
            public string Longitude { get; set; }
        }
        */
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
            

            XElement root = XElement.Load(@"C:\Users\Adam\Desktop\inlineTempOut.xml");

            XNamespace ns = root.Attribute("xmlns").Value;

            /*
             var trafficQuery =
                    from rws in root.Elements()
                    let fi = rws.Descendants(hereTrafficNamespace + "FI").FirstOrDefault()
                    let text = fi?.Descendants(hereTrafficNamespace + "SHP").FirstOrDefault()?.DescendantNodes().OfType<XText>()
                    let cf = fi?.Descendants(hereTrafficNamespace + "CF").FirstOrDefault()
                    select new TrafficParseData
                    {
                        // -1.0 if the default "unknown value" for confidence and jamFactor
                        // as per the XML Schema used by Here API for Traffic Data
                        Confidence = double.Parse(cf?.Attribute("CN")?.Value ?? "-1.0"),
                        JamFactor = double.Parse(cf?.Attribute("JF")?.Value ?? "-1.0"),
                        Shape = text?.FirstOrDefault()?.Value.Trim().Split(' ')
                                                        .Where(x => !string.IsNullOrEmpty(x))
                                                        .Select(x => x.Split(','))
                                                        .Select(x => new double[]
                                                        {
                                                            double.Parse(x[1]),
                                                            double.Parse(x[0])
                                                        })
                                                        .ToList()
                    };
             */




            /*from JavascripBoundClass.cs:
             
             var trafficQuery =
                    from rws in root.Elements()
                    let fi = rws.Descendants(hereTrafficNamespace + "FI").FirstOrDefault()
                    let text = fi?.Descendants(hereTrafficNamespace + "SHP").FirstOrDefault()?.DescendantNodes().OfType<XText>()
                    let cf = fi?.Descendants(hereTrafficNamespace + "CF").FirstOrDefault()
                    select new TrafficParseData
                    {
                        // -1.0 if the default "unknown value" for confidence and jamFactor
                        // as per the XML Schema used by Here API for Traffic Data
                        Confidence = double.Parse(cf?.Attribute("CN")?.Value ?? "-1.0"),
                        JamFactor = double.Parse(cf?.Attribute("JF")?.Value ?? "-1.0"),
                        Shape = text?.FirstOrDefault()?.Value.Trim().Split(' ')
                                                        .Where(x => !string.IsNullOrEmpty(x))
                                                        .Select(x => x.Split(','))
                                                        .Select(x => new double[]
                                                        {
                                                            double.Parse(x[1]),
                                                            double.Parse(x[0])
                                                        })
                                                        .ToList()
                    };
            */

            var query =
                from rw in root.Descendants(ns + "RW")
                let fi = rw.Descendants(ns + "FI")
                let tmc = fi.Descendants(ns + "TMC").FirstOrDefault()
                let cf = fi.Descendants(ns + "CF")
                select new
                {
                    RoadName = rw.Attribute("DE").Value,
                    LinearIdentifier = rw.Attribute("LI").Value,
                    FIList = 
                        (
                            from fi in rw.Descendants(ns + "FI")
                            let tmc             = fi.Descendants(ns + "TMC").FirstOrDefault()
                            let cf              = fi.Descendants(ns + "CF")

                            let shapeEnumerable =
                                from shpElements in fi.Descendants(ns + "SHP")
                                let shpList = shpElements.DescendantNodes().OfType<XText>()
                                select string.Join(" ", shpList) into shpLongString
                                select shpLongString.Trim().Split(' ')
                                                            .Where(token => !string.IsNullOrEmpty(token))
                                                            .Select(token => token.Split(','))
                                                            .Select(
                                                                coords => new double[]
                                                                            {
                                                                                double.Parse(coords[1]),
                                                                                double.Parse(coords[0])
                                                                            }
                                                            ).ToList() // select coords
                                                               
                            select new
                            {
                                PointTMCLocationCode    = int.Parse(tmc?.Attribute("PC").Value ?? "-1"),
                                RoadSegmentName         = tmc?.Attribute("DE").Value,
                                QueuingDirection        = tmc?.Attribute("QD").Value,
                                Length                  = double.Parse(tmc?.Attribute("LE").Value ?? "-1.0"),        // TODO: maybe don't need this one

                                JamFactor               = cf.Select(x => double.Parse(x.Attribute("JF").Value)).Average(),
                                Confidence              = cf.Select(x => double.Parse(x.Attribute("CN").Value)).Average(),

                                Shape                   = shapeEnumerable.SelectMany(element => element).ToList()
                            } // select new
                        ).ToList() // FIList 
                };
            var lst = query.ToList();
            
            /*
            let fi = rw.Descendants(ns + "FI")
                let tmc = fi.Descendants(ns + "TMC").FirstOrDefault()
                let cf = fi.Descendants(ns + "CF")*/
            /*
            var query =
                from rw in root.Descendants(ns + "RW")
                select new
                {
                    RoadName = rw.Attribute("DE").Value,
                    LinearIdentifier = rw.Attribute("LI").Value,
                    FIList = (from fi in rw.Descendants(ns + "FI")
                              let tmc = fi.Descendants(ns + "TMC").FirstOrDefault()
                              let cf = fi.Descendants(ns + "CF")

                              //let shpList = fi.Descendants(ns + "SHP").SelectMany(x => x.DescendantNodes().OfType<XText>()).ToList()
                              select new
                              {
                                  PointTMCLocationCode = tmc?.Attribute("PC").Value,
                                  RoadSegmentName = tmc?.Attribute("DE").Value,
                                  QueuingDirection = tmc?.Attribute("QD").Value,
                                  Length = tmc?.Attribute("LE").Value,        // TODO: maybe don't need this one

                                  JamFactor = cf.Select(x => double.Parse(x.Attribute("JF").Value)).Average(),
                                  Confidence = cf.Select(x => double.Parse(x.Attribute("CN").Value)).Average(),
                                  
                                  Shape = (from shpList in fi.Descendants(ns + "SHP").Select(x => x.DescendantNodes().OfType<XText>()).ToList()
                                          select string.Join(" ", shpList) into shpLongString
                                          select shpLongString.Trim().Split(' ')
                                                                        .Where(token => !string.IsNullOrEmpty(token))
                                                                        .Select(token => token.Split(','))
                                                                        .Select(coords => new double[]
                                                                        {
                                                                            double.Parse(coords[1]),
                                                                            double.Parse(coords[0])
                                                                        }).ToList() ).ToList()
                              }).ToList() // FIList - select new
                }; // query - select new
            var lst = query.ToList();
            */
            /*
           var query =
               from rws in root.Elements(ns + "RWS")
               select rws.Descendants(ns + "RW") into rw
               select rw.Descendants(ns + "FI").ToList() into fi
               select fi.Descendants(ns + "SHP").SelectMany(x => x.DescendantNodes().OfType<XText>()).ToList() into shpList
               select string.Join(" ", shpList) into shpLongString
               select shpLongString.Trim().Split(' ')
                                           .Where(x => !string.IsNullOrEmpty(x))
                                           .Select(x => x.Split(','))
                                           .Select(x => new double[]
                                           {
                                               double.Parse(x[1]),
                                               double.Parse(x[0])
                                           })
                                           .ToList();

           var lst = query.ToList();
           */


            // TODO TODO TODO: finish testing the query (now that the namespace is all handled)
            //          NOTE: only need to namespace-qualify element names, not attribute names
            //      ALSO: update it in JavascriptBoundClass.cs when you're done!


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
