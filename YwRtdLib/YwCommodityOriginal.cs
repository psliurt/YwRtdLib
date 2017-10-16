using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YwRtdLib
{
    class YwCommodityOriginal
    {
        /// <summary>
        /// 代號
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 名稱
        /// </summary>
        public string Symbol { get; set; }
        /// <summary>
        /// 成交/淨值
        /// </summary>
        public string Price { get; set; }
        /// <summary>
        /// 漲跌
        /// </summary>
        public string Change { get; set; }
        /// <summary>
        /// 振幅
        /// </summary>
        public string ChangeRange { get; set; }
        /// <summary>
        /// 漲跌幅
        /// </summary>
        public string ChangePercent { get; set; }
        /// <summary>
        /// 參考價
        /// </summary>
        public string Reference { get; set; }
        /// <summary>
        /// 開盤價
        /// </summary>
        public string Open { get; set; }
        /// <summary>
        /// 最高價
        /// </summary>
        public string High { get; set; }
        /// <summary>
        /// 最低價
        /// </summary>
        public string Low { get; set; }
        /// <summary>
        /// 漲停價
        /// </summary>
        public string Ceil { get; set; }
        /// <summary>
        /// 跌停價
        /// </summary>
        public string Floor { get; set; }
        /// <summary>
        /// 商品類別(市指數,市金融業,市指數,股票基金,期貨,選擇權,市認購權證,參考匯率,港指數,滬指數,美指數,日指數,韓指數)
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 資訊
        /// </summary>
        public string Information { get; set; }
        /// <summary>
        /// 單量
        /// </summary>
        public string Volume { get; set; }
        /// <summary>
        /// 成交量
        /// </summary>
        public string CumulativeVolume { get; set; }
        /// <summary>
        /// 預估量
        /// </summary>
        public string PredictVolume { get; set; }
        /// <summary>
        /// 量增減
        /// </summary>
        public string VolumeStrength { get; set; }
        /// <summary>
        /// 委買價
        /// </summary>
        public string BidPrice { get; set; }
        /// <summary>
        /// 委賣價
        /// </summary>
        public string AskPrice { get; set; }
        /// <summary>
        /// 買量
        /// </summary>
        public string BidVolume { get; set; }
        /// <summary>
        /// 賣量
        /// </summary>
        public string AskVolume { get; set; }
        /// <summary>
        /// 昨高
        /// </summary>
        public string PriorHigh { get; set; }
        /// <summary>
        /// 昨低
        /// </summary>
        public string PriorLow { get; set; }
        /// <summary>
        /// 昨價
        /// </summary>
        public string PriorPrice { get; set; }
        /// <summary>
        /// 昨量
        /// </summary>
        public string PriorVolume { get; set; }
        /// <summary>
        /// 資料時間
        /// </summary>
        public string Time { get; set; }
        /// <summary>
        /// 成交金額
        /// </summary>
        public string Amount { get; set; }
        /// <summary>
        /// 市值
        /// </summary>
        public string MarketValue { get; set; }
        public string Issue { get; set; }
        public string StrengthMarket { get; set; }
        public string StrengthGroup { get; set; }
        public string VolumeRatio { get; set; }
        public string BestBidPrice1 { get; set; }
        public string BestAskPrice1 { get; set; }
        public string BestBidVolume1 { get; set; }
        public string BestAskVolume1 { get; set; }
        public string BestBidPrice2 { get; set; }
        public string BestAskPrice2 { get; set; }
        public string BestBidVolume2 { get; set; }
        public string BestAskVolume2 { get; set; }
        public string BestBidPrice3 { get; set; }
        public string BestAskPrice3 { get; set; }
        public string BestBidVolume3 { get; set; }
        public string BestAskVolume3 { get; set; }
        public string BestBidPrice4 { get; set; }
        public string BestAskPrice4 { get; set; }
        public string BestBidVolume4 { get; set; }
        public string BestAskVolume4 { get; set; }
        public string BestBidPrice5 { get; set; }
        public string BestAskPrice5 { get; set; }
        public string BestBidVolume5 { get; set; }
        public string BestAskVolume5 { get; set; }
        public string BestBidVolumes { get; set; }
        public string BestAskVolumes { get; set; }
        public string AveragePrice { get; set; }
        /// <summary>
        /// 股本
        /// </summary>
        public string Capital { get; set; }
        public string Basis { get; set; }
        public string Bear { get; set; }
        public string Underlying { get; set; }
        public string SpotPrice { get; set; }
        public string Delta { get; set; }
        public string Gamma { get; set; }
        public string Theta { get; set; }
        public string Vega { get; set; }
        public string Rho { get; set; }
        public string TimeValue { get; set; }
        public string Implicit { get; set; }
        public string Implied { get; set; }
        public string Moneyness { get; set; }
        public string ParityPrice { get; set; }
        public string SpotSigma { get; set; }
        public string TheoryPrice { get; set; }
        public string StrikePrice { get; set; }
        public string Expire { get; set; }
        public string Due { get; set; }
        public string BarrierPrice { get; set; }
        public string Issuer { get; set; }
        public string Method { get; set; }
        public string Ratio { get; set; }
        public string CumulativeBidVolume { get; set; }
        public string CumulativeBidOrder { get; set; }
        public string CumulativeAskVolume { get; set; }
        public string CumulativeAskOrder { get; set; }
        public string NAVReference { get; set; }
        public string NAVPrice { get; set; }
        public string NAVChange { get; set; }
        public string NAVChangePercent { get; set; }
        public string PremiumDiscount { get; set; }
        public string PremiumDiscountPercent { get; set; }
        public string PreOpenAskPrice { get; set; }
        public string PreOpenAskVolume { get; set; }
        public string PreOpenBidPrice { get; set; }
        public string PreOpenBidVolume { get; set; }
        public string PreOpenPrice { get; set; }
        public string PreOpenVolume { get; set; }
        public string PreOpenBestBidPrice1 { get; set; }
        public string PreOpenBestAskPrice1 { get; set; }
        public string PreOpenBestBidVolume1 { get; set; }
        public string PreOpenBestAskVolume1 { get; set; }
        public string PreOpenBestBidPrice2 { get; set; }
        public string PreOpenBestAskPrice2 { get; set; }
        public string PreOpenBestBidVolume2 { get; set; }
        public string PreOpenBestAskVolume2 { get; set; }
        public string PreOpenBestBidPrice3 { get; set; }
        public string PreOpenBestAskPrice3 { get; set; }
        public string PreOpenBestBidVolume3 { get; set; }
        public string PreOpenBestAskVolume3 { get; set; }
        public string PreOpenBestBidPrice4 { get; set; }
        public string PreOpenBestAskPrice4 { get; set; }
        public string PreOpenBestBidVolume4 { get; set; }
        public string PreOpenBestAskVolume4 { get; set; }
        public string PreOpenBestBidPrice5 { get; set; }
        public string PreOpenBestAskPrice5 { get; set; }
        public string PreOpenBestBidVolume5 { get; set; }
        public string PreOpenBestAskVolume5 { get; set; }
        public string PreOpenBestBidVolumes { get; set; }
        public string PreOpenBestAskVolumes { get; set; }
    }
}
