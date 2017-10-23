using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartet.Packs.MoneyCome.RTD;
using Quartet.Packs.MoneyCome.COM;
using System.Reflection;
using System.Diagnostics;

namespace YwRtdLib
{
    public delegate void CommodityUpdater(YwCommodity commodity);
    public delegate void BasicQuoteUpdater(YwBasicQuote basicQuote);
    public delegate void Best5Updater(YwBest5 best5);
    public delegate void FOQuoteUpdater(YwFOQuote foQuote);
    public delegate void FoundQuoteUpdater(YwFoundQuote foundQuote);
    public delegate void OpenSimulateQuoteUpdater(YwOpenSimulateQuote openSimulateQuote);
    public delegate void DataChangeNotifier(DataChangeInfo dci);

    public class RtCore
    {
        public event CommodityUpdater CommodityChangeHandler;

        public event BasicQuoteUpdater BasicQuoteHandler;
        public event Best5Updater Best5Handler;
        public event FOQuoteUpdater FOQuoteHandler;
        public event FoundQuoteUpdater FoundQuoteHandler;
        public event OpenSimulateQuoteUpdater OpenSimulateQuoteHandler;
        public event DataChangeNotifier DataChangeHandler;

        private object _lockObj = new object();

        private Excel _ywRtdCom { get; set; }

        private RtdNotifyHandler _rtdEventHandler { get; set; }

        private static RtCore _instance { get; set; }

        private Dictionary<string, YwCommodity> _subscribeCommodities { get; set; }

        private List<string> _subscribeSymbols { get; set; }

        private List<string> _subscribeSymbolsForRemove { get; set; }

        private Dictionary<int, TopicInfo> _topicMap { get; set; }

        private int _currentTopicId { get; set; }

        private Dictionary<string, YwBasicQuote> _subscribeBasicQuote { get; set; }

        private Dictionary<string, YwBest5> _subscribeBest5 { get; set; }

        private Dictionary<string, YwFOQuote> _subscribeFOQuote { get; set; }

        private Dictionary<string, YwFoundQuote> _subscribeFoundQuote { get; set; }

        private Dictionary<string, YwOpenSimulateQuote> _subscribeOpenSimulateQuote { get; set; }

        public  ConcurrentQueue<ChangeData> ChangeDataQueue { get; set; }

        /// <summary>
        /// 建構子
        /// </summary>
        private RtCore()
        {
            this.ChangeDataQueue = new ConcurrentQueue<ChangeData>();
            this._ywRtdCom = new Excel();
            this._rtdEventHandler = new RtdNotifyHandler(this, this.ChangeDataQueue);
            this._ywRtdCom.ServerStart(_rtdEventHandler);

            this._subscribeCommodities = new Dictionary<string, YwCommodity>();
            this._subscribeBasicQuote = new Dictionary<string, YwBasicQuote>();
            this._subscribeBest5 = new Dictionary<string, YwBest5>();
            this._subscribeFOQuote = new Dictionary<string, YwFOQuote>();
            this._subscribeFoundQuote = new Dictionary<string, YwFoundQuote>();
            this._subscribeOpenSimulateQuote = new Dictionary<string, YwOpenSimulateQuote>();
            this._subscribeSymbols = new List<string>();
            this._subscribeSymbolsForRemove = new List<string>();

            this._topicMap = new Dictionary<int, TopicInfo>();
            this._currentTopicId = int.MinValue;            
        }

        public static RtCore Instance()
        {
            if (_instance == null)
            {
                _instance = new RtCore();
            }
            return _instance;
        }        

        /// <summary>
        /// 加入一個新的股票
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public int AddSymbol(string symbol)
        {
            lock (_lockObj)
            {
                if (this._subscribeSymbols.Contains(symbol) == false)
                {
                    YwCommodity commodity = CreateNewSymbol(symbol);
                    this._subscribeCommodities.Add(symbol, commodity);
                    this._subscribeSymbols.Add(symbol);
                    this._subscribeSymbolsForRemove.Add(symbol);
                }
            }
            return this._subscribeSymbols.Count;
        }

        /// <summary>
        /// 訂閱symbol的所有欄位資訊，並傳回symbol的整個物件
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        private YwCommodity CreateNewSymbol(string symbol)
        {
            //把所有的欄位都加入訂閱
            string serverSymbol = SubscribeAllField(symbol);

            YwCommodity stk = new YwCommodity
            {
                Symbol = symbol,
                LastUpdate = DateTime.Now,
                LastChangeField = YwField.None
            };
            return stk;
        }

        /// <summary>
        /// 向元大RTD Server訂閱一個Symbol的所有欄位資料
        /// </summary>
        /// <param name="symbol"></param>
        private string SubscribeAllField(string symbol)
        {
            Type fieldType = typeof(YwField);
            Type enumType = fieldType.UnderlyingSystemType;
            //把要訂閱的欄位一一列舉出來
            string[] fieldNames = System.Enum.GetNames(enumType);
            
            //一個一個向RTD訂閱
            foreach (var fn in fieldNames)
            {
                if (fn != "None")
                {
                    //topic的訂閱方式，第一個資料是股票代碼，第二個資料是要訂閱的欄位名稱
                    Array topic = new string[2] { symbol, fn };
                    bool useNewData = true;
                    //向RTD發出請求
                    this._ywRtdCom.ConnectData(this._currentTopicId, ref topic, ref useNewData);

                    this._topicMap.Add(this._currentTopicId, new TopicInfo
                    {
                        TopicId = this._currentTopicId,
                        FieldName = fn,
                        Symbol = symbol,
                        YwFieldType = (YwField)Enum.Parse(typeof(YwField), fn)
                    });

                    this._currentTopicId += 1;
                }                
            }
            return "";
        }

        /// <summary>
        /// 取消訂閱的商品
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public int RemoveSymbol(string symbol)
        {
            if (this._subscribeSymbols.Contains(symbol))
            {
                this._subscribeSymbols.Remove(symbol);
                this._subscribeCommodities.Remove(symbol);
                this._subscribeSymbolsForRemove.Remove(symbol);
            }
            return this._subscribeSymbols.Count;
        }

        /// <summary>
        /// 釋放RTD元件資源，並停止運作
        /// </summary>
        public void Terminate()
        {
            this._rtdEventHandler.Disconnect();

            foreach (var kv in this._topicMap)
            {
                this._ywRtdCom.DisconnectData(kv.Value.TopicId);
            }
            this._topicMap.Clear();
            this._subscribeSymbols.Clear();
            this._subscribeCommodities.Clear();           

            this._ywRtdCom.ServerTerminate();
        }



        class RtdNotifyHandler : IRTDUpdateEvent
        {
            private RtCore _rtdCore { get; set; }

            private ConcurrentQueue<ChangeData> _rtdCoreQueue { get; set; }

            private int _hbInterval { get; set; }

            private ConcurrentQueue<NotifyData> _inputQueue { get; set;}
            private ConcurrentQueue<NotifyData> _bufferQueue { get; set; }
            private ConcurrentQueue<NotifyData> _outputQueue { get; set; }

            private Thread _frontWorker { get; set; }
            private Thread _rearWorker { get; set; }
            private Thread _consumeWorker { get; set; }

