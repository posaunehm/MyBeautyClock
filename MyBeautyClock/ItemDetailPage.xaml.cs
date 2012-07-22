using MyBeautyClock.Data;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using System.Net.Http;
using System.Threading.Tasks;


// アイテム詳細ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234232 を参照してください

namespace MyBeautyClock
{
    /// <summary>
    /// グループ内の単一のアイテムに関する詳細情報を表示するページです。同じグループに属する他の
    /// アイテムにフリップするジェスチャを使用できます。
    /// </summary>
    public sealed partial class ItemDetailPage : MyBeautyClock.Common.LayoutAwarePage
    {
        public ItemDetailPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // Allow saved page state to override the initial item to display
            if (pageState != null && pageState.ContainsKey("SelectedItem"))
            {
                navigationParameter = pageState["SelectedItem"];
            }

            // TODO: Create an appropriate data model for your problem domain to replace the sample data
            var item = SampleDataSource.GetItem((String)navigationParameter);
            this.DefaultViewModel["Group"] = item.Group;
            this.DefaultViewModel["Items"] = item.Group.Items;
            this.flipView.SelectedItem = item;
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
            var selectedItem = (SampleDataItem)this.flipView.SelectedItem;
            pageState["SelectedItem"] = selectedItem.UniqueId;
        }

        private void btnBefore_Click(object sender, RoutedEventArgs e)
        {
            var item = flipView.SelectedItem as SampleDataItem;
            if (item != null)
            {
                item.ExecBefore();
            }
        }

        private void btnCurrent_Click(object sender, RoutedEventArgs e)
        {
            var item = flipView.SelectedItem as SampleDataItem;
            if (item != null)
            {
                item.ExecCurrent();
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            var item = flipView.SelectedItem as SampleDataItem;
            if (item != null)
            {
                item.ExecNext();
            }
        }

        private void btnKawaiiClick(object sender, RoutedEventArgs e)
        {
            var item = flipView.SelectedItem as SampleDataItem;
            if (item != null)
            {
                postKawaii(item.Path + item.Current.ToString("HHmm"));
                item.ExecRefresh();
            }

        }

        private async void postKawaii(string id)
        {
            var lid = "hogehoge1" + Guid.NewGuid().ToString();
            var url = "http://mybeautyclock.appspot.com/Kawaii";
            var path = url;
            path += "?id=" + id;
            path += "&lid=" + lid;


            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Referrer = new Uri(path);
            using (var strm = await client.GetStreamAsync(new Uri(path)))
            {
                //JSON result: true/false
            }
        }


    }
}
