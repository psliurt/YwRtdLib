using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using YwRtdAp.Db.DbObject;
using YwRtdAp.Db.Dal;
using YwRtdAp.CombineObject;
using YwRtdLib;

namespace YwRtdAp
{
    delegate void UpdateHandler(DataGridView gv);
    public class Dispatcher
    {
        private Dictionary<string, Dictionary<string,DataGridView>> _symbolToGrids { get; set; }
        private RtCore _rtdCore { get; set; }

        private static Dispatcher _instance { get; set; }

        public static Dispatcher Instance(RtCore rtdCore)
        {
            if (_instance == null)
            {
                _instance = new Dispatcher(rtdCore);
            }
            return _instance;
        }

        private Dispatcher(RtCore rtdCore)
        {
            this._rtdCore = rtdCore;
            this._symbolToGrids = new Dictionary<string, Dictionary<string, DataGridView>>();
            this._updateEventQueue = new ConcurrentQueue<ChangeData>();
            this._bufferEventQueue = new ConcurrentQueue<ChangeData>();
            this._bufferRearResetEvent = new AutoResetEvent(false);
            this._bufferFrontResetEvent = new AutoResetEvent(false);
            this._updateEventThread = new Thread(DoUpdateWork);
            this._bufferFrontThread = new Thread(DoFrontWork);
            this._bufferRearThread = new Thread(DoRearWork);
            this._updateEventThread.Start();
            this._bufferRearThread.Start();
            this._bufferFrontThread.Start();
        }


        private ConcurrentQueue<ChangeData> _updateEventQueue { get; set; }
        private ConcurrentQueue<ChangeData> _bufferEventQueue { get; set; }
        private volatile bool _runUpdateWork = true;
        private AutoResetEvent _bufferRearResetEvent { get; set; }

        private volatile bool _runRearWork = true;
        private AutoResetEvent _bufferFrontResetEvent { get; set; }

        private volatile bool _runFrontWork = true;

        private Thread _updateEventThread { get; set; }
        private Thread _bufferFrontThread { get; set; }
        private Thread _bufferRearThread { get; set; }

        public void AddSymbolGridMap(string symbol, DataGridView gv)
        {
            Dictionary<string, DataGridView> gridViewMap = null;
            if (this._symbolToGrids.TryGetValue(symbol, out gridViewMap))
            {
                DataGridView existGV = null;
                if (gridViewMap.TryGetValue(gv.Name, out existGV) == false)
                {
                    gridViewMap.Add(gv.Name, gv);
                }
            }
            else
            {
                gridViewMap = new Dictionary<string, DataGridView>();
                gridViewMap.Add(gv.Name, gv);
                this._symbolToGrids.Add(symbol, gridViewMap);
            }
            
        }

        public void Terminate()
        {
            this._bufferRearThread.Abort();
            this._bufferFrontThread.Abort();
            this._updateEventThread.Abort();
        }

        private void UpdateGridView(DataGridView gv)
        {
            if (gv.InvokeRequired)
            {
                UpdateHandler handler = new UpdateHandler(UpdateGridView);
                gv.Invoke(handler, gv);
            }
            else
            {
                //gv.DataSource = this._dayTradeQuoteDatas;                
                gv.Refresh();
            }
        }

        private void DoFrontWork()
        {
            int c = 0;
            while (this._runFrontWork)
            {
                this._bufferFrontResetEvent.WaitOne();
                if (this._rtdCore == null)
                {
                    continue;
                }
                c = this._rtdCore.ChangeDataQueue.Count;
                if (c > 0)
                {
                    Console.WriteLine("A ---> [[[ {0}  ]]] ---> B ", c);
                }


                while (c > 0)
                {
                    ChangeData newIncome = null;
                    if (this._rtdCore.ChangeDataQueue.TryDequeue(out newIncome))
                    {
                        this._bufferEventQueue.Enqueue(newIncome);
                    }
                    c--;
                }
            }
        }


        private void DoRearWork()
        {
            int c = 0;

            while (this._runRearWork)
            {
                this._bufferRearResetEvent.WaitOne();
                c = this._bufferEventQueue.Count;
                if (c > 0)
                {
                    Console.WriteLine("                         #B ---> [[[ {0}  ]]] ---> C ", c);
                }


                while (c > 0)
                {
                    ChangeData buffered = null;
                    if (this._bufferEventQueue.TryDequeue(out buffered))
                    {
                        this._updateEventQueue.Enqueue(buffered);
                    }
                    c--;
                }

                this._bufferFrontResetEvent.Set();

            }
        }

        private void DoUpdateWork()
        {
            int c = 0;
            while (this._runUpdateWork)
            {
                c = this._updateEventQueue.Count;

                if (c <= 0)
                {
                    this._bufferRearResetEvent.WaitOne(1);//利用這個waitone來讓CPU的使用率不要爆高
                }
                else
                {
                    Console.WriteLine("                                                   #C ---> [[[ {0}  ]]] ---> Update ", c);
                }

                while (c > 0)
                {
                    ChangeData newOutcome = null;
                    if (this._updateEventQueue.TryDequeue(out newOutcome))
                    {
                        DispatchNotify(newOutcome);
                        //UpdateGridView(this._symbolGV);
                        //Console.WriteLine("Data Change: [ {0}  => {1} ]", newOutcome.Topic.YwFieldType.ToString(), newOutcome.Data);
                    }
                    c--;
                }
                this._bufferRearResetEvent.Set();

            }
        }

        private void DispatchNotify(ChangeData notify)
        {
            string symbol = notify.Topic.Symbol;
            Dictionary<string, DataGridView> gridMap =null;
            if (this._symbolToGrids.TryGetValue(symbol, out gridMap))
            {
                foreach (DataGridView gv in gridMap.Values)
                {
                    UpdateGridView(gv);
                }
            }
        }
    }
}
