using CNB.Views;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace CNB
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Page1 : Page
    {
        private NewsCollectionList MyNewsList;

        public Page1()
        {
            this.InitializeComponent();
            SystemNavigationManager.GetForCurrentView().BackRequested += Page_GoBack;

            MyNewsList = new NewsCollectionList();
            MyNewsList.DataLoaded += MyNewsList_DataLoaded;
            MyNewsList.DataLoading += MyNewsList_DataLoading;
            NewsFrame.Navigate(typeof(Page2));
        }

        public static string Clear(string text)
        {
            text = Regex.Replace(text, "<embed.+?/>", "<p style=\"text-align:center;font-weight:bolder\">不能视频显示</p>");
            //text = Regex.Replace(text,"iframe","video");
            text = Regex.Replace(text, "\"", "\"");
            text = Regex.Replace(text, "\\/", "/");
            return text;
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MainPage.IsAboutClick = true;
            NewsFrame.Navigate(typeof(Page2));
            //MainPage.IsFirstPageLoad = false;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }

        private async void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MainPage.Count = false;
            SelectButton.Visibility = Visibility.Collapsed;
            RankButton.Visibility = Visibility.Collapsed;
            if (RItem.IsSelected)
            {
                MyTag.Text = "月度Top10";
                MainPage.Filter = 1;
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                MyNewsList.DoRefresh();
                await MyListView.LoadMoreItemsAsync();
            }
            else if (CItem.IsSelected)
            {
                MyTag.Text = "热门评论";
                MainPage.Filter = 2;
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                MyNewsList.DoRefresh();
                await MyListView.LoadMoreItemsAsync();
            }
            else if (TItem.IsSelected)
            {
                MyTag.Text = "";
                RankButton.Visibility = Visibility.Visible;
                MainPage.Filter = 3;
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                MyNewsList.DoRefresh();
                await MyListView.LoadMoreItemsAsync();
            }
            else if (TRItem.IsSelected)
            {
                MyTag.Text = "";
                SelectButton.Visibility = Visibility.Visible;
                MainPage.Filter = 4;
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                MyNewsList.DoRefresh();
                await MyListView.LoadMoreItemsAsync();
            }
            else if (SetItem.IsSelected)
            {
                NewsFrame.Navigate(typeof(Page4));
                SetItem.IsSelected = false;
                NItem.IsSelected = true;
            }
            else
            {
                MyTag.Text = "新闻资讯";
                MainPage.Filter = 0;
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                MyNewsList.DoRefresh();
                await MyListView.LoadMoreItemsAsync();
            }
        }

        private async void MyListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            MainPage.IsHotCommentsSelected = false;
            var mySeleted = (News)e.ClickedItem;
            MainPage.myDetialArticleId = mySeleted.sid;
            if (!string.IsNullOrEmpty(MainPage.myDetialArticleId))
            {
                try
                {
                    MainPage.myDetail = await NewsDetailProxy.GetNewsDetail(mySeleted.sid);
                }
                catch
                {
                }
                var ls = "p{letter-spacing:" + MainPage.MyLeSpacing + "px}";
                var pp = "p{padding:" + MainPage.MyPaPadding + "px 0}";
                var fz = "p{ font-size:" + MainPage.MyFontSize + "px}";
                var ff = "p{ font-family:\"微软雅黑\"}";

                var headtext = String.Format("<head><meta http-equiv=\"Content-Type\" content=\"text/html; charset = utf-8\"><style>{0}{1}{2}{3}</style></head>", ls, pp, fz, ff);
                var filtler = Clear(headtext + MainPage.myDetail.result.hometext + MainPage.myDetail.result.bodytext);
                try
                {
                    await WriteHtml(filtler);
                }
                catch
                {

                }
                NewsFrame.Navigate(typeof(Page2));
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }
        }

        private void MyNewsList_DataLoaded()
        {
            MyProcessRing.IsActive = false;
            MyNewsTotal.Text = MyNewsList.TotalCount.ToString();
        }

        private void MyNewsList_DataLoading()
        {
            MyProcessRing.IsActive = true;
        }

        private void Page_GoBack(object sender, BackRequestedEventArgs e)
        {
            if (NewsFrame.CanGoBack)
            {
                NewsFrame.GoBack();
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }
        }

        private async void RankA_Click(object sender, RoutedEventArgs e)
        {
            if (MainPage.RankSelected != 0)
            {
                MainPage.RankSelected = 0;
                MainPage.Count = false;
                RankButton.Content = "今日最多阅读";
                MyNewsList.DoRefresh();
                await MyListView.LoadMoreItemsAsync();
            }
        }

        private async void RankB_Click(object sender, RoutedEventArgs e)
        {
            if (MainPage.RankSelected != 1)
            {
                MainPage.RankSelected = 1;
                MainPage.Count = false;
                RankButton.Content = "今日热评";
                MyNewsList.DoRefresh();
                await MyListView.LoadMoreItemsAsync();
            }
        }

        private async void RankC_Click(object sender, RoutedEventArgs e)
        {
            if (MainPage.RankSelected != 2)
            {
                MainPage.RankSelected = 2;
                RankButton.Content = "热门推荐";
                MainPage.Count = false;
                MyNewsList.DoRefresh();
                await MyListView.LoadMoreItemsAsync();
            }
        }

        private async void Refresh_Click(object sender, RoutedEventArgs e)
        {
            MainPage.IsFirstPageLoad = false;
            NewsFrame.Navigate(typeof(Page2));

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            MyNewsList.DoRefresh();
            await MyListView.LoadMoreItemsAsync();
        }

        private void SplitButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private async void TopicA_Click(object sender, RoutedEventArgs e)
        {
            if (MainPage.TopicSelected != 0)
            {
                MainPage.TopicSelected = 0;
                SelectButton.Content = "Microsoft 微软";
                MyNewsList.DoRefresh();
                await MyListView.LoadMoreItemsAsync();
            }
        }

        private async void TopicB_Click(object sender, RoutedEventArgs e)
        {
            if (MainPage.TopicSelected != 1)
            {
                MainPage.TopicSelected = 1;
                SelectButton.Content = "Google 谷歌";
                MyNewsList.DoRefresh();
                await MyListView.LoadMoreItemsAsync();
            }
        }

        private async void TopicC_Click(object sender, RoutedEventArgs e)
        {
            if (MainPage.TopicSelected != 2)
            {
                MainPage.TopicSelected = 2;
                SelectButton.Content = "Apple 苹果";
                MyNewsList.DoRefresh();
                await MyListView.LoadMoreItemsAsync();
            }
        }

        private async void TopicD_Click(object sender, RoutedEventArgs e)
        {
            if (MainPage.TopicSelected != 3)
            {
                MainPage.TopicSelected = 3;
                SelectButton.Content = "Windows10";
                MyNewsList.DoRefresh();
                await MyListView.LoadMoreItemsAsync();
            }
        }

        private async void TopicE_Click(object sender, RoutedEventArgs e)
        {
            if (MainPage.TopicSelected != 4)
            {
                MainPage.TopicSelected = 4;
                SelectButton.Content = "Sony 索尼";
                MyNewsList.DoRefresh();
                await MyListView.LoadMoreItemsAsync();
            }
        }

        private async void TopicF_Click(object sender, RoutedEventArgs e)
        {
            if (MainPage.TopicSelected != 5)
            {
                MainPage.TopicSelected = 5;
                SelectButton.Content = "Android 安卓";
                MyNewsList.DoRefresh();
                await MyListView.LoadMoreItemsAsync();
            }
        }

        private async void TopicG_Click(object sender, RoutedEventArgs e)
        {
            if (MainPage.TopicSelected != 6)
            {
                MainPage.TopicSelected = 6;
                SelectButton.Content = "Samsung 三星";
                MyNewsList.DoRefresh();
                await MyListView.LoadMoreItemsAsync();
            }
        }

        private async Task WriteHtml(string filtler)
        {
            IStorageFolder local = ApplicationData.Current.LocalFolder;
            IStorageFolder dataFolder = await local.CreateFolderAsync("DataFile", CreationCollisionOption.OpenIfExists);
            IStorageFile file = await dataFolder.CreateFileAsync("HTMLPage1.html", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, filtler);
        }

        /*   public static string ClearHtmlCode(string text)
{
text = text.Trim();
if (string.IsNullOrEmpty(text))
return string.Empty;
text = Regex.Replace(text, "[/s]{2,}", " ");    //two or more spaces
text = Regex.Replace(text, "(<[b|B][r|R]/*>)+|(<[p|P](.|/n)*?>)", " ");    //<br>
text = Regex.Replace(text,"</p>","\n");
text = Regex.Replace(text, "(/s*&[n|N][b|B][s|S][p|P];/s*)+", " ");    //
text = Regex.Replace(text, "<(.|/n)*?>", string.Empty);    //any other tags
text = Regex.Replace(text, "/<//?[^>]*>/g", string.Empty);    //any other tags
text = Regex.Replace(text, "/[    | ]* /g", string.Empty);    //any other tags
//text = text.Replace("'", "''");
text = Regex.Replace(text, "/ [/s| |    ]* /g", string.Empty);
return text;
}*/
    }
}