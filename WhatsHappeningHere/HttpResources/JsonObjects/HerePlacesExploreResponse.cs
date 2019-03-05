using System.Collections.Generic;
using WhatsHappeningHere.HttpResources.HelperData;

namespace WhatsHappeningHere.HttpResources.JsonObjects
{
    public class HerePlacesExploreResponse
    {
        public List<HerePlacesItem> Results { get; set; }

        public string NextUrl { get; set; }

        public System.Collections.IEnumerator GetEnumerator()
        {
            return Results.GetEnumerator();
        }
    }
}
