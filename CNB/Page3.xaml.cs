using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class Page3 : Page
    {
        CommentsCollectionList MyCommentsList;
        Color darkRed = new Color
        {
            A = 255,
            R = 139,
            G = 0,
            B = 0
        };
        Color white = new Color
        {
            A = 255,
            R = 255,
            G = 255,
            B = 255
        };
        public Page3()
        {
            this.InitializeComponent();
            MyCommentsList = new CommentsCollectionList();
            
            

            if (MainPage.IsHotCommentsSelected == true)
            {
                HotComments.Foreground = new SolidColorBrush(darkRed);
                HotComments.Background = new SolidColorBrush(white);
                AllComments.Foreground = new SolidColorBrush(white);
                AllComments.Background = new SolidColorBrush(darkRed);
            }
            else
            {
                HotComments.Foreground = new SolidColorBrush(white);
                HotComments.Background = new SolidColorBrush(darkRed);
                AllComments.Foreground = new SolidColorBrush(darkRed);
                AllComments.Background = new SolidColorBrush(white);
            }
        }

        private void HotComments_Click(object sender, RoutedEventArgs e)
        {
            if (MainPage.IsHotCommentsSelected == false)
            {
                MainPage.IsHotCommentsSelected = true;
                HotComments.Foreground = new SolidColorBrush(darkRed);
                HotComments.Background = new SolidColorBrush(white);
                AllComments.Foreground = new SolidColorBrush(white);
                AllComments.Background = new SolidColorBrush(darkRed);
                MyCommentsList.DoRefresh();
            }
           
        }

        private void AllComments_Click(object sender, RoutedEventArgs e)
        {
            if (MainPage.IsHotCommentsSelected == true)
            {
                MainPage.IsHotCommentsSelected = false;
                HotComments.Foreground = new SolidColorBrush(white);
                HotComments.Background = new SolidColorBrush(darkRed);
                AllComments.Foreground = new SolidColorBrush(darkRed);
                AllComments.Background = new SolidColorBrush(white);
                MyCommentsList.DoRefresh();
            }
        }
    }
}
