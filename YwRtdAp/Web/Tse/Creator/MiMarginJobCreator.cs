using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpRS;
using System.IO;
using Newtonsoft.Json;

namespace YwRtdAp.Web.Tse.Creator
{
    /// <summary>
    /// MI_MARGIN,
    /// url:
    /// http://www.tse.com.tw/exchangeReport/MI_MARGN?response=json&date=20180206&selectType=MS&_=1517926557329
    /// by date & selectType
    /// header:
    /// GET /exchangeReport/MI_MARGN?response=json&date=20180206&selectType=ALL&_=1517926557334 HTTP/1.1
    ///Host: www.tse.com.tw
    ///Connection: keep-alive
    ///Accept: application/json, text/javascript, */*; q=0.01
    ///X-Requested-With: XMLHttpRequest
    ///User-Agent: Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36
    ///Referer: http://www.tse.com.tw/zh/page/trading/exchange/MI_MARGN.html
    ///Accept-Encoding: gzip, deflate
    ///Accept-Language: zh-TW,zh;q=0.9,en-US;q=0.8,en;q=0.7,zh-CN;q=0.6
    ///Cookie: _ga=GA1.3.1632887126.1516719777; JSESSIONID=81635DD25F163412411A50AA1D64EFFB; _gid=GA1.3.2080584169.1517926535
    /// </summary>
    public class MiMarginJobCreator : JobCreator
    {
        private List<string> _subTypeList { get; set; }
        private Dictionary<string, List<DateTime>> _subTypeToFileList { get; set; }

        public MiMarginJobCreator()
        {
            SetUpHeader();
            this._startDate = new DateTime(2001, 1, 1);
            this._endDate = GetLastEndDay();
            this._mainDir = "MI_MARGIN";
            CreateSubTypeList();
            CreateSubTypeFileMap();
            LoadCompleteFileData();
        }

        protected override void SetUpHeader()
        {
            this._httpHeader = new HttpHeaderList();
            this._httpHeader.AddHeader("Accept", "application/json, text/javascript, */*; q=0.01");
            this._httpHeader.AddHeader("Host", "www.tse.com.tw");
            this._httpHeader.AddHeader("Refer", "http://www.tse.com.tw/zh/page/trading/exchange/MI_MARGN.html");
            this._httpHeader.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36");
            this._httpHeader.AddHeader("X-Requested-With", "XMLHttpRequest");
        }

