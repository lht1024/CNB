using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace CNB
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>

    public sealed partial class MainPage : Page
    {
        public static int Filter = 0;
        public static bool IsAboutClick = false;
        public static bool IsFirstPageLoad = false;
        public static bool IsHotCommentsSelected = false;
        public static CommentsProxy.LCommentsRaw myComments;
        public static LNewsRaw myData;
        public static LNewsContentRaw myDetail;
        public static string myDetialArticleId;
        public static string myLastArticleId = "";
        public static bool Count = false;
        public static int TopicSelected = 0;
        public static int RankSelected = 0;

        public MainPage()
        {
            this.InitializeComponent();
            MySetting();
        }

        public static void MySetting(string setting)
        {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["FilterSetting"] = setting;
            //Filter = localSettings.Values["FilterSetting"].ToString();
        }

        public static void MySetting()
        {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
           /* if (localSettings.Values.ContainsKey("FilterSetting"))
                Filter = localSettings.Values["FilterSetting"].ToString();
            else
            {
                localSettings.Values["FilterSetting"] = "1";
                Filter = localSettings.Values["FilterSetting"].ToString();
            }*/
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(typeof(Page1));
        }
    }
}