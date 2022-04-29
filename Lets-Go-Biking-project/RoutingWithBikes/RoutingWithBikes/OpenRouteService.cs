using RoutingWithBikes.Models.JCDcauxModels;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RoutingWithBikes
{
    public class OpenRouteService
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly string API_KEY = "5b3ce3597851110001cf6248cf83d88051484f5cb2ee20351a89cec4";

        public OpenRouteService(){}

        public async Task<string> PostDirections(string request, string data)
        {
            try
            {
                Console.WriteLine("[{0}] Requête POST vers OpenRouteService avec l'URL : {1} et les données : {2}\n", DateTime.Now, request, data);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpRequestMessage httpRequest = new HttpRequestMessage()
                {
                    RequestUri = new Uri(request),
                    Method = HttpMethod.Post,
                    Content = content
                };
                httpRequest.Headers.Add("Authorization", API_KEY);

                HttpResponseMessage response = await client.SendAsync(httpRequest);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        public async Task<string> PostDirections(Position[] positions, string profile)
        {
            string request = "https://api.openrouteservice.org/v2/directions/" + profile + "/geojson";
            return await PostDirections(request, BuildDataForPOSTCall(positions));
        }

        private string BuildDataForPOSTCall(Position[] positions)
        {
            string data = "{\"coordinates\":[";

            foreach (Position position in positions)
            {
                data += "[" + position.lng.ToString().Replace(",", ".") + "," + position.lat.ToString().Replace(",", ".") + "],";
            }

            data += "],\"instructions\":\"true\",\"language\":\"fr\",\"preference\":\"shortest\",\"units\":\"m\"}";
            data = data.Replace("],],", "]],");
            return data;
        }
    }
}
