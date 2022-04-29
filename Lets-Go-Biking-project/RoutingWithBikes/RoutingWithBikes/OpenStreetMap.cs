using RoutingWithBikes.Models.OSMapItems;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace RoutingWithBikes
{
    public class OpenStreetMap
    {
        private static readonly HttpClient client = new HttpClient();

        public OpenStreetMap(){}

        // retourn les places condidates depuis une adresse
        public async Task<List<Place>> GetPlacesFromAddress(string address)
        {
            string request = "https://nominatim.openstreetmap.org/search?email=ghada.ajimi@etu.univ-cotedazur.fr&format=json&q=" + address;
            try
            {
                Console.WriteLine("[{0}] Requête GET pour récupérer le point le plus proche d'une adresse avec l'URL : \n{1}\n", DateTime.Now, request);
                HttpResponseMessage response = await client.GetAsync(request);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<List<Place>>(responseBody);
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }
    }
}
