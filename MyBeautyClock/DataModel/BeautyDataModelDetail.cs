using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBeautyClock.DataModel
{
    class BeautyDataModelDetail : BeautyClockDataCommon
    {
        private int _cuteCount = 0;
        public int CuteCount
        {
            get { return this._cuteCount; }
            set { this.SetProperty(ref this._cuteCount, value); }
        }
    }
}
