using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;

namespace CNB
{
    internal class CommentsCollectionList : ObservableCollection<Comment>, ISupportIncrementalLoading
    {
        private bool _busy = false;
        private int _current_page = 1;
        private bool _has_more_items = false;
        private static bool IsComLoad = false;
        public CommentsCollectionList()
        {
            HasMoreItems = true;
        }

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
            _current_page = 1;
            TotalCount = 0;
            Clear();
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
            try
            {
                if (IsComLoad == false)
                {
                    // MainPage.myComments = await CommentsProxy.GetResults((_current_page).ToString(), MainPage.myDetialArticleId);
                    MainPage.myComments = await CommentsProxy.GetResults(MainPage.myDetialArticleId);
                    IsComLoad = true;
                    if (MainPage.myComments.result != null && MainPage.myComments.result.Any())
                    {
                        actualCount = MainPage.myComments.result.Count;
                        TotalCount += actualCount;
                        _current_page++;
                        HasMoreItems = true;
                        if (MainPage.IsHotCommentsSelected == false)
                            foreach (var c in MainPage.myComments.result)
                            {
                                this.Add(new Comment
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
                        else
                            MainPage.myComments.result.ForEach((c) =>
                            {
                                if (int.Parse(c.support) > 7 || int.Parse(c.against) > 13)
                                    this.Add(new Comment
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
                    }
                    else
                    {
                        if (this.Count == 0)
                            this.Add(new Comment
                            {
                                //content = "似乎没有人评论"
                                comment = "似乎没有人评论"
                            });
                        HasMoreItems = false;
                    }
                }
                else
                {
                    HasMoreItems = false;
                }
                
            }
            catch (Exception)
            {
                HasMoreItems = false;
            }

            

            _busy = false;
            return new LoadMoreItemsResult
            {
                Count = (uint)actualCount
            };
        }
    }
}