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

        public JobCreatorType CreatorType { get; set; }

        public string MainDirName { get; set; }
        public string SubDirName { get; set; }

        public bool IsComplete { get; set; }
        public bool IsSaturdayOrSunday { get; set; }
        public bool WithErr { get; set; }

        public DateTime JobDate { get; set; }

        public string FilePath { get; set; }
    }
}
