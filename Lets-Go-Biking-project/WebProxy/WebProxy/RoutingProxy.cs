namespace WebProxy
{
    public class RoutingProxy : IRoutingService
    {
        public string myKey = "6bf7031c1935ec3f072187151623d0838c2c20a1";
        private static ProxyCache<JCDecauxItem> cache = null;

        private double date = 900.0;

        public RoutingProxy()
        {
            if (cache == null) cache = new ProxyCache<JCDecauxItem>();
        }

        public string getStationInfo(string contrat, string num)
        {
            string path = "https://api.jcdecaux.com/vls/v1/stations/"+num+"?contract=" + contrat + "&apiKey=" + myKey;
            return cache.Get(path, date).getObjects();
        }

        public string getAllStations()
        {
            string path = "https://api.jcdecaux.com/vls/v1/stations?apiKey=" + myKey;
            return cache.Get(path, date).getObjects();
        }
    }
}
