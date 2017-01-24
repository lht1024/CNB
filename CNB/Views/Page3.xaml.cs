using System.Collections.ObjectModel;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace CNB
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Page3 : Page
    {
        private Color darkRed = new Color
        {
            A = 255,
            R = 139,
            G = 0,
            B = 0
        };

        private ObservableCollection<Comment> MyCommentsList;

        private Color white = new Color
        {
            A = 255,
            R = 255,
            G = 255,
            B = 255
        };

        public Page3()
        {
            this.InitializeComponent();
            MyCommentsList = new ObservableCollection<Comment>();

            if (MainPage.IsHotCommentsSelected == true)
            {
                HotComments.Foreground = new SolidColorBrush(white);
                HotComments.Background = new SolidColorBrush(darkRed);
                AllComments.Foreground = new SolidColorBrush(darkRed);
                AllComments.Background = new SolidColorBrush(white);
            }
            else
            {
                HotComments.Foreground = new SolidColorBrush(darkRed);
                HotComments.Background = new SolidColorBrush(white);
                AllComments.Foreground = new SolidColorBrush(white);
                AllComments.Background = new SolidColorBrush(darkRed);
            }
            SetAllComments();
        }

        private void AllComments_Click(object sender, RoutedEventArgs e)
        {
            if (MainPage.IsHotCommentsSelected == true)
            {
                MainPage.IsHotCommentsSelected = false;
                HotComments.Foreground = new SolidColorBrush(darkRed);
                HotComments.Background = new SolidColorBrush(white);
                AllComments.Foreground = new SolidColorBrush(white);
                AllComments.Background = new SolidColorBrush(darkRed);
                MyCommentsList.Clear();
                SetAllComments();
            }
        }

        private void HotComments_Click(object sender, RoutedEventArgs e)
        {
            if (MainPage.IsHotCommentsSelected == false)
            {
                MainPage.IsHotCommentsSelected = true;
                HotComments.Foreground = new SolidColorBrush(white);
                HotComments.Background = new SolidColorBrush(darkRed);
                AllComments.Foreground = new SolidColorBrush(darkRed);
                AllComments.Background = new SolidColorBrush(white);
                MyCommentsList.Clear();
                SetHotComments();
            }
        }

        private void SetAllComments()
        {
            if (MainPage.myComments.result != null && MainPage.myComments.result.Count != 0)
            {
                foreach (var c in MainPage.myComments.result)
                {
                    MyCommentsList.Add(new Comment
                    {
                        //username = (c.username.Contains("") ? "匿名用户" : c.username),
                        //content = c.content,
                        //created_time = c.created_time,
                        name = (c.name.Contains("") ? "匿名用户" : c.name),
                        comment = c.comment,
                        date = c.date,
                        against = "反对(" + c.against + ")",
                        support = "支持(" + c.support + ")",
                        tid = c.tid
                    });
                }
            }
            else
            {
                MyCommentsList.Add(new Comment
                {
                    comment = "似乎没有人评论"
                });
            }
        }

        private void SetHotComments()
        {
            if (MainPage.myComments.result != null && MainPage.myComments.result.Count != 0)
            {
                MainPage.myComments.result.ForEach((c) =>
                {
                    if (int.Parse(c.support) > 7 || int.Parse(c.against) > 13)
                        MyCommentsList.Add(new Comment
                        {
                            //username = (c.username.Contains("") ? "匿名用户" : c.username),
                            //content = c.content,
                            //created_time = c.created_time,
                            name = (c.name.Contains("") ? "匿名用户" : c.name),
                            comment = c.comment,
                            date = c.date,
                            against = "反对(" + c.against + ")",
                            support = "支持(" + c.support + ")",
                            tid = c.tid
                        });
                });
                if (MyCommentsList.Count == 0)
                {
                    MyCommentsList.Add(new Comment
                    {
                        comment = "似乎没有人评论"
                    });
                }
            }
            else
            {
                MyCommentsList.Add(new Comment
                {
                    comment = "似乎没有人评论"
                });
            }
        }
    }
}