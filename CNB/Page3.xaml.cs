using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        CommentsProxy.RootObjectBack Back;
        CommentsCollectionList MyCommentsList;
        public static string tid;
        public Page3()
        {
            this.InitializeComponent();
            MyCommentsList = new CommentsCollectionList();
        }

        private void CommentsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Status.Text = "";
            var Item = (Comment)e.ClickedItem;
            tid =Item.tid;
            if (!string.IsNullOrEmpty(tid))
            {
                var t = sender as ListView;
                FlyoutBase.ShowAttachedFlyout(t);
            }

        }

        private async void SupportButton_Click(object sender, RoutedEventArgs e)
        {
           Back = await CommentsProxy.SupportComment(tid);
           Status.Text = Back.result;
        }

        private async void AgainstButton_Click(object sender, RoutedEventArgs e)
        {
            Back = await CommentsProxy.AgainstComment(tid);
            if (Back.status == "success")
            Status.Text = Back.result;
        }
    }
}
