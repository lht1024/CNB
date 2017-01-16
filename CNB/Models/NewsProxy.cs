using CNB.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CNB
{
    public class LNewsRaw
    {
        public List<NewsRaw> result { get; set; }
        public string status { get; set; }
    }

    public class NewsProxy
    {
        public async static Task<LNewsRaw> GetNews(string sid)
        {
            var timstamp = ComputeMD5.GetTimeStop();
            string mylink;
            if (sid == "")
            {
                var toBehashed = String.Format("app_key=10000&format=json&method=Article.Lists&timestamp={0}&v=1.0&mpuffgvbvbttn3Rc", timstamp);
                var Md5 = ComputeMD5.GetMD5(toBehashed);
                mylink = String.Format("http://api.cnbeta.com/capi?app_key=10000&format=json&method=Article.Lists&timestamp={0}&v=1.0&sign={1}", timstamp, Md5);
            }
            else
            {
                var toBehashed = String.Format("app_key=10000&end_sid={0}&format=json&method=Article.Lists&timestamp={1}&topicid=null&v=1.0&mpuffgvbvbttn3Rc", sid, timstamp);
                var Md5 = ComputeMD5.GetMD5(toBehashed);
                mylink = String.Format("http://api.cnbeta.com/capi?app_key=10000&end_sid={0}&format=json&method=Article.Lists&timestamp={1}&topicid=null&v=1.0&sign={2}", sid, timstamp, Md5);
            }
            

            var http = new HttpClient();
            var response = await http.GetAsync(mylink);
            var result = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<LNewsRaw>(result);
            return data;
        }
    }

    public class NewsRaw
    {
        public string comments { get; set; }
        public string counter { get; set; }
        public string pubtime { get; set; }
        public string sid { get; set; }
        public string summary { get; set; }
        public string thumb { get; set; }
        public string title { get; set; }
        public string topic_logo { get; set; }
    }
}