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

        private Dictionary<JobCreatorType, JobCreator> _jobCreatorMap { get; set; }

        private JobCreatorFactory()
        {
            this._jobCreatorMap = new Dictionary<JobCreatorType, JobCreator>();
        }

        public JobCreator RandomProduce()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            int rndNo = rnd.Next(4);

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
                    default:
                        creator = null;
                        break;
                }

                if (creator != null)
                {
                    this._jobCreatorMap.Add(t, creator);
                }
            }            

            return creator;
        }
    }

    public enum JobCreatorType : int
    { 
        /// <summary>
        /// MI_INDEX, 盤後資訊->每日收盤行情。date(yyyyMMdd), type。start from 2004/02/11
        /// </summary>
        MiIndex = 0,
        /// <summary>
        /// FMTQIK, 盤後資訊->每日市場成交資訊。date(yyyyMMdd)
        /// </summary>
        Fmtqik = 1,
        /// <summary>
        /// STOCK_FIRST, 盤後資訊->每日第一上市外國股票成交量值。date(yyyyMMdd)。start from 2004/02/11
        /// </summary>
        StockFirst = 2,
        /// <summary>
        /// MI_INDEX20, 盤後資訊->每日成交量前20名證券。date(yyyyMMdd)。start from 2004/02/11
        /// </summary>
        MiIndex20 = 3,
    }
}
