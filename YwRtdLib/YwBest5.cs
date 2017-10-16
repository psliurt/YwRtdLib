using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YwRtdLib
{
    public class YwBest5
    {
        private string _name;
        public bool NameSet = false;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NameSet = true;
            }
        }

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

        private decimal? _bestBidPrice1;
        public bool IsBestBidPrice1Update = false;
        public decimal? BestBidPrice1
        {
            get { return _bestBidPrice1; }
            set
            {
                if (value != _bestBidPrice1)
                {
                    _bestBidPrice1 = value;
                    IsBestBidPrice1Update = true;
                }
                else
                {
                    IsBestBidPrice1Update = false;
                }
            }
        }

        private decimal? _bestAskPrice1;
        public bool IsBestAskPrice1Update = false;
        public decimal? BestAskPrice1
        {
            get { return _bestAskPrice1; }
            set
            {
                if (value != _bestAskPrice1)
                {
                    _bestAskPrice1 = value;
                    IsBestAskPrice1Update = true;
                }
                else
                {
                    IsBestAskPrice1Update = false;
                }
            }
        }

        private decimal? _bestBidVolume1;
        public bool IsBestBidVolume1Update = false;
        public decimal? BestBidVolume1
        {
            get { return _bestBidVolume1; }
            set
            {
                if (value != _bestBidVolume1)
                {
                    _bestBidVolume1 = value;
                    IsBestBidVolume1Update = true;
                }
                else
                {
                    IsBestBidVolume1Update = false;
                }
            }
        }

        private decimal? _bestAskVolume1;
        public bool IsBestAskVolume1Update = false;
        public decimal? BestAskVolume1
        {
            get { return _bestAskVolume1; }
            set
            {
                if (value != _bestAskVolume1)
                {
                    _bestAskVolume1 = value;
                    IsBestAskVolume1Update = true;
                }
                else
                {
                    IsBestAskVolume1Update = false;
                }
            }
        }

        private decimal? _bestBidPrice2;
        public bool IsBestBidPrice2Update = false;
        public decimal? BestBidPrice2
        {
            get { return _bestBidPrice2; }
            set
            {
                if (value != _bestBidPrice2)
                {
                    _bestBidPrice2 = value;
                    IsBestBidPrice2Update = true;
                }
                else
                {
                    IsBestBidPrice2Update = false;
                }
            }
        }

        private decimal? _bestAskPrice2;
        public bool IsBestAskPrice2Update = false;
        public decimal? BestAskPrice2
        {
            get { return _bestAskPrice2; }
            set
            {
                if (value != _bestAskPrice2)
                {
                    _bestAskPrice2 = value;
                    IsBestAskPrice2Update = true;
                }
                else
                {
                    IsBestAskPrice2Update = false;
                }
            }
        }

        private decimal? _bestBidVolume2;
        public bool IsBestBidVolume2Update = false;
        public decimal? BestBidVolume2
        {
            get { return _bestBidVolume2; }
            set
            {
                if (value != _bestBidVolume2)
                {
                    _bestBidVolume2 = value;
                    IsBestBidVolume2Update = true;
                }
                else
                {
                    IsBestBidVolume2Update = false;
                }
            }
        }

        private decimal? _bestAskVolume2;
        public bool IsBestAskVolume2Update = false;
        public decimal? BestAskVolume2
        {
            get { return _bestAskVolume2; }
            set
            {
                if (value != _bestAskVolume2)
                {
                    _bestAskVolume2 = value;
                    IsBestAskVolume2Update = true;
                }
                else
                {
                    IsBestAskVolume2Update = false;
                }
            }
        }

        private decimal? _bestBidPrice3;
        public bool IsBestBidPrice3Update = false;
        public decimal? BestBidPrice3
        {
            get { return _bestBidPrice3; }
            set
            {
                if (value != _bestBidPrice3)
                {
                    _bestBidPrice3 = value;
                    IsBestBidPrice3Update = true;
                }
                else
                {
                    IsBestBidPrice3Update = false;
                }
            }
        }

        private decimal? _bestAskPrice3;
        public bool IsBestAskPrice3Update = false;
        public decimal? BestAskPrice3
        {
            get { return _bestAskPrice3; }
            set
            {
                if (value != _bestAskPrice3)
                {
                    _bestAskPrice3 = value;
                    IsBestAskPrice3Update = true;
                }
                else
                {
                    IsBestAskPrice3Update = false;
                }
            }
        }

        private decimal? _bestBidVolume3;
        public bool IsBestBidVolume3Update = false;
        public decimal? BestBidVolume3
        {
            get { return _bestBidVolume3; }
            set
            {
                if (value != _bestBidVolume3)
                {
                    _bestBidVolume3 = value;
                    IsBestBidVolume3Update = true;
                }
                else
                {
                    IsBestBidVolume3Update = false;
                }
            }
        }

        private decimal? _bestAskVolume3;
        public bool IsBestAskVolume3Update = false;
        public decimal? BestAskVolume3
        {
            get { return _bestAskVolume3; }
            set
            {
                if (value != _bestAskVolume3)
                {
                    _bestAskVolume3 = value;
                    IsBestAskVolume3Update = true;
                }
                else
                {
                    IsBestAskVolume3Update = false;
                }
            }
        }

        private decimal? _bestBidPrice4;
        public bool IsBestBidPrice4Update = false;
        public decimal? BestBidPrice4
        {
            get { return _bestBidPrice4; }
            set
            {
                if (value != _bestBidPrice4)
                {
                    _bestBidPrice4 = value;
                    IsBestBidPrice4Update = true;
                }
                else
                {
                    IsBestBidPrice4Update = false;
                }
            }
        }

        private decimal? _bestAskPrice4;
        public bool IsBestAskPrice4Update = false;
        public decimal? BestAskPrice4
        {
            get { return _bestAskPrice4; }
            set
            {
                if (value != _bestAskPrice4)
                {
                    _bestAskPrice4 = value;
                    IsBestAskPrice4Update = true;
                }
                else
                {
                    IsBestAskPrice4Update = false;
                }
            }
        }

        private decimal? _bestBidVolume4;
        public bool IsBestBidVolume4Update = false;
        public decimal? BestBidVolume4
        {
            get { return _bestBidVolume4; }
            set
            {
                if (value != _bestBidVolume4)
                {
                    _bestBidVolume4 = value;
                    IsBestBidVolume4Update = true;
                }
                else
                {
                    IsBestBidVolume4Update = false;
                }
            }
        }

        private decimal? _bestAskVolume4;
        public bool IsBestAskVolume4Update = false;
        public decimal? BestAskVolume4
        {
            get { return _bestAskVolume4; }
            set
            {
                if (value != _bestAskVolume4)
                {
                    _bestAskVolume4 = value;
                    IsBestAskVolume4Update = true;
                }
                else
                {
                    IsBestAskVolume4Update = false;
                }
            }
        }

        private decimal? _bestBidPrice5;
        public bool IsBestBidPrice5Update = false;
        public decimal? BestBidPrice5
        {
            get { return _bestBidPrice5; }
            set
            {
                if (value != _bestBidPrice5)
                {
                    _bestBidPrice5 = value;
                    IsBestBidPrice5Update = true;
                }
                else
                {
                    IsBestBidPrice5Update = false;
                }
            }
        }

        private decimal? _bestAskPrice5;
        public bool IsBestAskPrice5Update = false;
        public decimal? BestAskPrice5
        {
            get { return _bestAskPrice5; }
            set
            {
                if (value != _bestAskPrice5)
                {
                    _bestAskPrice5 = value;
                    IsBestAskPrice5Update = true;
                }
                else
                {
                    IsBestAskPrice5Update = false;
                }
            }
        }

        private decimal? _bestBidVolume5;
        public bool IsBestBidVolume5Update = false;
        public decimal? BestBidVolume5
        {
            get { return _bestBidVolume5; }
            set
            {
                if (value != _bestBidVolume5)
                {
                    _bestBidVolume5 = value;
                    IsBestBidVolume5Update = true;
                }
                else
                {
                    IsBestBidVolume5Update = false;
                }
            }
        }

        private decimal? _bestAskVolume5;
        public bool IsBestAskVolume5Update = false;
        public decimal? BestAskVolume5
        {
            get { return _bestAskVolume5; }
            set
            {
                if (value != _bestAskVolume5)
                {
                    _bestAskVolume5 = value;
                    IsBestAskVolume5Update = true;
                }
                else
                {
                    IsBestAskVolume5Update = false;
                }
            }
        }

        private decimal? _bestBidVolumes;
        public bool IsBestBidVolumesUpdate = false;
        public decimal? BestBidVolumes
        {
            get { return _bestBidVolumes; }
            set
            {
                if (value != _bestBidVolumes)
                {
                    _bestBidVolumes = value;
                    IsBestBidVolumesUpdate = true;
                }
                else
                {
                    IsBestBidVolumesUpdate = false;
                }
            }
        }

        private decimal? _bestAskVolumes;
        public bool IsBestAskVolumesUpdate = false;
        public decimal? BestAskVolumes
        {
            get { return _bestAskVolumes; }
            set
            {
                if (value != _bestAskVolumes)
                {
                    _bestAskVolumes = value;
                    IsBestAskVolumesUpdate = true;
                }
                else
                {
                    IsBestAskVolumesUpdate = false;
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
