using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YwRtdLib
{
    public class YwBasicQuote
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

        private string _price;
        public bool IsPriceUpdate = false;
        public string Price
        {
            get { return _price; }
            set
            {
                if (value != _price)
                {
                    _price = value;
                    IsPriceUpdate = true;
                }
                else
                {
                    IsPriceUpdate = false;
                }
            }
        }

        private string _change;
        public bool IsChangeUpdate = false;
        public string Change
        {
            get { return _change; }
            set
            {
                if (value != _change)
                {
                    _change = value;
                    IsChangeUpdate = true;
                }
                else
                {
                    IsChangeUpdate = false;
                }
            }
        }

        private decimal? _changeRange;
        public bool IsChangeRangeUpdate = false;
        public decimal? ChangeRange
        {
            get { return _changeRange; }
            set
            {
                if (value != _changeRange)
                {
                    _changeRange = value;
                    IsChangeRangeUpdate = true;
                }
                else
                {
                    IsChangeRangeUpdate = false;
                }
            }
        }

        private decimal? _changePercent;
        public bool IsChangePercentUpdate = false;
        public decimal? ChangePercent
        {
            get { return _changePercent; }
            set
            {
                if (value != _changePercent)
                {
                    _changePercent = value;
                    IsChangePercentUpdate = true;
                }
                else
                {
                    IsChangePercentUpdate = false;
                }
            }
        }

        private string _reference;
        public bool IsReferenceUpdate = false;
        public string Reference
        {
            get { return _reference; }
            set
            {
                if (value != _reference)
                {
                    _reference = value;
                    IsReferenceUpdate = true;
                }
                else
                {
                    IsReferenceUpdate = false;
                }
            }
        }

        private string _open;
        public bool IsOpenUpdate = false;
        public string Open
        {
            get { return _open; }
            set
            {
                if (value != _open)
                {
                    _open = value;
                    IsOpenUpdate = true;
                }
                else
                {
                    IsOpenUpdate = false;
                }
            }
        }

        private string _high;
        public bool IsHighUpdate = false;
        public string High
        {
            get { return _high; }
            set
            {
                if (value != _high)
                {
                    _high = value;
                    IsHighUpdate = true;
                }
                else
                {
                    IsHighUpdate = false;
                }
            }
        }


        private string _low;
        public bool IsLowUpdate = false;
        public string Low
        {
            get { return _low; }
            set
            {
                if (value != _low)
                {
                    _low = value;
                    IsLowUpdate = true;
                }
                else
                {
                    IsLowUpdate = false;
                }
            }
        }

        private string _ceil;
        public bool IsCeilUpdate = false;
        public string Ceil
        {
            get { return _ceil; }
            set
            {
                if (value != _ceil)
                {
                    _ceil = value;
                    IsCeilUpdate = true;
                }
                else
                {
                    IsCeilUpdate = false;
                }
            }
        }

        private string _floor;
        public bool IsFloorUpdate = false;
        public string Floor
        {
            get { return _floor; }
            set
            {
                if (value != _floor)
                {
                    _floor = value;
                    IsFloorUpdate = true;
                }
                else
                {
                    IsFloorUpdate = false;
                }
            }
        }

        private string _groupName;
        public bool GroupNameSet = false;
        public string GroupName
        {
            get
            {
                return _groupName;
            }
            set
            {
                _groupName = value;
                GroupNameSet = true;
            }
        }

        private string _information;
        public bool InformationSet = false;
        public string Information
        {
            get { return _information; }
            set
            {
                _information = value;
                InformationSet = true;
            }
        }

        private string _volume;
        public bool IsVolumeUpdate = false;
        public string Volume
        {
            get { return _volume; }
            set
            {
                if (value != _volume)
                {
                    _volume = value;
                    IsVolumeUpdate = true;
                }
                else
                {
                    IsVolumeUpdate = false;
                }
            }
        }

        private string _cumulativeVolume;
        public bool IsCumulativeVolumeUpdate = false;
        public string CumulativeVolume
        {
            get { return _cumulativeVolume; }
            set
            {
                if (value != _cumulativeVolume)
                {
                    _cumulativeVolume = value;
                    IsCumulativeVolumeUpdate = true;
                }
                else
                {
                    IsCumulativeVolumeUpdate = false;
                }
            }
        }

        private string _predictVolume;
        public bool IsPredictVolumeUpdate = false;
        public string PredictVolume
        {
            get { return _predictVolume; }
            set
            {
                if (value != _predictVolume)
                {
                    _predictVolume = value;
                    IsPredictVolumeUpdate = true;
                }
                else
                {
                    IsPredictVolumeUpdate = false;
                }
            }
        }

        private string _volumeStrength;
        public bool IsVolumeStrengthUpdate = false;
        public string VolumeStrength
        {
            get { return _volumeStrength; }
            set
            {
                if (value != _volumeStrength)
                {
                    _volumeStrength = value;
                    IsVolumeStrengthUpdate = true;
                }
                else
                {
                    IsVolumeStrengthUpdate = false;
                }
            }
        }

        private string _bidPrice;
        public bool IsBidPriceUpdate = false;
        public string BidPrice
        {
            get { return _bidPrice; }
            set
            {
                if (value != _bidPrice)
                {
                    _bidPrice = value;
                    IsBidPriceUpdate = true;
                }
                else
                {
                    IsBidPriceUpdate = false;
                }
            }
        }

        private string _askPrice;
        public bool IsAskPriceUpdate = false;
        public string AskPrice
        {
            get { return _askPrice; }
            set
            {
                if (value != _askPrice)
                {
                    _askPrice = value;
                    IsAskPriceUpdate = true;
                }
                else
                {
                    IsAskPriceUpdate = false;
                }
            }
        }

        private string _bidVolume;
        public bool IsBidVolumeUpdate = false;
        public string BidVolume
        {
            get { return _bidVolume; }
            set
            {
                if (value != _bidVolume)
                {
                    _bidVolume = value;
                    IsBidVolumeUpdate = true;
                }
                else
                {
                    IsBidVolumeUpdate = false;
                }
            }
        }

        private string _askVolume;
        public bool IsAskVolumeUpdate = false;
        public string AskVolume
        {
            get { return _askVolume; }
            set
            {
                if (value != _askVolume)
                {
                    _askVolume = value;
                    IsAskVolumeUpdate = true;
                }
                else
                {
                    IsAskVolumeUpdate = false;
                }
            }
        }

        private string _priorHigh;
        public bool IsPriorHighUpdate = false;
        public string PriorHigh
        {
            get { return _priorHigh; }
            set
            {
                if (value != _priorHigh)
                {
                    _priorHigh = value;
                    IsPriorHighUpdate = true;
                }
                else
                {
                    IsPriorHighUpdate = false;
                }
            }
        }

        private string _priorLow;
        public bool IsPriorLowUpdate = false;
        public string PriorLow
        {
            get { return _priorLow; }
            set
            {
                if (value != _priorLow)
                {
                    _priorLow = value;
                    IsPriorLowUpdate = true;
                }
                else
                {
                    IsPriorLowUpdate = false;
                }
            }
        }

        private string _priorPrice;
        public bool IsPriorPriceUpdate = false;
        public string PriorPrice
        {
            get { return _priorPrice; }
            set
            {
                if (value != _priorPrice)
                {
                    _priorPrice = value;
                    IsPriorPriceUpdate = true;
                }
                else
                {
                    IsPriorPriceUpdate = false;
                }
            }
        }

        private string _priorVolume;
        public bool IsPriorVolumeUpdate = false;
        public string PriorVolume
        {
            get { return _priorVolume; }
            set
            {
                if (value != _priorVolume)
                {
                    _priorVolume = value;
                    IsPriorVolumeUpdate = true;
                }
                else
                {
                    IsPriorVolumeUpdate = false;
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

        private string _amount;
        public bool IsAmountUpdate = false;
        public string Amount
        {
            get { return _amount; }
            set
            {
                if (value != _amount)
                {
                    _amount = value;
                    IsAmountUpdate = true;
                }
                else
                {
                    IsAmountUpdate = false;
                }
            }
        }

        private string _marketValue;
        public bool IsMarketValueUpdate = false;
        public string MarketValue
        {
            get { return _marketValue; }
            set
            {
                if (value != _marketValue)
                {
                    _marketValue = value;
                    IsMarketValueUpdate = true;
                }
                else
                {
                    IsMarketValueUpdate = false;
                }
            }
        }

        private string _issue;
        public bool IsIssueUpdate = false;
        public string Issue
        {
            get { return _issue; }
            set
            {
                if (value != _issue)
                {
                    _issue = value;
                    IsIssueUpdate = true;
                }
                else
                {
                    IsIssueUpdate = false;
                }
            }
        }

        private decimal? _strengthMarket;
        public bool IsStrengthMarketUpdate = false;
        public decimal? StrengthMarket
        {
            get { return _strengthMarket; }
            set
            {
                if (value != _strengthMarket)
                {
                    _strengthMarket = value;
                    IsStrengthMarketUpdate = true;
                }
                else
                {
                    IsStrengthMarketUpdate = false;
                }
            }
        }

        private decimal? _strengthGroup;
        public bool IsStrengthGroupUpdate = false;
        public decimal? StrengthGroup
        {
            get { return _strengthGroup; }
            set
            {
                if (value != _strengthGroup)
                {
                    _strengthGroup = value;
                    IsStrengthGroupUpdate = true;
                }
                else
                {
                    IsStrengthGroupUpdate = false;
                }
            }
        }

        private decimal? _volumeRatio;
        public bool IsVolumeRatioUpdate = false;
        public decimal? VolumeRatio
        {
            get { return _volumeRatio; }
            set
            {
                if (value != _volumeRatio)
                {
                    _volumeRatio = value;
                    IsVolumeRatioUpdate = true;
                }
                else
                {
                    IsVolumeRatioUpdate = false;
                }
            }
        }

        private string _averagePrice;
        public bool IsAveragePriceUpdate = false;
        public string AveragePrice
        {
            get { return _averagePrice; }
            set
            {
                if (value != _averagePrice)
                {
                    _averagePrice = value;
                    IsAveragePriceUpdate = true;
                }
                else
                {
                    IsAveragePriceUpdate = false;
                }
            }
        }


        private string _capital;
        public bool CapitalSet = false;
        public string Capital
        {
            get { return _capital; }
            set
            {
                _capital = value;
                CapitalSet = true;
            }
        }

        private string _cumulativeBidVolume;
        public bool IsCumulativeBidVolumeUpdate = false;
        public string CumulativeBidVolume
        {
            get { return _cumulativeBidVolume; }
            set
            {
                if (value != _cumulativeBidVolume)
                {
                    _cumulativeBidVolume = value;
                    IsCumulativeBidVolumeUpdate = true;
                }
                else
                {
                    IsCumulativeBidVolumeUpdate = false;
                }
            }
        }

        private string _cumulativeBidOrder;
        public bool IsCumulativeBidOrderUpdate = false;
        public string CumulativeBidOrder
        {
            get { return _cumulativeBidOrder; }
            set
            {
                if (value != _cumulativeBidOrder)
                {
                    _cumulativeBidOrder = value;
                    IsCumulativeBidOrderUpdate = true;
                }
                else
                {
                    IsCumulativeBidOrderUpdate = false;
                }
            }
        }

        private string _cumulativeAskVolume;
        public bool IsCumulativeAskVolumeUpdate = false;
        public string CumulativeAskVolume
        {
            get { return _cumulativeAskVolume; }
            set
            {
                if (value != _cumulativeAskVolume)
                {
                    _cumulativeAskVolume = value;
                    IsCumulativeAskVolumeUpdate = true;
                }
                else
                {
                    IsCumulativeAskVolumeUpdate = false;
                }
            }
        }

        private string _cumulativeAskOrder;
        public bool IsCumulativeAskOrderUpdate = false;
        public string CumulativeAskOrder
        {
            get { return _cumulativeAskOrder; }
            set
            {
                if (value != _cumulativeAskOrder)
                {
                    _cumulativeAskOrder = value;
                    IsCumulativeAskOrderUpdate = true;
                }
                else
                {
                    IsCumulativeAskOrderUpdate = false;
                }
            }
        }

        private string _bear;
        public bool IsBearUpdate = false;
        public string Bear
        {
            get { return _bear; }
            set
            {
                if (value != _bear)
                {
                    _bear = value;
                    IsBearUpdate = true;
                }
                else
                {
                    IsBearUpdate = false;
                }
            }
        }



        
    }
}
