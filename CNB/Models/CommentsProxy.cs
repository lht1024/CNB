using CNB.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CNB
{
    public class CommentsProxy
    {
        public async static Task<LCommentsRaw> GetResults(string page, string article_id)
        {
            var timstamp = ComputeMD5.GetTimeStop();
            var toBehashed = String.Format("app_key=10000&format=json&method=Article.Comment&page={0}&sid={1}&timestamp={2}&v=1.0&mpuffgvbvbttn3Rc", page, article_id, timstamp);
            var Md5 = ComputeMD5.GetMD5(toBehashed);
            var mylink = String.Format("http://api.cnbeta.com/capi?app_key=10000&format=json&method=Article.Comment&page={0}&sid={1}&timestamp={2}&v=1.0&sign={3}", page, article_id, timstamp, Md5);

            var http = new HttpClient();
            var response = await http.GetAsync(mylink);
            var result = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<LCommentsRaw>(result);
            return data;
        }

        public class CommentsRaw
        {
            public string against { get; set; }

            public string content { get; set; }

            public string created_time { get; set; }

            public string pid { get; set; }

            public string support { get; set; }

            public string tid { get; set; }

            public string username { get; set; }
        }

        public class LCommentsRaw
        {
            public List<CommentsRaw> result { get; set; }

            public string status { get; set; }
        }
    }
}