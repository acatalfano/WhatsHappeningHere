﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace WhatsHappeningHere.HttpResources.JsonObjects
{
    internal class GeoJsonTraffic
    {
        [JsonProperty]
        public const string Type = "FeatureCollection";
        public List<Feature> Features { get; set; }
    }
}