            private volatile bool _runFrontWork = true;
            private volatile bool _runRearWork = true;
            private volatile bool _runConsumeWork = true;

            private AutoResetEvent _frontResetEvent { get; set; }
            private AutoResetEvent _rearResetEvent { get; set; }
            private AutoResetEvent _outputResetEvent { get; set; }


            public RtdNotifyHandler(RtCore rtdSvr, ConcurrentQueue<ChangeData> changeDataQueue)
            {
                this._rtdCore = rtdSvr;
                this._rtdCoreQueue = changeDataQueue;
                this._inputQueue = new ConcurrentQueue<NotifyData>();
                this._bufferQueue = new ConcurrentQueue<NotifyData>();
                this._outputQueue = new ConcurrentQueue<NotifyData>();
                this._frontResetEvent = new AutoResetEvent(false);
                this._rearResetEvent = new AutoResetEvent(false);
                this._outputResetEvent = new AutoResetEvent(false);
                //this._runRearWork = true;
                //this._runConsumeWork = true;
                this._consumeWorker = new Thread(DoConsume);
                this._consumeWorker.Name = "ConsumeWorker-Thread #" + this._consumeWorker.ManagedThreadId;
                this._rearWorker = new Thread(DoRearWork);
                this._rearWorker.Name = "RearWorker-Thread #" + this._rearWorker.ManagedThreadId;
                this._frontWorker = new Thread(DoFrontWork);
                this._frontWorker.Name = "FrontWorker-Thread #" + this._frontWorker.ManagedThreadId;                
                this._consumeWorker.Start();
                this._rearWorker.Start();
                this._frontWorker.Start();
            }

