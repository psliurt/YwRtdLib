using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YwRtdLib;

namespace YwRtdAp.CombineObject
{
    public class CombineQuote 
    {
        protected YwCommodity _core;
        public CombineQuote(ref YwCommodity instance)
        {
            this._core = instance;
        }
    }
}
