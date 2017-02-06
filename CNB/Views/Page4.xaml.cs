using System;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace CNB.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Page4 : Page
    {
        public Page4()
        {
            this.InitializeComponent();
            Update();
            MyConDirSwitch.IsOn = (MainPage.MyCommentDirection == "0") ? true : false;
            HateAppleSwitch.IsOn = (MainPage.IHateApple == "1") ? true : false;
            MyFontSizeSlider.Value = Convert.ToDouble(MainPage.MyFontSize);
            MyLeSpacingSlider.Value = Convert.ToDouble(MainPage.MyLeSpacing);
            MyPaPaddingSlider.Value = Convert.ToDouble(MainPage.MyPaPadding);
        }

        private void MyFontSizeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            MyFontSizeSlider.Header = "字号： " + MyFontSizeSlider.Value.ToString();
            MainPage.MyFontSize = MyFontSizeSlider.Value.ToString();
            MainPage.SetMySetting(MyFontSizeSlider.Value.ToString(), "MyFontSize");
        }

        private void MyLeSpacingSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            MyLeSpacingSlider.Header = "字间距： " + MyLeSpacingSlider.Value.ToString();
            MainPage.MyLeSpacing = MyLeSpacingSlider.Value.ToString();
            MainPage.SetMySetting(MyLeSpacingSlider.Value.ToString(), "MyLeSpacing");
        }

        private void MyPaPaddingSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            MyPaPaddingSlider.Header = "段间距： " + MyPaPaddingSlider.Value.ToString();
            MainPage.MyPaPadding = MyPaPaddingSlider.Value.ToString();
            MainPage.SetMySetting(MyPaPaddingSlider.Value.ToString(), "MyPaPadding");
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            MainPage.MyFontSize = "16";
            MainPage.MyLeSpacing = "0";
            MainPage.MyPaPadding = "0";
            MyFontSizeSlider.Value = Convert.ToDouble(MainPage.MyFontSize);
            MyLeSpacingSlider.Value = Convert.ToDouble(MainPage.MyLeSpacing);
            MyPaPaddingSlider.Value = Convert.ToDouble(MainPage.MyPaPadding);
            Update();
        }

        private async void Update()
        {
            var ls = "p{letter-spacing:" + MainPage.MyLeSpacing + "px}";
            var pp = "p{padding:" + MainPage.MyPaPadding + "px 0}";
            var fz = "p{ font-size:" + MainPage.MyFontSize + "px}";
            var ff = "p{ font-family:\"微软雅黑\"}";

            var bodytext = "<body><p> cnBeta.com成立于 2003 年，是中国领先的即时科技资讯站点，已成为重要的互联网IT消息集散地，提供软件更新，互联网、IT业界资讯、评论、观点和访谈。</p><p> 我们的核心竞争力：快速响应；报道立场公正中立；尽可能提供关联信息；网友讨论气氛浓厚。</p></body> ";
            var headtext = String.Format("<head><meta http-equiv=\"Content-Type\" content=\"text/html; charset = utf-8\"><style>{0}{1}{2}{3}</style></head>", ls, pp, fz, ff);
            var filtler = Page1.Clear(headtext + bodytext);

            IStorageFolder local = ApplicationData.Current.LocalFolder;
            IStorageFolder dataFolder = await local.CreateFolderAsync("DataFile", CreationCollisionOption.OpenIfExists);
            IStorageFile file = await dataFolder.CreateFileAsync("SettingPage.html", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, filtler);
            SetWebView.Source = new Uri("ms-appdata:///local/DataFile/SettingPage.html", UriKind.RelativeOrAbsolute);
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            Update(); ;
        }

        private void MyConDirSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (MyConDirSwitch.IsOn)
            {
                MainPage.SetMySetting("0", "MyConDir");
                MainPage.MyCommentDirection = "0";
            }
            else
            {
                MainPage.SetMySetting("1", "MyConDir");
                MainPage.MyCommentDirection = "1";
            }

        }

        private void HateAppleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (HateAppleSwitch.IsOn)
            {
                MainPage.SetMySetting("1", "IHA");
                if(MainPage.IHateApple != "1")
                    MainPage.IHateApple = "1";
            }
            else
            {
                MainPage.SetMySetting("0", "IHA");
                if (MainPage.IHateApple != "0")
                    MainPage.IHateApple = "0";
            }
        }
    }
}