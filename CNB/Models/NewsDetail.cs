using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNB
{
    public class NewsDetail
    {
        public int id { get; set; }
        public string title { get; set; }
        public string date { get; set; }
        public string source { get; set; }
        public string sourceLink { get; set; }
        public string intro { get; set; }
        public int topicId { get; set; }
        public string topicTitle { get; set; }
        public string topicImage { get; set; }
        public string content { get; set; }
        public string author { get; set; }
    }
}
