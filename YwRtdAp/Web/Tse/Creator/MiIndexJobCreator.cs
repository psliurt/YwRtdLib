using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HttpRS;

namespace YwRtdAp.Web.Tse.Creator
{
    public class MiIndexJobCreator : JobCreator
    {
        private HttpHeaderList _httpHeader { get; set; }
        private DateTime _startDate { get; set; }
        private DateTime _endDate { get; set; }
        private string _mainDir { get; set; }
        private List<string> _subTypeList { get; set; }

        public MiIndexJobCreator()
        {
            SetUpHeader();
            this._startDate = new DateTime(2004, 2, 11);
            this._endDate = DateTime.Now;
            this._mainDir = "MI_INDEX";
            CreateSubTypeList();
        }

        public override TseJob CreateJob()
        {
            //TODO:要想辦法隨機產生要抓的日期，這個日期最好是可以檢查目前還沒有抓取到的資料

            string subType = RandomSelectSubType();
            string url = string.Format("http://www.tse.com.tw/exchangeReport/MI_INDEX?response=json&date={0}&type={1}", "", subType);
            TseJob job = new TseJob { 
                JobType = "MI_INDEX",
                HttpHeader = this._httpHeader,  
                Url = url,  
                MainDirName = this._mainDir,
                SubDirName = subType,                

            };

            return job;
        }

        private void SetUpHeader()
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
    }
}
