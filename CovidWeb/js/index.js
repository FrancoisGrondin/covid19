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
    $.ajax ({
        url:"http://192.168.0.104:8000",
        method:"POST",
        data:'{"action":"map"}',
        success:function(response)
        {
            response.dots.forEach(dot => add_marker(dot.latitude, dot.longitude, dot.contracted));
        },
        error:function(response)
        {
            console.log("ERREUR");
        }
    });
}


function add_marker(latitude, longitude, contracted = false)
{
    if (contracted) {
        icon = 'images/red.png'
    }
    else {
        icon = 'images/blue.png'
    }

    // show a marker on the map
    var dot = new L.Icon({
        iconUrl: icon,
        iconSize: [5, 5],
        iconAnchor: [5, 5],
        popupAnchor: [5, 5],
        shadowSize: [0, 0]
    });
            
    var marker = L.marker([latitude, longitude], {icon: dot}).bindPopup(latitude + ' ' + longitude);
    marker.addTo(MAP);
}        
