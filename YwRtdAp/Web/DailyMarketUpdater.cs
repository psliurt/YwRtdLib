using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using YwRtdAp.CombineObject;
using YwRtdAp.Db.Dal;
using HttpRS;
using Newtonsoft.Json;
using Newtonsoft;

namespace YwRtdAp.Web
{
    public class DailyMarketUpdater
    {        
        private static DailyMarketUpdater _instance { get; set; }        

        private ConcurrentDictionary<string, PriorAfterMarketStatistic> _afterMarketDatas { get; set; }
        private RtdRepository _rep { get; set; }
        private DateTime _lastDate { get; set; }

        public static DailyMarketUpdater Initialize(ConcurrentDictionary<string, PriorAfterMarketStatistic> marketDic,RtdRepository rep)
        { 
            if(_instance == null)
            {
                _instance = new DailyMarketUpdater(marketDic,rep);
            }
            return _instance;
        }

        private DailyMarketUpdater(ConcurrentDictionary<string, PriorAfterMarketStatistic> marketDic,RtdRepository rep)
        {
            this._afterMarketDatas = marketDic;
            this._rep = rep;
            InitDataFile();
        }

        private void InitDataFile()
        {
            SetUpLastestDay();

            DirectoryInfo afterMarketDir = null;
            if (Directory.Exists("./Data/AfterMarket") == false)
            {
                afterMarketDir = Directory.CreateDirectory("./Data/AfterMarket");
            }
            else
            {
                afterMarketDir = new DirectoryInfo("./Data/AfterMarket");
            }

            DirectoryInfo[] childDirs = afterMarketDir.GetDirectories();
            if (childDirs.Where(x => x.Name == "Exchange").Count() == 0) //上市
            {
                Directory.CreateDirectory("./Data/AfterMarket/Exchange");
            }
            if (childDirs.Where(x => x.Name == "OverTheCounter").Count() == 0) //上櫃
            {
                Directory.CreateDirectory("./Data/AfterMarket/OverTheCounter");
            }
            if (childDirs.Where(x => x.Name == "Emerging").Count() == 0)
            {
                Directory.CreateDirectory("./Data/AfterMarket/Emerging");
            }
        }

        private void SetUpLastestDay()
        { 
            DateTime yesterday = DateTime.Today.AddDays(-1);
            DateTime currentTime = DateTime.Now;

            if (currentTime.Hour <= 17)
            {
                this._lastDate = yesterday;
            }
            else
            {
                this._lastDate = DateTime.Today;
            }
        }


    }
}
