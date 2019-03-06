using System;
using CefSharp.WinForms;

using WhatsHappeningHere.HttpResources;
using WhatsHappeningHere.HttpResources.JsonObjects;
using CefSharp;
using System.Collections.Generic;
using WhatsHappeningHere.HttpResources.Requests;
using WhatsHappeningHere.HttpResources.HelperData;

namespace WhatsHappeningHere
{
    public class JavascriptBoundClass
    {
        private HereTrafficFlowRequest TrafficFlowRequest = new HereTrafficFlowRequest();

        private MapboxStyleRequest StyleRequest = new MapboxStyleRequest();
        
        private ParseHereSearchResponse _searchObject = new ParseHereSearchResponse();

        private static ChromiumWebBrowser _instanceBrowser = null;
        private static Form1 _instanceForm = null;
        


        public JavascriptBoundClass(ChromiumWebBrowser originalBrowser, Form1 originalForm)
        {
            _instanceBrowser = originalBrowser;
            _instanceForm = originalForm;
        }


        // Make a GET request to the Mapbox Styles API to retrieve the style JSON
        // return the JSON value as a string
        // (the result is parsed as a JSON object in JavaScript,
        //  then assigned to the map object from Mapbox GL JavaScript)
        //
        // Note: needs to remain non-static for Javascript Binding
        public string GetMapboxStyle(string styleID, string accessToken) =>
            StyleRequest.MakeRequest(styleID, accessToken);


        public void HandleOnLoadEvent()
        {
            BoundingBox bbox = GetCurrentBBox();

            if (BoundsInvalid(bbox))
            {
                return;
            }

            string geojsonString = GenerateTrafficFlowGeoJson(bbox);

            _instanceBrowser.ExecuteScriptAsync("addSourceAndLayer", geojsonString);

            RunSearchAndPlaceMarkers(bbox);
        }



        public void HandleOnMoveEndEvent()
        {
            BoundingBox bbox = GetCurrentBBox();

            if (BoundsInvalid(bbox))
            {
                return;
            }

            string geojsonString = GenerateTrafficFlowGeoJson(bbox);
            
            _instanceBrowser.ExecuteScriptAsync(string.Format(@"
                (function(){{
                    var layerRemoved = false;
                    while(!layerRemoved) {{
                        try {{
                            map.removeLayer('trafficFlow-lines');
                            layerRemoved = true;
                        }}
                        catch(err) {{ }}
                    }}
                    
                    map.removeSource('trafficFlow');
                    addSourceAndLayer(JSON.stringify({0}));
                }})();", geojsonString));

            RunSearchAndPlaceMarkers(bbox);
        }



        private static bool BoundsInvalid(BoundingBox bbox) =>
            (Math.Abs(bbox.NW.Latitude - bbox.SE.Latitude) > 1.0) ||
            (Math.Abs(bbox.NW.Longitude - bbox.SE.Longitude) > 1.0);

        
        private void RunSearchAndPlaceMarkers(BoundingBox bbox)
        {
            // remove all current markers and popups, so new ones can be added
            _instanceBrowser.ExecuteScriptAsync(@"
                (function(){{
                    if(mapboxMarkerArray.length != mapboxPopupArray.length){{
                        mapboxMarkerArray = [];
                        mapboxPopupArray = [];
                        return;
                    }}
                    while(mapboxMarkerArray.length > 0) {{
                        mapboxMarkerArray.pop().remove();
                        mapboxPopupArray.pop().remove();
                    }}
                }})();");


            _searchObject.SetSearchBBox(bbox);
            _searchObject.PerformSearch();

            foreach (HerePlacesItem result in _searchObject.SearchResponse)
            {
                var innerText = result.Name;
                var iconName = result.Icon;
                var latitude = result.Latitude;
                var longitude = result.Longitude;

                _instanceBrowser.ExecuteScriptAsync("addMarkerAndPopup", innerText, iconName, longitude, latitude);
            }
        }


        private BoundingBox GetCurrentBBox()
        {
            JavascriptResponse scriptResponse =
                _instanceBrowser.EvaluateScriptAsync(methodName: "getBBoxParams").GetAwaiter().GetResult();

            var result = (IDictionary<string, object>)scriptResponse.Result;
            var northwest = (IDictionary<string, object>)result["NW"];
            var southeast = (IDictionary<string, object>)result["SE"];


            var coords = new BoundingBox
            {
                NW = new Coordinates
                {
                    Latitude = Convert.ToDouble(northwest["lat"].ToString()),
                    Longitude = Convert.ToDouble(northwest["lng"].ToString())
                },
                SE = new Coordinates
                {
                    Latitude = Convert.ToDouble(southeast["lat"].ToString()),
                    Longitude = Convert.ToDouble(southeast["lng"].ToString())
                }
            };

            return coords;
        }




        private string GenerateTrafficFlowGeoJson(BoundingBox bbox)
        {
            TrafficFlowRequest.BBox = bbox;
            string responseXML = TrafficFlowRequest.MakeRequest();

            List<TrafficParseData> trafficFlowData = HereTrafficToMapboxLayer.ParseTrafficData(responseXML);
            string geoJson = HereTrafficToMapboxLayer.GenerateGeoJSON(trafficFlowData);

            return geoJson;
        }
    }
}
