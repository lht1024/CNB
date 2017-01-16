﻿using System;
using System.Collections.ObjectModel;
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

        

        private async void MyListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            MainPage.IsHotCommentsSelected = false;
            var mySeleted = (News)e.ClickedItem;
            MainPage.myDetialArticleId = mySeleted.sid;
            if (!string.IsNullOrEmpty(MainPage.myDetialArticleId))
            {
                MainPage.myDetail = await NewsDetailProxy.GetNewsDetail(mySeleted.sid);

                var filtler = Clear("<head><meta http-equiv=\"Content-Type\" content=\"text/html; charset = utf-8\"></head>" +
                    MainPage.myDetail.result.hometext + MainPage.myDetail.result.bodytext);
                await WriteHtml(filtler);
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

        private async void Refresh_Click(object sender, RoutedEventArgs e)
        {
            MainPage.IsFirstPageLoad = false;
            NewsFrame.Navigate(typeof(Page2));

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            MyNewsList.DoRefresh();
            await MyListView.LoadMoreItemsAsync();
        }

        private async Task WriteHtml(string filtler)
        {
            IStorageFolder local = ApplicationData.Current.LocalFolder;
            IStorageFolder dataFolder = await local.CreateFolderAsync("DataFile", CreationCollisionOption.OpenIfExists);
            IStorageFile file = await dataFolder.CreateFileAsync("HTMLPage1.html", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, filtler);
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MainPage.IsAboutClick = true;
            NewsFrame.Navigate(typeof(Page2));
            MainPage.IsFirstPageLoad = false;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
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
    }
}