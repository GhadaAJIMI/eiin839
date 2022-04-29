using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebProxy
{
    internal class JCDecauxItem
    {
        protected HttpClient req = new HttpClient();
        protected string objects = "";

        public JCDecauxItem() { }

        public JCDecauxItem(string url)
        {
            Task<HttpResponseMessage> res = req.GetAsync(url);
            objects = res.Result.Content.ReadAsStringAsync().Result.ToString();
        }

        public string getObjects() { return objects; }
    }
}
