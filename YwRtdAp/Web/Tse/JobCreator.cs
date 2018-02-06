using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HttpRS;
using Newtonsoft.Json;
using System.IO;

namespace YwRtdAp.Web.Tse
{
    public abstract class JobCreator
    {
        protected HttpHeaderList _httpHeader { get; set; }
        protected DateTime _startDate { get; set; }
        protected DateTime _endDate { get; set; }
        protected string _mainDir { get; set; }

        protected abstract void SetUpHeader();
        protected abstract void LoadCompleteFileData();

        public abstract TseJob CreateJob();        
        public abstract void AddCompleteJob(TseJob job);

        /// <summary>
        /// 這個日期可能要參考國定假日跟補假的相關規定
        /// </summary>
        /// <returns></returns>
        protected virtual DateTime GetLastEndDay()
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
            {
                return DateTime.Today.AddDays(-1);
            }

            if (DateTime.Now.Hour > 16)
            {
                return DateTime.Today;
            }
            else
            {
                return DateTime.Today.AddDays(-1);
            }
        }

        protected virtual void UpdateMetaRecord(string fileName, string metaRecord)
        {            
            string filePath = string.Format("./Data/TseMeta/{0}.txt", fileName);
            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine(metaRecord);
            }
        }
    }
}
