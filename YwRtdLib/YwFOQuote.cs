using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YwRtdLib
{
    public class YwFOQuote
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

        private decimal? _timeValue;
        public bool IsTimeValueUpdate = false;
        public decimal? TimeValue
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

        private decimal? _implicit;
        public bool IsImplicitUpdate = false;
        public decimal? Implicit
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

        private decimal? _strikePrice;
        public bool IsStrikePriceUpdate = false;
        public decimal? StrikePrice
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
