using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace YwRtdAp.Web.Tse
{
    public class TseDownloader
    {
        private static TseDownloader _instance { get; set; }

        private ConcurrentQueue<TseJob> _jobQueue { get; set; }

        private Timer _timerThread { get; set; }

        private static object _lockObj = new object();

        private int _doJobTimes { get; set; }

        public static TseDownloader Instance()
        {
            lock(_lockObj)
            {
                if (_instance == null)
                {
                    _instance = new TseDownloader();
                }
            }
            
            return _instance;
        }

        private TseDownloader()
        {
            this._jobQueue = new ConcurrentQueue<TseJob>();
            this._doJobTimes = 0;
            this._timerThread = new Timer(DoTseJob, DateTime.Now, new TimeSpan(0, 0, 1), new TimeSpan(0, 0, 2));
        }

        public void AddJob(TseJob job)
        {
            //這裡可能就可以先檢查資料是否存在了

            this._jobQueue.Enqueue(job);
        }

        private void DoTseJob(object state)
        {
            this._doJobTimes += 1;
            Console.WriteLine("Current Time : {0}", DateTime.Now.ToString("HH:mm:ss fff"));
            if (this._doJobTimes > 3)
            {
                this._timerThread.Change(new TimeSpan(0, 0, GetNextSecond()), new TimeSpan(0, 0, GetNextSecond()));
                ResetDoJobTimes();
            }            
        }

        private void ResetDoJobTimes()
        {
            this._doJobTimes = 0;
        }

        private int GetNextSecond()
        {
            int second = 0;
            Random rnd = new Random((int)DateTime.Now.Ticks);
            second = 0 + rnd.Next(120);
            Console.WriteLine("---------------------------Next Second: {0}", second);
            return second;
        }

    }
}
