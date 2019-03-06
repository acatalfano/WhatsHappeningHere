using System.Collections.Generic;

namespace WhatsHappeningHere.HttpResources.DataObjects.ResponseObjects
{
    public class RoadwayData
    {
        public double Confidence { get; set; }
        public double JamFactor { get; set; }
        public double Length { get; set; } = -1.0;
        public int PointTMCLocationCode { get; set; } = -1;
        public string RoadSegmentName { get; set; }
        public string QueuingDirection { get; set; }
        public List<double[]> Shape { get; set; } = new List<double[]>();
    }
}
