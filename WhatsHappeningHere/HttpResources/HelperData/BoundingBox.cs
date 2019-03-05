namespace WhatsHappeningHere.HttpResources.HelperData
{
    public class Coordinates
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class BoundingBox
    {
        public Coordinates NW { get; set; }
        public Coordinates SE { get; set; }
    }
}
