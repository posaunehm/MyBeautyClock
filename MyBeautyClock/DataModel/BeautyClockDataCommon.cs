using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBeautyClock.Common;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace MyBeautyClock.DataModel
{
    class BeautyClockDataCommon : BindableBase
    {
        private ImageSource _image = null;
        private String _imagePath = null;
        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._imagePath != null)
                {
                    this._image = new BitmapImage(new Uri(this._imagePath));
                }
                return this._image;
            }
            set
            {
                this._imagePath = null;
                this.SetProperty(ref this._image, value);
            }
        }

        public void SetImage(String path)
        {
            this._image = null;
            this._imagePath = path;
            this.OnPropertyChanged("Image");
        }

        private string _beautyName = string.Empty;
        public string BeautyName
        {
            get { return this._beautyName; }
            set { this.SetProperty(ref this._beautyName, value); }
        }

        private string _regionName = string.Empty;
        public string Region
        {
            get { return this._regionName; }
            set { this.SetProperty(ref this._regionName, value); }
        }
    }
}
