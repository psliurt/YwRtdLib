using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YwRtdAp.CombineObject
{
    /// <summary>
    /// 每日市場盤後資料
    /// </summary>
    public class DailyMarketTradeData
    {
        public string Symbol { get; set; }

        public string SymbolName { get; set; }

        public decimal MarginBuy { get; set; }
        public decimal MarginSell  { get; set; }
        public decimal MarginYesterdayBalance { get; set; }
        public decimal MarginTodayBalance { get; set; }
        public decimal MarginChange { get; set; }

        public decimal LoanBuy { get; set; }
        public decimal LoanSell { get; set; }
        public decimal LoanYesterdayBalance { get; set; }
        public decimal LoanTodayBalance { get; set; }
        public decimal LoanChange { get; set; }

        #region 借券資料

        /// <summary>
        /// 前日借券餘額張數
        /// </summary>
        public decimal YesterdayLendStockBalance { get; set; }

        /// <summary>
        /// 本日借券增加張數
        /// </summary>
        public decimal TodayLendStockPlusCount { get; set; }

        /// <summary>
        /// 本日還券張數
        /// </summary>
        public decimal TodayLendStockReturnCount { get; set; }

        /// <summary>
        /// 本日借券餘額張數 = 前日借券餘額張數 + 本日借券增加張數 - 本日還券張數
        /// </summary>
        public decimal TodayLendStockBalance { get; set; }

        #endregion
    }
}
