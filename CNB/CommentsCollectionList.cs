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
    class CommentsCollectionList : ObservableCollection<Comment>, ISupportIncrementalLoading
    {
        private bool _busy = false;
        private bool _has_more_items = false;
        private int _current_page = 1;

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
        public CommentsCollectionList()
        {
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

                MainPage.myComments = await CommentsProxy.GetResults((_current_page).ToString(), MainPage.myDetialArticleId);


            }
            catch (Exception)
            {
                HasMoreItems = false;
            }

            if (MainPage.myComments.result != null && MainPage.myComments.result.Any())
            {
                actualCount = MainPage.myComments.result.Count;
                TotalCount += actualCount;
                _current_page++;
                HasMoreItems = true;
                MainPage.myComments.result.ForEach((c) =>
                {
                    this.Add(new Comment
                    {

                        username = (c.username.Contains("") ? "匿名用户" : c.username),
                        content = c.content,
                        created_time = c.created_time,
                        against = "反对(" + c.against +")",
                        support = "支持(" + c.support +")",
                        tid = c.tid
                    });
                });
            }
            else
            {
                if(this.Count == 0)
                this.Add(new Comment
                {
                    content = "似乎没有人评论"
                });
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
