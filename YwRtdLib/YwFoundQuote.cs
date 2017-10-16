using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YwRtdLib
{
    public class YwFoundQuote
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

        private decimal? _nAVReference;
        public bool IsNAVReferenceUpdate = false;
        public decimal? NAVReference
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

        private decimal? _nAVPrice;
        public bool IsNAVPriceUpdate = false;
        public decimal? NAVPrice
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

        private decimal? _nAVChange;
        public bool IsNAVChangeUpdate = false;
        public decimal? NAVChange
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

        private decimal? _premiumDiscount;
        public bool IsPremiumDiscountUpdate = false;
        public decimal? PremiumDiscount
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
