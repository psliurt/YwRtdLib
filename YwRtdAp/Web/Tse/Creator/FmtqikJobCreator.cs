using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using HttpRS;
using Newtonsoft.Json;

namespace YwRtdAp.Web.Tse.Creator
{
    public class FmtqikJobCreator : JobCreator
    {
        private bool _isThisMonthJobCreated { get; set; }

        private List<DateTime> _monthFileList { get; set; }

        public FmtqikJobCreator()
        {
            SetUpHeader();
            this._startDate = new DateTime(1990, 1, 4);
            this._endDate = GetLastEndDay();
            this._mainDir = "FMTQIK";
            this._monthFileList = new List<DateTime>();
            this._isThisMonthJobCreated = false;
            LoadCompleteFileData();
        }


        public override TseJob CreateJob()
        {
            DateTime? jobMonth = GetJobMonth();
            if (jobMonth.HasValue == false)
            {
                return null;
            }

            string url = string.Format("http://www.tse.com.tw/exchangeReport/FMTQIK?response=json&date={0}&", jobMonth.Value.ToString("yyyyMMdd"));

            TseJob job = new TseJob
            {
                CreatorType = JobCreatorType.Fmtqik,
                JobType = "FMTQIK",
                HttpHeader = this._httpHeader,
                Url = url,
                MainDirName = this._mainDir,                
                JobDate = jobMonth.Value,
                IsSaturdayOrSunday = (jobMonth.Value.DayOfWeek == DayOfWeek.Saturday || jobMonth.Value.DayOfWeek == DayOfWeek.Sunday) ? true : false,
                FilePath = string.Format("./{0}/{1}.json", this._mainDir, jobMonth.Value.ToString("yyyy_MM"))
            };

            return job;
        }

        /// <summary>
        /// 載入到目前為止已經完成的工作資料
        /// </summary>
        protected override void LoadCompleteFileData()
        {
            string fileContent = null;
            using (StreamReader sr = new StreamReader("./Data/TseMeta/FMTQIK.txt"))
            {
                fileContent = sr.ReadToEnd();
            }
            string[] jsonStrings = fileContent.Split(new string[] { "\n\r" }, StringSplitOptions.None);
            foreach (string json in jsonStrings)
            {
                if (string.IsNullOrEmpty(json) == false)
                {
                    FmtqikMeta metaData = JsonConvert.DeserializeObject<FmtqikMeta>(json);

                    if (this._monthFileList.Contains(metaData.Dm) == false)
                    {
                        this._monthFileList.Add(metaData.Dm);
                    }                    
                }
            }  
        }

        public override void AddCompleteJob(TseJob job)
        {
            this._monthFileList.Add(job.JobDate);

            FmtqikMeta meta = new FmtqikMeta
            {
                Dm = job.JobDate,
                HasErr = job.WithErr,
                IsE = job.IsComplete,
                IsH = job.IsSaturdayOrSunday
            };
            string metaRow = JsonConvert.SerializeObject(meta);
            UpdateMetaRecord(job.MainDirName, metaRow);

        }

        protected override void SetUpHeader()
        {
            this._httpHeader = new HttpHeaderList();
            this._httpHeader.AddHeader("Accept", "application/json, text/javascript, */*; q=0.01");
            this._httpHeader.AddHeader("Host", "www.tse.com.tw");
            this._httpHeader.AddHeader("Refer", "http://www.tse.com.tw/zh/page/trading/exchange/FMTQIK.html");
            this._httpHeader.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36");
            this._httpHeader.AddHeader("X-Requested-With", "XMLHttpRequest");
        }

        private DateTime? GetJobMonth()
        {
            if (this._monthFileList.Contains(this._endDate) == false)
            {
                this._isThisMonthJobCreated = true;
                return DateTime.Today;
            }

            if (this._isThisMonthJobCreated == false)
            {
                this._isThisMonthJobCreated = true;
                return DateTime.Today;
            }

            DateTime earlyMonth = this._monthFileList.OrderBy(x => x).FirstOrDefault();
            
            DateTime monthBeforeEarlyMonth =  earlyMonth.AddMonths(-1);

            DateTime firstDayOfMonthBeforeEarlyMonth = new DateTime(monthBeforeEarlyMonth.Year, monthBeforeEarlyMonth.Month, 1);

            DateTime monthOfStartDate = new DateTime(this._startDate.Year, this._startDate.Month, 1);
            if (firstDayOfMonthBeforeEarlyMonth < monthOfStartDate)
            {
                return null;
            }
            else
            {
                return firstDayOfMonthBeforeEarlyMonth;
            }
        }
    }

    class FmtqikMeta
    {
        /// <summary>
        /// DataMonth 資料月份，這個類型的資料是一個月一個月去查的
        /// </summary>
        public DateTime Dm { get; set; }

        public bool IsH { get; set; }

        public bool IsE { get; set; }

        public bool HasErr { get; set; }
    }
}
