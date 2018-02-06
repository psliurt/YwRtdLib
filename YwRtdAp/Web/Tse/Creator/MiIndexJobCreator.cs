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
    public class MiIndexJobCreator : JobCreator
    {
        //private HttpHeaderList _httpHeader { get; set; }
        //private DateTime _startDate { get; set; }
        //private DateTime _endDate { get; set; }
        //private string _mainDir { get; set; }
        private List<string> _subTypeList { get; set; }

        private Dictionary<string, List<DateTime>> _subTypeToFileList { get; set; }

        public MiIndexJobCreator()
        {
            SetUpHeader();
            this._startDate = new DateTime(2004, 2, 11);
            this._endDate = GetLastEndDay();
            this._mainDir = "MI_INDEX";            
            CreateSubTypeList();
            CreateSubTypeFileMap();
            LoadCompleteFileData();
        }

        public override TseJob CreateJob()
        {            
            string subType = RandomSelectSubType();
            DateTime? jobDate = GetJobDate(subType);
            if (jobDate.HasValue == false)
            {
                return null;
            }

            string url = string.Format("http://www.tse.com.tw/exchangeReport/MI_INDEX?response=json&date={0}&type={1}", jobDate.Value.ToString("yyyyMMdd"), subType);
            TseJob job = new TseJob
            {
                CreatorType = JobCreatorType.MiIndex,
                JobType = "MI_INDEX",
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
        /// <summary>
        /// 設定好每一個TseJob要發送出去的Http Request Header
        /// </summary>
        protected override void SetUpHeader()
        {
            this._httpHeader = new HttpHeaderList();
            this._httpHeader.AddHeader("Accept", "application/json, text/javascript, */*; q=0.01");
            this._httpHeader.AddHeader("Host", "www.tse.com.tw");
            this._httpHeader.AddHeader("Refer", "http://www.tse.com.tw/zh/page/trading/exchange/MI_INDEX.html");
            this._httpHeader.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36");
            this._httpHeader.AddHeader("X-Requested-With", "XMLHttpRequest");
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
            this._subTypeList.Add("MS");
            this._subTypeList.Add("MS2");
            this._subTypeList.Add("ALL");
            this._subTypeList.Add("ALLBUT0999");
            this._subTypeList.Add("0049");
            this._subTypeList.Add("019919T");
            this._subTypeList.Add("0999");
            this._subTypeList.Add("0999P");
            this._subTypeList.Add("0999C");
            this._subTypeList.Add("0999X");
            this._subTypeList.Add("0999Y");
            this._subTypeList.Add("0999GA");
            this._subTypeList.Add("0999GD");
            this._subTypeList.Add("0999G9");
            this._subTypeList.Add("CB");
            this._subTypeList.Add("01");
            this._subTypeList.Add("02");
            this._subTypeList.Add("03");
            this._subTypeList.Add("04");
            this._subTypeList.Add("05");
            this._subTypeList.Add("06");
            this._subTypeList.Add("07");            
            this._subTypeList.Add("21");
            this._subTypeList.Add("22");
            this._subTypeList.Add("08");            
            this._subTypeList.Add("09");
            this._subTypeList.Add("10");
            this._subTypeList.Add("11");
            this._subTypeList.Add("12");
            this._subTypeList.Add("13");
            this._subTypeList.Add("24");
            this._subTypeList.Add("25");
            this._subTypeList.Add("26");
            this._subTypeList.Add("27");
            this._subTypeList.Add("28");
            this._subTypeList.Add("29");
            this._subTypeList.Add("30");
            this._subTypeList.Add("31");
            this._subTypeList.Add("14");
            this._subTypeList.Add("15");
            this._subTypeList.Add("16");
            this._subTypeList.Add("17");
            this._subTypeList.Add("18");
            this._subTypeList.Add("9299");
            this._subTypeList.Add("23");
            this._subTypeList.Add("19");
            this._subTypeList.Add("20");
        }

        private void CreateSubTypeFileMap()
        {
            this._subTypeToFileList = new Dictionary<string, List<DateTime>>();
            this._subTypeToFileList.Add("MS", new List<DateTime>());
            this._subTypeToFileList.Add("MS2", new List<DateTime>());
            this._subTypeToFileList.Add("ALL", new List<DateTime>());
            this._subTypeToFileList.Add("ALLBUT0999", new List<DateTime>());
            this._subTypeToFileList.Add("0049", new List<DateTime>());
            this._subTypeToFileList.Add("019919T", new List<DateTime>());
            this._subTypeToFileList.Add("0999", new List<DateTime>());
            this._subTypeToFileList.Add("0999P", new List<DateTime>());
            this._subTypeToFileList.Add("0999C", new List<DateTime>());
            this._subTypeToFileList.Add("0999X", new List<DateTime>());
            this._subTypeToFileList.Add("0999Y", new List<DateTime>());
            this._subTypeToFileList.Add("0999GA", new List<DateTime>());
            this._subTypeToFileList.Add("0999GD", new List<DateTime>());
            this._subTypeToFileList.Add("0999G9", new List<DateTime>());
            this._subTypeToFileList.Add("CB", new List<DateTime>());
            this._subTypeToFileList.Add("01", new List<DateTime>());
            this._subTypeToFileList.Add("02", new List<DateTime>());
            this._subTypeToFileList.Add("03", new List<DateTime>());
            this._subTypeToFileList.Add("04", new List<DateTime>());
            this._subTypeToFileList.Add("05", new List<DateTime>());
            this._subTypeToFileList.Add("06", new List<DateTime>());
            this._subTypeToFileList.Add("07", new List<DateTime>());
            this._subTypeToFileList.Add("21", new List<DateTime>());
            this._subTypeToFileList.Add("22", new List<DateTime>());
            this._subTypeToFileList.Add("08", new List<DateTime>());
            this._subTypeToFileList.Add("09", new List<DateTime>());
            this._subTypeToFileList.Add("10", new List<DateTime>());
            this._subTypeToFileList.Add("11", new List<DateTime>());
            this._subTypeToFileList.Add("12", new List<DateTime>());
            this._subTypeToFileList.Add("13", new List<DateTime>());
            this._subTypeToFileList.Add("24", new List<DateTime>());
            this._subTypeToFileList.Add("25", new List<DateTime>());
            this._subTypeToFileList.Add("26", new List<DateTime>());
            this._subTypeToFileList.Add("27", new List<DateTime>());
            this._subTypeToFileList.Add("28", new List<DateTime>());
            this._subTypeToFileList.Add("29", new List<DateTime>());
            this._subTypeToFileList.Add("30", new List<DateTime>());
            this._subTypeToFileList.Add("31", new List<DateTime>());
            this._subTypeToFileList.Add("14", new List<DateTime>());
            this._subTypeToFileList.Add("15", new List<DateTime>());
            this._subTypeToFileList.Add("16", new List<DateTime>());
            this._subTypeToFileList.Add("17", new List<DateTime>());
            this._subTypeToFileList.Add("18", new List<DateTime>());
            this._subTypeToFileList.Add("9299", new List<DateTime>());
            this._subTypeToFileList.Add("23", new List<DateTime>());
            this._subTypeToFileList.Add("19", new List<DateTime>());
            this._subTypeToFileList.Add("20", new List<DateTime>());
        }

        /// <summary>
        /// 載入目前已完成的工作資料
        /// </summary>
        protected override void LoadCompleteFileData()
        {
            //FileInfo recordFile = new FileInfo("./Data/TseMeta/MI_INDEX.txt");
            string fileContent = null;
            using (StreamReader sr = new StreamReader("./Data/TseMeta/MI_INDEX.txt"))
            {
                fileContent = sr.ReadToEnd();
            }

            string[] jsonStrings = fileContent.Split(new string[] { "\n\r" }, StringSplitOptions.None);
            foreach (string json in jsonStrings)
            {
                if (string.IsNullOrEmpty(json) == false)
                {
                    MiIndexMeta metaData = JsonConvert.DeserializeObject<MiIndexMeta>(json);
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

        public override void AddCompleteJob(TseJob job)
        {
            List<DateTime> dateList = null;
            if (this._subTypeToFileList.TryGetValue(job.SubDirName, out dateList))
            {
                dateList.Add(job.JobDate);
            }
            //TODO:把TseJob Map成MiIndexMeta  然後寫入檔案內

            MiIndexMeta meta = new MiIndexMeta
            {
                St = job.SubDirName,
                Dd = job.JobDate,
                IsH = job.IsSaturdayOrSunday,
                IsE = job.IsComplete,
                HasErr = job.WithErr
            };

            string metaRow = JsonConvert.SerializeObject(meta);
            using (StreamWriter sw = new StreamWriter("./Data/TseMeta/MI_INDEX.txt", true))
            {
                sw.WriteLine(metaRow);
            }
        }

        
    }

    class MiIndexMeta
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
