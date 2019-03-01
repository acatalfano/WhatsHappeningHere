﻿using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Xml;

namespace WhatsHappeningHere.HttpResources.JsonObjects
{
    public static class HereTrafficToMapboxLayer
    {
        public static string GenerateGeoJSON(List<TrafficParseData> data)
        {
            var jsonQuery =
                from datum in data
                from FI in datum.FIList
                select new Feature
                {
                    Properties = new PropertyData
                    {
                        Confidence = FI.Confidence,
                        JamFactor = FI.JamFactor,
                        PointTMCLocationCode = FI.PointTMCLocationCode,
                        Length = FI.Length,
                        RoadSegmentName = FI.RoadSegmentName,
                        QueuingDirection = FI.QueuingDirection,
                        RoadName = datum.RoadName,
                        LinearIdentifier = datum.LinearIdentifier
                    },
                    Geometry = new GeoData
                    {
                        Coordinates = (FI.Shape != null) ? new List<double[]>(FI.Shape) : null
                    }
                };
            
            GeoJsonTraffic geoJsonData = new GeoJsonTraffic
            {
                Features = jsonQuery.ToList()
            };

            return
                JsonConvert.SerializeObject(
                    geoJsonData,
                    new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }
                );
        }




        public static List<TrafficParseData> ParseTrafficData(string trafficFlowXML)
        {
            using(StringReader trafficXmlReader = new StringReader(trafficFlowXML))
            {
                XElement root;
                try
                {
                    root = XElement.Load(trafficXmlReader);
                }
                catch(XmlException e)
                {
                    return new List<TrafficParseData>();
                }
                XNamespace hereTrafficNamespace = root.Attribute("xmlns").Value;

                var trafficQuery =
                    from rw in root.Descendants(hereTrafficNamespace + "RW")
                    let fi = rw.Descendants(hereTrafficNamespace + "FI")
                    let tmc = fi.Descendants(hereTrafficNamespace + "TMC").FirstOrDefault()
                    let cf = fi.Descendants(hereTrafficNamespace + "CF")
                    select new TrafficParseData
                    {
                        RoadName = rw?.Attribute("DE")?.Value,
                        LinearIdentifier = rw?.Attribute("LI")?.Value,
                        FIList =
                            (
                                from fi in rw.Descendants(hereTrafficNamespace + "FI")
                                let tmc = fi.Descendants(hereTrafficNamespace + "TMC").FirstOrDefault()
                                let cf = fi.Descendants(hereTrafficNamespace + "CF")

                                let shapeEnumerable =
                                    from shpElements in fi.Descendants(hereTrafficNamespace + "SHP")
                                    let shpList = shpElements?.DescendantNodes().OfType<XText>()
                                    select string.Join(" ", shpList) into shpLongString
                                    select shpLongString?.Trim().Split(' ')
                                                                .Where(token => !string.IsNullOrEmpty(token))
                                                                .Select(token => token.Split(','))
                                                                .Select(
                                                                    coords => new double[]
                                                                                {
                                                                                    // swap latitude/longitude order to prepare for GeoJSON format
                                                                                    double.Parse(coords[1]),
                                                                                    double.Parse(coords[0])
                                                                                }
                                                                )?.ToList() // select coords

                                select new RoadwayData
                                {
                                    PointTMCLocationCode = int.Parse(tmc?.Attribute("PC")?.Value ?? "-1"),
                                    RoadSegmentName = tmc?.Attribute("DE")?.Value,
                                    QueuingDirection = tmc?.Attribute("QD").Value,
                                    Length = double.Parse(tmc?.Attribute("LE")?.Value ?? "-1.0"),

                                    JamFactor = cf.Select(x => double.Parse(x.Attribute("JF").Value)).Average(),
                                    Confidence = cf.Select(x => double.Parse(x.Attribute("CN").Value)).Average(),

                                    Shape = shapeEnumerable?.SelectMany(element => element).ToList()
                                } // select new
                            ).ToList() // FIList 
                    };

                return trafficQuery.ToList();
            }
        }
    }
}
