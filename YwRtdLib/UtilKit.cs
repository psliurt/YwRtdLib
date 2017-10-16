using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YwRtdLib
{
    public class UtilKit
    {
        public static decimal? RoundToDecimal(string numberStr)
        {
            decimal d;
            if (decimal.TryParse(numberStr, out d) == false)
            {
                return null;
            }

            return decimal.Round(d, 2);
        }        
    }
}
