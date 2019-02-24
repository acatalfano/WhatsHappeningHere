using System;
using System.Linq;
using System.Text;

namespace WhatsHappeningHere.HttpResources.QueryStringObjects
{
    public class HerePlacesExploreRequest
    {
        private const string _endpoint = "places/v1/discover/explore";

        // TODO: maybe simplify which categories to use, later
        private readonly string[] _validCategories =
            {   "eat-drink",                        "restaurant",                       "coffee-tea",
                "snacks-fast-food",                 "bar-club",                         "coffee",
                "tea",                              "going-out",                        "sights-museums",
                "transport",                        "airport",                          "accomodation",
                "shopping",                         "leisure-outdoor",                  "administrative-areas-buildings",
                "natural-geographical",             "petrol-station",                   "atm-bank-exchange",
                "toilet-rest-area",                 "hospital-healthcare-facility"  };

        private string[] _categories;
        /*
         *  "eat-drink",                        "restaurant",                       "coffee-tea",
            (restaurant)                        (restaurant)                        (cafe)

            "snacks-fast-food",                 "bar-club",                         "coffee",
            (fast-food)                         (bar)                               (cafe)

            "tea",                              "going-out",                        "sights-museums",
            (cafe)                              (bar)                               (museum)

            "transport",                        "airport",                          "accomodation",
            (bus)                               (airport)                           (lodging)

            "shopping",                         "leisure-outdoor",                  "administrative-areas-buildings",
            (shop)                              (natural)                           (building)
            
            "natural-geographical",             "petrol-station",                   "atm-bank-exchange",
            (natural)                           (fuel)                              (bank)
            
            "toilet-rest-area",                 "hospital-healthcare-facility"
            (toilet)                            (hospital)

            else: (marker)
         * */
        public string[] Categories
        {
            get => _categories;
            // throws ArgumentException if any element of "value" is not in "_validCategories"
            set
            {
                // loop over each element of value and ensure that each is a valid category
                if (_categories != null)
                {
                    foreach (var val in value)
                    {
                        if (!_validCategories.Contains(val))
                            throw new ArgumentException("Invalid Category");
                    }
                }

                // if no exception thrown, then assign the new value
                _categories = value;
            }
        }
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
        private string QueryString()
        {
            StringBuilder url = new StringBuilder("?");
            
            // append the circle-radius search format
            url.Append($"in={West},{South},{East},{North}&");

            // if any categories are specified, append "cat=<comma-separated categories>&"
            if (Categories != null && Categories.Length != 0)
            {
                url.Append("cat=");

                string last = Categories.Last();
                foreach (string cat in Categories)
                {
                    url.Append(cat +
                        ((cat == last) ? '&' : ','));
                }
            }

            // append credentials
            url.Append($"app_id={HereHttpClient.appID}&app_code={HereHttpClient.appCode}");

            return url.ToString();
        }
    }
}
