using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace CNB
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Page1 : Page
    {
        NewsCollectionList MyNewsList;
        public Page1()
        {
            this.InitializeComponent();
            SystemNavigationManager.GetForCurrentView().BackRequested += Page_GoBack;
            MyNewsList = new NewsCollectionList();
            MyNewsList.DataLoaded += MyNewsList_DataLoaded;
            NewsFrame.Navigate(typeof(Page2));


        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (MySwitch.IsOn)
                MyNewsTotal.Visibility = Visibility.Visible;
            else
                MyNewsTotal.Visibility = Visibility.Collapsed;
        }

        private void Page_GoBack(object sender, BackRequestedEventArgs e)
        {
            if (NewsFrame.CanGoBack)
            {
                NewsFrame.GoBack();
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }
        }

        private void MyNewsList_DataLoaded()
        {
            MyNewsTotal.Text = MyNewsList.TotalCount.ToString();
        }

        private async void MyListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var mySeleted = (News)e.ClickedItem;
            MainPage.myDetialArticleId = mySeleted.article_id;
            MainPage.myDetail = await NewsDetailProxy.GetNewsDetail(mySeleted.article_id);
            
            var filtler = Clear("<head><meta http-equiv=\"Content-Type\" content=\"text/html; charset = utf-8\"></head>" + "<p>" 
                + MainPage.myDetail.intro + "</p>" + MainPage.myDetail.content);
            await WriteHtml(filtler);

            NewsFrame.Navigate(typeof(Page2));
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
          //  text = text.Replace("'", "''");
            text = Regex.Replace(text, "/ [/s| |    ]* /g", string.Empty);
            return text;
        }*/

        public static string Clear(string text)
        {
            text = Regex.Replace(text, "<embed.+?/>", "<p style=\"text-align:center;font-weight:bolder\">不能视频显示</p>");
            text = Regex.Replace(text, "\"", "\"");
            text = Regex.Replace(text,"\\/","/");
            return text;
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            MainPage.IsFirstPageLoad = false;
            NewsFrame.Navigate(typeof(Page2));

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            MyNewsList.DoRefresh();
        }

        

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MainPage.IsAboutClick = true;
            NewsFrame.Navigate(typeof(Page2));
        }

    }

}
