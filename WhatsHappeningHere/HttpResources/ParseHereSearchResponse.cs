using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WhatsHappeningHere.HttpResources.HelperData;
using WhatsHappeningHere.HttpResources.Clients;

namespace WhatsHappeningHere.HttpResources
{
    public class ParseHereSearchResponse
    {
        private const string _endpoint = "places/v1/discover/explore";
        
        public double West { get; set; }
        public double South { get; set; }
        public double East { get; set; }
        public double North { get; set; }
        
        // assign bounding box search area and mark the search as ready to be performed
        public void SetSearchBBox(BoundingBox bbox)
        {
            West = bbox.NW.Longitude;
            South = bbox.SE.Latitude;
            East = bbox.SE.Longitude;
            North = bbox.NW.Latitude;

            _searchReady = true;
        }

        // return the entire request URL: "{baseAddress}{endpoint}{queryString}"
        public string RequestUrl() => HereHttpClient.PlacesClient.BaseAddress.ToString() + _endpoint + QueryString();

        // build and return the query portion of the url
        // i.e. the entire suffix beginning with "?" and followed by all the GET parameters
        private string QueryString() => $"?in={West},{South},{East},{North}" + HereHttpClient.CredentialsUrlSegment();


        // flag used to ensure that the search parameters are either set or refreshed
        // before the search request is performed
        private bool _searchReady = false;

        // response property
        public JsonObjects.HerePlacesExploreResponse SearchResponse { get; set; }
        
        // if the search is ready to be performed,
        //      makes the async call to the RetrieveResponse(string) method
        //          to set the SearchResponse property
        //      and unset the _searchReady flag
        // if the search is not ready,
        //      throws InvalidOperationException
        public void PerformSearch()
        {
            // check that the SetSearchRequest() method has been called since the last call to RetrieveResponse
            if (!_searchReady)
                throw new InvalidOperationException(
                    "Attempted a search request before refreshing or assigning the search parameters");
            
            
            var resp = RetrieveResponse(RequestUrl());
            SearchResponse = resp.GetAwaiter().GetResult();
            
            _searchReady = false;
        }


        
        // async helper method to perform the search request
        private async Task<JsonObjects.HerePlacesExploreResponse> RetrieveResponse(string requestUrl)
        {
            string responseContent = await HereHttpClient.PlacesClient.GetStringAsync(requestUrl);

            var jsonParse = JObject.Parse(responseContent);
            var query =
                from item in jsonParse["results"]["items"]
                select new HerePlacesItem
                {
                    Latitude    = (double)item["position"][0],
                    Longitude   = (double)item["position"][1],
                    Name        = (string)item["title"],
                    Distance    = (double)item["distance"],
                    Category    = (string)item["category"]["title"],
                    Icon        = (string)item["icon"]
                };

            return new JsonObjects.HerePlacesExploreResponse
            {
                Results = query.ToList(),
                NextUrl = (string)jsonParse["results"]["next"]
            };
        }
    }
}
