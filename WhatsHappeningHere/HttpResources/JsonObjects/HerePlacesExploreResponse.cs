using System.Collections.Generic;

namespace WhatsHappeningHere.HttpResources.JsonObjects
{

    // TODO maybe don't use indexer and instead implement System.Collections.IEnumerable
    //              or System.Collections.Generic.IEnumerable<T>

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