            private void DoFrontWork()
            {
                int c = 0;
                while (this._runFrontWork)
                {
                    this._frontResetEvent.WaitOne();
                    c = this._inputQueue.Count;                    

                    while (c > 0)
                    {                        
                        NotifyData newIncome = null;
                        if (this._inputQueue.TryDequeue(out newIncome))
                        {
                            this._bufferQueue.Enqueue(newIncome);                    
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
                    this._rearResetEvent.WaitOne();
                    c = this._bufferQueue.Count;                    

                    while (c > 0)
                    {
                        NotifyData buffered = null;
                        if (this._bufferQueue.TryDequeue(out buffered))
                        {
                            this._outputQueue.Enqueue(buffered);                 
                        }
                        c--;
                    }
                    
                    this._frontResetEvent.Set();
                    
                }
            }

            private void DoConsume()
            {                
                int c = 0;
                while (this._runConsumeWork)
                {                    
                    c = this._outputQueue.Count;

                    if (c <= 0)
                    {
                        this._outputResetEvent.WaitOne(1);//利用這個waitone來讓CPU的使用率不要爆高
                    }                    
                   
                    while (c > 0)
                    {
                        NotifyData newOutcome = null;
                        if (this._outputQueue.TryDequeue(out newOutcome))
                        {
                            if (newOutcome.TopicCount > 0)
                            {
                                GetAndFireEvent(newOutcome.TopicCount, newOutcome.NewData);
                            }
                        }
                        c--;
                    }
                    this._rearResetEvent.Set();
                    
                }
            }


            public void Disconnect()
            {
                this._runFrontWork = false;
                this._runRearWork = false;
                this._runConsumeWork = false;
                this._outputResetEvent.Set();
                this._consumeWorker.Abort();
                this._frontResetEvent.Set();
                this._rearResetEvent.Set();
                
                Console.WriteLine("NotifyHandler Disconnect");
            }

            public int HeartbeatInterval
            {
                get
                {
                    return this._hbInterval;
                }
                set
                {
                    this._hbInterval = value;
                }
            }

            

            public void UpdateNotify()
            {
                int topicCount = 0;
                Array rawNewData = this._rtdCore._ywRtdCom.RefreshData(ref topicCount);
                this._inputQueue.Enqueue(new NotifyData
                {
                    TopicCount = topicCount,
                    NewData = rawNewData
                });
                

                //int topicCount = 0;
                //Array rawNewData = this._rtdCore._ywRtdCom.RefreshData(ref topicCount);

                //if (topicCount > 0)
                //{
                //    GetAndFireEvent(topicCount, rawNewData);
                //}
            
            }

            /// <summary>
            /// 試作版本
            /// </summary>
            /// <param name="topicCount"></param>
            /// <param name="newData"></param>
            private void GetAndFireEvent(int topicCount, Array newData)
            {
                Stopwatch st = Stopwatch.StartNew();

                List<string> updateSymbols = new List<string>();
                //因為每次取回Topic資料時，不一定只有一個資料，有可能會有多個topic都有變動，所以用while迴圈遞減處理
                while (topicCount > 0)
                {
                    //先從之前訂閱欄位資料時，所儲存的對映表中取出當初訂閱資料時所記錄的Symbol與欄位名稱的資訊。
                    TopicInfo info;
                    if (this._rtdCore._topicMap.TryGetValue(Convert.ToInt32(newData.GetValue(0, topicCount - 1)), out info) == true)
                    {                        
                        string data = newData.GetValue(1, topicCount - 1).ToString();
                        if (data.Trim() != "無資料")
                        {
                            this._rtdCoreQueue.Enqueue(new ChangeData
                            {
                                Data = data,
                                Topic = info
                            });                            
                        }

                        YwCommodity commodity = null;
                        if (this._rtdCore._subscribeCommodities.TryGetValue(info.Symbol, out commodity))
                        {
                            SetObjectPropData<YwCommodity>(ref commodity, ref info, data);

                            if(this._rtdCore._subscribeSymbolsForRemove.Contains(info.Symbol))
                            {
                                if (this._rtdCore.CommodityChangeHandler != null)
                                {
                                    this._rtdCore.CommodityChangeHandler(commodity);
                                    this._rtdCore._subscribeSymbolsForRemove.Remove(info.Symbol);
                                }
                            }
                        }                        

                    }

                    
                    topicCount = topicCount - 1;
                }//end while(topicCount > 0)

                st.Stop();
                //Console.WriteLine("+ Fire use T={0} , M={1}", st.ElapsedTicks, st.ElapsedMilliseconds);
            }

            /// <summary>
            /// 這個Method在發送event出去時，太耗費時間了
            /// 原始版本
            /// </summary>
            /// <param name="topicCount"></param>
            /// <param name="newData"></param>
            //private void GetAndFireEvent(int topicCount, Array newData)
            //{
            //    Stopwatch st = Stopwatch.StartNew();                

            //    List<string> updateSymbols = new List<string>();
            //    //因為每次取回Topic資料時，不一定只有一個資料，有可能會有多個topic都有變動，所以用while迴圈遞減處理
            //    while (topicCount > 0)
            //    {
            //        //先從之前訂閱欄位資料時，所儲存的對映表中取出當初訂閱資料時所記錄的Symbol與欄位名稱的資訊。
            //        TopicInfo info;
            //        //有找到資料才做處理
            //        if (this._rtdCore._topicMap.TryGetValue(Convert.ToInt32(newData.GetValue(0, topicCount - 1)), out info) == true)
            //        {
            //            YwFieldGroup group = GetAttributeEnumOfYwField(info.YwFieldType);

            //            string data = newData.GetValue(1, topicCount - 1).ToString();

            //            YwFOQuote foQuote = null;

            //            if ((group & YwFieldGroup.OptionSpec) == YwFieldGroup.OptionSpec)
            //            {
            //                if (this._rtdCore._subscribeFOQuote.TryGetValue(info.Symbol, out foQuote) == false)
            //                {
            //                    foQuote = new YwFOQuote
            //                    {
            //                        Symbol = info.Symbol
            //                    };
            //                    this._rtdCore._subscribeFOQuote.Add(info.Symbol, foQuote);
            //                }
            //                SetObjectPropData<YwFOQuote>(ref foQuote, ref info, data);
            //            }

            //            YwFoundQuote foundQuote = null;
            //            if ((group & YwFieldGroup.FoundSpec) == YwFieldGroup.FoundSpec)
            //            {
            //                if (this._rtdCore._subscribeFoundQuote.TryGetValue(info.Symbol, out foundQuote) == false)
            //                {
            //                    foundQuote = new YwFoundQuote
            //                    {
            //                        Symbol = info.Symbol
            //                    };
            //                    if (this._rtdCore._subscribeFoundQuote.ContainsKey(info.Symbol) == false)
            //                    {
            //                        this._rtdCore._subscribeFoundQuote.Add(info.Symbol, foundQuote);
            //                    }                                
            //                }
            //                SetObjectPropData<YwFoundQuote>(ref foundQuote, ref info, data);
            //            }

            //            YwOpenSimulateQuote openSimulateQuote = null;
            //            if ((group & YwFieldGroup.OpenSimulate) == YwFieldGroup.OpenSimulate)
            //            {
            //                if (this._rtdCore._subscribeOpenSimulateQuote.TryGetValue(info.Symbol, out openSimulateQuote) == false)
            //                {
            //                    openSimulateQuote = new YwOpenSimulateQuote
            //                    {
            //                        Symbol = info.Symbol
            //                    };
            //                    this._rtdCore._subscribeOpenSimulateQuote.Add(info.Symbol, openSimulateQuote);
            //                }

            //                SetObjectPropData<YwOpenSimulateQuote>(ref openSimulateQuote, ref info, data);
            //            }

            //            YwBasicQuote basicQuote = null;
            //            if ((group & YwFieldGroup.OpenInitOnce) == YwFieldGroup.OpenInitOnce ||
            //                (group & YwFieldGroup.IntraChange) == YwFieldGroup.IntraChange ||
            //                (group & YwFieldGroup.LastNightFix) == YwFieldGroup.LastNightFix)
            //            {
            //                if (this._rtdCore._subscribeBasicQuote.TryGetValue(info.Symbol, out basicQuote) == false)
            //                {
            //                    basicQuote = new YwBasicQuote
            //                    {
            //                        Symbol = info.Symbol
            //                    };
            //                    if (this._rtdCore._subscribeBasicQuote.ContainsKey(info.Symbol) == false)
            //                    {
            //                        this._rtdCore._subscribeBasicQuote.Add(info.Symbol, basicQuote);
            //                    }                                
            //                }

            //                SetObjectPropData<YwBasicQuote>(ref basicQuote, ref info, data);
            //            }

            //            YwBest5 best5 = null;
            //            if ((group & YwFieldGroup.BestFive) == YwFieldGroup.BestFive)
            //            {
            //                if (this._rtdCore._subscribeBest5.TryGetValue(info.Symbol, out best5) == false)
            //                {
            //                    best5 = new YwBest5
            //                    {
            //                        Symbol = info.Symbol
            //                    };
            //                    if (this._rtdCore._subscribeBest5.ContainsKey(info.Symbol) == false)
            //                    {
            //                        this._rtdCore._subscribeBest5.Add(info.Symbol, best5);
            //                    }
                                
            //                }

            //                SetObjectPropData<YwBest5>(ref best5, ref info, data);
            //            }

            //            if ((group & YwFieldGroup.All) == YwFieldGroup.All)
            //            {
            //                if (this._rtdCore._subscribeFOQuote.TryGetValue(info.Symbol, out foQuote) == false)
            //                {
            //                    foQuote = new YwFOQuote
            //                    {
            //                        Symbol = info.Symbol
            //                    };
            //                    this._rtdCore._subscribeFOQuote.Add(info.Symbol, foQuote);
            //                }
            //                SetObjectPropData<YwFOQuote>(ref foQuote, ref info, data);

            //                if (this._rtdCore._subscribeFoundQuote.TryGetValue(info.Symbol, out foundQuote) == false)
            //                {
            //                    foundQuote = new YwFoundQuote
            //                    {
            //                        Symbol = info.Symbol
            //                    };
            //                    this._rtdCore._subscribeFoundQuote.Add(info.Symbol, foundQuote);
            //                }
            //                SetObjectPropData<YwFoundQuote>(ref foundQuote, ref info, data);

            //                if (this._rtdCore._subscribeOpenSimulateQuote.TryGetValue(info.Symbol, out openSimulateQuote) == false)
            //                {
            //                    openSimulateQuote = new YwOpenSimulateQuote
            //                    {
            //                        Symbol = info.Symbol
            //                    };
            //                    this._rtdCore._subscribeOpenSimulateQuote.Add(info.Symbol, openSimulateQuote);
            //                }

            //                SetObjectPropData<YwOpenSimulateQuote>(ref openSimulateQuote, ref info, data);

            //                if (this._rtdCore._subscribeBasicQuote.TryGetValue(info.Symbol, out basicQuote) == false)
            //                {
            //                    basicQuote = new YwBasicQuote
            //                    {
            //                        Symbol = info.Symbol
            //                    };
            //                    this._rtdCore._subscribeBasicQuote.Add(info.Symbol, basicQuote);
            //                }

            //                SetObjectPropData<YwBasicQuote>(ref basicQuote, ref info, data);

            //                if (this._rtdCore._subscribeBest5.TryGetValue(info.Symbol, out best5) == false)
            //                {
            //                    best5 = new YwBest5
            //                    {
            //                        Symbol = info.Symbol
            //                    };
            //                    this._rtdCore._subscribeBest5.Add(info.Symbol, best5);
            //                }

            //                SetObjectPropData<YwBest5>(ref best5, ref info, data);
            //            }

            //            if (basicQuote != null)
            //            {
            //                if (IsPropSetUp<YwBasicQuote>(info.YwFieldType, ref basicQuote, basicQuote.GetType(), info.FieldName) == false &&
            //                            IsPropUpdated<YwBasicQuote>(basicQuote, basicQuote.GetType(), info, data) == true)
            //                {
            //                    if (this._rtdCore.BasicQuoteHandler != null)
            //                    {
            //                        this._rtdCore.BasicQuoteHandler(basicQuote);
            //                    }
            //                }
            //            }

            //            if (best5 != null)
            //            {
            //                if (IsPropSetUp<YwBest5>(info.YwFieldType, ref best5, best5.GetType(), info.FieldName) == false &&
            //                            IsPropUpdated<YwBest5>(best5, best5.GetType(), info, data) == true)
            //                {
            //                    if (this._rtdCore.Best5Handler != null)
            //                    {
            //                        this._rtdCore.Best5Handler(best5);
            //                    }
            //                }
            //            }

            //            if (foQuote != null)
            //            {
            //                if (IsPropSetUp<YwFOQuote>(info.YwFieldType, ref foQuote, foQuote.GetType(), info.FieldName) == false &&
            //                            IsPropUpdated<YwFOQuote>(foQuote, foQuote.GetType(), info, data) == true)
            //                {
            //                    if (this._rtdCore.FOQuoteHandler != null)
            //                    {
            //                        this._rtdCore.FOQuoteHandler(foQuote);
            //                    }
            //                }
            //            }

            //            if (foundQuote != null)
            //            {
            //                if (IsPropSetUp<YwFoundQuote>(info.YwFieldType, ref foundQuote, foundQuote.GetType(), info.FieldName) == false &&
            //                            IsPropUpdated<YwFoundQuote>(foundQuote, foundQuote.GetType(), info, data) == true)
            //                {
            //                    if (this._rtdCore.FoundQuoteHandler != null)
            //                    {
            //                        this._rtdCore.FoundQuoteHandler(foundQuote);
            //                    }
            //                }
            //            }

            //            if (openSimulateQuote != null)
            //            {
            //                if (IsPropSetUp<YwOpenSimulateQuote>(info.YwFieldType, ref openSimulateQuote, openSimulateQuote.GetType(), info.FieldName) == false &&
            //                            IsPropUpdated<YwOpenSimulateQuote>(openSimulateQuote, openSimulateQuote.GetType(), info, data) == true)
            //                {
            //                    if (this._rtdCore.OpenSimulateQuoteHandler != null)
            //                    {
            //                        this._rtdCore.OpenSimulateQuoteHandler(openSimulateQuote);
            //                    }
            //                }
            //            }

            //            updateSymbols.Add(info.Symbol);
                        
            //        }
            //        else
            //        {
            //            Console.WriteLine("TopicId:[ {0} ] Can't Find in Dictionary, Data->[ {1} ]",
            //                newData.GetValue(0, topicCount - 1),
            //                newData.GetValue(1, topicCount - 1));
            //        }
            //        topicCount = topicCount - 1;
            //    }//end while(topicCount > 0)

            //    if (this._rtdCore.DataChangeHandler != null)
            //    {
            //        this._rtdCore.DataChangeHandler(new DataChangeInfo
            //        {
            //            FireTime = DateTime.Now,
            //            Symbols = updateSymbols.Distinct().ToList()
            //        });
            //    }


            //    st.Stop();
            //    //Console.WriteLine("+ Fire use T={0} , M={1}", st.ElapsedTicks, st.ElapsedMilliseconds);
            //}

            private YwFieldGroup GetAttributeEnumOfYwField(YwField field)
            {
                var enumMember = field.GetType().GetMember(field.ToString()).FirstOrDefault();
                var categoryAttr =
                    enumMember == null
                        ? default(FieldCategoryAttribute)
                        : enumMember.GetCustomAttribute(typeof(FieldCategoryAttribute)) as FieldCategoryAttribute;
                return categoryAttr.Group;
            }

            

            private void SetObjectPropData<T>(ref T o, ref TopicInfo info, string data) where T : class
            {
                Type t = o.GetType();
                PropertyInfo p = t.GetProperty(info.FieldName);
                if (p == null) { return; }

                if (info.YwFieldType == YwField.ChangeRange ||
                    info.YwFieldType == YwField.ChangePercent ||
                    info.YwFieldType == YwField.VolumeRatio ||
                    info.YwFieldType == YwField.StrengthGroup ||
                    info.YwFieldType == YwField.StrengthMarket||
                    info.YwFieldType == YwField.BestAskPrice1 ||
                    info.YwFieldType == YwField.BestAskPrice2 ||
                    info.YwFieldType == YwField.BestAskPrice3 ||
                    info.YwFieldType == YwField.BestAskPrice4 ||
                    info.YwFieldType == YwField.BestAskPrice5 ||
                    info.YwFieldType == YwField.BestAskVolume1 ||
                    info.YwFieldType == YwField.BestAskVolume2 ||
                    info.YwFieldType == YwField.BestAskVolume3 ||
                    info.YwFieldType == YwField.BestAskVolume4 ||
                    info.YwFieldType == YwField.BestAskVolume5 ||
                    info.YwFieldType == YwField.BestAskVolumes ||
                    info.YwFieldType == YwField.BestBidPrice1 ||
                    info.YwFieldType == YwField.BestBidPrice2 ||
                    info.YwFieldType == YwField.BestBidPrice3 ||
                    info.YwFieldType == YwField.BestBidPrice4 ||
                    info.YwFieldType == YwField.BestBidPrice5 ||
                    info.YwFieldType == YwField.BestBidVolume1 ||
                    info.YwFieldType == YwField.BestBidVolume2 ||
                    info.YwFieldType == YwField.BestBidVolume3 ||
                    info.YwFieldType == YwField.BestBidVolume4 ||
                    info.YwFieldType == YwField.BestBidVolume5 ||
                    info.YwFieldType == YwField.BestBidVolumes ||
                    info.YwFieldType == YwField.TimeValue ||
                    info.YwFieldType == YwField.Implicit ||
                    info.YwFieldType == YwField.StrikePrice||
                    info.YwFieldType == YwField.NAVReference ||
                    info.YwFieldType == YwField.NAVPrice ||
                    info.YwFieldType == YwField.NAVChange ||
                    info.YwFieldType == YwField.PremiumDiscount||
                    info.YwFieldType == YwField.PreOpenAskPrice ||
                    info.YwFieldType == YwField.PreOpenAskVolume ||
                    info.YwFieldType == YwField.PreOpenBestAskPrice1 ||
                    info.YwFieldType == YwField.PreOpenBestAskPrice2 ||
                    info.YwFieldType == YwField.PreOpenBestAskPrice3 ||
                    info.YwFieldType == YwField.PreOpenBestAskPrice4 ||
                    info.YwFieldType == YwField.PreOpenBestAskPrice5 ||
                    info.YwFieldType == YwField.PreOpenBestAskVolume1 ||
                    info.YwFieldType == YwField.PreOpenBestAskVolume2 ||
                    info.YwFieldType == YwField.PreOpenBestAskVolume3 ||
                    info.YwFieldType == YwField.PreOpenBestAskVolume4 ||
                    info.YwFieldType == YwField.PreOpenBestAskVolume5 ||
                    info.YwFieldType == YwField.PreOpenBestAskVolumes ||
                    info.YwFieldType == YwField.PreOpenBestBidPrice1 ||
                    info.YwFieldType == YwField.PreOpenBestBidPrice2 ||
                    info.YwFieldType == YwField.PreOpenBestBidPrice3 ||
                    info.YwFieldType == YwField.PreOpenBestBidPrice4 ||
                    info.YwFieldType == YwField.PreOpenBestBidPrice5 ||
                    info.YwFieldType == YwField.PreOpenBestBidVolume1 ||
                    info.YwFieldType == YwField.PreOpenBestBidVolume2 ||
                    info.YwFieldType == YwField.PreOpenBestBidVolume3 ||
                    info.YwFieldType == YwField.PreOpenBestBidVolume4 ||
                    info.YwFieldType == YwField.PreOpenBestBidVolume5 ||
                    info.YwFieldType == YwField.PreOpenBestBidVolumes ||
                    info.YwFieldType == YwField.PreOpenBidPrice ||
                    info.YwFieldType == YwField.PreOpenBidVolume ||
                    info.YwFieldType == YwField.PreOpenPrice ||
                    info.YwFieldType == YwField.PreOpenVolume)
                {
                    p.SetValue(o, UtilKit.RoundToDecimal(data));
                    return;
                }

                p.SetValue(o, data);
            }

            private bool IsPropSetUp<T>(YwField changedField,ref T obj, Type objType, string fieldName) where T:class
            {                
                FieldInfo fi = objType.GetField(fieldName + "Set");

                if (fi == null) { return false; }

                bool fieldSet = Convert.ToBoolean(fi.GetValue(obj));
                if (changedField == YwField.Name && fieldSet)
                {
                    return fieldSet;
                }
                if (changedField == YwField.Information && fieldSet)
                {
                    return fieldSet;
                }
                if (changedField == YwField.Symbol && fieldSet)
                {
                    return fieldSet;
                }
                if (changedField == YwField.Issuer && fieldSet)
                {
                    return fieldSet;
                }
                if (changedField == YwField.GroupName && fieldSet)
                {
                    return fieldSet;
                }
                if (changedField == YwField.Capital && fieldSet)
                {
                    return fieldSet;
                }
                if (changedField == YwField.Underlying && fieldSet)
                {
                    return fieldSet;
                }
                return false;
            }

            private bool IsPropUpdated<T>(T obj, Type objType, TopicInfo info, string propData) where T: class
            {                
                FieldInfo fi = objType.GetField("Is" + info.FieldName + "Update");
                if (fi == null)
                { return false; }

                bool update = Convert.ToBoolean(fi.GetValue(obj));

                PropertyInfo p = objType.GetProperty(info.FieldName);
                if (info.YwFieldType == YwField.ChangeRange ||
                    info.YwFieldType == YwField.ChangePercent ||
                    info.YwFieldType == YwField.VolumeRatio ||
                    info.YwFieldType == YwField.StrengthGroup ||
                    info.YwFieldType == YwField.StrengthMarket ||
                    info.YwFieldType == YwField.BestAskPrice1 ||
                    info.YwFieldType == YwField.BestAskPrice2 ||
                    info.YwFieldType == YwField.BestAskPrice3 ||
                    info.YwFieldType == YwField.BestAskPrice4 ||
                    info.YwFieldType == YwField.BestAskPrice5 ||
                    info.YwFieldType == YwField.BestAskVolume1 ||
                    info.YwFieldType == YwField.BestAskVolume2 ||
                    info.YwFieldType == YwField.BestAskVolume3 ||
                    info.YwFieldType == YwField.BestAskVolume4 ||
                    info.YwFieldType == YwField.BestAskVolume5 ||
                    info.YwFieldType == YwField.BestAskVolumes ||
                    info.YwFieldType == YwField.BestBidPrice1 ||
                    info.YwFieldType == YwField.BestBidPrice2 ||
                    info.YwFieldType == YwField.BestBidPrice3 ||
                    info.YwFieldType == YwField.BestBidPrice4 ||
                    info.YwFieldType == YwField.BestBidPrice5 ||
                    info.YwFieldType == YwField.BestBidVolume1 ||
                    info.YwFieldType == YwField.BestBidVolume2 ||
                    info.YwFieldType == YwField.BestBidVolume3 ||
                    info.YwFieldType == YwField.BestBidVolume4 ||
                    info.YwFieldType == YwField.BestBidVolume5 ||
                    info.YwFieldType == YwField.BestBidVolumes ||
                    info.YwFieldType == YwField.TimeValue ||
                    info.YwFieldType == YwField.Implicit ||
                    info.YwFieldType == YwField.StrikePrice ||
                    info.YwFieldType == YwField.NAVReference ||
                    info.YwFieldType == YwField.NAVPrice ||
                    info.YwFieldType == YwField.NAVChange ||
                    info.YwFieldType == YwField.PremiumDiscount)
                {
                    p.SetValue(obj, UtilKit.RoundToDecimal(propData));

                }
                else
                {
                    p.SetValue(obj, propData); //再設定一次值，是為了讓IsXXXXUpdate變為false
                }


                return update;
            }            
        }                

        class NotifyData
        {
            public int TopicCount { get; set; }
            public Array NewData { get; set; }
        }
    }

