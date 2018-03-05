using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using HttpRS;
using System.IO;

namespace YwRtdAp.Web.Tse
{
    public class TseDownloader
    {
        private static TseDownloader _instance { get; set; }

        private ConcurrentQueue<TseJob> _jobQueue { get; set; }

        //private int _baseSecond = 20; //20 sec
        /// <summary>
        /// 發出request給證交所的基本間隔秒數
        /// </summary>
        private int _baseSecond = 2; //2sec

        /// <summary>
        /// 下載資料的TimerThread
        /// </summary>
        private Timer _timerThread { get; set; }

        /// <summary>
        /// 產生任務的TimerThread
        /// </summary>
        private Timer _creatorThread { get; set; }

        private static object _lockObj = new object();

        private int _doJobTimes { get; set; }

        private JobCreatorFactory _creatorFactory { get; set; }

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
            this._creatorFactory = JobCreatorFactory.GetFactory();
            this._jobQueue = new ConcurrentQueue<TseJob>();
            this._doJobTimes = 0;
            //建立一個thread來執行TseJob，每隔2+N秒(N取決於亂數)會執行一次TseJob
            this._timerThread = new Timer(DoTseJob, DateTime.Now, new TimeSpan(0, 0, 1), new TimeSpan(0, 0, _baseSecond));
            //建立一個thread用來建立TseJob，每隔10秒會建立一個TseJob
            this._creatorThread = new Timer(CreateJob, DateTime.Now, 100, 10000);
        }

        /// <summary>
        /// 定時產生一個任務
        /// </summary>
        /// <param name="state"></param>
        public void CreateJob(object state)
        {
            Console.WriteLine("[ CreateJob ] Thread Timer 開始定時產生一個TseJob");

            JobCreator creator = null;
            do
            {
                creator = this._creatorFactory.RandomProduce();
                if (creator != null)
                {
                    TseJob aJob = null;
                    do
                    {
                        aJob = creator.CreateJob();
                    } while (aJob == null);                    

                    AddJob(aJob);
                }            

            } while (creator == null);            
        }

        /// <summary>
        /// 把TseJob放到Queue內
        /// </summary>
        /// <param name="job"></param>
        private void AddJob(TseJob job)
        {
            if (job != null)
            {
                Console.WriteLine("[ AddJob ] 把任務類型[ {0} ]的任務加入到Queue裡面", job.JobType.ToUpper());
                this._jobQueue.Enqueue(job);
                return;
            }          
  
        }

        /// <summary>
        /// Timer Thread在約3到5分鐘內會執行一次這個Method
        /// </summary>
        /// <param name="state"></param>
        private void DoTseJob(object state)
        {               
            TseJob job = null;
            if (this._jobQueue.TryDequeue(out job))
            {

                Console.WriteLine("[ DoTseJob ] 從Queue裡面取得一個任務，並準備開始執行這項任務 T: {0}", DateTime.Now.ToString("HH:mm:ss fff"));
                try
                {
                    this._doJobTimes += 1;
                    HttpSender sender = new HttpSender(job.Url);
                    ResponseResult result = sender.SendRequest(HttpRequestMethod.Get, "", job.HttpHeader);
                    if (result.IsResultError == false)
                    {
                        //Save data to folder
                        FileInfo fi = new FileInfo(job.FilePath);
                        if (fi.Exists == false)
                        {
                            Directory.CreateDirectory(fi.Directory.FullName);                            
                        }
                        using (StreamWriter sw = new StreamWriter(job.FilePath, false, Encoding.UTF8))
                        {
                            sw.Write(result.ResponseBody);
                        }
                        job.IsComplete = true;
                        job.WithErr = false;
                        this._creatorFactory.SetCompleteJob(job);
                    }

                    if (this._doJobTimes > 2)
                    {
                        this._timerThread.Change(new TimeSpan(0, 0, GetNextSecond()), new TimeSpan(0, 0, GetNextSecond()));
                        ResetDoJobTimes();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("+---------------------------------------------------------");
                    Console.WriteLine("|EE        EEEE        EE    EEEEEEEEEEEE    EEEEEEEEEE");
                    Console.WriteLine("| RR      RR  RR      RR          RR         RR");
                    Console.WriteLine("|  RR    RR    RR    RR           RR         RRRRRR");
                    Console.WriteLine("|   OO  OO      OO  OO            OO         OO");
                    Console.WriteLine("|    RRRR        RRRR             RR         RR");
                    Console.WriteLine("+---------------------------------------------------------");
                    job.IsComplete = false;
                    job.WithErr = true;
                    Console.WriteLine(e.Message);
                    Console.Write(e.StackTrace);
                }                
            }
            else
            {
                this._timerThread.Change(1000, _baseSecond * 1000);
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
            second = _baseSecond + rnd.Next(4);
            Console.WriteLine("---------------------------Next Second: {0}", second);
            return second;
        }

    }
}
