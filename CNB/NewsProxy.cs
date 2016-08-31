using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace CNB
{
    public class NewsProxy
    {
        public async static Task<RootObject> GetNews()
        {
            var mylink = "http://cnbeta1.com/api/getArticles";
            var http = new HttpClient();
            var response = await http.GetAsync(mylink);
            var result = await response.Content.ReadAsStringAsync();
            var Test = "{\"Results\":" + result + "}";
            var serializer = new DataContractJsonSerializer(typeof(RootObject));

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(Test));
            var data = (RootObject)serializer.ReadObject(ms);

            return data;
        }

        public async static Task<RootObject> GetNews(string MoreNews)
        {
            var mylink = String.Format( "http://cnbeta1.com/api/getMoreArticles/{0}",MoreNews);
            var http = new HttpClient();
            var response = await http.GetAsync(mylink);
            var result = await response.Content.ReadAsStringAsync();
            var Test = "{\"Results\":" + result + "}";
            var serializer = new DataContractJsonSerializer(typeof(RootObject));

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(Test));
            var data = (RootObject)serializer.ReadObject(ms);

            return data;
        }
    }
    [DataContract]
    public class Result
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string article_id { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string date { get; set; }
        [DataMember]
        public string intro { get; set; }
        [DataMember]
        public string topic { get; set; }
        [DataMember]
        public string view_num { get; set; }
        [DataMember]
        public string comment_num { get; set; }
        [DataMember]
        public string source { get; set; }
        [DataMember]
        public string source_link { get; set; }
        [DataMember]
        public string hot { get; set; }
        [DataMember]
        public string pushed { get; set; }
    }
    [DataContract]
    public class RootObject
    {
        [DataMember]
        public List<Result> Results { get; set; }
    }
}
