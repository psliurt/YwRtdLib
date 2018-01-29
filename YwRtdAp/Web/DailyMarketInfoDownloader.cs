using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YwRtdAp.CombineObject;
using System.Threading;
using HttpRS;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YwRtdAp.Web
{
    public class DailyMarketInfoDownloader
    {
        ILog _log = LogManager.GetLogger(typeof(DailyMarketInfoDownloader));
             
        private static DailyMarketInfoDownloader _instance;
        private Dictionary<string, DailyMarketTradeData> _dataContainer;

        private Thread _downloadThread { get; set; }
        private DateTime _lastTradeDate;

        private DailyMarketInfoDownloader(Dictionary<string, DailyMarketTradeData> dataContainer)
        {
            this._dataContainer = dataContainer;
            _lastTradeDate = GetLastTradeDate();
            this._downloadThread = new Thread(DoDownload);
            this._downloadThread.Start();
        }

        public static DailyMarketInfoDownloader Instance(Dictionary<string, DailyMarketTradeData> dataContainer)
        {
            if (_instance == null)
            {
                _instance = new DailyMarketInfoDownloader(dataContainer);
            }
            return _instance;
        }

        private void DoDownload()
        {
            //下載借券資料
            //DownloadLendStockData();
            DownloadMarginTradeData();
        }

        //下載融資融券資料
        private void DownloadMarginTradeData()
        {
            string lastDateStr = _lastTradeDate.ToString("yyyyMMdd");
            //http://www.tse.com.tw/zh/page/trading/exchange/MI_MARGN.html
            string[] selectType = new string[] { "01", "02", "03", "04", "05", "06", "07", "21", "22", "08", "09", "10", "11", "12", "13", "24", "25", "26", "27", "28", "29", "30", "31", "14", "15", "16", "17", "18", "23", "19", "20" };
            foreach (string s in selectType)
            {
                DownloadMarginTradeDataByType(lastDateStr, s);
            }
            
        }

        private void DownloadMarginTradeDataByType(string date, string st)
        {
            string url = string.Format("http://www.tse.com.tw/exchangeReport/MI_MARGN?response=json&date={0}&selectType={1}", date, st);
            HttpSender marginTradeHttp = new HttpSender(url);
            HttpHeaderList marginTradeHeader = new HttpHeaderList();
            ResponseResult marginTradeResult = marginTradeHttp.SendRequest(HttpRequestMethod.Get, "", marginTradeHeader);
            bool isJson = false;
            try
            {
                JObject.Parse(marginTradeResult.ResponseBody);
                isJson = true;
            }
            catch (Exception e)
            {
                isJson = false;
            }

            if (isJson == false)
            { return; }

            MarginTradeRspMsg rspMsg = JsonConvert.DeserializeObject<MarginTradeRspMsg>(marginTradeResult.ResponseBody);

            foreach (List<string> stockData in rspMsg.data)
            {
                if (this._dataContainer.ContainsKey(stockData[0]))
                {
                    DailyMarketTradeData tradeData = this._dataContainer[stockData[0]];
                    tradeData.SymbolName = string.IsNullOrEmpty(tradeData.SymbolName) ? stockData[1] : tradeData.SymbolName;
                    tradeData.MarginBuy = Convert.ToDecimal(stockData[2].Replace(",", ""));
                    tradeData.MarginSell = Convert.ToDecimal(stockData[3].Replace(",", ""));
                    tradeData.MarginYesterdayBalance = Convert.ToDecimal(stockData[5].Replace(",", ""));
                    tradeData.MarginTodayBalance = Convert.ToDecimal(stockData[6].Replace(",", ""));
                    tradeData.MarginChange = Convert.ToDecimal(stockData[6].Replace(",", "")) - Convert.ToDecimal(stockData[5].Replace(",", ""));

                    tradeData.LoanBuy = Convert.ToDecimal(stockData[8].Replace(",", ""));
                    tradeData.LoanSell = Convert.ToDecimal(stockData[9].Replace(",", ""));
                    tradeData.LoanYesterdayBalance = Convert.ToDecimal(stockData[11].Replace(",", ""));
                    tradeData.LoanTodayBalance = Convert.ToDecimal(stockData[12].Replace(",", ""));
                    tradeData.LoanChange = Convert.ToDecimal(stockData[12].Replace(",", "")) - Convert.ToDecimal(stockData[11].Replace(",", ""));
                }
                else
                {
                    DailyMarketTradeData tradeData = new DailyMarketTradeData
                    {
                        Symbol = stockData[0],
                        SymbolName = stockData[1],
                        MarginBuy = Convert.ToDecimal(stockData[2].Replace(",", "")),
                        MarginSell = Convert.ToDecimal(stockData[3].Replace(",", "")),
                        MarginYesterdayBalance = Convert.ToDecimal(stockData[5].Replace(",", "")),
                        MarginTodayBalance = Convert.ToDecimal(stockData[6].Replace(",", "")),
                        MarginChange = Convert.ToDecimal(stockData[6].Replace(",", "")) - Convert.ToDecimal(stockData[5].Replace(",", "")),

                        LoanBuy = Convert.ToDecimal(stockData[8].Replace(",", "")),
                        LoanSell = Convert.ToDecimal(stockData[9].Replace(",", "")),
                        LoanYesterdayBalance = Convert.ToDecimal(stockData[11].Replace(",", "")),
                        LoanTodayBalance = Convert.ToDecimal(stockData[12].Replace(",", "")),
                        LoanChange = Convert.ToDecimal(stockData[12].Replace(",", "")) - Convert.ToDecimal(stockData[11].Replace(",", "")),
                    };
                    this._dataContainer.Add(stockData[0], tradeData);
                }
            }
            
        }

        /// <summary>
        /// 下載借券資料
        /// </summary>
        private void DownloadLendStockData()
        {
            string lastDateStr = _lastTradeDate.ToString("yyyyMMdd");
            //http://www.tse.com.tw/zh/page/trading/exchange/TWT72U.html
            //證交所借券系統與證商/證金營業處所借券餘額合計表
            string url = string.Format("http://www.tse.com.tw/exchangeReport/TWT72U?response=json&date={0}&selectType=SLBNLB", lastDateStr);
            HttpSender lendStockHttp = new HttpSender(url);
            HttpHeaderList lendStockHeader = new HttpHeaderList();
            ResponseResult lendStockHttpResult = lendStockHttp.SendRequest(HttpRequestMethod.Get, "", lendStockHeader);            

            LendStockRspMsg rspMsg = JsonConvert.DeserializeObject<LendStockRspMsg>(lendStockHttpResult.ResponseBody);

            //TODO:要不要把資料寫入檔案呢?

            foreach (List<string> stockData in rspMsg.data)
            {
                if (this._dataContainer.ContainsKey(stockData[0]))
                {
                    DailyMarketTradeData tradeData = this._dataContainer[stockData[0]];
                    tradeData.SymbolName = string.IsNullOrEmpty(tradeData.SymbolName) ? stockData[1] : tradeData.SymbolName;
                    tradeData.YesterdayLendStockBalance = Convert.ToDecimal(stockData[2].Replace(",", "")) / 1000;
                    tradeData.TodayLendStockPlusCount = Convert.ToDecimal(stockData[3].Replace(",", "")) / 1000;
                    tradeData.TodayLendStockReturnCount = Convert.ToDecimal(stockData[4].Replace(",", "")) / 1000;
                    tradeData.TodayLendStockBalance = (Convert.ToDecimal(stockData[2].Replace(",", "")) + Convert.ToDecimal(stockData[3].Replace(",", "")) - Convert.ToDecimal(stockData[4].Replace(",", ""))) / 1000;
                }
                else
                {
                    DailyMarketTradeData tradeData = new DailyMarketTradeData
                    {
                        Symbol = stockData[0],
                        SymbolName = stockData[1],
                        YesterdayLendStockBalance = Convert.ToDecimal(stockData[2].Replace(",", "")) / 1000,
                        TodayLendStockPlusCount = Convert.ToDecimal(stockData[3].Replace(",", "")) / 1000,
                        TodayLendStockReturnCount = Convert.ToDecimal(stockData[4].Replace(",", "")) / 1000,
                        TodayLendStockBalance = (Convert.ToDecimal(stockData[2].Replace(",", "")) + Convert.ToDecimal(stockData[3].Replace(",", "")) - Convert.ToDecimal(stockData[4].Replace(",", ""))) / 1000
                    };
                    this._dataContainer.Add(stockData[0], tradeData);
                }
            }
        }


        //取得上一個交易日的資料，這邊可能要參考每一年的交易日跟放假日設定
        private DateTime GetLastTradeDate()
        {
            //可能要以當下時間點去判斷這個lastDate的值，例如目前時間是下午1700，那很有可能已經有今天剛剛收盤後的最新資料了
            //如果目前時間是在上午1200，那能夠抓到的資料就只有昨天的資料
            DateTime lastDate = DateTime.Today.AddDays(-1);
            bool keepLoop = true;
            do {

                if (lastDate.DayOfWeek == DayOfWeek.Friday ||
                    lastDate.DayOfWeek == DayOfWeek.Monday ||
                     lastDate.DayOfWeek == DayOfWeek.Thursday ||
                    lastDate.DayOfWeek == DayOfWeek.Tuesday ||
                    lastDate.DayOfWeek == DayOfWeek.Wednesday)
                {
                    keepLoop = false;
                }
                else //六、日
                {
                    lastDate = lastDate.AddDays(-1);
                }
            }
            while (keepLoop);

            return lastDate;
        }
    }

    public class LendStockRspMsg
    {
        public string stat { get; set; }
        public string title { get; set; }
        public List<string> fields { get; set; }
        public string date { get; set; }
        public string selectType { get; set; }
        public List<List<string>>  data { get; set; }
        public List<string> notes { get; set; }
    }

    public class MarginTradeRspMsg
    {
        public string stat { get; set; }
        public string creditTitle { get; set; }
        public string creditFields { get; set; }
        public string creditList { get; set; }
        public List<string> creditNotes { get; set; }
        public string title { get; set; }
        public List<string> fields { get; set; }
        public List<string> notes { get; set; }
        public List<List<string>> data { get; set; }
        public string date { get; set; }
        public string selectType { get; set; }
    }
}
