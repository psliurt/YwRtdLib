using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using YwRtdLib;

namespace YwRtdAp.CombineObject
{
    public class BizGroupQuote : CombineQuote
    {
        public BizGroupQuote(ref YwCommodity commodityQuote)
            : base(ref commodityQuote)
        {

        }

        /// <summary>
        /// 代號
        /// </summary>
        [DisplayName("代號")]
        public string Symbol { get { return this._core.Symbol; } }
        /// <summary>
        /// 名稱
        /// </summary>
        [DisplayName("名稱")]
        public string Name { get { return this._core.Name; } }

        /// <summary>
        /// 股本
        /// </summary>
        [DisplayName("股本")]
        public string Capital { get { return this._core.Capital; } }
        /// <summary>
        /// 預估量
        /// </summary>
        [DisplayName("預估量")]
        public string PredictVolume { get { return this._core.PredictVolume; } }
        /// <summary>
        /// 成交量
        /// </summary>
        [DisplayName("成交量")]
        public string CumulativeVolume { get { return this._core.CumulativeVolume; } }
        /// <summary>
        /// 昨量
        /// </summary>
        [DisplayName("昨量")]
        public string PriorVolume { get { return this._core.PriorVolume; } }
        /// <summary>
        /// 量增減
        /// </summary>
        [DisplayName("量增減")]
        public string VolumeStrength { get { return this._core.VolumeStrength; } }
        /// <summary>
        /// 成交/淨值
        /// </summary>
        [DisplayName("成交/淨值")]
        public string Price { get { return this._core.Price; } }
        /// <summary>
        /// 漲跌
        /// </summary>
        [DisplayName("漲跌")]
        public string Change { get { return this._core.Change; } }
        /// <summary>
        /// 漲跌幅
        /// </summary>
        [DisplayName("漲跌幅")]
        public decimal? ChangePercent { get { return this._core.ChangePercent; } }
        /// <summary>
        /// 開盤價
        /// </summary>
        [DisplayName("開盤價")]
        public string Open { get { return this._core.Open; } }
        /// <summary>
        /// 最高價
        /// </summary>
        [DisplayName("最高價")]
        public string High { get { return this._core.High; } }
        /// <summary>
        /// 最低價
        /// </summary>
        [DisplayName("最低價")]
        public string Low { get { return this._core.Low; } }
        /// <summary>
        /// 漲停價
        /// </summary>
        [DisplayName("漲停價")]
        public string Ceil { get { return this._core.Ceil; } }
        /// <summary>
        /// 跌停價
        /// </summary>
        [DisplayName("跌停價")]
        public string Floor { get { return this._core.Floor; } }
        /// <summary>
        /// 昨高
        /// </summary>
        [DisplayName("昨高")]
        public string PriorHigh { get { return this._core.PriorHigh; } }
        /// <summary>
        /// 昨低
        /// </summary>
        [DisplayName("昨低")]
        public string PriorLow { get { return this._core.PriorLow; } }
        /// <summary>
        /// 昨價
        /// </summary>
        [DisplayName("昨價")]
        public string PriorPrice { get { return this._core.PriorPrice; } }
        /// <summary>
        /// 資訊
        /// </summary>
        [DisplayName("資訊")]
        public string Information { get { return this._core.Information; } }

    }
}
