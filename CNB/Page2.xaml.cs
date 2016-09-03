using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class Page2 : Page
    {
        
        public Page2()
        {
            this.InitializeComponent();


            if (MainPage.Filter == "0")
            {
                FilterSwitch.IsOn = false;
            }
            else
            {
                FilterSwitch.IsOn = true;
            }

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
                MyDetailSource.Text = MainPage.myDetail.source;
                MyDetailDate.Text = MainPage.myDetail.date;
                MyWebView.Source = new Uri("ms-appdata:///local/DataFile/HTMLPage1.html", UriKind.RelativeOrAbsolute);
            }

        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (FilterSwitch.IsOn)
            {
                MainPage.MySetting("1");
            }
            else
            {
                MainPage.MySetting("0");
            }
        }

        private void LoadComments_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Page3));
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }
    }
}
