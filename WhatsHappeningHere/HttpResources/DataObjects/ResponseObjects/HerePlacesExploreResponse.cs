using System.Collections.Generic;
using WhatsHappeningHere.HttpResources.DataObjects.HelperData;

namespace WhatsHappeningHere.HttpResources.DataObjects.ResponseObjects
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
