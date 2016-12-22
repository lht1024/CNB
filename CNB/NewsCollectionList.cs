using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;

namespace CNB
{
    class NewsCollectionList : ObservableCollection<News>, ISupportIncrementalLoading
    {
        private bool _busy = false;
        private bool _has_more_items = false;
        public event DataLoadedEventHandler DataLoaded;
        public event DataLoadingEventHandler DataLoading;

        public int TotalCount
        {
            get; set;
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
        public NewsCollectionList()
        {
            HasMoreItems = true;
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
                    MainPage.myLastArticleId = MainPage.myData.Results[MainPage.myData.Results.Count - 1].article_id;
            }
            catch (Exception)
            {
                HasMoreItems = false;
            }


            if (MainPage.myData != null && MainPage.myData.Results.Any())
            {
                actualCount = MainPage.myData.Results.Count;
                TotalCount += actualCount;
                HasMoreItems = true;
                if (MainPage.Filter == "0")
                    MainPage.myData.Results.ForEach((c) =>
                    {
                        this.Add(new News
                        {
                            title = c.title,
                            date = c.date,
                            intro = c.intro,
                            article_id = c.article_id,
                            source = c.source
                        });
                    });
                else
                    foreach(var item in MainPage.myData.Results)
                    {
                        if(item.source != "威锋网")
                        this.Add(new News
                        {
                            title = item.title,
                            date = item.date,
                            intro = item.intro,
                            article_id = item.article_id,
                            source = item.source
                        });
                    }
            }
            else
            {
                this.Add(new News
                {
                    intro = "似乎并没有网络连接"
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
