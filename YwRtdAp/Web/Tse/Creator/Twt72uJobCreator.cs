using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpRS;
using Newtonsoft.Json;
using System.IO;


namespace YwRtdAp.Web.Tse.Creator
{
    /// <summary>
    /// TWT72U,
    /// url:
    /// http://www.tse.com.tw/exchangeReport/TWT72U?response=json&date=20180202&selectType=SLBNLB&_=1517750570220
    /// by date & selectType 
    /// 
    /// header:
    /// GET /exchangeReport/TWT72U?response=json&date=20180202&selectType=SLBNLB&_=1517750570220 HTTP/1.1
    ///Host: www.tse.com.tw
    ///Connection: keep-alive
    ///Accept: application/json, text/javascript, */*; q=0.01
    ///X-Requested-With: XMLHttpRequest
    ///User-Agent: Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36
    ///Referer: http://www.tse.com.tw/zh/page/trading/exchange/TWT72U.html
    ///Accept-Encoding: gzip, deflate
    ///Accept-Language: zh-TW,zh;q=0.9,en-US;q=0.8,en;q=0.7,zh-CN;q=0.6
    ///Cookie: _ga=GA1.3.1632887126.1516719777; _gid=GA1.3.1615513931.1517641069; JSESSIONID=AD9029180BF11DD4CE402F4CC5223E56
    /// </summary>
    public class Twt72uJobCreator : JobCreator
    {
        private List<string> _subTypeList { get; set; }
        private Dictionary<string, List<DateTime>> _subTypeToFileList { get; set; }

        public Twt72uJobCreator()
        {
            SetUpHeader();
            this._startDate = new DateTime(2004, 11, 22);
            this._endDate = GetLastEndDay();
            this._mainDir = "TWT72U";
            CreateSubTypeList();
            CreateSubTypeFileMap();
            LoadCompleteFileData();
        }

        protected override void SetUpHeader()
        {
            this._httpHeader = new HttpHeaderList();
            this._httpHeader.AddHeader("Accept", "application/json, text/javascript, */*; q=0.01");
            this._httpHeader.AddHeader("Host", "www.tse.com.tw");
            this._httpHeader.AddHeader("Refer", "http://www.tse.com.tw/zh/page/trading/exchange/TWT72U.html");
            this._httpHeader.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36");
            this._httpHeader.AddHeader("X-Requested-With", "XMLHttpRequest");
        }

        protected override void LoadCompleteFileData()
        {
            string fileContent = null;

            FileInfo fi = new FileInfo("./Data/TseMeta/TWT72U.txt");
            if (fi.Exists == false)
            {
                fi.Create().Close();
            }

            using (StreamReader sr = new StreamReader("./Data/TseMeta/TWT72U.txt"))
            {
                fileContent = sr.ReadToEnd();
            }

            string[] jsonStrings = fileContent.Split(new string[] { "\n\r" }, StringSplitOptions.None);
            foreach (string json in jsonStrings)
            {
                if (string.IsNullOrEmpty(json) == false)
                {
                    Twt72uMeta metaData = JsonConvert.DeserializeObject<Twt72uMeta>(json);
                    List<DateTime> fileList = null;
                    if (this._subTypeToFileList.TryGetValue(metaData.St, out fileList))
                    {
                        if (fileList.Contains(metaData.Dd) == false)
                        {
                            fileList.Add(metaData.Dd);
                        }
                    }
                    else
                    {
                        fileList = new List<DateTime>();
                        fileList.Add(metaData.Dd);
                        this._subTypeToFileList.Add(metaData.St, fileList);
                    }
                }
            } 
        }

