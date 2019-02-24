using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace WhatsHappeningHere.HttpResources
{
    public class HereSearch
    {
        // flag used to ensure that the search parameters are either set or refreshed
        // before the search request is performed
        private bool _searchReady = false;
        // object internally storing the request data
        private QueryStringObjects.HerePlacesExploreRequest _requestData;

        // response property
        public JsonObjects.HerePlacesExploreResponse SearchResponse { get; set; }
        

        // assign search request parameters
        // marks the search as ready to be performed
        public void SetSearchRequest(double lngWest, double latSouth, double lngEast, double latNorth)
        {
            _requestData = new QueryStringObjects.HerePlacesExploreRequest(
                _west: lngWest,
                _south: latSouth,
                _east: lngEast,
                _north: latNorth);

            _searchReady = true;
        }


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

            string reqURL = _requestData.RequestUrl();
            
            //SearchResponse = RetrieveResponse(reqURL).GetAwaiter().GetResult();
            var resp = RetrieveResponse(reqURL);
            SearchResponse = resp.GetAwaiter().GetResult();

            //SearchResponse = RetrieveResponse(_requestData.RequestUrl()).GetAwaiter().GetResult();

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
