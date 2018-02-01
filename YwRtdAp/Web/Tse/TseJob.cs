using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpRS;

namespace YwRtdAp.Web.Tse
{
    public class TseJob
    {
        public HttpHeaderList HttpHeader { get; set; }
        public string Url { get; set; }

        public string JobType { get; set; }
        



        public string DateStr { get; set; }

        public DateTime JobDate { get; set; }

        public string MainDirName { get; set; }
        public string SubDirName { get; set; }

        public string SaveDirPath { get; set; }
        public string SaveFileName { get; set; }
    }
}
