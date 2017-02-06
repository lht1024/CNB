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
        public static bool Count = false;
        public static int Filter = 0;
        public static bool IsAboutClick = false;
        public static bool IsFirstPageLoad = false;
        public static bool IsHotCommentsSelected = false;
        public static CommentsProxy.LCommentsRaw myComments;
        public static LNewsRaw myData;
        public static LNewsContentRaw myDetail;
        public static string myDetialArticleId;
        public static string MyFontSize;
        public static string myLastArticleId = "";
        public static string MyLeSpacing;
        public static string MyPaPadding;
        public static int RankSelected = 0;
        public static int TopicSelected = 0;
        public static string MyCommentDirection;
        public static string IHateApple;

        public MainPage()
        {
            this.InitializeComponent();
            LoadMySetting();
        }

        public static void LoadMySetting()
        {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey("MyFontSize"))
            {
                MainPage.MyFontSize = localSettings.Values["MyFontSize"].ToString();
                MainPage.MyPaPadding = localSettings.Values["MyPaPadding"].ToString();
                MainPage.MyLeSpacing = localSettings.Values["MyLeSpacing"].ToString();
            }
            else
            {
                localSettings.Values["MyFontSize"] = "22";
                localSettings.Values["MyPaPadding"] = "2";
                localSettings.Values["MyLeSpacing"] = "0";
                MainPage.MyFontSize = "16";
                MainPage.MyPaPadding = "0";
                MainPage.MyLeSpacing = "0";
            }
            if (localSettings.Values.ContainsKey("MyConDir"))
            {
                MainPage.MyCommentDirection = localSettings.Values["MyConDir"].ToString();
            }
            else
            {
                localSettings.Values["MyConDir"] = "0";
                MainPage.MyCommentDirection = "0";
            }
            if (localSettings.Values.ContainsKey("IHA"))
            {
                MainPage.IHateApple = localSettings.Values["IHA"].ToString();
            }
            else
            {
                localSettings.Values["IHA"] = "0";
                MainPage.IHateApple = "0";
            }
        }

        public static void SetMySetting(string setting, string setname)
        {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values[setname] = setting;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(typeof(Page1));
        }
    }
}