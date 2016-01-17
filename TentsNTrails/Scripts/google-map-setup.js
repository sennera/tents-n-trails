function TntMap(centerLat, centerLong, addMarkersFunction, minZoom, clickToAddMarkers) {
    var markers = [];
    var map;
    var mapCenter = new google.maps.LatLng(centerLat, centerLong);
    console.log("clickToAddMarkers: " + clickToAddMarkers);

    // ************************************
    // Initialize the Map details.
    // ************************************
    function initialize() {
        var mapProp = {
            center: mapCenter,
            zoom: 4,
            mapTypeId: google.maps.MapTypeId.ROADMAP // types include ROADMAP, SATELLITE, HYBRID, and TERRAIN
        };

        // make the map.
        map = new google.maps.Map(document.getElementById("googleMap"), mapProp);

        // add markers to the map
        if (typeof addMarkersFunction != 'undefined') {
            addMarkersFunction(map, markers)
        }

        //fit bounds
        var bounds = new google.maps.LatLngBounds();
        if (markers.length === 0) {
            // ensure map is centered after bounds if no markers.
            bounds.extend(mapCenter);
        }
        else {
            // fit bounds by the markers.
            for (var i=0; i < markers.length; i++) {
                bounds.extend(markers[i].getPosition());
            }
        }
        
        // ensure a minimum zoom level on page load (only once)
        minZoom = typeof minZoom === "undefined" ? 9 : minZoom;
        minZoom = markers.length === 0 ? 4 : minZoom;

        // add a one-time event listener to fix the zoom level.
        google.maps.event.addListener(map, 'zoom_changed', function() {
            zoomChangeBoundsListener = 
                google.maps.event.addListener(map, 'bounds_changed', function(event) {
                    if (this.initialZoom == true && this.getZoom() > minZoom) {
                        // Change max/min zoom here
                        this.setZoom(minZoom);
                        this.initialZoom = false;
                    }
                    google.maps.event.removeListener(zoomChangeBoundsListener);
                });
        });
        map.initialZoom = true;
        map.fitBounds(bounds);

        // add a click listener.
        if (clickToAddMarkers) {
            console.log("add event listener");
            google.maps.event.addListener(map, 'click', function (event) {
                console.log("map click");
                placeMarker(event.latLng);
                var form = document.forms['locationForm'];
                form.elements["Latitude"].value = event.latLng.lat();
                form.elements["Longitude"].value = event.latLng.lng();
            });
        }
        else {
            console.log("did not add event listener");
        }

        // ******************************************************
        // the place marker function used by the click listener.
        // ******************************************************
        function placeMarker(location) {
            var marker = markers[0];
            // remove old marker
            if (marker != null || marker != undefined) {
                marker.setMap(null);
            }
            //  add a new marker
            marker = new google.maps.Marker({
                position: location,
                map: map,
            });
            markers[0] = marker;

            // show an info window with the information.
            var infowindow = new google.maps.InfoWindow({
                content: 'Latitude: ' + location.lat() +
                '<br>Longitude: ' + location.lng()
            });
            infowindow.open(map, marker);
        }
    }

    // **************************************************************************
    // Entry point, create the map.
    // **************************************************************************
    this.create = function () {
        google.maps.event.addDomListener(window, 'load', initialize);
    }
}