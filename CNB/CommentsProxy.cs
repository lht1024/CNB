using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace CNB
{
    public class CommentsProxy
    {
        public async static Task<RootObject> GetResults(string page, string article_id)
        {
            var timstamp = ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString();
            var toBehashed = String.Format("app_key=10000&format=json&method=Article.Comment&page={0}&sid={1}&timestamp={2}&v=1.0&mpuffgvbvbttn3Rc", page, article_id, timstamp);
            var Md5 = ComputeMD5(toBehashed);
            var mylink = String.Format("http://api.cnbeta.com/capi?app_key=10000&format=json&method=Article.Comment&page={0}&sid={1}&timestamp={2}&v=1.0&sign={3}", page, article_id, timstamp, Md5);

            var http = new HttpClient();
            var response = await http.GetAsync(mylink);
            var result = await response.Content.ReadAsStringAsync();

            var serializer = new DataContractJsonSerializer(typeof(RootObject));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            var data = (RootObject)serializer.ReadObject(ms);
            return data;
        }
        [DataContract]
        public class Result
        {
            [DataMember]
            public string tid { get; set; }
            [DataMember]
            public string pid { get; set; }
            [DataMember]
            public string username { get; set; }
            [DataMember]
            public string content { get; set; }
            [DataMember]
            public string created_time { get; set; }
            [DataMember]
            public string support { get; set; }
            [DataMember]
            public string against { get; set; }
        }

        [DataContract]
        public class RootObject
        {
            [DataMember]
            public string status { get; set; }
            [DataMember]
            public List<Result> result { get; set; }
        }



        private static string ComputeMD5(string str)
        {
            var alg = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            IBuffer buff = CryptographicBuffer.ConvertStringToBinary(str, BinaryStringEncoding.Utf8);
            var hashed = alg.HashData(buff);
            var res = CryptographicBuffer.EncodeToHexString(hashed);
            return res;
        }

    }
}
