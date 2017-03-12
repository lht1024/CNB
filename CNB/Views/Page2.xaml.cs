using System;
using System.Text.RegularExpressions;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using CNB.Views;
using CNB.Models;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace CNB
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Page2 : Page
    {
        public Page2()
        {
            this.InitializeComponent();

            if (MainPage.IsFirstPageLoad == false)
            {
                MainPage.IsFirstPageLoad = true;
                MyBlock.Visibility = Visibility.Collapsed;
                LoadComments.Visibility = Visibility.Collapsed;
                ToSourceButton.Visibility = Visibility.Collapsed;
                MyWebView.Source = new Uri("ms-appx-web:///Assets/FirstPage.html", UriKind.RelativeOrAbsolute);
            }
            else if (MainPage.IsAboutClick == true)
            {
                MainPage.IsAboutClick = false;
                LoadComments.Visibility = Visibility.Collapsed;
                ToSourceButton.Visibility = Visibility.Collapsed;
                MyBlock.Visibility = Visibility.Visible;
                ToSourceButton.Content = "";
                MyDetailSource.Text = "关于";
                MyDetailDate.Text = "";
                MyWebView.Source = new Uri("ms-appx-web:///Assets/AboutPage.html", UriKind.RelativeOrAbsolute);
                ToSourceButton.Width = 100;
                ToSourceButton.NavigateUri = new Uri("https://github.com/lht1024/CNB-UWP", UriKind.Absolute);
            }
            else
            {
                MyBlock.Visibility = Visibility.Visible;
                LoadComments.Visibility = Visibility.Visible;
                ToSourceButton.Visibility = Visibility.Visible;
                MyDetailSource.Text = FormatSourceID(MainPage.myDetail.result.source);
                MyDetailDate.Text = MainPage.myDetail.result.time;
                ToSourceButton.NavigateUri = new Uri(string.Format("http://www.cnbeta.com/articles/{0}.htm",MainPage.myDetail.result.sid),UriKind.Absolute);
                MyWebView.Source = new Uri("ms-appdata:///local/DataFile/HTMLPage1.html", UriKind.RelativeOrAbsolute);
            }
        }

        /// <summary>
        /// 评论按钮点击后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void LoadComments_Click(object sender, RoutedEventArgs e)
        {
            var TS = CalTimeSpan(MainPage.myDetail.result.time);
            if (TS.Days < 1 && TS.Hours < 24)
            {
                MainPage.myWinthin24Comments = await CommentsWinthin24Proxy.GetResults("1",MainPage.myDetialArticleId);
                Frame.Navigate(typeof(Page5));
            }
            else
            {
                MainPage.myComments = await CommentsProxy.GetResults(MainPage.myDetialArticleId);
                Frame.Navigate(typeof(Page3));
            } 
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }

        /// <summary>
        /// 计算输入的新闻时间与当前时间的间隔
        /// </summary>
        /// <param name="NewsTimeRaw">输入的新闻时间</param>
        /// <returns>输入时间与当前时间的间隔</returns>
        private TimeSpan CalTimeSpan(string NewsTimeRaw)
        {
            var Fir = NewsTimeRaw.Split(' ');
            var YMD = Fir[0].Split('-');
            var HMS = Fir[1].Split(':');
            DateTime NewsTime = new DateTime (Convert.ToInt16(YMD[0]), Convert.ToInt16(YMD[1]), Convert.ToInt16(YMD[2]), 
                Convert.ToInt16(HMS[0]), Convert.ToInt16(HMS[1]), Convert.ToInt16(HMS[2]));
            var TimeSpan = DateTime.Now - NewsTime;
            return TimeSpan;
        }

        /// <summary>
        /// 处理文章来源网站的字符串
        /// </summary>
        /// <param name="RawText">源字符串</param>
        /// <returns>处理后的字符串</returns>
        private string FormatSourceID(string RawText)
        {
            RawText = Regex.Replace(RawText, "<a.+?>", "");
            RawText = Regex.Replace(RawText, "</a>", "");
            RawText = RawText.Replace("<span>", "");
            RawText = RawText.Replace("</span>", "");
            return RawText;
        }
    }
}