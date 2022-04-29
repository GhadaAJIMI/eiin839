"use strict";

const API = Object.freeze("http://localhost:8084/Design_Time_Addresses/RoutingWithBikes/REST/");

const p0 = Object.freeze({
    lat: 43.604,
    lng: 1.44305
}); // Position de Toulouse

const fromIcon = Object.freeze(new L.Icon({
    iconUrl: "resources/marker-from-icon.png",
    iconSize: [41, 41],
    iconAnchor: [12, 41],
    popupAnchor: [1, -34]
}));

const toIcon = Object.freeze(new L.Icon({
    iconUrl: "resources/marker-from-icon.png",
    iconSize: [41, 41],
    iconAnchor: [12, 41],
    popupAnchor: [1, -34]
}));

let map = L.map("map", { worldCopyJump: true });
let pathLayer = L.layerGroup();

let p1;
let p2;

let stations;

let s1;
let s2;

let d1;
let d2;
let d3;

window.onload = () => {
    map = map.setView([p0.lat, p0.lng], 12);
    initMap(map);
}

const getAllStations = () => {
    const targetUrl = API + "getstations";
    const requestType = "GET";

    function callback() {
        stations = JSON.parse(this.responseText);
        showAllStations();
    }

    request(targetUrl, requestType, callback);
}

const showAllStations = () => {
    let markers = L.markerClusterGroup();

    stations.forEach(station => {
        markers.addLayer(
            L.marker([station.position.lat, station.position.lng], { title: station.name })
                .bindPopup(`<b>` + station.name + `</b>`)
        );
    });
    map.addLayer(markers);
}

const initMap = (map) => {
    getAllStations();

    L.tileLayer("http://{s}.tile.openstreetmap.fr/osmfr/{z}/{x}/{y}.png", {
        attribution: "Map data &copy; <a href=\"https://www.openstreetmap.org/copyright\">OpenStreetMap</a> contributors",
        minZoom: 1,
        maxZoom: 17
    }).addTo(map);

    L.control.scale().addTo(map);

    map.addLayer(pathLayer);

    // créer l'input de l'adresse de départ en haut à droite
    L.Control.geocoder({
        position: "topright",
        collapsed: false,
        placeholder: "Adresse de départ",
        defaultMarkGeocode: false
    }).on("markgeocode", (event) => {
        const center = event.geocode.center;
        pathLayer.addLayer(L
            .marker(center,{
                icon: fromIcon,
                title: event.geocode.name})
            .bindPopup(event.geocode.html)
        );
        map.fitBounds(event.geocode.bbox);
        p1 = {
            lat: center.lat,
            lng: center.lng
        };
        getStation1(p1);
      })
      .addTo(map);

    // créer l'input de la destination en haut à droite
    L.Control.geocoder({
        position: "topright",
        collapsed: false,
        placeholder: "Destination",
        defaultMarkGeocode: false
    }).on("markgeocode", (event) => {
        const center = event.geocode.center;
        pathLayer.addLayer(L
            .marker(center, {
                icon: toIcon,
                title: event.geocode.name
            })
            .bindPopup(event.geocode.html)
        );
        map.fitBounds(event.geocode.bbox);
        p2 = {
            lat: center.lat,
            lng: center.lng
        };
        getStation2(p2);
      })
      .addTo(map);

    // créer le bouton corbeille
    L.easyButton("<img src=\"resources/remove.png\" alt=\"Remove Paths\">", () => {
        p1 = null;
        p2 = null;

        s1 = null;
        s2 = null;

        pathLayer.clearLayers();

        const inputs = document.getElementsByTagName("input");
        for (let i = 0; i < inputs.length; i++) {
            if (inputs[i].type === "text") {
                inputs[i].value = "";
            }
        }
    }, "Réinitialiser").addTo(map);
}

function sqr(a) {
    return a * a;
}

function Distance(p1, p2) {
    return Math.sqrt(sqr(p2.lng - p1.lng) + sqr(p2.lat - p1.lat));
}

const getPath = () => {
    const targetUrl = API + "path";
    const requestType = "POST";
    let positions;

    d1 = d2 + d3 + Distance(s1, s2);
    if (d2 > d1 || d3 > d1) {
        // parcour à pied
        positions = [p1, p2];
    } else {
        // parcour mixte
        positions = [p1, s1, s2, p2];
    }

    const data = "{\"positions\": " + JSON.stringify(positions) + "}";

    function callback() {
        if (this.readyState === XMLHttpRequest.DONE && this.status === 200) {
            const geoJSON = JSON.parse(this.responseText);
            for (let i = 0; i < geoJSON.length; i++) {
                let color;
                if (i == 0 || i == 2) {
                    // le parcour à pied est en rouge
                    color = '#DC143C';
                }
                else {
                    // le parcour à velo est en bleu
                    color = '#0000FF';
                }
                pathLayer.addLayer(L.geoJSON(geoJSON[i], { color: color }));
                map.fitBounds(L.geoJSON(geoJSON[i]).getBounds());
            }
        }
    }

    var t0 = performance.now();
    request(targetUrl, requestType, callback, data);
    var t1 = performance.now();
    console.log("L'appel de getPath a demandé " + (t1 - t0) + " millisecondes.");
}

const getStation1 = (p1) => {
    const targetUrl = API + "getStation1?lat=" + p1.lat + "&lng=" + p1.lng;
    const requestType = "GET";

    function callback() {
        if (this.responseText) {
            const station = JSON.parse(this.responseText);
            s1 = station.position;
            d2 = Distance(p1, s1);
        }
        if (s1 && s2) {
            getPath();
        }
    }

    request(targetUrl, requestType, callback);
}

const getStation2 = (p2) => {
    const targetUrl = API + "getStation2?lat=" + p2.lat + "&lng=" + p2.lng;
    const requestType = "GET";

    function callback() {
        if (this.responseText) {
            const station = JSON.parse(this.responseText);
            s2 = station.position;
            d3 = Distance(p2, s2);
        }
        if (s1 && s2) {
            getPath();
        }
    }

    request(targetUrl, requestType, callback);
}

const request = (targetUrl, requestType, callback, data = null) => {
    const caller = new XMLHttpRequest();

    caller.open(requestType, targetUrl, true);

    if (requestType === "GET") {
        caller.setRequestHeader("Accept", "application/json");
        caller.onload = callback;
        caller.send();
    }
    else {
        caller.setRequestHeader("Content-Type", "application/json");
        caller.onreadystatechange = callback;
        caller.send(data);
    }
}