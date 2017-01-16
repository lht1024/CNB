using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;

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
            try
            {
                    MainPage.myData = await NewsProxy.GetNews(MainPage.myLastArticleId);
                    MainPage.myLastArticleId = MainPage.myData.result[MainPage.myData.result.Count - 1].sid;
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
                foreach (var item in MainPage.myData.result)
                {
                    this.Add(new News
                    {
                        title = item.title,
                        pubtime = item.pubtime,
                        summary = item.summary,
                        sid = item.sid,
                        counter = item.counter,
                        comments = item.comments,
                        thumb = item.thumb
                    });
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
            DataLoaded?.Invoke();
            _busy = false;
            return new LoadMoreItemsResult
            {
                Count = (uint)actualCount
            };
        }
    }
}