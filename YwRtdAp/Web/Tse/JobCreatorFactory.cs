using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YwRtdAp.Web.Tse.Creator;

namespace YwRtdAp.Web.Tse
{
    public class JobCreatorFactory
    {
        private static JobCreatorFactory _instance { get; set; }        

        public static JobCreatorFactory GetFactory()
        {
            if (_instance == null)
            {
                _instance = new JobCreatorFactory();
            }
            return _instance;
        }

        private int _mainCreatorQty { get; set; }

        private Dictionary<JobCreatorType, JobCreator> _jobCreatorMap { get; set; }

        private JobCreatorFactory()
        {
            this._jobCreatorMap = new Dictionary<JobCreatorType, JobCreator>();
            this._mainCreatorQty = 2;
        }

        public JobCreator RandomProduce()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            int rndNo = rnd.Next(this._mainCreatorQty);

            return Produce((JobCreatorType)rndNo);
        }

        private JobCreator Produce(JobCreatorType t)
        {
            JobCreator creator = null;
            if (this._jobCreatorMap.TryGetValue(t, out creator) == false)
            {
                switch (t)
                {
                    case JobCreatorType.MiIndex:
                        creator = new MiIndexJobCreator();
                        break;
                    case JobCreatorType.Fmtqik:
                        creator = new FmtqikJobCreator();
                        break;
                    case JobCreatorType.StockFirst:
                        creator = new StockFirstJobCreator();
                        break;
                    case JobCreatorType.MiIndex20:
                        creator = new MiIndex20JobCreator();
                        break;
                    case JobCreatorType.Mi5Mins:
                        creator = new Mi5MinsJobCreator();
                        break;
                    case JobCreatorType.Bfiamu:
                        creator = new BfiamuJobCreator();
                        break;
                    case JobCreatorType.StockDay:
                        creator = new StockDayJobCreator();
                        break;
                    case JobCreatorType.Twtasu:
                        creator = new TwtasuJobCreator();
                        break;
                    case JobCreatorType.StockDayAvg:
                        creator = new StockDayAvgJobCreator();
                        break;
                    case JobCreatorType.Fmsrfk:
                        creator = new FmsrfkJobCreator();
                        break;
                    case JobCreatorType.Fmnptk:
                        creator = new FmnptkJobCreator();
                        break;
                    case JobCreatorType.Twt72u:
                        creator = new Twt72uJobCreator();
                        break;
                    case JobCreatorType.Twt93u:
                        creator = new Twt93uJobCreator();
                        break;
                    case JobCreatorType.MiMargin:
                        creator = new MiMarginJobCreator();
                        break;
                    default:
                        creator = null;
                        break;
                }

                if (creator != null)
                {
                    this._jobCreatorMap.Add(t, creator);
                }
            }

            if (creator != null)
            {
                Console.WriteLine("[ Produce ] 這次產生[ {0} ]類型的JobCreator", t.ToString().ToUpper());
            }           

            return creator;
        }

        public void SetCompleteJob(TseJob job)
        { 
            JobCreator creator = null;
            if (this._jobCreatorMap.TryGetValue(job.CreatorType, out creator))
            {
                creator.AddCompleteJob(job);
            }
        }
    }

    public enum JobCreatorType : int
    {
        /// <summary>
        /// MI_MARGIN, 交易資訊 > 融資融券與可借券賣出額度 > 融資融券餘額。date(yyyyMMdd)。start from 2001/01/01
        /// 資券餘額資料
        /// </summary>
        MiMargin = 0,

        /// <summary>
        /// TWT72U, http://www.tse.com.tw/zh/page/trading/exchange/TWT72U.html
        /// date(yyyyMMdd)。start from 2004/11/22
        /// </summary>
        Twt72u = 1,
        /// <summary>
        /// TWT93U, 交易資訊 > 融資融券與可借券賣出額度 > 融券借券賣出餘額。date(yyyyMMdd)。start from 2005/07/01
        /// </summary>
        Twt93u = 2,

        /// <summary>
        /// FMTQIK, 盤後資訊->每日市場成交資訊。month(yyyyMMdd)
        /// </summary>
        Fmtqik = 3,
        /// <summary>
        /// STOCK_FIRST, 盤後資訊->每日第一上市外國股票成交量值。date(yyyyMMdd)。start from 2004/02/11
        /// </summary>
        StockFirst = 4,
        /// <summary>
        /// MI_INDEX20, 盤後資訊->每日成交量前20名證券。date(yyyyMMdd)。start from 2004/02/11
        /// </summary>
        MiIndex20 = 5,
        /// <summary>
        /// MI_5MINS, 盤後資訊 -> 每5秒委託成交統計。 date(yyyyMMdd)。start from 2004/10/15
        /// </summary>
        Mi5Mins = 6,
        /// <summary>
        /// BFIAMU, 盤後資訊 > 各類指數日成交量值。date(yyyyMMdd)。start from 2001/7/9
        /// </summary>
        Bfiamu = 7,
        /// <summary>
        /// STOCK_DAY, 盤後資訊 > 個股日成交資訊。month(yyyyMMdd)。start from 1992/01/04
        /// </summary>
        StockDay = 8,
        /// <summary>
        /// TWTASU, 盤後資訊 > 當日融券賣出與借券賣出成交量值。month(yyyyMMdd)。start 2008/09/26
        /// </summary>
        Twtasu = 9,
        /// <summary>
        /// STOCK_DAY_AVG, 盤後資訊 > 個股日收盤價及月平均價。month(yyyyMMdd)。start 1999/01/05
        /// </summary>
        StockDayAvg = 10,
        /// <summary>
        /// FMSRFK, 盤後資訊 > 個股月成交資訊。year(yyyyMMdd)。start from 1992/01/01
        /// </summary>
        Fmsrfk = 11,
        /// <summary>
        /// FMNPTK, 盤後資訊 > 個股年成交資訊。start from 1991/01/01
        /// </summary>
        Fmnptk = 12,
        /// <summary>
        /// MI_INDEX, 盤後資訊->每日收盤行情。date(yyyyMMdd), type。start from 2004/02/11
        /// </summary>
        MiIndex = 13,
        

    }
}
