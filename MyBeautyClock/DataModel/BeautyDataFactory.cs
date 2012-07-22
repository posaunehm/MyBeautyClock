using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using System.IO;

namespace MyBeautyClock.DataModel
{
    public class BeautyDataFactory
    {
        public async Task<BitmapImage> GetBijinPicture(string Path)
        {
            var date = DateTime.Now;
            var url = "http://www.bijint.com/" + Path + "/tokei_images/" + date.ToString("HHmm") + ".jpg";

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Referrer = new Uri("http://www.bijint.com/" + Path);
            BitmapImage bitmap;
            using (var strm = await client.GetStreamAsync(new Uri(url)))
            {
                // BitmapImageインスタンスへはStream型をそのまま読み込ませることができないため、
                // InMemoryRandomAccessStreamへソースストリームをコピーする
                InMemoryRandomAccessStream ims = new InMemoryRandomAccessStream();
                var output = ims.GetOutputStreamAt(0);
                await RandomAccessStream.CopyAsync(strm.AsInputStream(), output);

                // BitmapImageへソースを設定し、Imageコントロールで表示させる
                bitmap = new BitmapImage();
                bitmap.SetSource(ims);
            }
            return bitmap;

            //var html = await client.GetStringAsync("http://www.bijint.com/kobe/cache/" + date.ToString("HHmm") + ".html");
            //this.html.NavigateToString(html);
        }
    }
}
