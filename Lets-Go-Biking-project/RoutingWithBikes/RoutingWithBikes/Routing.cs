using RoutingWithBikes.Models.JCDcauxModels;
using RoutingWithBikes.Models.OSMapItems;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Web;
using System.Text.Json;

namespace RoutingWithBikes
{
    public class Routing : IRouting
    {
        private WebProxy.RoutingServiceClient client;
        protected string city;
        protected HttpClient ht = new HttpClient();
        private readonly OpenStreetMap openStreetMapNomatim = new OpenStreetMap();
        private readonly OpenRouteService openRouteService = new OpenRouteService();

        private List<Station> allStations;

        private static readonly int THRESHOLD_AVAILABLE_BIKES = 2;
        private static readonly int THRESHOLD_AVAILABLE_BIKES_STANDS = 2;

        public Routing()
        { 
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            this.client = new WebProxy.RoutingServiceClient();
            this.allStations = getAllStations().ToList();
        }

        public Station[] getListeStations(string contrat)
        {
            try
            {
                String responseBody = client.getListeStations(contrat);
                Station[] listeStations = JsonSerializer.Deserialize<Station[]>(responseBody);
                return listeStations;
            }
            catch (Exception e)
            {
                throw new Exception();
            }
        }

        public Station getStationInfo(string contrat, int num)
        {
            try
            {
                String responseBody = client.getStationInfo(contrat, num.ToString());
                Station station = JsonSerializer.Deserialize<Station>(responseBody);
                return station;
            }
            catch (Exception e)
            {
                throw new Exception();
            }
        }

        // récupérer la position de la place la plus importante depuis une adresse
        public Position GetPosition(string address)
        {
            address = address.Trim();
            if (address.Equals("null") || address.Equals(string.Empty))
            {
                return null;
            }

            List<Place> places = openStreetMapNomatim.GetPlacesFromAddress(address).Result;
            Place bestPlace = null;
            double importance = double.MinValue;

            foreach (Place place in places)
            {
                if (place.importance > importance)
                {
                    bestPlace = place;
                    importance = place.importance;
                }
            }

            return (bestPlace == null) ? null : new Position
            {
                lat = float.Parse(bestPlace.lat, new System.Globalization.CultureInfo("en-US")),
                lng = float.Parse(bestPlace.lon, new System.Globalization.CultureInfo("en-US"))
            };
        }

        public GeoJson[] GetPath(Position[] positions)
        {
            GeoJson cyclingRegularPath;
            GeoJson footWalkingPath;
            GeoJson footWalkingPath2;

            if (Array.Exists(positions, position => position == null))
            {
                return null;
            }

            if(positions.Length == 2)
            {
                // parcour à pied
                footWalkingPath = GetPath(positions, "foot-walking");
                return new GeoJson[] { footWalkingPath };
            }
            else
            {
                Console.WriteLine("parcour à vélo");
                footWalkingPath = GetPath(new Position[] { positions[0], positions[1] }, "foot-walking");
                cyclingRegularPath = GetPath(new Position[] { positions[1], positions[2] }, "cycling-regular");
                footWalkingPath2 = GetPath(new Position[] { positions[2], positions[3] }, "foot-walking");
            }
            return new GeoJson[] { footWalkingPath, cyclingRegularPath, footWalkingPath2 };
        }

        public Station getStation1(double latitude, double longitude)
        {
            GeoCoordinate userPosition = new GeoCoordinate(latitude, longitude);
            Station nearestStation = null;
            double distance = double.MaxValue;

            foreach (Station station in allStations)
            {
                if (userPosition.GetDistanceTo(new GeoCoordinate(station.position.lat, station.position.lng)) < distance)
                {
                    Station potentialNearestStation = getStationInfo(station.contract_name, station.number);
                    if (potentialNearestStation.available_bikes >= THRESHOLD_AVAILABLE_BIKES && potentialNearestStation.status.Equals("OPEN"))
                    {
                        nearestStation = potentialNearestStation;
                        distance = userPosition.GetDistanceTo(new GeoCoordinate(potentialNearestStation.position.lat, potentialNearestStation.position.lng));
                    }
                }
            }

            return nearestStation;
        }

        public Station getStation2(double latitude, double longitude)
        {
            GeoCoordinate userPosition = new GeoCoordinate(latitude, longitude);
            Station nearestStation = null;
            double distance = double.MaxValue;

            foreach (Station station in allStations)
            {
                if (userPosition.GetDistanceTo(new GeoCoordinate(station.position.lat, station.position.lng)) < distance)
                {
                    Station potentialNearestStation = getStationInfo(station.contract_name, station.number);
                    if (potentialNearestStation.available_bike_stands >= THRESHOLD_AVAILABLE_BIKES_STANDS && potentialNearestStation.status.Equals("OPEN"))
                    {
                        nearestStation = potentialNearestStation;
                        distance = userPosition.GetDistanceTo(new GeoCoordinate(potentialNearestStation.position.lat, potentialNearestStation.position.lng));
                    }
                }
            }

            return nearestStation;
        }

        private GeoJson GetPath(Position[] positions, string profile)
        {
            string result = openRouteService.PostDirections(positions, profile).Result;
            return JsonSerializer.Deserialize<GeoJson>(result);
        }

        public void PathOptions()
        {
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Methods", "POST");
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
        }

        public Station[] getAllStations()
        {
            try
            {
                String responseBody = client.getAllStations();
                Station[] listeStations = JsonSerializer.Deserialize<Station[]>(responseBody);
                return listeStations;
            }
            catch (Exception e)
            {
                throw new Exception();
            }
        }
    }
}