        public override TseJob CreateJob()
        {
            string subType = RandomSelectSubType();
            DateTime? jobDate = GetJobDate(subType);
            if (jobDate.HasValue == false)
            {
                return null;
            }

            string url = string.Format("http://www.tse.com.tw/exchangeReport/TWT72U?response=json&date={0}&selectType={1}", jobDate.Value.ToString("yyyyMMdd"), subType);
            TseJob job = new TseJob
            {
                CreatorType = JobCreatorType.Twt72u,
                JobType = "TWT72U",
                HttpHeader = this._httpHeader,
                Url = url,
                MainDirName = this._mainDir,
                SubDirName = subType,
                JobDate = jobDate.Value,
                IsSaturdayOrSunday = (jobDate.Value.DayOfWeek == DayOfWeek.Saturday || jobDate.Value.DayOfWeek == DayOfWeek.Sunday) ? true : false,
                FilePath = string.Format("./{0}/{1}/{2}.json", this._mainDir, subType, jobDate.Value.ToString("yyyy_MM_dd"))
            };

            return job;
        }

        public override void AddCompleteJob(TseJob job)
        {
            List<DateTime> dateList = null;
            Console.WriteLine("[ AddCompleteJob ] 把任務的次類型[ {0} ]設定為已完成，任務日期[ {1} ]", job.SubDirName, job.JobDate.ToString("yyyy-MM-dd"));
            if (this._subTypeToFileList.TryGetValue(job.SubDirName, out dateList))
            {
                dateList.Add(job.JobDate);
            }
            //TODO:把TseJob Map成MiIndexMeta  然後寫入檔案內

            Twt72uMeta meta = new Twt72uMeta
            {
                St = job.SubDirName,
                Dd = job.JobDate,
                IsH = job.IsSaturdayOrSunday,
                IsE = job.IsComplete,
                HasErr = job.WithErr
            };

            string metaRow = JsonConvert.SerializeObject(meta);
            using (StreamWriter sw = new StreamWriter("./Data/TseMeta/TWT72U.txt", true))
            {
                Console.WriteLine("[ AddCompleteJob ] 把任務的次類型[ {0} ]的完成資訊寫入檔案，任務日期[ {1} ]", job.SubDirName, job.JobDate.ToString("yyyy-MM-dd"));
                sw.WriteLine(metaRow);
            }
        }

        private DateTime? GetJobDate(string subType)
        {
            List<DateTime> dateList = null;
            if (this._subTypeToFileList.TryGetValue(subType, out dateList))
            {
                if (dateList.Contains(this._endDate) == false)
                {
                    return this._endDate;
                }

                DateTime earlyDate = dateList.OrderBy(x => x).FirstOrDefault();
                DateTime dayBeforeEarlyDate = earlyDate.AddDays(-1);
                if (dayBeforeEarlyDate < this._startDate)
                {
                    return null;
                }
                else
                {
                    return dayBeforeEarlyDate;
                }

            }
            return null;
        }

        private string RandomSelectSubType()
        {
            Random rnd = new Random((int)DateTime.Now.Ticks);
            int idx = rnd.Next(this._subTypeList.Count);
            return this._subTypeList[idx];
        }

        private void CreateSubTypeList()
        {
            this._subTypeList = new List<string>();
            this._subTypeList.Add("SLBNLB");
            //this._subTypeList.Add("SLB");
            //this._subTypeList.Add("NLB");
            //this._subTypeList.Add("LON");
            //this._subTypeList.Add("TWTBBU");            
        }

        private void CreateSubTypeFileMap()
        {
            this._subTypeToFileList = new Dictionary<string, List<DateTime>>();
            this._subTypeToFileList.Add("SLBNLB", new List<DateTime>());
            //this._subTypeToFileList.Add("SLB", new List<DateTime>());
            //this._subTypeToFileList.Add("NLB", new List<DateTime>());
            //this._subTypeToFileList.Add("LON", new List<DateTime>());
            //this._subTypeToFileList.Add("TWTBBU", new List<DateTime>());            
        }
    }

    class Twt72uMeta
    {
        /// <summary>
        /// SubType
        /// </summary>
        public string St { get; set; }

        /// <summary>
        /// DataDate
        /// </summary>
        public DateTime Dd { get; set; }

        /// <summary>
        /// IsHoilday
        /// </summary>
        public bool IsH { get; set; }

        /// <summary>
        /// IsEnd
        /// </summary>
        public bool IsE { get; set; }

        /// <summary>
        /// HasError
        /// </summary>
        public bool HasErr { get; set; }
    }
}
