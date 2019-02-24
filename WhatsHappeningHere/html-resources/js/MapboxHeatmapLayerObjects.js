const metersToPixelsAtMaxZoom = (meters, latitude) =>
    meters / 0.075 / Math.cos(latitude * Math.PI / 180);

var trafficFlowLineLayer = {
    "id": "trafficFlow-lines",
    "type": "line",
    "source": "trafficFlow",
    "minzoom": 11,
    /*
    "layout": {
        "line-cap": "", // "butt"(default) | "round" | "square"
        "line-join": "", // "bevel" | "round" | "miter"(default)
        "line-miter-limit": 2, // 2=default (requires line-join=miter)
        "line-round-limit": 1.05, // 1.05=default (requires line-join=round)
        "visibility": "", // "none" | "visible"(default)

    },
    "paint": {
        "line-opacity": 1, // number [0,1] default=1, transitionable
        "line-color": "#000000", // color default="#000000", disabled by line-pattern, transitionable
        "line-translate": [0, 0], // array of numbers in pixels, default=[0,0], [x,y] to offset geometry, transitionable
        "line-translate-anchor": "map", // "map"(default) | "viewport", requires line-translate (frame of reference)
        "line-width": 1, // number >= 0, in pixels, default=1, transitionable
        "line-gap-width": 0, // number >= 0, in pixels, default=0, transitionable
        "line-offset": 0, // default=0, in pixels, transitionable
        "line-blur": 0, // >=0, pixels, default=0, transitionable
        "line-dasharray": [], // array of numbers >= 0, units in "line widths", disabled by line-pattern, transitionable
        "line-pattern": "", // name of image in sprite to use for drawing image lines, transitionable
                            // for seamless patterns, image width must be a foctor of 2 (2, 4, 8, ..., 512)
                            // zoom-level expressions only evaluated at integer zoom levels
        "line-gradient": "" // color type (a color in the sRGB color space)
    }
     */
    "paint": {
        "line-width": [
            "interpolate",
            ["linear"],
            ["zoom"],
            11, 1,
            22, 25
        ],
        "line-color":
        //["let", "jFact", ["get", "jamFactor"],
            [
                "interpolate",
                ["linear"],
                //["case",
                //    ["<=", ["var", "jFact"], 0.0],
                //    0.0, ["var", "jFact"]
                //],
                ["get", "jamFactor"],
                -1, "rgba(0,0,0,0)",
                0, "rgba(33,102,172,0)",
                //1, "",
                2, "rgb(103,169,207)",
                //3, "",
                4, "rgb(209,229,240)",
                //5, "",
                6, "rgb(253,219,199)",
                //7, "",
                8, "rgb(239,138,98)",
                //9, "",
                10, "rgb(178,24,43)"
            ]
        //]
    },
    "layout": {
        "line-cap": "round",
        //"line-join": "round",
        //"line-miter-limit": 1
    }




    ///////////////////////////////////////////////////////////////////////////




    //"paint": {
        // Size circle radius by earthquake magnitude and zoom level
        /*
        "circle-radius": [
            "interpolate",
            ["linear"],
            ["zoom"],
            7, [
                "interpolate",
                ["linear"],
                ["get", "jamFactor"],
                1, 1,
                6, 4
            ],
            16, [
                "interpolate",
                ["linear"],
                ["get", "jamFactor"],
                1, 5,
                6, 50
            ]
        ],
        // Color circle by earthquake magnitude
        "circle-color": [
            "interpolate",
            ["linear"],
            ["get", "jamFactor"],
            1, "rgba(33,102,172,0)",
            2, "rgb(103,169,207)",
            3, "rgb(209,229,240)",
            4, "rgb(253,219,199)",
            5, "rgb(239,138,98)",
            6, "rgb(178,24,43)"
        ],
        "circle-stroke-color": "white",
        "circle-stroke-width": 1,
        // Transition from heatmap to circle layer by zoom level
        "circle-opacity": [
            "interpolate",
            ["linear"],
            ["zoom"],
            7, 0,
            8, 1
        ]
        */
    //}
}




/*
var lineLayer = {
    "id": "",
    "source": "",
    "type": "line",
    //"minzoom": number [0,24]
    //"maxzoom": number [0,24]
    "layout": {
        "line-cap": "", // "butt"(default) | "round" | "square"
        "line-join": "", // "bevel" | "round" | "miter"(default) 
        "line-miter-limit": 2, // 2=default (requires line-join=miter)
        "line-round-limit": 1.05, // 1.05=default (requires line-join=round)
        "visibility": "", // "none" | "visible"(default)

    },
    "paint": {
        "line-opacity": 1, // number [0,1] default=1, transitionable
        "line-color": "#000000", // color default="#000000", disabled by line-pattern, transitionable
        "line-translate": [0, 0], // array of numbers in pixels, default=[0,0], [x,y] to offset geometry, transitionable
        "line-translate-anchor": "map", // "map"(default) | "viewport", requires line-translate (frame of reference)
        "line-width": 1, // number >= 0, in pixels, default=1, transitionable
        "line-gap-width": 0, // number >= 0, in pixels, default=0, transitionable
        "line-offset": 0, // default=0, in pixels, transitionable
        "line-blur": 0, // >=0, pixels, default=0, transitionable
        "line-dasharray": [], // array of numbers >= 0, units in "line widths", disabled by line-pattern, transitionable
        "line-pattern": "", // name of image in sprite to use for drawing image lines, transitionable
                            // for seamless patterns, image width must be a foctor of 2 (2, 4, 8, ..., 512)
                            // zoom-level expressions only evaluated at integer zoom levels
        "line-gradient": "" // color type (a color in the sRGB color space)
    }
};



var heatLayer = {
    "id": "",
    "source": "",
    "type": "heatmap",
    //"minzoom": number [0,24]
    //"maxzoom": number [0,24]
    "layout": {
        "visibility": "visible" // "visible"(default) | "none"
    },
    "paint": {
        "heatmap-radius": 30, // number >= 1 in pixels default=30, transitionable
                                // radius of influence of one heatmap point in pixels, increasing the value
                                // makes heatmap smoother, but less detailed
        "heatmap-weight": 1, // number >= 0, how much an individual point contributes to the heatmap
                                // e.g. 10 is like having 10 points of weight 1 in the same spot, good when
                                //      combined with clustering
        "heatmap-intensity": 1, // number >= 0, default=1, like heatmap-weight, but controls intensity globally,
                                        // good for adjusting the heatmap based on zoom level, transitionable

        "heatmap-color": [      // <---- default value. defines the color of each pixel vased on its density value
            "interpolate",          // should be expression that uses ["heatmap-density"] as input
            ["linear"],
            ["heatmap-density"],
            0, "rgba(0, 0, 255, 0)",
            0.1, "royalblue",
            0.3, "cyan",
            0.5, "lime",
            0.7, "yellow",
            1, "red"
        ],

        "heatmap-opacity": 1 // number [0,1] default=1, transitionable
    }
};
*/

// any layout or paint property (or filter) can be specified as an "expression"