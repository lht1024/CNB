using Windows.UI.Xaml;

namespace CNB
{
    public class Comment
    {
        public string against { get; set; }
        public string comment { get; set; }

        //public string content { get; set; }
        //public string created_time { get; set; }
        public string date { get; set; }

        //public string username { get; set; }
        public string name { get; set; }

        public string support { get; set; }
        public string tid { get; set; }

        public string preComments { get; set; }

        public string pid { get; set; }

        public string locate { get; set; }

        public Thickness myBorderThickness { get; set; }
    }
}