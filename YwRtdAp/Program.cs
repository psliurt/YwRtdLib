using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;

namespace YwRtdAp
{
    static class Program
    {
        static ILog _log = LogManager.GetLogger(typeof(Program));

        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            log4net.Config.XmlConfigurator.Configure();

            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            catch (Exception e)
            {
                _log.Error(e);
                _log.Error(e.Message);
                _log.Error(e.StackTrace);
                if (e.InnerException != null)
                {
                    _log.Error(e.InnerException);
                    _log.Error(e.InnerException.Message);
                    _log.Error(e.InnerException.StackTrace);
                }
            }            
        }
    }
}
