using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace YwRtdAp.CombineObject
{
    public class PriorAfterMarketStatistic
    {
        /// <summary>
        /// 資料抓取時間
        /// </summary>
        [DisplayName("資料抓取時間")]
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 資料時間
        /// </summary>
        [DisplayName("資料時間")]
        public DateTime? StatisticDate { get; set; }

        /// <summary>
        /// 股票代碼
        /// </summary>
        [DisplayName("股票代碼")]
        public string Symbol { get; set; }

        /// <summary>
        /// 股票名稱
        /// </summary>
        [DisplayName("股票名稱")]
        public string StockName { get; set; }

        /// <summary>
        /// 周轉率(%)
        /// </summary>
        [DisplayName("周轉率")]
        public string Turnover { get; set; }

        /// <summary>
        /// 買賣超張數
        /// </summary>
        [DisplayName("買賣超張數")]
        public string NetBuySell { get; set; }

        /// <summary>
        /// 外資買賣超張數
        /// </summary>
        [DisplayName("外資買賣超張數")]
        public string ForeignNetBuySell { get; set; }

        /// <summary>
        /// 投信買賣超張數
        /// </summary>
        [DisplayName("投信買賣超張數")]
        public string InvestTrustNetBuySell { get; set; }

        /// <summary>
        /// 自營商買賣超張數
        /// </summary>
        [DisplayName("自營商買賣超張數")]
        public string DealerNetBuySell { get; set; }

        /// <summary>
        /// 融資張數
        /// </summary>
        [DisplayName("融資張數")]
        public string MarginPurchase { get; set; }

        /// <summary>
        /// 融券張數
        /// </summary>
        [DisplayName("融券張數")]
        public string ShortSale { get; set; }

        /// <summary>
        /// 券資比(%)
        /// </summary>
        [DisplayName("券資比")]
        public string MarginShortPercent { get; set; }


        //資用

        //券用

        /// <summary>
        /// 當沖
        /// </summary>
        [DisplayName("當沖")]
        public string DayTrade { get; set; }

    }
}
