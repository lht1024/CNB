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
    public class NewsDetailProxy
    {
        public async static Task<RootObject1> GetNewsDetail(string article_id)
        {
            var mylink = string.Format("http://cnbeta1.com/api/getArticleDetail/{0}/", article_id);
            var http = new HttpClient();
            var response = await http.GetAsync(mylink);
            var result = await response.Content.ReadAsStringAsync();
            var serializer = new DataContractJsonSerializer(typeof(RootObject1));

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            var data = (RootObject1)serializer.ReadObject(ms);

            return data;
        }
    }
    [DataContract]
    public class RootObject1
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string date { get; set; }
        [DataMember]
        public string source { get; set; }
        [DataMember]
        public string sourceLink { get; set; }
        [DataMember]
        public string intro { get; set; }
        [DataMember]
        public int topicId { get; set; }
        [DataMember]
        public string topicTitle { get; set; }
        [DataMember]
        public string topicImage { get; set; }
        [DataMember]
        public string content { get; set; }
        [DataMember]
        public string author { get; set; }
    }

}
