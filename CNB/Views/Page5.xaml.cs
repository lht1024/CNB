using CNB.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace CNB.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Page5 : Page
    {
        /// <summary>
        /// 可用于自身或导航至 Frame 内部的空白页。
        /// </summary>
        
        private Color darkRed = new Color
        {
            A = 255,
            R = 139,
            G = 0,
            B = 0
        };
        private Color white = new Color
        {
            A = 255,
            R = 255,
            G = 255,
            B = 255
        };

        public int MyNum = 2;
        public int MyLocate = 1;
        private ObservableCollection<Comment> MyCommentsList;
        List<Comment> MyTolComments = new List<Comment>();
        Dictionary<string, string> MyPastCommentsDic = new Dictionary<string, string>();
        Dictionary<string, string> MyTidPidDic = new Dictionary<string, string>();

       
        public Page5()
        {
            FormatMyList();
            ForwardToGetData();
            this.InitializeComponent();
            

            if (MainPage.MyCommentDirection == "0")
            {
                ReverseMyContent.Content = "正序";
            }
            else
            {
                ReverseMyContent.Content = "倒序";
            }
            MyCommentsList = new ObservableCollection<Comment>();

            if (MainPage.IsHotCommentsSelected)
            {
                ChangeDarkRed(HotComments);
                ChangeWhite(AllComments);
            }
            else
            {
                ChangeDarkRed(AllComments);
                ChangeWhite(HotComments);
            }
            
            MyCommentsList.Clear();
            SetAllComments();
            if (MyCommentsList.Count < 10)
            {
                LoadMoreButton.Visibility = Visibility.Collapsed;
            }
        }

        private void AllComments_Click(object sender, RoutedEventArgs e)
        {
            if (MainPage.IsHotCommentsSelected == true)
            {
                MainPage.IsHotCommentsSelected = false;
                ChangeDarkRed(AllComments);
                ChangeWhite(HotComments);
                ChangeWhite(ReverseMyContent);
                MyCommentsList.Clear();
                SetAllComments();
            }
        }

            private void HotComments_Click(object sender, RoutedEventArgs e)
            {
                if (MainPage.IsHotCommentsSelected == false)
                {
                    MainPage.IsHotCommentsSelected = true;
                    ChangeDarkRed(HotComments);
                    ChangeWhite(AllComments);
                    ChangeDarkRed(ReverseMyContent);
                    MyCommentsList.Clear();
                    SetHotComments();
                    LoadMoreButton.Visibility = Visibility.Collapsed;
                }
            }

            private void SetAllComments()
            {
                ReverseMyContent.Visibility = Visibility.Visible;
                if (MyTolComments != null && MyTolComments.Count != 0)
                {
                    foreach (var c in MyTolComments)
                    {
                        MyCommentsList.Add(new Comment
                        {
                            name = c.name,
                            comment = c.comment,
                            date = c.date,
                            against = "反对(" + c.against + ")",
                            support = "支持(" + c.support + ")",
                            preComments = String.Equals(c.pid, "0") ? null : RemoveMySpecialString(GetMyPastComments(c.tid)),
                            locate = c.locate,
                            myBorderThickness = c.pid.Equals("0") ? SetMyThickness(0) : SetMyThickness(2)

                        });
                    }
                    if (MyCommentsList.Count > 1)
                    {
                        if (MainPage.MyCommentDirection == "1")
                        {
                            MyReverse();
                        }
                    }
                    else
                    {
                        ReverseMyContent.Visibility = Visibility.Collapsed;
                    }

                }
                else
                {
                    MyCommentsList.Add(new Comment
                    {
                        comment = "似乎没有人评论"
                    });
                    ReverseMyContent.Visibility = Visibility.Collapsed;
                }
            }

            private void SetHotComments()
            {
                if (MyTolComments != null && MyTolComments.Count != 0)
                {
                    MyTolComments.ForEach((c) =>
                    {
                        if (int.Parse(c.support) > 7 || int.Parse(c.against) > 13)
                            MyCommentsList.Add(new Comment
                            {
                                name = c.name,
                                comment = c.comment,
                                date = c.date,
                                against = "反对(" + c.against + ")",
                                support = "支持(" + c.support + ")",
                                preComments = String.Equals(c.pid, "0") ? null : RemoveMySpecialString(GetMyPastComments(c.tid)),
                                locate = c.locate,
                                myBorderThickness = c.pid.Equals("0") ? SetMyThickness(0): SetMyThickness(2)
                            });
                    });
                    if (MyCommentsList.Count == 0)
                    {
                        MyCommentsList.Add(new Comment
                        {
                            comment = "似乎没有人评论"
                        });
                    }
                    if (MyCommentsList.Count > 1)
                    {
                        if (MainPage.MyCommentDirection == "1")
                        {
                            MyReverse();
                        }
                    }
                    else
                    {
                        ReverseMyContent.Visibility = Visibility.Collapsed;
                    }

                }
                else
                {
                    MyCommentsList.Add(new Comment
                    {
                        comment = "似乎没有人评论"
                    });
                    ReverseMyContent.Visibility = Visibility.Collapsed;
                }
            }

            private void ReverseMyContent_Click(object sender, RoutedEventArgs e)
            {
                if (MainPage.MyCommentDirection == "0")
                {
                    MainPage.MyCommentDirection = "1";
                    ReverseMyContent.Content = "倒序";
                }
                else
                {
                    MainPage.MyCommentDirection = "0";
                    ReverseMyContent.Content = "正序";
                }
                MyReverse();
            }

            private void MyReverse()
            {
                Comment t = new Comment();
                var num = MyCommentsList.Count;
                for (int i = 0; i < num / 2; i++)
                {
                    t = MyCommentsList[i];
                    MyCommentsList[i] = MyCommentsList[num - i - 1];
                    MyCommentsList[num - 1 - i] = t;
                }
            }

        private async void ForwardToGetData()
        {
            
            while (MainPage.myWinthin24Comments.result != null && MainPage.myWinthin24Comments.result.Count != 0)
            {
                
                MainPage.myWinthin24Comments = await CommentsWinthin24Proxy.GetResults(MyNum.ToString(), MainPage.myDetialArticleId);
                if (MainPage.myWinthin24Comments.result != null && MainPage.myWinthin24Comments.result.Count != 0)
                {
                    FormatMyList();
                }
                MyNum += 1;
            }
        }

        private void FormatMyList()
        {
            foreach (var c in MainPage.myWinthin24Comments.result)
            {
                MyTolComments.Add(new Comment
                {
                    name = string.IsNullOrEmpty(c.username) ? "匿名用户" : FormatUsername(c.username),
                    comment = c.content,
                    date = c.created_time,
                    against = c.against,
                    support = c.support,
                    tid = c.tid,
                    pid = c.pid,
                    locate = (MyLocate++).ToString() + "楼"
                });
                MyPastCommentsDic.Add(c.tid,c.content);
                MyTidPidDic.Add(c.tid,c.pid);
            }
                
        }

        private string GetMyPastComments(string tid)
        {
            string myReturnString = "";
            var curtid = tid;
            while (MyTidPidDic[curtid] != "0")
            {
                myReturnString = MyPastCommentsDic[MyTidPidDic[curtid]] + "\r\n\n" + myReturnString;
                curtid = MyTidPidDic[curtid];
            }



            //var curItem = Item;
            //while (curItem.pid != "0")
            //{
            //    foreach (var item in MyTolComments)
            //    {
            //        if (item.tid == curItem.pid)
            //        {
            //            myReturnString =item.locate + "  " + item.comment + "\r\n\n" + myReturnString;
            //            curItem = item;
            //            break;
            //        }
            //    }
            //}

            return myReturnString;
        }




        private Thickness SetMyThickness(int myThickness)
        {
            Thickness th = new Thickness(Convert.ToDouble(myThickness));
            return th;
        }

        private string RemoveMySpecialString(string MyText)
        {
            return MyText.Remove(MyText.LastIndexOf("\r\n\n"));
        }

        private void ChangeDarkRed(Button MyButton)
        {
            MyButton.Background = new SolidColorBrush(darkRed);
            MyButton.Foreground = new SolidColorBrush(white);
        }

        private void ChangeWhite(Button MyButton)
        {
            MyButton.Background = new SolidColorBrush(white);
            MyButton.Foreground = new SolidColorBrush(darkRed);
        }

        private string FormatUsername(string username)
        {
            
            username = username.Replace("android", "Android");
            username = username.Replace("ios","iOS");
            return username;
        }

        private void LoadMoreButton_Click(object sender, RoutedEventArgs e)
        {
            MyCommentsList.Clear();
            SetAllComments();
            LoadMoreButton.Visibility = Visibility.Collapsed;
        }
    }
    
}
