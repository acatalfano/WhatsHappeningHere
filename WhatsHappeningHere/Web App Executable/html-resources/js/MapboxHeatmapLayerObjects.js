const metersToPixelsAtMaxZoom = (meters, latitude) =>
    meters / 0.075 / Math.cos(latitude * Math.PI / 180);

var trafficFlowLineLayer = {
    "id": "trafficFlow-lines",
    "type": "line",
    "source": "trafficFlow",
    "minzoom": 11,
    "paint": {
        "line-width": [
            "interpolate",
            ["linear"],
            ["zoom"],
            11, 1,
            22, 25
        ],
        "line-color": [
            "interpolate",
            ["linear"],
            ["get", "jamFactor"],
            -1, "rgba(0,0,0,0)",
            0, "rgba(33,102,172,0)",
            2, "rgb(103,169,207)",
            4, "rgb(209,229,240)",
            6, "rgb(253,219,199)",
            8, "rgb(239,138,98)",
            10, "rgb(178,24,43)"
        ]
    },
    "layout": {
        "line-cap": "round"
    }
}