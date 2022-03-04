using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Device.Location;

namespace Client_REST_Console
{
    internal class Position
    {
        public float lat { get; set; }
        public float lng { get; set; }

        public Position(string latitude, string longitude)
        {
            this.lat = float.Parse(latitude);
            this.lng = float.Parse(longitude);
        }

        public string toString()
        {
            return "latitude: "+lat+ ", longitude: "+lng+"\n";
        }
    }

    internal class Availabilitie
    {
        public int bikes { get; set; }
        public int stands { get; set; }
        public int mechanicalBikes { get; set; }
        public int electricalBikes { get; set; }
        public int electricalInternalBatteryBikes { get; set; }
        public int electricalRemovableBatteryBikes { get; set; }

        public string toString()
        {
            string result = "bikes: "+bikes+ "\nstands: "+stands+"\nmechanicalBikes: "+mechanicalBikes +"\n";
            result += "electricalBikes: "+electricalBikes+ "\nelectricalInternalBatteryBikes: "+electricalInternalBatteryBikes+"\nelectricalRemovableBatteryBikes: "+electricalRemovableBatteryBikes +"\n";
            return result;
        }
    }

    internal class Stand
    {
        public Availabilitie availabilities { get; set; }
        public int capacity { get; set; }

        public string toString()
        {
            return "availabilities: "+availabilities+ "\ncapacity: "+capacity+"\n"; ;
        }
    }

    internal class Station
    {
        public int number { get; set; }
        public string contract_name { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public Position position { get; set; }
        public Boolean banking { get; set; }
        public Boolean bonus { get; set; }
        public string status { get; set; }
        public int last_update { get; set; }
        //public Boolean connected { get; set; }
        //public Boolean overflow { get; set; }
        //public object shape { get; set; }
        public int bike_stands { get; set; }
        public int available_bike_stands { get; set; }
        public int available_bikes { get; set; }
        //public int overflowStands { get; set; }

        public string toString()
        {
            string result = "number: "+number+ "\ncontract_name: "+contract_name+"\nname: "+name +"\n";
            result += "address: "+address+ "\nposition: "+position+"\nbanking: "+banking +"\n";
            result += "bonus: "+bonus+ "\nstatus: "+status+"\nlast_update: "+last_update +"\n";
            //result += "connected: "+connected+ "\noverflow: "+overflow+"\nshape: "+shape +"\noverflowStands: "+overflowStands +"\n";
            result += "available_bike_stands: "+available_bike_stands+ "\navailable_bikes: "+available_bikes+"\n";
            return result;
        }
    }

    internal class Contract
    {
        public string name { get; set; }
        public string commercial_name { get; set; }
        public string country_code { get; set; }
        public string[] cities { get; set; }

        public string toString() {
            string result = "name: "+name+ "\ncommercial_name: "+commercial_name+"\ncountry_code: "+country_code +"\n";
            if (cities!=null)
            {
                result += "Cities:\n";
                foreach (string city in cities)
                {
                    result += "  - " + city + "\n";
                }
            }
            return result; 
        }
    }


    internal class Parks
    {
        public string contractName { get; set; }
        public string name { get; set; }
        public int number { get; set; }
        public string status { get; set; }
        public Position position { get; set; }
        public string accessType { get; set; }
        public string lockerType { get; set; }
        public Boolean hasSurveillance { get; set; }
        public Boolean isFree { get; set; }
        public string address { get; set; }
        public string zipCode { get; set; }
        public string city { get; set; }
        public Boolean isOffStreet { get; set; }
        public Boolean hasElectricSupport { get; set; }
        public Boolean hasPhysicalReception { get; set; }
    }

    internal class Program
    {
        static readonly HttpClient client = new HttpClient();

        static void Main(string[] args)
        {
            var myKey = "6bf7031c1935ec3f072187151623d0838c2c20a1";

            Console.WriteLine("La liste des contrats est : \n");
            getListeContrats(myKey).Wait();

            Console.WriteLine("Veuillez choisir un contrat\n");
            string myContract = Console.ReadLine();
            getListeStations(myKey, myContract).Wait();

            Console.WriteLine("Veuillez choisir le numero de la station à consulter\n");
            string num = Console.ReadLine();
            getStationInfo(myKey, myContract, num).Wait();

            Console.WriteLine("Veuillez choisir un contrat\n");
            myContract = Console.ReadLine();

            Console.WriteLine("Veuillez choisir la position GPS\nLatitude: ");
            string latitude = Console.ReadLine();

            Console.WriteLine("Langitude: ");
            string langitude = Console.ReadLine();
            getClosestStation(myKey, myContract, new Position(latitude, langitude)).Wait();

            Console.ReadLine();
        }

        static async Task getListeContrats(string key)
        {
            var path = "https://api.jcdecaux.com/vls/v1/contracts?apiKey=" + key;

            try
            {
                HttpResponseMessage response = await client.GetAsync(path);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Contract[] listContracts = JsonSerializer.Deserialize<Contract[]>(responseBody);
                foreach (Contract contract in listContracts)
                {
                    Console.WriteLine(contract.name);
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }

        static async Task getListeStations(string key, string contrat)
        {
            var path = "https://api.jcdecaux.com/vls/v1/stations?contract=" + contrat + "&apiKey=" + key;

            try
            {
                HttpResponseMessage response = await client.GetAsync(path);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Station[] listStations = JsonSerializer.Deserialize<Station[]>(responseBody);
                foreach (Station station in listStations)
                {
                    Console.WriteLine(station.number);
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }

        static async Task getStationInfo(string key, string contrat, string num)
        {
            var path = "https://api.jcdecaux.com/vls/v1/stations/"+num+"?contract=" + contrat + "&apiKey=" + key;

            try
            {
                HttpResponseMessage response = await client.GetAsync(path);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Station stationInfo = JsonSerializer.Deserialize<Station>(responseBody);
                Console.WriteLine(stationInfo.toString());
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }

        static async Task getClosestStation(string key, string contrat, Position position)
        {
            var path = "https://api.jcdecaux.com/vls/v1/stations?contract=" + contrat + "&apiKey=" + key;
            GeoCoordinate x = new GeoCoordinate(position.lat, position.lng);

            try
            {
                HttpResponseMessage response = await client.GetAsync(path);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                Station[] listeStations = JsonSerializer.Deserialize<Station[]>(responseBody);

                double min = double.PositiveInfinity;
                int j = 0;

                for (int i = 0; i < listeStations.Length; i++)
                {
                    GeoCoordinate y = new GeoCoordinate(listeStations[i].position.lat, listeStations[i].position.lng);
                    double d = y.GetDistanceTo(x);
                    if (d < min)
                    {
                        min = d;
                        j = i;
                    }
                }

                Console.WriteLine(listeStations[j]);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }
    }
}