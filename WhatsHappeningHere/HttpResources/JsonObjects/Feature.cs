using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsHappeningHere.HttpResources.JsonObjects
{
    public class Feature
    {
        public PropertyData Properties { get; set; }
        public GeoData Geometry { get; set; }
    }


    public class PropertyData
    {
        public double Confidence { get; set; }
        public double JamFactor { get; set; }
        public double Length { get; set; } = -1.0;
        public int PointTMCLocationCode { get; set; } = -1;
        public string RoadSegmentName { get; set; }
        public string QueuingDirection { get; set; }
        public string RoadName { get; set; }
        public string LinearIdentifier { get; set; }
    }

    public class GeoData
    {
        [JsonProperty]
        public const string Type = "LineString";
        public List<double[]> Coordinates { get; set; }
    }
}
