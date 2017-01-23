﻿using System;
using System.Text.RegularExpressions;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
                MyWebView.Source = new Uri("ms-appx-web:///Assets/FirstPage.html", UriKind.RelativeOrAbsolute);
            }
            else if (MainPage.IsAboutClick == true)
            {
                MainPage.IsAboutClick = false;
                LoadComments.Visibility = Visibility.Collapsed;
                MyBlock.Visibility = Visibility.Visible;
                MyDetailSource.Text = "关于";
                MyDetailDate.Text = "";
                MyWebView.Source = new Uri("ms-appx-web:///Assets/AboutPage.html", UriKind.RelativeOrAbsolute);
            }
            else
            {
                MyBlock.Visibility = Visibility.Visible;
                LoadComments.Visibility = Visibility.Visible;
                MyDetailSource.Text = FormatSourceID(MainPage.myDetail.result.source);
                MyDetailDate.Text = MainPage.myDetail.result.time;
                MyWebView.Source = new Uri("ms-appdata:///local/DataFile/HTMLPage1.html", UriKind.RelativeOrAbsolute);
            }
        }

        private string FormatSourceID(string RawText)
        {
            RawText = Regex.Replace(RawText, "<a.+?>", "");
            RawText = Regex.Replace(RawText, "</a>", "");
            return RawText;
        }

        private async void LoadComments_Click(object sender, RoutedEventArgs e)
        {
            MainPage.myComments = await CommentsProxy.GetResults(MainPage.myDetialArticleId);
            Frame.Navigate(typeof(Page3));
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }
    }
}