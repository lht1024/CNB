using CNB.ViewModels;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CNB
{
    public class LNewsContentRaw
    {
        public NewsContentRaw result { get; set; }
        public string status { get; set; }
    }

    public class NewsContentRaw
    {
        public string aid { get; set; }
        public string bad { get; set; }
        public string bodytext { get; set; }
        public string catid { get; set; }
        public string collectnum { get; set; }
        public string comments { get; set; }
        public string counter { get; set; }
        public string good { get; set; }
        public string hometext { get; set; }
        public string mview { get; set; }
        public string ratings { get; set; }
        public string ratings_story { get; set; }
        public string score { get; set; }
        public string score_story { get; set; }
        public string sid { get; set; }
        public string source { get; set; }
        public string time { get; set; }
        public string title { get; set; }
        public string topic { get; set; }
    }

    public class NewsDetailProxy
    {
        public async static Task<LNewsContentRaw> GetNewsDetail(string sid)
        {
            var timstamp = ComputeMD5.GetTimeStop();
            var toBehashed = String.Format("app_key=10000&format=json&method=Article.NewsContent&sid={0}&timestamp={1}&v=1.0&mpuffgvbvbttn3Rc", sid, timstamp);
            var Md5 = ComputeMD5.GetMD5(toBehashed);
            var mylink = String.Format("http://api.cnbeta.com/capi?app_key=10000&format=json&method=Article.NewsContent&sid={0}&timestamp={1}&v=1.0&sign={2}", sid, timstamp, Md5);

            var http = new HttpClient();
            var response = await http.GetAsync(mylink);
            var result = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<LNewsContentRaw>(result);
            return data;
        }
    }
}