    public struct DataChangeInfo
    {            
        public List<string> Symbols { get; set; }
        public DateTime FireTime { get; set; }
    }

    public struct TopicInfo
    {
        public int TopicId { get; set; }
        public string Symbol { get; set; }
        public string FieldName { get; set; }
        public YwField YwFieldType { get; set; }
    }

    public class ChangeData
    {
        public TopicInfo Topic { get; set; }
        public string Data { get; set; }
    }

    /// <summary>
    /// 元大RTD所提供的所有欄位資訊
    /// </summary>
    public enum YwField
    {
        [FieldCategory(YwFieldGroup.NotSpecific)]
        None,
        [FieldCategory(YwFieldGroup.Once)]
        Name,
        [FieldCategory(YwFieldGroup.Once)]
        Symbol,
        [FieldCategory(YwFieldGroup.Frequently)]
        Price,
        [FieldCategory(YwFieldGroup.Frequently)]
        Change,
        [FieldCategory(YwFieldGroup.Frequently)]
        ChangeRange,
        [FieldCategory(YwFieldGroup.Frequently)]
        ChangePercent,
        [FieldCategory(YwFieldGroup.Frequently)]
        Reference,
        [FieldCategory(YwFieldGroup.Frequently)]
        Open,
        [FieldCategory(YwFieldGroup.Frequently)]
        High,
        [FieldCategory(YwFieldGroup.Frequently)]
        Low,
        [FieldCategory(YwFieldGroup.Once)]
        Ceil,
        [FieldCategory(YwFieldGroup.Once)]
        Floor,
        [FieldCategory(YwFieldGroup.Once)]
        GroupName,
        [FieldCategory(YwFieldGroup.Once)]
        Information,
        [FieldCategory(YwFieldGroup.Frequently)]
        Volume,
        [FieldCategory(YwFieldGroup.Once)]
        CumulativeVolume,
        [FieldCategory(YwFieldGroup.Frequently)]
        PredictVolume,
        [FieldCategory(YwFieldGroup.Frequently)]
        VolumeStrength,
        [FieldCategory(YwFieldGroup.Frequently)]
        BidPrice,
        [FieldCategory(YwFieldGroup.Frequently)]
        AskPrice,
        [FieldCategory(YwFieldGroup.Frequently)]
        BidVolume,
        [FieldCategory(YwFieldGroup.Frequently)]
        AskVolume,
        [FieldCategory(YwFieldGroup.Once)]
        PriorHigh,
        [FieldCategory(YwFieldGroup.Once)]
        PriorLow,
        [FieldCategory(YwFieldGroup.Once)]
        PriorPrice,
        [FieldCategory(YwFieldGroup.Once)]
        PriorVolume,
        [FieldCategory(YwFieldGroup.Frequently)]
        Time,
        [FieldCategory(YwFieldGroup.Frequently)]
        Amount,
        [FieldCategory(YwFieldGroup.Once)]
        MarketValue,
        [FieldCategory(YwFieldGroup.Once)]
        Issue,
        [FieldCategory(YwFieldGroup.Frequently)]
        StrengthMarket,
        [FieldCategory(YwFieldGroup.Frequently)]
        StrengthGroup,
        [FieldCategory(YwFieldGroup.Frequently)]
        VolumeRatio,
        [FieldCategory(YwFieldGroup.Frequently)]
        BestBidPrice1,
        [FieldCategory(YwFieldGroup.Frequently)]
        BestAskPrice1,
        [FieldCategory(YwFieldGroup.Frequently)]
        BestBidVolume1,
        [FieldCategory(YwFieldGroup.Frequently)]
        BestAskVolume1,
        [FieldCategory(YwFieldGroup.Frequently)]
        BestBidPrice2,
        [FieldCategory(YwFieldGroup.Frequently)]
        BestAskPrice2,
        [FieldCategory(YwFieldGroup.Frequently)]
        BestBidVolume2,
        [FieldCategory(YwFieldGroup.Frequently)]
        BestAskVolume2,
        [FieldCategory(YwFieldGroup.Frequently)]
        BestBidPrice3,
        [FieldCategory(YwFieldGroup.Frequently)]
        BestAskPrice3,
        [FieldCategory(YwFieldGroup.Frequently)]
        BestBidVolume3,
        [FieldCategory(YwFieldGroup.Frequently)]
        BestAskVolume3,
        [FieldCategory(YwFieldGroup.Frequently)]
        BestBidPrice4,
        [FieldCategory(YwFieldGroup.Frequently)]
        BestAskPrice4,
        [FieldCategory(YwFieldGroup.Frequently)]
        BestBidVolume4,
        [FieldCategory(YwFieldGroup.Frequently)]
        BestAskVolume4,
        [FieldCategory(YwFieldGroup.Frequently)]
        BestBidPrice5,
        [FieldCategory(YwFieldGroup.Frequently)]
        BestAskPrice5,
        [FieldCategory(YwFieldGroup.Frequently)]
        BestBidVolume5,
        [FieldCategory(YwFieldGroup.Frequently)]
        BestAskVolume5,
        [FieldCategory(YwFieldGroup.Frequently)]
        BestBidVolumes,
        [FieldCategory(YwFieldGroup.Frequently)]
        BestAskVolumes,
        [FieldCategory(YwFieldGroup.Frequently)]
        AveragePrice,
        [FieldCategory(YwFieldGroup.Once)]
        Capital,
        [FieldCategory(YwFieldGroup.Once)]
        Basis,
        [FieldCategory(YwFieldGroup.Once )]
        Bear,
        [FieldCategory(YwFieldGroup.Once)]
        Underlying,
        [FieldCategory(YwFieldGroup.Once )]
        SpotPrice,
        [FieldCategory(YwFieldGroup.Once )]
        Delta,
        [FieldCategory(YwFieldGroup.Once )]
        Gamma,
        [FieldCategory(YwFieldGroup.Once )]
        Theta,
        [FieldCategory(YwFieldGroup.Once )]
        Vega,
        [FieldCategory(YwFieldGroup.Once )]
        Rho,
        [FieldCategory(YwFieldGroup.Once )]
        TimeValue,
        [FieldCategory(YwFieldGroup.Once )]
        Implicit,
        [FieldCategory(YwFieldGroup.Once )]
        Implied,
        [FieldCategory(YwFieldGroup.Once )]
        Moneyness,
        [FieldCategory(YwFieldGroup.Once )]
        ParityPrice,
        [FieldCategory(YwFieldGroup.Once )]
        SpotSigma,
        [FieldCategory(YwFieldGroup.Once )]
        TheoryPrice,
        [FieldCategory(YwFieldGroup.Once )]
        StrikePrice,
        [FieldCategory(YwFieldGroup.Once )]
        Expire,
        [FieldCategory(YwFieldGroup.Once )]
        Due,
        [FieldCategory(YwFieldGroup.Once )]
        BarrierPrice,
        [FieldCategory(YwFieldGroup.Once )]
        Issuer,
        [FieldCategory(YwFieldGroup.Once )]
        Method,
        [FieldCategory(YwFieldGroup.Once )]
        Ratio,
        [FieldCategory(YwFieldGroup.Frequently)]
        CumulativeBidVolume,
        [FieldCategory(YwFieldGroup.Frequently)]
        CumulativeBidOrder,
        [FieldCategory(YwFieldGroup.Frequently)]
        CumulativeAskVolume,
        [FieldCategory(YwFieldGroup.Frequently)]
        CumulativeAskOrder,
        [FieldCategory(YwFieldGroup.Once )]
        NAVReference,
        [FieldCategory(YwFieldGroup.Once )]
        NAVPrice,
        [FieldCategory(YwFieldGroup.Once )]
        NAVChange,
        [FieldCategory(YwFieldGroup.Once )]
        NAVChangePercent,
        [FieldCategory(YwFieldGroup.Once )]
        PremiumDiscount,
        [FieldCategory(YwFieldGroup.Once )]
        PremiumDiscountPercent,
        [FieldCategory(YwFieldGroup.Frequently)]
        PreOpenAskPrice,
        [FieldCategory(YwFieldGroup.Frequently)]
        PreOpenAskVolume,
        [FieldCategory(YwFieldGroup.Frequently)]
        PreOpenBidPrice,
        [FieldCategory(YwFieldGroup.Frequently)]
        PreOpenBidVolume,
        [FieldCategory(YwFieldGroup.Frequently)]
        PreOpenPrice,
        [FieldCategory(YwFieldGroup.Frequently)]
        PreOpenVolume,
        [FieldCategory(YwFieldGroup.Frequently)]
        PreOpenBestBidPrice1,
        [FieldCategory(YwFieldGroup.Frequently)]
        PreOpenBestAskPrice1,
        [FieldCategory(YwFieldGroup.Frequently)]
        PreOpenBestBidVolume1,
        [FieldCategory(YwFieldGroup.Frequently)]
        PreOpenBestAskVolume1,
        [FieldCategory(YwFieldGroup.Frequently)]
        PreOpenBestBidPrice2,
        [FieldCategory(YwFieldGroup.Frequently)]
        PreOpenBestAskPrice2,
        [FieldCategory(YwFieldGroup.Frequently)]
        PreOpenBestBidVolume2,
        [FieldCategory(YwFieldGroup.Frequently)]
        PreOpenBestAskVolume2,
        [FieldCategory(YwFieldGroup.Frequently)]
        PreOpenBestBidPrice3,
        [FieldCategory(YwFieldGroup.Frequently)]
        PreOpenBestAskPrice3,
        [FieldCategory(YwFieldGroup.Frequently)]
        PreOpenBestBidVolume3,
        [FieldCategory(YwFieldGroup.Frequently)]
        PreOpenBestAskVolume3,
        [FieldCategory(YwFieldGroup.Frequently)]
        PreOpenBestBidPrice4,
        [FieldCategory(YwFieldGroup.Frequently)]
        PreOpenBestAskPrice4,
        [FieldCategory(YwFieldGroup.Frequently)]
        PreOpenBestBidVolume4,
        [FieldCategory(YwFieldGroup.Frequently)]
        PreOpenBestAskVolume4,
        [FieldCategory(YwFieldGroup.Frequently)]
        PreOpenBestBidPrice5,
        [FieldCategory(YwFieldGroup.Frequently)]
        PreOpenBestAskPrice5,
        [FieldCategory(YwFieldGroup.Frequently)]
        PreOpenBestBidVolume5,
        [FieldCategory(YwFieldGroup.Frequently)]
        PreOpenBestAskVolume5,
        [FieldCategory(YwFieldGroup.Frequently)]
        PreOpenBestBidVolumes,
        [FieldCategory(YwFieldGroup.Frequently)]
        PreOpenBestAskVolumes
    }

