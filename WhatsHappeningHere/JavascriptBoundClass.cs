using System;
using CefSharp.WinForms;
using RestSharp;

using WhatsHappeningHere.HttpResources;
using WhatsHappeningHere.HttpResources.JsonObjects;
using CefSharp;
using System.Collections.Generic;

namespace WhatsHappeningHere
{
    public class JavascriptBoundClass
    {
        class BBoxData
        {
            public class Coordinates
            {
                public double Latitude { get; set; }
                public double Longitude { get; set; }
            }

            public Coordinates NW { get; set; }
            public Coordinates SE { get; set; }
        }



        private class HereAPICredentials
        {
            public string AppID { get; set; }
            public string AppCode { get; set; }
        }

        private readonly HereAPICredentials _hereCredentials =
            new HereAPICredentials
            {
                AppID = "EVeUmiv6qrPEaoKK9zNI",
                AppCode = "Elq5ypOXQKI9VMQUOUE01w"
            };

        

        
        private HereSearch _searchObject = new HereSearch();

        private static ChromiumWebBrowser _instanceBrowser = null;
        private static Form1 _instanceForm = null;

        public JavascriptBoundClass()
        {

        }


        public JavascriptBoundClass(ChromiumWebBrowser originalBrowser, Form1 originalForm)
        {
            _instanceBrowser = originalBrowser;
            _instanceForm = originalForm;
        }
        

        // Make a GET request to the Mapbox Styles API to retrieve the style JSON
        // return the JSON value as a string
        // (the result is parsed as a JSON object in JavaScript,
        //  then assigned to the map object from Mapbox GL JavaScript)
        public string GetMapboxStyle(string styleID, string accessToken)
        {
            var client = new RestClient(@"https://api.mapbox.com");
            
            var request = new RestRequest("styles/v1/{username}/{style_id}", Method.GET);
            request.AddUrlSegment("username", "acatalfano");
            request.AddUrlSegment("style_id", styleID);
            request.AddParameter("access_token", accessToken);

            var response = client.ExecuteTaskAsync(request).GetAwaiter().GetResult();

            return response.Content;
        }


        public void HandleOnLoadEvent()
        {
            BBoxData bbox = GetCurrentBBox();

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
            BBoxData bbox = GetCurrentBBox();

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



        private bool BoundsInvalid(BBoxData bbox) =>
            (Math.Abs(bbox.NW.Latitude - bbox.SE.Latitude) > 1.0) ||
            (Math.Abs(bbox.NW.Longitude - bbox.SE.Longitude) > 1.0);

        
        private void RunSearchAndPlaceMarkers(BBoxData bbox)
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


            _searchObject.SetSearchRequest(lngWest: bbox.NW.Longitude,
                                            latSouth: bbox.SE.Latitude,
                                            lngEast: bbox.SE.Longitude,
                                            latNorth: bbox.NW.Latitude);
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


        private BBoxData GetCurrentBBox()
        {
            JavascriptResponse scriptResponse =
                _instanceBrowser.EvaluateScriptAsync(methodName: "getBBoxParams").GetAwaiter().GetResult();

            var result = (IDictionary<string, object>)scriptResponse.Result;
            var northwest = (IDictionary<string, object>)result["NW"];
            var southeast = (IDictionary<string, object>)result["SE"];


            var coords = new BBoxData
            {
                NW = new BBoxData.Coordinates
                {
                    Latitude = Convert.ToDouble(northwest["lat"].ToString()),
                    Longitude = Convert.ToDouble(northwest["lng"].ToString())
                },
                SE = new BBoxData.Coordinates
                {
                    Latitude = Convert.ToDouble(southeast["lat"].ToString()),
                    Longitude = Convert.ToDouble(southeast["lng"].ToString())
                }
            };

            return coords;
        }




        private string GenerateTrafficFlowGeoJson(BBoxData bbox)
        {
            string responseXML = GetTrafficXMLFromHereAPI(bbox.NW.Latitude, bbox.NW.Longitude, bbox.SE.Latitude, bbox.SE.Longitude);
            List<TrafficParseData> trafficFlowData = HereTrafficToMapboxLayer.ParseTrafficData(responseXML);
            string geoJson = HereTrafficToMapboxLayer.GenerateGeoJSON(trafficFlowData);

            return geoJson;
        }

        
        private string GetTrafficXMLFromHereAPI(double lat1, double lng1, double lat2, double lng2)
        {
            
            var client = new RestClient(@"https://traffic.api.here.com");

            var request = new RestRequest("traffic/6.2/flow.xml", Method.GET);
            request.AddParameter("app_id", _hereCredentials.AppID);
            request.AddParameter("app_code", _hereCredentials.AppCode);
            request.AddParameter("bbox", $"{lat1},{lng1};{lat2},{lng2}");
            request.AddParameter("responseattributes", "shape");
            request.AddParameter("units", "imperial");
            
            var response = client.ExecuteTaskAsync(request).GetAwaiter().GetResult();

            return response.Content;
            
        }
        
    }
}
