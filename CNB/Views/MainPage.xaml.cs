using CNB.Models;
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
        public static bool Count = false;//避免Top10和热门评论重复加载
        public static int Filter = 0;//汉堡菜单选择 0:所有新闻,1:本月Top10,2:热门评论,3:热门内容,4:专项内容
        public static bool IsAboutClick = false;//判断关于菜单是否按下，点击后为True
        public static bool IsFirstPageLoad = false;//判断List是否点击，点击则为True
        public static bool IsHotCommentsSelected = false;//判断评论选择的类型
        public static CommentsProxy.LCommentsRaw myComments;//24小时后评论源数据
        public static CommentsWinthin24Proxy.LCommentsRaw myWinthin24Comments;//24小时内评论源数据
        public static LNewsRaw myData;//新闻列表的源数据
        public static LNewsContentRaw myDetail;//新闻详细内容的源数据
        public static string myDetialArticleId;//点击新闻的具体ID
        public static string MyFontSize;//字大小
        public static string myLastArticleId = "";//List最后一项的文章ID
        public static string MyLeSpacing;//字间隔
        public static string MyPaPadding;//段间隔
        public static int RankSelected = 0;//热门内容选择 0:今日最多阅读,1:今日热评,2:热门推荐
        public static int TopicSelected = 0;//专项内容选择 0:微软,1:谷歌,2:苹果,3:Win10,4:索尼,5:安卓,6:三星
        public static string MyCommentDirection;//评论显示方向，正向为0
        public static string IHateApple;//判断是否屏蔽所有苹果相关新闻，屏蔽为1

        public MainPage()
        {
            this.InitializeComponent();
            LoadMySetting();
        }

        /// <summary>
        /// 初始化设置
        /// </summary>
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
                MainPage.MyFontSize = "22";
                MainPage.MyPaPadding = "2";
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

        /// <summary>
        /// 设置相关内容
        /// </summary>
        /// <param name="setting">要设置的值</param>
        /// <param name="setname">要设置的项目</param>
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