    ///// <summary>
    ///// 元大RTD所提供的所有欄位資訊
    ///// </summary>
    //public enum YwField
    //{
    //    [FieldCategory(YwFieldGroup.NotSpecific)]
    //    None,
    //    [FieldCategory(YwFieldGroup.All)]
    //    Name,
    //    [FieldCategory(YwFieldGroup.All)]
    //    Symbol,
    //    [FieldCategory(YwFieldGroup.IntraChange)]
    //    Price,
    //    [FieldCategory(YwFieldGroup.IntraChange)]
    //    Change,
    //    [FieldCategory(YwFieldGroup.IntraChange)]
    //    ChangeRange,
    //    [FieldCategory(YwFieldGroup.IntraChange)]
    //    ChangePercent,
    //    [FieldCategory(YwFieldGroup.IntraChange)]
    //    Reference,
    //    [FieldCategory(YwFieldGroup.OpenInitOnce)]
    //    Open,
    //    [FieldCategory(YwFieldGroup.IntraChange)]
    //    High,
    //    [FieldCategory(YwFieldGroup.IntraChange)]
    //    Low,
    //    [FieldCategory(YwFieldGroup.OpenInitOnce)]
    //    Ceil,
    //    [FieldCategory(YwFieldGroup.OpenInitOnce)]
    //    Floor,
    //    [FieldCategory(YwFieldGroup.LastNightFix)]
    //    GroupName,
    //    [FieldCategory(YwFieldGroup.LastNightFix)]
    //    Information,
    //    [FieldCategory(YwFieldGroup.IntraChange)]
    //    Volume,
    //    [FieldCategory(YwFieldGroup.LastNightFix)]
    //    CumulativeVolume,
    //    [FieldCategory(YwFieldGroup.IntraChange)]
    //    PredictVolume,
    //    [FieldCategory(YwFieldGroup.IntraChange)]
    //    VolumeStrength,
    //    [FieldCategory(YwFieldGroup.IntraChange)]
    //    BidPrice,
    //    [FieldCategory(YwFieldGroup.IntraChange)]
    //    AskPrice,
    //    [FieldCategory(YwFieldGroup.IntraChange)]
    //    BidVolume,
    //    [FieldCategory(YwFieldGroup.IntraChange)]
    //    AskVolume,
    //    [FieldCategory(YwFieldGroup.LastNightFix)]
    //    PriorHigh,
    //    [FieldCategory(YwFieldGroup.LastNightFix)]
    //    PriorLow,
    //    [FieldCategory(YwFieldGroup.LastNightFix)]
    //    PriorPrice,
    //    [FieldCategory(YwFieldGroup.LastNightFix)]
    //    PriorVolume,
    //    [FieldCategory(YwFieldGroup.All)]
    //    Time,
    //    [FieldCategory(YwFieldGroup.IntraChange)]
    //    Amount,
    //    [FieldCategory(YwFieldGroup.LastNightFix)]
    //    MarketValue,
    //    [FieldCategory(YwFieldGroup.LastNightFix)]
    //    Issue,
    //    [FieldCategory(YwFieldGroup.IntraChange)]
    //    StrengthMarket,
    //    [FieldCategory(YwFieldGroup.IntraChange)]
    //    StrengthGroup,
    //    [FieldCategory(YwFieldGroup.IntraChange)]
    //    VolumeRatio,
    //    [FieldCategory(YwFieldGroup.BestFive)]
    //    BestBidPrice1,
    //    [FieldCategory(YwFieldGroup.BestFive)]
    //    BestAskPrice1,
    //    [FieldCategory(YwFieldGroup.BestFive)]
    //    BestBidVolume1,
    //    [FieldCategory(YwFieldGroup.BestFive)]
    //    BestAskVolume1,
    //    [FieldCategory(YwFieldGroup.BestFive)]
    //    BestBidPrice2,
    //    [FieldCategory(YwFieldGroup.BestFive)]
    //    BestAskPrice2,
    //    [FieldCategory(YwFieldGroup.BestFive)]
    //    BestBidVolume2,
    //    [FieldCategory(YwFieldGroup.BestFive)]
    //    BestAskVolume2,
    //    [FieldCategory(YwFieldGroup.BestFive)]
    //    BestBidPrice3,
    //    [FieldCategory(YwFieldGroup.BestFive)]
    //    BestAskPrice3,
    //    [FieldCategory(YwFieldGroup.BestFive)]
    //    BestBidVolume3,
    //    [FieldCategory(YwFieldGroup.BestFive)]
    //    BestAskVolume3,
    //    [FieldCategory(YwFieldGroup.BestFive)]
    //    BestBidPrice4,
    //    [FieldCategory(YwFieldGroup.BestFive)]
    //    BestAskPrice4,
    //    [FieldCategory(YwFieldGroup.BestFive)]
    //    BestBidVolume4,
    //    [FieldCategory(YwFieldGroup.BestFive)]
    //    BestAskVolume4,
    //    [FieldCategory(YwFieldGroup.BestFive)]
    //    BestBidPrice5,
    //    [FieldCategory(YwFieldGroup.BestFive)]
    //    BestAskPrice5,
    //    [FieldCategory(YwFieldGroup.BestFive)]
    //    BestBidVolume5,
    //    [FieldCategory(YwFieldGroup.BestFive)]
    //    BestAskVolume5,
    //    [FieldCategory(YwFieldGroup.BestFive)]
    //    BestBidVolumes,
    //    [FieldCategory(YwFieldGroup.BestFive)]
    //    BestAskVolumes,
    //    [FieldCategory(YwFieldGroup.IntraChange)]
    //    AveragePrice,
    //    [FieldCategory(YwFieldGroup.LastNightFix)]
    //    Capital,
    //    [FieldCategory(YwFieldGroup.OpenInitOnce)]
    //    Basis,
    //    [FieldCategory(YwFieldGroup.OpenInitOnce | YwFieldGroup.OptionSpec)]
    //    Bear,
    //    [FieldCategory(YwFieldGroup.LastNightFix)]
    //    Underlying,
    //    [FieldCategory(YwFieldGroup.OpenInitOnce | YwFieldGroup.OptionSpec)]
    //    SpotPrice,
    //    [FieldCategory(YwFieldGroup.OpenInitOnce | YwFieldGroup.OptionSpec)]
    //    Delta,
    //    [FieldCategory(YwFieldGroup.OpenInitOnce | YwFieldGroup.OptionSpec)]
    //    Gamma,
    //    [FieldCategory(YwFieldGroup.OpenInitOnce | YwFieldGroup.OptionSpec)]
    //    Theta,
    //    [FieldCategory(YwFieldGroup.OpenInitOnce | YwFieldGroup.OptionSpec)]
    //    Vega,
    //    [FieldCategory(YwFieldGroup.OpenInitOnce | YwFieldGroup.OptionSpec)]
    //    Rho,
    //    [FieldCategory(YwFieldGroup.OpenInitOnce | YwFieldGroup.OptionSpec)]
    //    TimeValue,
    //    [FieldCategory(YwFieldGroup.OpenInitOnce | YwFieldGroup.OptionSpec)]
    //    Implicit,
    //    [FieldCategory(YwFieldGroup.OpenInitOnce | YwFieldGroup.OptionSpec)]
    //    Implied,
    //    [FieldCategory(YwFieldGroup.OpenInitOnce | YwFieldGroup.OptionSpec)]
    //    Moneyness,
    //    [FieldCategory(YwFieldGroup.OpenInitOnce | YwFieldGroup.OptionSpec)]
    //    ParityPrice,
    //    [FieldCategory(YwFieldGroup.OpenInitOnce | YwFieldGroup.OptionSpec)]
    //    SpotSigma,
    //    [FieldCategory(YwFieldGroup.OpenInitOnce | YwFieldGroup.OptionSpec)]
    //    TheoryPrice,
    //    [FieldCategory(YwFieldGroup.OpenInitOnce | YwFieldGroup.OptionSpec)]
    //    StrikePrice,
    //    [FieldCategory(YwFieldGroup.OpenInitOnce | YwFieldGroup.OptionSpec)]
    //    Expire,
    //    [FieldCategory(YwFieldGroup.OpenInitOnce | YwFieldGroup.OptionSpec)]
    //    Due,
    //    [FieldCategory(YwFieldGroup.OpenInitOnce | YwFieldGroup.OptionSpec)]
    //    BarrierPrice,
    //    [FieldCategory(YwFieldGroup.OpenInitOnce | YwFieldGroup.OptionSpec)]
    //    Issuer,
    //    [FieldCategory(YwFieldGroup.OpenInitOnce | YwFieldGroup.OptionSpec)]
    //    Method,
    //    [FieldCategory(YwFieldGroup.OpenInitOnce | YwFieldGroup.OptionSpec)]
    //    Ratio,
    //    [FieldCategory(YwFieldGroup.IntraChange)]
    //    CumulativeBidVolume,
    //    [FieldCategory(YwFieldGroup.IntraChange)]
    //    CumulativeBidOrder,
    //    [FieldCategory(YwFieldGroup.IntraChange)]
    //    CumulativeAskVolume,
    //    [FieldCategory(YwFieldGroup.IntraChange)]
    //    CumulativeAskOrder,
    //    [FieldCategory(YwFieldGroup.LastNightFix | YwFieldGroup.FoundSpec)]
    //    NAVReference,
    //    [FieldCategory(YwFieldGroup.LastNightFix | YwFieldGroup.FoundSpec)]
    //    NAVPrice,
    //    [FieldCategory(YwFieldGroup.LastNightFix | YwFieldGroup.FoundSpec)]
    //    NAVChange,
    //    [FieldCategory(YwFieldGroup.LastNightFix | YwFieldGroup.FoundSpec)]
    //    NAVChangePercent,
    //    [FieldCategory(YwFieldGroup.LastNightFix | YwFieldGroup.FoundSpec)]
    //    PremiumDiscount,
    //    [FieldCategory(YwFieldGroup.LastNightFix | YwFieldGroup.FoundSpec)]
    //    PremiumDiscountPercent,
    //    [FieldCategory(YwFieldGroup.OpenSimulate)]
    //    PreOpenAskPrice,
    //    [FieldCategory(YwFieldGroup.OpenSimulate)]
    //    PreOpenAskVolume,
    //    [FieldCategory(YwFieldGroup.OpenSimulate)]
    //    PreOpenBidPrice,
    //    [FieldCategory(YwFieldGroup.OpenSimulate)]
    //    PreOpenBidVolume,
    //    [FieldCategory(YwFieldGroup.OpenSimulate)]
    //    PreOpenPrice,
    //    [FieldCategory(YwFieldGroup.OpenSimulate)]
    //    PreOpenVolume,
    //    [FieldCategory(YwFieldGroup.OpenSimulate)]
    //    PreOpenBestBidPrice1,
    //    [FieldCategory(YwFieldGroup.OpenSimulate)]
    //    PreOpenBestAskPrice1,
    //    [FieldCategory(YwFieldGroup.OpenSimulate)]
    //    PreOpenBestBidVolume1,
    //    [FieldCategory(YwFieldGroup.OpenSimulate)]
    //    PreOpenBestAskVolume1,
    //    [FieldCategory(YwFieldGroup.OpenSimulate)]
    //    PreOpenBestBidPrice2,
    //    [FieldCategory(YwFieldGroup.OpenSimulate)]
    //    PreOpenBestAskPrice2,
    //    [FieldCategory(YwFieldGroup.OpenSimulate)]
    //    PreOpenBestBidVolume2,
    //    [FieldCategory(YwFieldGroup.OpenSimulate)]
    //    PreOpenBestAskVolume2,
    //    [FieldCategory(YwFieldGroup.OpenSimulate)]
    //    PreOpenBestBidPrice3,
    //    [FieldCategory(YwFieldGroup.OpenSimulate)]
    //    PreOpenBestAskPrice3,
    //    [FieldCategory(YwFieldGroup.OpenSimulate)]
    //    PreOpenBestBidVolume3,
    //    [FieldCategory(YwFieldGroup.OpenSimulate)]
    //    PreOpenBestAskVolume3,
    //    [FieldCategory(YwFieldGroup.OpenSimulate)]
    //    PreOpenBestBidPrice4,
    //    [FieldCategory(YwFieldGroup.OpenSimulate)]
    //    PreOpenBestAskPrice4,
    //    [FieldCategory(YwFieldGroup.OpenSimulate)]
    //    PreOpenBestBidVolume4,
    //    [FieldCategory(YwFieldGroup.OpenSimulate)]
    //    PreOpenBestAskVolume4,
    //    [FieldCategory(YwFieldGroup.OpenSimulate)]
    //    PreOpenBestBidPrice5,
    //    [FieldCategory(YwFieldGroup.OpenSimulate)]
    //    PreOpenBestAskPrice5,
    //    [FieldCategory(YwFieldGroup.OpenSimulate)]
    //    PreOpenBestBidVolume5,
    //    [FieldCategory(YwFieldGroup.OpenSimulate)]
    //    PreOpenBestAskVolume5,
    //    [FieldCategory(YwFieldGroup.OpenSimulate)]
    //    PreOpenBestBidVolumes,
    //    [FieldCategory(YwFieldGroup.OpenSimulate)]
    //    PreOpenBestAskVolumes
    //}



    
}
