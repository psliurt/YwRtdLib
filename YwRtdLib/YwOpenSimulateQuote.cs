using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YwRtdLib
{
    public class YwOpenSimulateQuote
    {
        //private string _name;
        //public bool NameSet = false;
        //public string Name
        //{
        //    get { return _name; }
        //    set
        //    {
        //        _name = value;
        //        NameSet = true;
        //    }
        //}

        private string _symbol;
        public bool SymbolSet = false;
        public string Symbol
        {
            get { return _symbol; }
            set
            {
                _symbol = value;
                SymbolSet = true;
            }
        }

        private string _preOpenAskPrice;
        public bool IsPreOpenAskPriceUpdate = false;
        public string PreOpenAskPrice
        {
            get { return _preOpenAskPrice; }
            set
            {
                if (value != _preOpenAskPrice)
                {
                    _preOpenAskPrice = value;
                    IsPreOpenAskPriceUpdate = true;
                }
                else
                {
                    IsPreOpenAskPriceUpdate = false;
                }
            }
        }

        private string _preOpenAskVolume;
        public bool IsPreOpenAskVolumeUpdate = false;
        public string PreOpenAskVolume
        {
            get { return _preOpenAskVolume; }
            set
            {
                if (value != _preOpenAskVolume)
                {
                    _preOpenAskVolume = value;
                    IsPreOpenAskVolumeUpdate = true;
                }
                else
                {
                    IsPreOpenAskVolumeUpdate = false;
                }
            }
        }

        private string _preOpenBidPrice;
        public bool IsPreOpenBidPriceUpdate = false;
        public string PreOpenBidPrice
        {
            get { return _preOpenBidPrice; }
            set
            {
                if (value != _preOpenBidPrice)
                {
                    _preOpenBidPrice = value;
                    IsPreOpenBidPriceUpdate = true;
                }
                else
                {
                    IsPreOpenBidPriceUpdate = false;
                }
            }
        }

        private string _preOpenBidVolume;
        public bool IsPreOpenBidVolumeUpdate = false;
        public string PreOpenBidVolume
        {
            get { return _preOpenBidVolume; }
            set
            {
                if (value != _preOpenBidVolume)
                {
                    _preOpenBidVolume = value;
                    IsPreOpenBidVolumeUpdate = true;
                }
                else
                {
                    IsPreOpenBidVolumeUpdate = false;
                }
            }
        }

        private string _preOpenPrice;
        public bool IsPreOpenPriceUpdate = false;
        public string PreOpenPrice
        {
            get { return _preOpenPrice; }
            set
            {
                if (value != _preOpenPrice)
                {
                    _preOpenPrice = value;
                    IsPreOpenPriceUpdate = true;
                }
                else
                {
                    IsPreOpenPriceUpdate = false;
                }
            }
        }

        private string _preOpenVolume;
        public bool IsPreOpenVolumeUpdate = false;
        public string PreOpenVolume
        {
            get { return _preOpenVolume; }
            set
            {
                if (value != _preOpenVolume)
                {
                    _preOpenVolume = value;
                    IsPreOpenVolumeUpdate = true;
                }
                else
                {
                    IsPreOpenVolumeUpdate = false;
                }
            }
        }

        private string _preOpenBestBidPrice1;
        public bool IsPreOpenBestBidPrice1Update = false;
        public string PreOpenBestBidPrice1
        {
            get { return _preOpenBestBidPrice1; }
            set
            {
                if (value != _preOpenBestBidPrice1)
                {
                    _preOpenBestBidPrice1 = value;
                    IsPreOpenBestBidPrice1Update = true;
                }
                else
                {
                    IsPreOpenBestBidPrice1Update = false;
                }
            }
        }

        private string _preOpenBestAskPrice1;
        public bool IsPreOpenBestAskPrice1Update = false;
        public string PreOpenBestAskPrice1
        {
            get { return _preOpenBestAskPrice1; }
            set
            {
                if (value != _preOpenBestAskPrice1)
                {
                    _preOpenBestAskPrice1 = value;
                    IsPreOpenBestAskPrice1Update = true;
                }
                else
                {
                    IsPreOpenBestAskPrice1Update = false;
                }
            }
        }

        private string _preOpenBestBidVolume1;
        public bool IsPreOpenBestBidVolume1Update = false;
        public string PreOpenBestBidVolume1
        {
            get { return _preOpenBestBidVolume1; }
            set
            {
                if (value != _preOpenBestBidVolume1)
                {
                    _preOpenBestBidVolume1 = value;
                    IsPreOpenBestBidVolume1Update = true;
                }
                else
                {
                    IsPreOpenBestBidVolume1Update = false;
                }
            }
        }

        private string _preOpenBestAskVolume1;
        public bool IsPreOpenBestAskVolume1Update = false;
        public string PreOpenBestAskVolume1
        {
            get { return _preOpenBestAskVolume1; }
            set
            {
                if (value != _preOpenBestAskVolume1)
                {
                    _preOpenBestAskVolume1 = value;
                    IsPreOpenBestAskVolume1Update = true;
                }
                else
                {
                    IsPreOpenBestAskVolume1Update = false;
                }
            }
        }

        private string _preOpenBestBidPrice2;
        public bool IsPreOpenBestBidPrice2Update = false;
        public string PreOpenBestBidPrice2
        {
            get { return _preOpenBestBidPrice2; }
            set
            {
                if (value != _preOpenBestBidPrice2)
                {
                    _preOpenBestBidPrice2 = value;
                    IsPreOpenBestBidPrice2Update = true;
                }
                else
                {
                    IsPreOpenBestBidPrice2Update = false;
                }
            }
        }

        private string _preOpenBestAskPrice2;
        public bool IsPreOpenBestAskPrice2Update = false;
        public string PreOpenBestAskPrice2
        {
            get { return _preOpenBestAskPrice2; }
            set
            {
                if (value != _preOpenBestAskPrice2)
                {
                    _preOpenBestAskPrice2 = value;
                    IsPreOpenBestAskPrice2Update = true;
                }
                else
                {
                    IsPreOpenBestAskPrice2Update = false;
                }
            }
        }

        private string _preOpenBestBidVolume2;
        public bool IsPreOpenBestBidVolume2Update = false;
        public string PreOpenBestBidVolume2
        {
            get { return _preOpenBestBidVolume2; }
            set
            {
                if (value != _preOpenBestBidVolume2)
                {
                    _preOpenBestBidVolume2 = value;
                    IsPreOpenBestBidVolume2Update = true;
                }
                else
                {
                    IsPreOpenBestBidVolume2Update = false;
                }
            }
        }

        private string _preOpenBestAskVolume2;
        public bool IsPreOpenBestAskVolume2Update = false;
        public string PreOpenBestAskVolume2
        {
            get { return _preOpenBestAskVolume2; }
            set
            {
                if (value != _preOpenBestAskVolume2)
                {
                    _preOpenBestAskVolume2 = value;
                    IsPreOpenBestAskVolume2Update = true;
                }
                else
                {
                    IsPreOpenBestAskVolume2Update = false;
                }
            }
        }

        private string _preOpenBestBidPrice3;
        public bool IsPreOpenBestBidPrice3Update = false;
        public string PreOpenBestBidPrice3
        {
            get { return _preOpenBestBidPrice3; }
            set
            {
                if (value != _preOpenBestBidPrice3)
                {
                    _preOpenBestBidPrice3 = value;
                    IsPreOpenBestBidPrice3Update = true;
                }
                else
                {
                    IsPreOpenBestBidPrice3Update = false;
                }
            }
        }

        private string _preOpenBestAskPrice3;
        public bool IsPreOpenBestAskPrice3Update = false;
        public string PreOpenBestAskPrice3
        {
            get { return _preOpenBestAskPrice3; }
            set
            {
                if (value != _preOpenBestAskPrice3)
                {
                    _preOpenBestAskPrice3 = value;
                    IsPreOpenBestAskPrice3Update = true;
                }
                else
                {
                    IsPreOpenBestAskPrice3Update = false;
                }
            }
        }

        private string _preOpenBestBidVolume3;
        public bool IsPreOpenBestBidVolume3Update = false;
        public string PreOpenBestBidVolume3
        {
            get { return _preOpenBestBidVolume3; }
            set
            {
                if (value != _preOpenBestBidVolume3)
                {
                    _preOpenBestBidVolume3 = value;
                    IsPreOpenBestBidVolume3Update = true;
                }
                else
                {
                    IsPreOpenBestBidVolume3Update = false;
                }
            }
        }

        private string _preOpenBestAskVolume3;
        public bool IsPreOpenBestAskVolume3Update = false;
        public string PreOpenBestAskVolume3
        {
            get { return _preOpenBestAskVolume3; }
            set
            {
                if (value != _preOpenBestAskVolume3)
                {
                    _preOpenBestAskVolume3 = value;
                    IsPreOpenBestAskVolume3Update = true;
                }
                else
                {
                    IsPreOpenBestAskVolume3Update = false;
                }
            }
        }

        private string _preOpenBestBidPrice4;
        public bool IsPreOpenBestBidPrice4Update = false;
        public string PreOpenBestBidPrice4
        {
            get { return _preOpenBestBidPrice4; }
            set
            {
                if (value != _preOpenBestBidPrice4)
                {
                    _preOpenBestBidPrice4 = value;
                    IsPreOpenBestBidPrice4Update = true;
                }
                else
                {
                    IsPreOpenBestBidPrice4Update = false;
                }
            }
        }

        private string _preOpenBestAskPrice4;
        public bool IsPreOpenBestAskPrice4Update = false;
        public string PreOpenBestAskPrice4
        {
            get { return _preOpenBestAskPrice4; }
            set
            {
                if (value != _preOpenBestAskPrice4)
                {
                    _preOpenBestAskPrice4 = value;
                    IsPreOpenBestAskPrice4Update = true;
                }
                else
                {
                    IsPreOpenBestAskPrice4Update = false;
                }
            }
        }

        private string _preOpenBestBidVolume4;
        public bool IsPreOpenBestBidVolume4Update = false;
        public string PreOpenBestBidVolume4
        {
            get { return _preOpenBestBidVolume4; }
            set
            {
                if (value != _preOpenBestBidVolume4)
                {
                    _preOpenBestBidVolume4 = value;
                    IsPreOpenBestBidVolume4Update = true;
                }
                else
                {
                    IsPreOpenBestBidVolume4Update = false;
                }
            }
        }

        private string _preOpenBestAskVolume4;
        public bool IsPreOpenBestAskVolume4Update = false;
        public string PreOpenBestAskVolume4
        {
            get { return _preOpenBestAskVolume4; }
            set
            {
                if (value != _preOpenBestAskVolume4)
                {
                    _preOpenBestAskVolume4 = value;
                    IsPreOpenBestAskVolume4Update = true;
                }
                else
                {
                    IsPreOpenBestAskVolume4Update = false;
                }
            }
        }

        private string _preOpenBestBidPrice5;
        public bool IsPreOpenBestBidPrice5Update = false;
        public string PreOpenBestBidPrice5
        {
            get { return _preOpenBestBidPrice5; }
            set
            {
                if (value != _preOpenBestBidPrice5)
                {
                    _preOpenBestBidPrice5 = value;
                    IsPreOpenBestBidPrice5Update = true;
                }
                else
                {
                    IsPreOpenBestBidPrice5Update = false;
                }
            }
        }

        private string _preOpenBestAskPrice5;
        public bool IsPreOpenBestAskPrice5Update = false;
        public string PreOpenBestAskPrice5
        {
            get { return _preOpenBestAskPrice5; }
            set
            {
                if (value != _preOpenBestAskPrice5)
                {
                    _preOpenBestAskPrice5 = value;
                    IsPreOpenBestAskPrice5Update = true;
                }
                else
                {
                    IsPreOpenBestAskPrice5Update = false;
                }
            }
        }

        private string _preOpenBestBidVolume5;
        public bool IsPreOpenBestBidVolume5Update = false;
        public string PreOpenBestBidVolume5
        {
            get { return _preOpenBestBidVolume5; }
            set
            {
                if (value != _preOpenBestBidVolume5)
                {
                    _preOpenBestBidVolume5 = value;
                    IsPreOpenBestBidVolume5Update = true;
                }
                else
                {
                    IsPreOpenBestBidVolume5Update = false;
                }
            }
        }

        private string _preOpenBestAskVolume5;
        public bool IsPreOpenBestAskVolume5Update = false;
        public string PreOpenBestAskVolume5
        {
            get { return _preOpenBestAskVolume5; }
            set
            {
                if (value != _preOpenBestAskVolume5)
                {
                    _preOpenBestAskVolume5 = value;
                    IsPreOpenBestAskVolume5Update = true;
                }
                else
                {
                    IsPreOpenBestAskVolume5Update = false;
                }
            }
        }

        private string _preOpenBestBidVolumes;
        public bool IsPreOpenBestBidVolumesUpdate = false;
        public string PreOpenBestBidVolumes
        {
            get { return _preOpenBestBidVolumes; }
            set
            {
                if (value != _preOpenBestBidVolumes)
                {
                    _preOpenBestBidVolumes = value;
                    IsPreOpenBestBidVolumesUpdate = true;
                }
                else
                {
                    IsPreOpenBestBidVolumesUpdate = false;
                }
            }
        }

        private string _preOpenBestAskVolumes;
        public bool IsPreOpenBestAskVolumesUpdate = false;
        public string PreOpenBestAskVolumes
        {
            get { return _preOpenBestAskVolumes; }
            set
            {
                if (value != _preOpenBestAskVolumes)
                {
                    _preOpenBestAskVolumes = value;
                    IsPreOpenBestAskVolumesUpdate = true;
                }
                else
                {
                    IsPreOpenBestAskVolumesUpdate = false;
                }
            }
        }

        private string _time;
        public bool IsTimeUpdate = false;
        public string Time
        {
            get { return _time; }
            set
            {
                if (value != _time)
                {
                    _time = value;
                    IsTimeUpdate = true;
                }
                else
                {
                    IsTimeUpdate = false;
                }
            }
        }
    }
}
