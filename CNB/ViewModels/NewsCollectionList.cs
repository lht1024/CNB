﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace CNB
{
    internal class NewsCollectionList : ObservableCollection<News>, ISupportIncrementalLoading
    {
        private bool _busy = false;
        private bool _has_more_items = false;

        public NewsCollectionList()
        {
            HasMoreItems = true;
        }

        public event DataLoadedEventHandler DataLoaded;

        public event DataLoadingEventHandler DataLoading;

        public bool HasMoreItems
        {
            get
            {
                if (_busy)
                    return false;
                else
                    return _has_more_items;
            }
            private set
            {
                _has_more_items = value;
            }
        }

        public int TotalCount
        {
            get; set;
        }

        public void DoRefresh()
        {
            TotalCount = 0;
            Clear();
            MainPage.myLastArticleId = "";
            MainPage.Count = false;
            HasMoreItems = true;
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return InnerLoadMoreItemsAsync(count).AsAsyncOperation();
        }

        private async Task<LoadMoreItemsResult> InnerLoadMoreItemsAsync(uint expectedCount)
        {
            _busy = true;
            var actualCount = 0;
            DataLoading?.Invoke();
            if (MainPage.Count == false)
            {
                try
                {
                    if (MainPage.Filter == 1)
                    {
                        MainPage.myData = await NewsProxy.GetTop10();
                        MainPage.Count = true;
                    }
                    else if (MainPage.Filter == 2)
                    {
                        MainPage.myData = await NewsProxy.GetHotComments();
                        MainPage.Count = true;
                    }
                    else if (MainPage.Filter == 3)
                    {
                        MainPage.myData = await NewsProxy.GetNewsByRank(MainPage.RankSelected);
                        MainPage.Count = true;
                    }
                    else if (MainPage.Filter == 4)
                    {
                        MainPage.myData = await NewsProxy.GetNewsByTopic(MainPage.myLastArticleId, MainPage.TopicSelected);
                        MainPage.myLastArticleId = MainPage.myData.result[MainPage.myData.result.Count - 1].sid;
                    }
                    else
                    {
                        MainPage.myData = await NewsProxy.GetNews(MainPage.myLastArticleId);
                        MainPage.myLastArticleId = MainPage.myData.result[MainPage.myData.result.Count - 1].sid;
                    }
                }
                catch (Exception)
                {
                    HasMoreItems = false;
                }
                if (MainPage.myData != null && MainPage.myData.result.Any())
                {
                    actualCount = MainPage.myData.result.Count;
                    TotalCount += actualCount;
                    HasMoreItems = true;
                    if (MainPage.Filter == 2)
                    {
                        foreach (var item in MainPage.myData.result)
                        {
                                this.Add(new News
                                {
                                    title = ResetMyTitle(item.title),
                                    summary = ResetMySummary(item.description),
                                    //sid = item.from_id,
                                    sid = ResetMySid(item.description),
                                    pubtime = "",
                                    comments = "",
                                    counter = ""
                            });
                        }
                    }
                    else
                    {
                        if (MainPage.Filter == 0 && MainPage.IHateApple == "1")
                        {
                            MainPage.myData.result = MainPage.myData.result.Where(p => p.topic != "9" && p.topic != "464" && p.topic != "379"
                                         && p.topic != "343" && p.topic != "79" && p.topic != "66" && p.topic != "158" && p.topic != "535").ToList();
                        }
                        
                        foreach (var item in MainPage.myData.result)
                        {
                            this.Add(new News
                            {
                                title = item.title,
                                pubtime = item.pubtime,
                                summary = item.summary.Replace("&quot;", "\""),
                                sid = item.sid,
                                counter = item.counter + "次点击",
                                comments = item.comments + "条评论",
                                thumb = new BitmapImage(new Uri(item.thumb, UriKind.Absolute))
                            });
                        }
                    }
                }
                else
                {
                    this.Add(new News
                    {
                        summary = "似乎并没有网络连接"
                    });
                    HasMoreItems = false;
                }
            }
            else
            {
                HasMoreItems = false;
            }

            DataLoaded?.Invoke();
            _busy = false;
            return new LoadMoreItemsResult
            {
                Count = (uint)actualCount
            };
        }

        private string ResetMyTitle(string title)
        {
            string MyText = Regex.Replace(title, "<a.+?>", "");
            MyText = MyText.Replace("</a>","");
            return MyText;
        }

        private string ResetMySummary(string description)
        {
            string MyText = description.Replace("<strong>","");
            MyText = MyText.Replace("</strong>", "");
            MyText = MyText.Replace("</a>", "");
            MyText = Regex.Replace(MyText, "<a.+?>","");
            MyText = MyText.Replace(":", "  ");
            return MyText;
        }

        private string ResetMySid(string description)
        {
            var data  = Regex.Split(description, ".htm", RegexOptions.IgnoreCase);
            var sid = data[0].Split('/');
            var num = sid.Count() - 1;
            return sid[num];
        }
    }
}