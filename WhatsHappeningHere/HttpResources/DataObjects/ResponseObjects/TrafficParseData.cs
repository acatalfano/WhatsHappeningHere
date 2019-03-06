using System.Collections.Generic;

namespace WhatsHappeningHere.HttpResources.DataObjects.ResponseObjects
{
    public class TrafficParseData
    {
        public string LinearIdentifier { get; set; }
        public string RoadName { get; set; }
        public List<RoadwayData> FIList { get; set; }
    }
}
