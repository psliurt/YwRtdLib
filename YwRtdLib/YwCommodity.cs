using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YwRtdLib
{
    public class YwCommodity
    {
        public YwField LastChangeField { get; set; }
        public DateTime? LastUpdate { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
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

        /// <summary>
        /// 商品符號
        /// </summary>
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

        /// <summary>
        /// 當前價格
        /// </summary>
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

        private string _basis;
        public bool IsBasisUpdate = false;
        public string Basis
        {
            get { return _basis; }
            set
            {
                if (value != _basis)
                {
                    _basis = value;
                    IsBasisUpdate = true;
                }
                else
                {
                    IsBasisUpdate = false;
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


        private string _underlying;
        public bool UnderlyingSet = false;
        public string Underlying
        {
            get { return _underlying; }
            set
            {
                _underlying = value;
                UnderlyingSet = true;
            }
        }

        private string _spotPrice;
        public bool IsSpotPriceUpdate = false;
        public string SpotPrice
        {
            get { return _spotPrice; }
            set
            {
                if (value != _spotPrice)
                {
                    _spotPrice = value;
                    IsSpotPriceUpdate = true;
                }
                else
                {
                    IsSpotPriceUpdate = false;
                }
            }
        }

        private string _delta;
        public bool IsDeltaUpdate = false;
        public string Delta
        {
            get { return _delta; }
            set
            {
                if (value != _delta)
                {
                    _delta = value;
                    IsDeltaUpdate = true;
                }
                else
                {
                    IsDeltaUpdate = false;
                }
            }
        }

        private string _gamma;
        public bool IsGammaUpdate = false;
        public string Gamma
        {
            get { return _gamma; }
            set
            {
                if (value != _gamma)
                {
                    _gamma = value;
                    IsGammaUpdate = true;
                }
                else
                {
                    IsGammaUpdate = false;
                }
            }
        }

        private string _theta;
        public bool IsThetaUpdate = false;
        public string Theta
        {
            get { return _theta; }
            set
            {
                if (value != _theta)
                {
                    _theta = value;
                    IsThetaUpdate = true;
                }
                else
                {
                    IsThetaUpdate = false;
                }
            }
        }

        private string _vega;
        public bool IsVegaUpdate = false;
        public string Vega
        {
            get { return _vega; }
            set
            {
                if (value != _vega)
                {
                    _vega = value;
                    IsVegaUpdate = true;
                }
                else
                {
                    IsVegaUpdate = false;
                }
            }
        }

        private string _rho;
        public bool IsRhoUpdate = false;
        public string Rho
        {
            get { return _rho; }
            set
            {
                if (value != _rho)
                {
                    _rho = value;
                    IsRhoUpdate = true;
                }
                else
                {
                    IsRhoUpdate = false;
                }
            }
        }

        private string _timeValue;
        public bool IsTimeValueUpdate = false;
        public string TimeValue
        {
            get { return _timeValue; }
            set
            {
                if (value != _timeValue)
                {
                    _timeValue = value;
                    IsTimeValueUpdate = true;
                }
                else
                {
                    IsTimeValueUpdate = false;
                }
            }
        }

        private string _implicit;
        public bool IsImplicitUpdate = false;
        public string Implicit
        {
            get { return _implicit; }
            set
            {
                if (value != _implicit)
                {
                    _implicit = value;
                    IsImplicitUpdate = true;
                }
                else
                {
                    IsImplicitUpdate = false;
                }
            }
        }

        private string _implied;
        public bool IsImpliedUpdate = false;
        public string Implied
        {
            get { return _implied; }
            set
            {
                if (value != _implied)
                {
                    _implied = value;
                    IsImpliedUpdate = true;
                }
                else
                {
                    IsImpliedUpdate = false;
                }
            }
        }

        private string _moneyness;
        public bool IsMoneynessUpdate = false;
        public string Moneyness
        {
            get { return _moneyness; }
            set
            {
                if (value != _moneyness)
                {
                    _moneyness = value;
                    IsMoneynessUpdate = true;
                }
                else
                {
                    IsMoneynessUpdate = false;
                }
            }
        }

        private string _parityPrice;
        public bool IsParityPriceUpdate = false;
        public string ParityPrice
        {
            get { return _parityPrice; }
            set
            {
                if (value != _parityPrice)
                {
                    _parityPrice = value;
                    IsParityPriceUpdate = true;
                }
                else
                {
                    IsParityPriceUpdate = false;
                }
            }
        }

        private string _spotSigma;
        public bool IsSpotSigmaUpdate = false;
        public string SpotSigma
        {
            get { return _spotSigma; }
            set
            {
                if (value != _spotSigma)
                {
                    _spotSigma = value;
                    IsSpotSigmaUpdate = true;
                }
                else
                {
                    IsSpotSigmaUpdate = false;
                }
            }
        }

        private string _theoryPrice;
        public bool IsTheoryPriceUpdate = false;
        public string TheoryPrice
        {
            get { return _theoryPrice; }
            set
            {
                if (value != _theoryPrice)
                {
                    _theoryPrice = value;
                    IsTheoryPriceUpdate = true;
                }
                else
                {
                    IsTheoryPriceUpdate = false;
                }
            }
        }

        private string _strikePrice;
        public bool IsStrikePriceUpdate = false;
        public string StrikePrice
        {
            get { return _strikePrice; }
            set
            {
                if (value != _strikePrice)
                {
                    _strikePrice = value;
                    IsStrikePriceUpdate = true;
                }
                else
                {
                    IsStrikePriceUpdate = false;
                }
            }
        }

        private string _expire;
        public bool IsExpireUpdate = false;
        public string Expire
        {
            get { return _expire; }
            set
            {
                if (value != _expire)
                {
                    _expire = value;
                    IsExpireUpdate = true;
                }
                else
                {
                    IsExpireUpdate = false;
                }
            }
        }

        private string _due;
        public bool IsDueUpdate = false;
        public string Due
        {
            get { return _due; }
            set
            {
                if (value != _due)
                {
                    _due = value;
                    IsDueUpdate = true;
                }
                else
                {
                    IsDueUpdate = false;
                }
            }
        }

        private string _barrierPrice;
        public bool IsBarrierPriceUpdate = false;
        public string BarrierPrice
        {
            get { return _barrierPrice; }
            set
            {
                if (value != _barrierPrice)
                {
                    _barrierPrice = value;
                    IsBarrierPriceUpdate = true;
                }
                else
                {
                    IsBarrierPriceUpdate = false;
                }
            }
        }


        private string _issuer;
        public bool IssuerSet = false;
        public string Issuer
        {
            get { return _issuer; }
            set
            {
                _issuer = value;
                IssuerSet = true;
            }
        }

        private string _method;
        public bool IsMethodUpdate = false;
        public string Method
        {
            get { return _method; }
            set
            {
                if (value != _method)
                {
                    _method = value;
                    IsMethodUpdate = true;
                }
                else
                {
                    IsMethodUpdate = false;
                }
            }
        }

        private string _ratio;
        public bool IsRatioUpdate = false;
        public string Ratio
        {
            get { return _ratio; }
            set
            {
                if (value != _ratio)
                {
                    _ratio = value;
                    IsRatioUpdate = true;
                }
                else
                {
                    IsRatioUpdate = false;
                }
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

        private string _nAVReference;
        public bool IsNAVReferenceUpdate = false;
        public string NAVReference
        {
            get { return _nAVReference; }
            set
            {
                if (value != _nAVReference)
                {
                    _nAVReference = value;
                    IsNAVReferenceUpdate = true;
                }
                else
                {
                    IsNAVReferenceUpdate = false;
                }
            }
        }

        private string _nAVPrice;
        public bool IsNAVPriceUpdate = false;
        public string NAVPrice
        {
            get { return _nAVPrice; }
            set
            {
                if (value != _nAVPrice)
                {
                    _nAVPrice = value;
                    IsNAVPriceUpdate = true;
                }
                else
                {
                    IsNAVPriceUpdate = false;
                }
            }
        }

        private string _nAVChange;
        public bool IsNAVChangeUpdate = false;
        public string NAVChange
        {
            get { return _nAVChange; }
            set
            {
                if (value != _nAVChange)
                {
                    _nAVChange = value;
                    IsNAVChangeUpdate = true;
                }
                else
                {
                    IsNAVChangeUpdate = false;
                }
            }
        }

        private string _nAVChangePercent;
        public bool IsNAVChangePercentUpdate = false;
        public string NAVChangePercent
        {
            get { return _nAVChangePercent; }
            set
            {
                if (value != _nAVChangePercent)
                {
                    _nAVChangePercent = value;
                    IsNAVChangePercentUpdate = true;
                }
                else
                {
                    IsNAVChangePercentUpdate = false;
                }
            }
        }

        private string _premiumDiscount;
        public bool IsPremiumDiscountUpdate = false;
        public string PremiumDiscount
        {
            get { return _premiumDiscount; }
            set
            {
                if (value != _premiumDiscount)
                {
                    _premiumDiscount = value;
                    IsPremiumDiscountUpdate = true;
                }
                else
                {
                    IsPremiumDiscountUpdate = false;
                }
            }
        }

        private string _premiumDiscountPercent;
        public bool IsPremiumDiscountPercentUpdate = false;
        public string PremiumDiscountPercent
        {
            get { return _premiumDiscountPercent; }
            set
            {
                if (value != _premiumDiscountPercent)
                {
                    _premiumDiscountPercent = value;
                    IsPremiumDiscountPercentUpdate = true;
                }
                else
                {
                    IsPremiumDiscountPercentUpdate = false;
                }
            }
        }

        private decimal? _preOpenAskPrice;
        public bool IsPreOpenAskPriceUpdate = false;
        public decimal? PreOpenAskPrice
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

        private decimal? _preOpenAskVolume;
        public bool IsPreOpenAskVolumeUpdate = false;
        public decimal? PreOpenAskVolume
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

        private decimal? _preOpenBidPrice;
        public bool IsPreOpenBidPriceUpdate = false;
        public decimal? PreOpenBidPrice
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

        private decimal? _preOpenBidVolume;
        public bool IsPreOpenBidVolumeUpdate = false;
        public decimal? PreOpenBidVolume
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

        private decimal? _preOpenPrice;
        public bool IsPreOpenPriceUpdate = false;
        public decimal? PreOpenPrice
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

        private decimal? _preOpenVolume;
        public bool IsPreOpenVolumeUpdate = false;
        public decimal? PreOpenVolume
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

        private decimal? _preOpenBestBidPrice1;
        public bool IsPreOpenBestBidPrice1Update = false;
        public decimal? PreOpenBestBidPrice1
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

        private decimal? _preOpenBestAskPrice1;
        public bool IsPreOpenBestAskPrice1Update = false;
        public decimal? PreOpenBestAskPrice1
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

        private decimal? _preOpenBestBidVolume1;
        public bool IsPreOpenBestBidVolume1Update = false;
        public decimal? PreOpenBestBidVolume1
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

        private decimal? _preOpenBestAskVolume1;
        public bool IsPreOpenBestAskVolume1Update = false;
        public decimal? PreOpenBestAskVolume1
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

        private decimal? _preOpenBestBidPrice2;
        public bool IsPreOpenBestBidPrice2Update = false;
        public decimal? PreOpenBestBidPrice2
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

        private decimal? _preOpenBestAskPrice2;
        public bool IsPreOpenBestAskPrice2Update = false;
        public decimal? PreOpenBestAskPrice2
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

        private decimal? _preOpenBestBidVolume2;
        public bool IsPreOpenBestBidVolume2Update = false;
        public decimal? PreOpenBestBidVolume2
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

        private decimal? _preOpenBestAskVolume2;
        public bool IsPreOpenBestAskVolume2Update = false;
        public decimal? PreOpenBestAskVolume2
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

        private decimal? _preOpenBestBidPrice3;
        public bool IsPreOpenBestBidPrice3Update = false;
        public decimal? PreOpenBestBidPrice3
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

        private decimal? _preOpenBestAskPrice3;
        public bool IsPreOpenBestAskPrice3Update = false;
        public decimal? PreOpenBestAskPrice3
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

        private decimal? _preOpenBestBidVolume3;
        public bool IsPreOpenBestBidVolume3Update = false;
        public decimal? PreOpenBestBidVolume3
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

        private decimal? _preOpenBestAskVolume3;
        public bool IsPreOpenBestAskVolume3Update = false;
        public decimal? PreOpenBestAskVolume3
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

        private decimal? _preOpenBestBidPrice4;
        public bool IsPreOpenBestBidPrice4Update = false;
        public decimal? PreOpenBestBidPrice4
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

        private decimal? _preOpenBestAskPrice4;
        public bool IsPreOpenBestAskPrice4Update = false;
        public decimal? PreOpenBestAskPrice4
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

        private decimal? _preOpenBestBidVolume4;
        public bool IsPreOpenBestBidVolume4Update = false;
        public decimal? PreOpenBestBidVolume4
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

        private decimal? _preOpenBestAskVolume4;
        public bool IsPreOpenBestAskVolume4Update = false;
        public decimal? PreOpenBestAskVolume4
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

        private decimal? _preOpenBestBidPrice5;
        public bool IsPreOpenBestBidPrice5Update = false;
        public decimal? PreOpenBestBidPrice5
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

        private decimal? _preOpenBestAskPrice5;
        public bool IsPreOpenBestAskPrice5Update = false;
        public decimal? PreOpenBestAskPrice5
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

        private decimal? _preOpenBestBidVolume5;
        public bool IsPreOpenBestBidVolume5Update = false;
        public decimal? PreOpenBestBidVolume5
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

        private decimal? _preOpenBestAskVolume5;
        public bool IsPreOpenBestAskVolume5Update = false;
        public decimal? PreOpenBestAskVolume5
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

        private decimal? _preOpenBestBidVolumes;
        public bool IsPreOpenBestBidVolumesUpdate = false;
        public decimal? PreOpenBestBidVolumes
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

        private decimal? _preOpenBestAskVolumes;
        public bool IsPreOpenBestAskVolumesUpdate = false;
        public decimal? PreOpenBestAskVolumes
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
    }
}
