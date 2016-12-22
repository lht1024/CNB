using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace CNB
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>

    public sealed partial class MainPage : Page
    {
        public static RootObject myData;
        public static CommentsProxy.RootObject myComments;
        public static RootObject1 myDetail;
        public static string myDetialArticleId;
        public static string myLastArticleId = "";
        public static string Filter;
        public static bool IsFirstPageLoad = false;
        public static bool IsAboutClick = false;
        public static bool IsHotCommentsSelected = false;

        public MainPage()
        {
            
            this.InitializeComponent();
            MySetting();
        }


       

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(typeof(Page1));
        }

        public static void MySetting(string setting)
        {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["FilterSetting"] = setting;
            Filter = localSettings.Values["FilterSetting"].ToString();
        }
        public static void MySetting()
        {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if(localSettings.Values.ContainsKey("FilterSetting"))
                Filter = localSettings.Values["FilterSetting"].ToString();
            else
            {
                localSettings.Values["FilterSetting"] = "1";
                Filter = localSettings.Values["FilterSetting"].ToString();
            }

        }
    }
}
