using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YwRtdLib;
using System.Reflection;

namespace YwRtdConsole
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //Type t = typeof(YwField);
            //Type ut = t.UnderlyingSystemType;
            //string[] enumValues = System.Enum.GetNames(t);
            //foreach (var p in enumValues)
            //{
            //    Console.WriteLine("{0}", p);
            //}
            //Console.Read();
            
            RtCore rtd = YwRtdLib.RtCore.Instance();
            rtd.CommodityChangeHandler += DataChangeHandler;
            rtd.AddSymbol("2317");
            rtd.AddSymbol("2498");
            rtd.AddSymbol("2330");
            Console.Read();
            rtd.Terminate();
        }
        

        static void DataChangeHandler(YwCommodity commodity)
        {
            Console.WriteLine("{0} $ {1}", commodity.Symbol, commodity.LastChangeField);
        }
    }
}
