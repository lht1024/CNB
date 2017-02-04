using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.UI.Notifications;
using NotificationsExtensions.Tiles;

namespace BackgroundTasks
{
    public sealed class ComputeMD5
    {
        public static string GetMD5(string str)
        {
            var alg = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            IBuffer buff = CryptographicBuffer.ConvertStringToBinary(str, BinaryStringEncoding.Utf8);
            var hashed = alg.HashData(buff);
            var res = CryptographicBuffer.EncodeToHexString(hashed);
            return res;
        }

        public static string GetTimeStop()
        {
            return ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString();
        }
    }

    public sealed class NewsRefreshBackgroundTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            try
            {
                var feed = await GetNewsByRank();
                UpdateTile(feed);
            }
            catch
            {
            }

            deferral.Complete();
        }

        private async static Task<ULNewsRaw> GetNewsByRank()
        {
            var timstamp = ComputeMD5.GetTimeStop();
            var toBehashed = String.Format("app_key=10000&format=json&method=Article.TodayRank&timestamp={0}&type={1}&v=1.0&mpuffgvbvbttn3Rc", timstamp, "dig");
            var Md5 = ComputeMD5.GetMD5(toBehashed);
            var mylink = String.Format("http://api.cnbeta.com/capi?app_key=10000&format=json&method=Article.TodayRank&timestamp={0}&type={1}&v=1.0&sign={2}", timstamp, "dig", Md5);

            var http = new HttpClient();
            var response = await http.GetAsync(mylink);
            var result = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ULNewsRaw>(result);
            return data;
        }

        private void UpdateTile(ULNewsRaw feed)
        {
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.EnableNotificationQueueForWide310x150(true);
            updater.EnableNotificationQueueForSquare310x310(true);
            updater.EnableNotificationQueueForSquare150x150(true);
            updater.EnableNotificationQueue(true);
            updater.Clear();
            int itemCount = 0;
            foreach (var item in feed.result)
            {
                TileContent content = new TileContent()
                {
                    Visual = new TileVisual()
                    {
                        TileMedium = new TileBinding()
                        {
                            Content = new TileBindingContentAdaptive()
                            {
                                Children =
                                {
                                    new NotificationsExtensions.AdaptiveText()
                                    {
                                        Text = item.title,
                                        HintWrap = true
                                    }
                                }
                            }
                        },

                        TileWide = new TileBinding()
                        {
                            Content = new TileBindingContentAdaptive()
                            {
                                Children =
                                {
                                    new NotificationsExtensions.AdaptiveText()
                                    {
                                        Text = item.title,
                                        HintWrap = true,
                                        HintStyle = NotificationsExtensions.AdaptiveTextStyle.Base
                                    }
                                    
                                }
                            }
                        },
                        TileLarge = new TileBinding()
                        {
                            Content = new TileBindingContentAdaptive()
                            {
                                Children =
                                {
                                    new NotificationsExtensions.AdaptiveImage()
                                    {
                                        Source = item.thumb,
                                        HintCrop = NotificationsExtensions.AdaptiveImageCrop.Circle,
                                        HintAlign = NotificationsExtensions.AdaptiveImageAlign.Left,
                                        AlternateText = item.title
                                    },
                                    new NotificationsExtensions.AdaptiveText()
                                    {
                                        Text = item.title,
                                        HintAlign = NotificationsExtensions.AdaptiveTextAlign.Left,
                                        HintWrap = true,
                                        HintStyle = NotificationsExtensions.AdaptiveTextStyle.Base
                                    }
                                }
                            }
                        }
                    }
                };
                var notification = new TileNotification(content.GetXml());
                updater.Update(notification);
                if (itemCount++ > 5) break;
            }
            

        }
    }

    public sealed class ULNewsRaw
    {
        public IList<UNewsRaw> result { get; set; }
        public string status { get; set; }
    }

    public sealed class UNewsRaw
    {
        public string pubtime { get; set; }
        public string summary { get; set; }
        public string title { get; set; }
        public string thumb { get; set; }
    }
}