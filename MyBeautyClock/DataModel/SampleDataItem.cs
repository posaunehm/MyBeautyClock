using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using System.IO;
using MyBeautyClock.Logic;
using System.Text.RegularExpressions;
using System.Windows.Input;
using MyBeautyClock.Common;

namespace MyBeautyClock.Data
{
    public class SampleDataItem : SampleDataCommon
    {
        public SampleDataItem(String uniqueId, String title, String subtitle, String imagePath, String description, String content, SampleDataGroup group)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            this._content = content;
            this._group = group;

        }

        private string _content = string.Empty;
        public string Content
        {
            get { return this._content; }
            set { this.SetProperty(ref this._content, value); }
        }

        private SampleDataGroup _group;
        public SampleDataGroup Group
        {
            get { return this._group; }
            set { this.SetProperty(ref this._group, value); }
        }

        private string path;

        public string Path
        {
            get { return path; }
            set
            {
                path = value;
                this.Current = DateTime.Now;
                this.GetBijinInfo();
            }
        }
        //string _name;
        //public string Name
        //{
        //    get { return this.Title; }
        //    set { this.SetProperty(ref this._name, value); }
        //}

        public DateTime Current { get; set; }

        private int _point = 0;
        public int Point
        {
            get { return this._point; }
            set { this.SetProperty(ref this._point, value); }
        }


        // TODO: 一分ごとに画像と名前を取得する
        private async void GetBijinInfo()
        {
            var date = this.Current;
            getKawaii(this.path + this.Current.ToString("HHmm"));
            var url = "http://www.bijint.com/" + Path + "/tokei_images/" + date.ToString("HHmm") + ".jpg";

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Referrer = new Uri("http://www.bijint.com/" + path);
            using (var strm = await client.GetStreamAsync(new Uri(url)))
            {
                // BitmapImageインスタンスへはStream型をそのまま読み込ませることができないため、
                // InMemoryRandomAccessStreamへソースストリームをコピーする

                InMemoryRandomAccessStream ims = new InMemoryRandomAccessStream();
                
                var output = ims.GetOutputStreamAt(0);
                await RandomAccessStream.CopyAsync(strm.AsInputStream(), output);

                // BitmapImageへソースを設定し、Imageコントロールで表示させる
                var bitmap = new BitmapImage();
                bitmap.SetSource(ims);
                this.Image = bitmap;
            }

            var html = await client.GetStringAsync("http://www.bijint.com/" + Path + "/cache/" + date.ToString("HHmm") + ".html");
            var lines = html.Split('\n').Select(input => input.Trim()).Where(input => input != "\r" || input != "").ToList();
            var nameLineIndex = lines.IndexOf("<thead>") + 2;
            var nameLine = lines[nameLineIndex];
            var temp = nameLine.Substring(0, nameLine.Length - 5);
            var lastEnd = temp.LastIndexOf('>');
            this.Subtitle = temp.Substring(lastEnd + 1);


        }

        public void ExecNext()
        {
            this.Current = this.Current.AddMinutes(1);
            this.GetBijinInfo();
        }
        public void ExecBefore()
        {
            this.Current = this.Current.AddMinutes(-1);
            this.GetBijinInfo();
        }
        public void ExecCurrent()
        {
            this.Current = DateTime.Now;
            this.GetBijinInfo();
        }
        public void ExecRefresh()
        {
            this.GetBijinInfo();
        }



        private async void getKawaii(string id)
        {
            var url = "http://mybeautyclock.appspot.com/Dodeska";
            var path = url;
            path += "?id=" + id;

            int kawaii = 0;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Referrer = new Uri(path);
            //using (var strm = await client.GetStreamAsync(new Uri(url)))
            //{
            //    //JSON result: true/false, kawaii:<point>, id:<url>

            //}

            var json = await client.GetStringAsync(path);
            var k = json.Split(',').FirstOrDefault(s => s.Contains("kawaii"));
            if (k != null)
            {
                int.TryParse(k.Split(':')[1], out kawaii);
            }
            this.Point = kawaii;
        }
    }
}