        protected override void LoadCompleteFileData()
        {            
            string fileContent = null;

            //檢查Meta檔案是否存在，不存在會建立出來
            FileInfo fi = new FileInfo("./Data/TseMeta/MI_MARGIN.txt");
            if (fi.Exists == false)
            {
                fi.Create().Close();
            }
            //Meat檔案會記錄到目前為止，MiMargin這類型的資料已經處理到哪一天了
            using (StreamReader sr = new StreamReader("./Data/TseMeta/MI_MARGIN.txt"))
            {
                fileContent = sr.ReadToEnd();
            }

            string[] jsonStrings = fileContent.Split(new string[] { "\n\r" }, StringSplitOptions.None);
            foreach (string json in jsonStrings)
            {
                if (string.IsNullOrEmpty(json) == false)
                {
                    MiMarginMeta metaData = JsonConvert.DeserializeObject<MiMarginMeta>(json);
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

        /// <summary>
        /// 建立一個MiMargin類型的TseJob
        /// </summary>
        /// <returns></returns>
        public override TseJob CreateJob()
        {
            string subType = RandomSelectSubType();
            DateTime? jobDate = GetJobDate(subType);
            if (jobDate.HasValue == false)
            {
                return null;
            }

            string url = string.Format("http://www.tse.com.tw/exchangeReport/MI_MARGN?response=json&date={0}&selectType={1}&", jobDate.Value.ToString("yyyyMMdd"), subType);
            TseJob job = new TseJob
            {
                CreatorType = JobCreatorType.MiMargin,
                JobType = "MI_MARGIN",
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
        private string RandomSelectSubType()
        {
            Random rnd = new Random((int)DateTime.Now.Ticks);
            int idx = rnd.Next(this._subTypeList.Count);
            return this._subTypeList[idx];
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

        public override void AddCompleteJob(TseJob job)
        {
            List<DateTime> dateList = null;
            if (this._subTypeToFileList.TryGetValue(job.SubDirName, out dateList))
            {
                dateList.Add(job.JobDate);
            }
            //TODO:把TseJob Map成MiIndexMeta  然後寫入檔案內

            MiMarginMeta meta = new MiMarginMeta
            {
                St = job.SubDirName,
                Dd = job.JobDate,
                IsH = job.IsSaturdayOrSunday,
                IsE = job.IsComplete,
                HasErr = job.WithErr
            };

            string metaRow = JsonConvert.SerializeObject(meta);
            using (StreamWriter sw = new StreamWriter("./Data/TseMeta/MI_MARGIN.txt", true))
            {
                sw.WriteLine(metaRow);
            }
        }

        private void CreateSubTypeList()
        {
            this._subTypeList = new List<string>();
            //this._subTypeList.Add("MS");
            //this._subTypeList.Add("MS2");
            this._subTypeList.Add("ALL");
            //this._subTypeList.Add("ALLBUT0999");
            //this._subTypeList.Add("0049");
            //this._subTypeList.Add("019919T");
            //this._subTypeList.Add("0999");
            //this._subTypeList.Add("0999P");
            //this._subTypeList.Add("0999C");
            //this._subTypeList.Add("0999X");
            //this._subTypeList.Add("0999Y");
            //this._subTypeList.Add("0999GA");
            //this._subTypeList.Add("0999GD");
            //this._subTypeList.Add("0999G9");
            //this._subTypeList.Add("CB");
            //this._subTypeList.Add("01");
            //this._subTypeList.Add("02");
            //this._subTypeList.Add("03");
            //this._subTypeList.Add("04");
            //this._subTypeList.Add("05");
            //this._subTypeList.Add("06");
            //this._subTypeList.Add("07");
            //this._subTypeList.Add("21");
            //this._subTypeList.Add("22");
            //this._subTypeList.Add("08");
            //this._subTypeList.Add("09");
            //this._subTypeList.Add("10");
            //this._subTypeList.Add("11");
            //this._subTypeList.Add("12");
            //this._subTypeList.Add("13");
            //this._subTypeList.Add("24");
            //this._subTypeList.Add("25");
            //this._subTypeList.Add("26");
            //this._subTypeList.Add("27");
            //this._subTypeList.Add("28");
            //this._subTypeList.Add("29");
            //this._subTypeList.Add("30");
            //this._subTypeList.Add("31");
            //this._subTypeList.Add("14");
            //this._subTypeList.Add("15");
            //this._subTypeList.Add("16");
            //this._subTypeList.Add("17");
            //this._subTypeList.Add("18");
            //this._subTypeList.Add("9299");
            //this._subTypeList.Add("23");
            //this._subTypeList.Add("19");
            //this._subTypeList.Add("20");
        }

        private void CreateSubTypeFileMap()
        {
            this._subTypeToFileList = new Dictionary<string, List<DateTime>>();
            //this._subTypeToFileList.Add("MS", new List<DateTime>());
            //this._subTypeToFileList.Add("MS2", new List<DateTime>());
            this._subTypeToFileList.Add("ALL", new List<DateTime>());
            //this._subTypeToFileList.Add("ALLBUT0999", new List<DateTime>());
            //this._subTypeToFileList.Add("0049", new List<DateTime>());
            //this._subTypeToFileList.Add("019919T", new List<DateTime>());
            //this._subTypeToFileList.Add("0999", new List<DateTime>());
            //this._subTypeToFileList.Add("0999P", new List<DateTime>());
            //this._subTypeToFileList.Add("0999C", new List<DateTime>());
            //this._subTypeToFileList.Add("0999X", new List<DateTime>());
            //this._subTypeToFileList.Add("0999Y", new List<DateTime>());
            //this._subTypeToFileList.Add("0999GA", new List<DateTime>());
            //this._subTypeToFileList.Add("0999GD", new List<DateTime>());
            //this._subTypeToFileList.Add("0999G9", new List<DateTime>());
            //this._subTypeToFileList.Add("CB", new List<DateTime>());
            //this._subTypeToFileList.Add("01", new List<DateTime>());
            //this._subTypeToFileList.Add("02", new List<DateTime>());
            //this._subTypeToFileList.Add("03", new List<DateTime>());
            //this._subTypeToFileList.Add("04", new List<DateTime>());
            //this._subTypeToFileList.Add("05", new List<DateTime>());
            //this._subTypeToFileList.Add("06", new List<DateTime>());
            //this._subTypeToFileList.Add("07", new List<DateTime>());
            //this._subTypeToFileList.Add("21", new List<DateTime>());
            //this._subTypeToFileList.Add("22", new List<DateTime>());
            //this._subTypeToFileList.Add("08", new List<DateTime>());
            //this._subTypeToFileList.Add("09", new List<DateTime>());
            //this._subTypeToFileList.Add("10", new List<DateTime>());
            //this._subTypeToFileList.Add("11", new List<DateTime>());
            //this._subTypeToFileList.Add("12", new List<DateTime>());
            //this._subTypeToFileList.Add("13", new List<DateTime>());
            //this._subTypeToFileList.Add("24", new List<DateTime>());
            //this._subTypeToFileList.Add("25", new List<DateTime>());
            //this._subTypeToFileList.Add("26", new List<DateTime>());
            //this._subTypeToFileList.Add("27", new List<DateTime>());
            //this._subTypeToFileList.Add("28", new List<DateTime>());
            //this._subTypeToFileList.Add("29", new List<DateTime>());
            //this._subTypeToFileList.Add("30", new List<DateTime>());
            //this._subTypeToFileList.Add("31", new List<DateTime>());
            //this._subTypeToFileList.Add("14", new List<DateTime>());
            //this._subTypeToFileList.Add("15", new List<DateTime>());
            //this._subTypeToFileList.Add("16", new List<DateTime>());
            //this._subTypeToFileList.Add("17", new List<DateTime>());
            //this._subTypeToFileList.Add("18", new List<DateTime>());
            //this._subTypeToFileList.Add("9299", new List<DateTime>());
            //this._subTypeToFileList.Add("23", new List<DateTime>());
            //this._subTypeToFileList.Add("19", new List<DateTime>());
            //this._subTypeToFileList.Add("20", new List<DateTime>());
        }
    }
    class MiMarginMeta 
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
