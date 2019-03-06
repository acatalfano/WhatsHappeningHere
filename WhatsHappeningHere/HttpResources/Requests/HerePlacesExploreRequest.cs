/*using WhatsHappeningHere.HttpResources.Clients;

namespace WhatsHappeningHere.HttpResources.Requests
{
    public class HerePlacesExploreRequest
    {
        private const string _endpoint = "places/v1/discover/explore";
        
        public double West { get; set; }
        public double South { get; set; }
        public double East { get; set; }
        public double North { get; set; }
        
        public HerePlacesExploreRequest( double _west, double _south, double _east, double _north)
        {
            West = _west;
            South = _south;
            East = _east;
            North = _north;
        }
        
        // return the entire request URL: "{baseAddress}{endpoint}{queryString}"
        public string RequestUrl() => HereHttpClient.PlacesClient.BaseAddress.ToString() + _endpoint + QueryString();
        
        // build and return the query portion of the url
        // i.e. the entire suffix beginning with "?" and followed by all the GET parameters
        private string QueryString() => $"?in={West},{South},{East},{North}" + HereHttpClient.CredentialsUrlSegment();
    }
}
*/