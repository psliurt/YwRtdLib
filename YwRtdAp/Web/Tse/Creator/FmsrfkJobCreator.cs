using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YwRtdAp.Web.Tse.Creator
{
    /// <summary>
    /// FMSRFK
    /// url:
    /// http://www.tse.com.tw/exchangeReport/FMSRFK?response=json&date=20180204&stockNo=2330&_=1517747315686
    /// by year & stockNo
    /// 
    /// header:
    ///GET /exchangeReport/FMSRFK?response=json&date=20180204&stockNo=2330&_=1517747315686 HTTP/1.1
    ///Host: www.tse.com.tw
    ///Connection: keep-alive
    ///Accept: application/json, text/javascript, */*; q=0.01
    ///X-Requested-With: XMLHttpRequest
    ///User-Agent: Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36
    ///Referer: http://www.tse.com.tw/zh/page/trading/exchange/FMSRFK.html
    ///Accept-Encoding: gzip, deflate
    ///Accept-Language: zh-TW,zh;q=0.9,en-US;q=0.8,en;q=0.7,zh-CN;q=0.6
    ///Cookie: _ga=GA1.3.1632887126.1516719777; _gid=GA1.3.1615513931.1517641069; JSESSIONID=9A37B60AFB8C596CC07C202B6DBA28E8
    /// </summary>
    public class FmsrfkJobCreator : JobCreator
    {
        protected override void SetUpHeader()
        {
            throw new NotImplementedException();
        }

        protected override void LoadCompleteFileData()
        {
            throw new NotImplementedException();
        }

        public override TseJob CreateJob()
        {
            throw new NotImplementedException();
        }

        public override void AddCompleteJob(TseJob job)
        {
            throw new NotImplementedException();
        }
    }
}
