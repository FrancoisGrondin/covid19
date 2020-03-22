var MAP;


$(document).ready(function()
{
    initialize();
    query_and_plot_data();
});


function initialize()
{
    // initialize Leaflet
    MAP = L.map('map').setView([45.51997, -73.61624], 15);

    // add the OpenStreetMap tiles
    L.tileLayer('https://{s}.tile.openstreetmap.fr/hot/{z}/{x}/{y}.png', {
        maxZoom: 19,
        attribution: '&copy; <a href="https://openstreetmap.org/copyright">OpenStreetMap contributors</a>'
    }).addTo(MAP);

    // show the scale bar on the lower left corner
    L.control.scale().addTo(MAP);
}


function query_and_plot_data()
{
    // TEMPORARY CODE TO SHOW THE POTENTIAL
    // TODO -- Replace by a query to the API
    /* Possible JSON return from API:
       {
           [
               {
                   lat: 45.51997,
                   lon: -73.61624,
                   contracted: true
               },
               {
                   lat: 45.41263,
                   lon: -73.85912,
                   contracted: true
               },
               ...
           ]
       }        
    */
    lat = 45.51997;
    lon = -73.61624;
    for (let step = 0; step < 500; step++) {
        add_marker((lat + (Math.random() / 100)), (lon + (Math.random() / 100)), (Math.random() > 0.5));
    }
}


function add_marker(latitude, longitude, contracted = false)
{
    if (contracted) {
        icon = 'red.png'
    }
    else {
        icon = 'blue.png'
    }

    // show a marker on the map
    var dot = new L.Icon({
        iconUrl: icon,
        iconSize: [10, 10],
        iconAnchor: [5, 5],
        popupAnchor: [5, 5],
        shadowSize: [0, 0]
    });
            
    var marker = L.marker([latitude, longitude], {icon: dot}).bindPopup(latitude + ' ' + longitude);
    marker.addTo(MAP);
}        
