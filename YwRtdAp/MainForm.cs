using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
using YwRtdAp.Db.DbObject;
using YwRtdAp.Db.Dal;
using YwRtdAp.CombineObject;
using YwRtdLib;

namespace YwRtdAp
{
    delegate void UpdateHandler(DataGridView gv);
    public partial class MainForm : Form
    {
        private RtCore _rtdCore = RtCore.Instance();

        private ConcurrentDictionary<string, YwCommodity> _commodities { get; set; }
        private ConcurrentDictionary<string, YwBasicQuote> _subscribeBasicQuote { get; set; }
        private ConcurrentDictionary<string, YwBest5> _subscribeBest5 { get; set; }
        private ConcurrentDictionary<string, YwFOQuote> _subscribeFOQuote { get; set; }
        private ConcurrentDictionary<string, YwFoundQuote> _subscribeFoundQuote { get; set; }
        private ConcurrentDictionary<string, YwOpenSimulateQuote> _subscribeOpenSimulateQuote { get; set; }
        
        private ConcurrentBag<DayTradeQuote> _dayTradeQuoteDatas { get; set; }

        private ConcurrentBag<DayTradeQuote> _unfilterRawDatas { get; set; }

        private List<string> _dayTradeGVSymbols { get; set; }

        private List<string> _selectedIndustrySymbols { get; set; }
        private List<IndustryQuote> _selectedIndustryQuote { get; set; }

        private List<string> _selectedBizGroupSymbols { get; set; }
        private List<DayTradeQuote> _selectedBizGroupQuote { get; set; }

        private List<string> _selectedConceptSymbols { get; set; }
        private List<DayTradeQuote> _selectedConceptQuote { get; set; }        

        private RtdRepository _rep { get; set; }


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
                        UpdateGridView(this._symbolGV);
                        //Console.WriteLine("Data Change: [ {0}  => {1} ]", newOutcome.Topic.YwFieldType.ToString(), newOutcome.Data);
                    }
                    c--;
                }
                this._bufferRearResetEvent.Set();

            }
        }

        public MainForm()
        {
            InitializeComponent();
            SetUpDbPath();           

            this._rep = new RtdRepository();
            this._rtdCore.CommodityChangeHandler += _rtdCore_CommodityChangeHandler;
            this._rtdCore.BasicQuoteHandler += _rtdCore_BasicQuoteHandler;
            this._rtdCore.Best5Handler += _rtdCore_Best5Handler;
            this._rtdCore.FOQuoteHandler += _rtdCore_FOQuoteHandler;
            this._rtdCore.FoundQuoteHandler += _rtdCore_FoundQuoteHandler;
            this._rtdCore.OpenSimulateQuoteHandler += _rtdCore_OpenSimulateQuoteHandler;
            this._rtdCore.DataChangeHandler += _rtdCore_DataChangeHandler;
            this._commodities = new ConcurrentDictionary<string, YwCommodity>();
            this._subscribeBasicQuote = new ConcurrentDictionary<string, YwBasicQuote>();
            this._subscribeBest5 = new ConcurrentDictionary<string, YwBest5>();
            this._subscribeFOQuote = new ConcurrentDictionary<string, YwFOQuote>();
            this._subscribeFoundQuote = new ConcurrentDictionary<string, YwFoundQuote>();
            this._subscribeOpenSimulateQuote = new ConcurrentDictionary<string, YwOpenSimulateQuote>();
            this._dayTradeQuoteDatas = new ConcurrentBag<DayTradeQuote>();

            this._unfilterRawDatas = new ConcurrentBag<DayTradeQuote>();

            this._selectedIndustrySymbols = new List<string>();
            this._selectedIndustryQuote = new List<IndustryQuote>();

            this._selectedBizGroupSymbols = new List<string>();
            this._selectedBizGroupQuote = new List<DayTradeQuote>();

            this._selectedConceptSymbols = new List<string>();
            this._selectedConceptQuote = new List<DayTradeQuote>();

            this._dayTradeGVSymbols = new List<string>();
            this._symbolGV.AutoGenerateColumns = true;
            this.DoubleBuffered = true;
            Type dgvType = _symbolGV.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                  BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(_symbolGV, true, null);
            LoadAllSymbol();

            //LoadAllConcept();
            //LoadAllBizGroup();
            //LoadAllIndustry();
            //LoadAllPointerIndex();

            //LoadConceptSymbol();
            //LoadIndustrySymbol();
            //LoadBizGroupSymbol();
            //LoadPointerIndexSymbol();

            LoadIndustryDropListData();
            LoadBizGroupDropListData();
            LoadConceptDropListData();

            LoadManagePageSymbolsData();

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

        private void SetUpDbPath()
        {
            FileInfo dbRealPath = new FileInfo("./RtdBase.mdf");
            if (dbRealPath.Exists == false)
            {
                FileInfo devDbPath = new FileInfo("./Db/RtdBase.mdf");
                devDbPath.CopyTo(dbRealPath.FullName, true);
            }
        }
        

        void _rtdCore_DataChangeHandler(DataChangeInfo dci)
        {
            if (this._dayTradeGVSymbols.Exists(x=>dci.Symbols.Contains(x)))
            {
                UpdateGridView(this._dayTradeStockGV);
                
            }
            
        }

        private void LoadIndustryDropListData()
        {
            var industryList = this._rep.FetchAll<Industry>().ToList();
            this._industrySelectCmb.ValueMember = "Code";
            this._industrySelectCmb.DisplayMember = "IndustryName";
            this._industrySelectCmb.DataSource = industryList;            
        }

        private void LoadBizGroupDropListData()
        {
            var bizGroupList = this._rep.FetchAll<BizGroup>().ToList();
            this._bizGroupSelectCmb.ValueMember = "Code";
            this._bizGroupSelectCmb.DisplayMember = "GroupName";
            this._bizGroupSelectCmb.DataSource = bizGroupList;
        }

        private void LoadConceptDropListData()
        {
            var conceptList = this._rep.FetchAll<Concept>().ToList();
            this._conceptSelectCmb.ValueMember = "Code";
            this._conceptSelectCmb.DisplayMember = "ConceptName";
            this._conceptSelectCmb.DataSource = conceptList;
        }

        private void LoadBizGroupSymbol()
        {
            int d = 0;
            List<string> notMatch = new List<string>();
            using (StreamReader sr = new StreamReader("./Data/groupSymbol.csv", Encoding.Default))
            {
                string content = sr.ReadToEnd();
                string[] groupToSymbol = content.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string gts in groupToSymbol)
                {
                    string[] groupSymbolPair = gts.Trim().Split(',');
                    if (groupSymbolPair.Length == 2)
                    {
                        string groupName = groupSymbolPair[0].Trim();
                        string symbol = groupSymbolPair[1].Trim();

                        if (string.IsNullOrEmpty(groupName) == false &&
                            string.IsNullOrEmpty(symbol) == false)
                        {
                            BizGroup groupObj = this._rep.DefaultOne<BizGroup>(x => x.GroupName == groupName);
                            Symbol symbolObj = this._rep.DefaultOne<Symbol>(x => x.Code == symbol);
                            if (groupObj != null &&
                                symbolObj != null)
                            {
                                BizGroupSymbol ids = this._rep.DefaultOne<BizGroupSymbol>(x => x.SymbolId == symbolObj.Id && x.BizGroupId == groupObj.Id);
                                if (ids == null)
                                {
                                    this._rep.Insert<BizGroupSymbol>(new BizGroupSymbol
                                    {
                                        CreateTime = DateTime.Now,
                                        BizGroupId = groupObj.Id,
                                        SymbolId = symbolObj.Id
                                    });
                                    this._rep.Commit();
                                    d += 1;
                                }
                            }
                            else
                            {
                                notMatch.Add(string.Format("{0},{1}", groupName, symbol));
                            }
                        }
                    }
                }
            }

            using (StreamWriter sw = new StreamWriter("./Data/NoMatchGroupSymbol.txt", false))
            {
                foreach (var s in notMatch)
                {
                    sw.WriteLine(s);
                }
            }
            MessageBox.Show(d.ToString());
        }

        private void LoadIndustrySymbol()
        {
            int d = 0;
            List<string> notMatch = new List<string>();
            using (StreamReader sr = new StreamReader("./Data/industrySymbol.csv", Encoding.Default))
            {
                string content = sr.ReadToEnd();
                string[] industryToSymbol = content.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string its in industryToSymbol)
                {
                    string[] industrySymbolPair = its.Trim().Split(',');
                    if (industrySymbolPair.Length == 2)
                    {
                        string industryName = industrySymbolPair[0].Trim();
                        string symbol = industrySymbolPair[1].Trim();

                        if (string.IsNullOrEmpty(industryName) == false &&
                            string.IsNullOrEmpty(symbol) == false)
                        {
                            Industry industryObj = this._rep.DefaultOne<Industry>(x => x.IndustryName == industryName);
                            Symbol symbolObj = this._rep.DefaultOne<Symbol>(x => x.Code == symbol);
                            if (industryObj != null &&
                                symbolObj != null)
                            {
                                IndustrySymbol ids = this._rep.DefaultOne<IndustrySymbol>(x => x.SymbolId == symbolObj.Id && x.IndustryId == industryObj.Id);
                                if (ids == null)
                                {
                                    this._rep.Insert<IndustrySymbol>(new IndustrySymbol
                                    {
                                        CreateTime = DateTime.Now,
                                        IndustryId = industryObj.Id,
                                        SymbolId = symbolObj.Id
                                    });
                                    this._rep.Commit();
                                    d += 1;
                                }
                            }
                            else
                            {
                                notMatch.Add(string.Format("{0},{1}", industryName, symbol));
                            }
                        }
                    }
                }
            }

            using (StreamWriter sw = new StreamWriter("./Data/NoMatchIndustrySymbol.txt", false))
            {
                foreach (var s in notMatch)
                {
                    sw.WriteLine(s);
                }
            }
            MessageBox.Show(d.ToString());
        }

        private void LoadConceptSymbol()
        {
            int d = 0;
            List<string> notMatch = new List<string>();
            using (StreamReader sr = new StreamReader("./Data/conceptSymbol.csv", Encoding.Default))
            {
                string content = sr.ReadToEnd();
                string[] conceptToSymbol = content.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string cts in conceptToSymbol)
                {
                    string[] conceptSymbolPair = cts.Trim().Split(',');
                    if (conceptSymbolPair.Length == 2)
                    {
                        string conceptName = conceptSymbolPair[0].Trim();
                        string symbol = conceptSymbolPair[1].Trim();

                        if (string.IsNullOrEmpty(conceptName) == false &&
                            string.IsNullOrEmpty(symbol) == false)
                        {
                            Concept conceptObj = this._rep.DefaultOne<Concept>(x => x.ConceptName == conceptName);
                            Symbol symbolObj = this._rep.DefaultOne<Symbol>(x => x.Code == symbol);
                            if (conceptObj != null &&
                                symbolObj != null)
                            {
                                ConceptSymbol cs = this._rep.DefaultOne<ConceptSymbol>(x => x.SymbolId == symbolObj.Id && x.ConceptId == conceptObj.Id);
                                if (cs == null)
                                {
                                    this._rep.Insert<ConceptSymbol>(new ConceptSymbol
                                    {
                                        CreateTime = DateTime.Now,
                                        ConceptId = conceptObj.Id,
                                        SymbolId = symbolObj.Id
                                    });
                                    this._rep.Commit();
                                    d += 1;
                                }
                            }
                            else
                            {
                                notMatch.Add(string.Format("{0},{1}", conceptName, symbol));
                            }
                        }                        
                    }                    
                }
            }

            using (StreamWriter sw = new StreamWriter("./Data/NoMatchConceptSymbol.txt", false))
            {
                foreach (var s in notMatch)
                {
                    sw.WriteLine(s);
                }
            }
            MessageBox.Show(d.ToString());
        }

        private void LoadPointerIndexSymbol()
        {
            int d = 0;
            List<string> notMatch = new List<string>();
            using (StreamReader sr = new StreamReader("./Data/pointerIndexSymbol.csv", Encoding.Default))
            {
                string content = sr.ReadToEnd();
                string[] pointerIndexToSymbol = content.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string pits in pointerIndexToSymbol)
                {
                    string[] pointerIndexSymbolPair = pits.Trim().Split(',');
                    if (pointerIndexSymbolPair.Length == 2)
                    {
                        string pointerIndexName = pointerIndexSymbolPair[0].Trim();
                        string symbol = pointerIndexSymbolPair[1].Trim();

                        if (string.IsNullOrEmpty(pointerIndexName) == false &&
                            string.IsNullOrEmpty(symbol) == false)
                        {
                            PointerIndex pointerIndexObj = this._rep.DefaultOne<PointerIndex>(x => x.PointerName == pointerIndexName);
                            Symbol symbolObj = this._rep.DefaultOne<Symbol>(x => x.Code == symbol);
                            if (pointerIndexObj != null &&
                                symbolObj != null)
                            {
                                PointerIndexSymbol pis = this._rep.DefaultOne<PointerIndexSymbol>(x => x.SymbolId == symbolObj.Id && x.PointerIndexId == pointerIndexObj.Id);
                                if (pis == null)
                                {
                                    this._rep.Insert<PointerIndexSymbol>(new PointerIndexSymbol
                                    {
                                        CreateTime = DateTime.Now,
                                        PointerIndexId = pointerIndexObj.Id,
                                        SymbolId = symbolObj.Id
                                    });
                                    this._rep.Commit();
                                    d += 1;
                                }
                            }
                            else
                            {
                                notMatch.Add(string.Format("{0},{1}", pointerIndexName, symbol));
                            }
                        }
                    }
                }
            }

            using (StreamWriter sw = new StreamWriter("./Data/NoMatchPointerIndexSymbol.txt", false))
            {
                foreach (var s in notMatch)
                {
                    sw.WriteLine(s);
                }
            }
            MessageBox.Show(d.ToString());
        }

        private void LoadManagePageSymbolsData()
        {
            var allSymbols = this._rep.FetchAll<Symbol>();
            this._symbolManageLV.Columns.Add("股票代碼");
            this._symbolManageLV.Columns.Add("名稱");
            foreach (var s in allSymbols)
            {
                ListViewItem lvi = new ListViewItem(new string[] { s.Code, s.SymbolName });
                this._symbolManageLV.Items.Add(lvi);
            }
            this._symbolManageLV.Refresh();  

            var allGroupNames = this._rep.FetchAll<BizGroup>().Select(x => x.GroupName);
            this._manageGroupLB.DataSource = allGroupNames.ToList();
            this._manageGroupLB.Refresh();

            var allIndNames = this._rep.FetchAll<Industry>().Select(x => x.IndustryName);
            this._manageIndustryLB.DataSource = allIndNames.ToList();
            this._manageIndustryLB.Refresh();

            var allConceptNames = this._rep.FetchAll<Concept>().Select(x => x.ConceptName);
            this._manageConceptLB.DataSource = allConceptNames.ToList();
            this._manageConceptLB.Refresh();

            var allPointerIndexNames = this._rep.FetchAll<PointerIndex>().Select(x => x.PointerName);
            this._managePILB.DataSource = allPointerIndexNames.ToList();
            this._managePILB.Refresh();

        }

        private void LoadAllSymbol()
        {
            var allSymbols= this._rep.FetchAll<Symbol>();
            foreach (Symbol s in allSymbols)
            { 
                string symbolCode = s.Code.Trim();
                if(string.IsNullOrEmpty(symbolCode) == false)
                {
                    this._rtdCore.AddSymbol(symbolCode);
                }
            }

            //using (StreamReader sr = new StreamReader("./Data/SymbolList.txt", Encoding.UTF8))
            //{
            //    string content = sr.ReadToEnd();
            //    string[] symbols = content.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            //    foreach (string s in symbols)
            //    {
            //        string symbol = s.Trim();
            //        if (string.IsNullOrEmpty(symbol) == false)
            //        {
            //            this._rtdCore.AddSymbol(s.Trim());                       
            //        }                    
            //    }                
            //}
        }

        private void LoadAllConcept()
        {
            using (StreamReader sr = new StreamReader("./Data/ConceptList.txt", Encoding.UTF8))
            {
                string content = sr.ReadToEnd();
                string[] concepts = content.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string s in concepts)
                {
                    string concept = s.Trim();
                    if (string.IsNullOrEmpty(concept) == false)
                    {
                                               
                    }
                }
                
            }
        }

        private void LoadAllBizGroup()
        {
            using (StreamReader sr = new StreamReader("./Data/BizGroupList.txt", Encoding.UTF8))
            {
                string bizGroup = sr.ReadToEnd();
                string[] groups = bizGroup.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string s in groups)
                {
                    string grp = s.Trim();
                    if (string.IsNullOrEmpty(grp) == false)
                    {
                        
                    }
                }

            }
        }

        private void LoadAllIndustry()
        {
            using (StreamReader sr = new StreamReader("./Data/IndustryList.txt", Encoding.UTF8))
            {
                string industryData = sr.ReadToEnd();
                string[] industries = industryData.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string s in industries)
                {
                    string industry = s.Trim();
                    if (string.IsNullOrEmpty(industry) == false)
                    {
                        
                    }
                }

            }
        }

        private void LoadAllPointerIndex()
        {
            using (StreamReader sr = new StreamReader("./Data/PointerIndexList.txt", Encoding.UTF8))
            {
                string pointerIndexData = sr.ReadToEnd();
                string[] pointerIndexs = pointerIndexData.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string s in pointerIndexs)
                {
                    string pointerIdx = s.Trim();
                    if (string.IsNullOrEmpty(pointerIdx) == false)
                    {
                        int maxId = 0;
                        PointerIndex lastPiObj = this._rep.FetchAll<PointerIndex>().OrderByDescending(x => x.Id).FirstOrDefault();
                        if (lastPiObj != null)
                        {
                            maxId = lastPiObj.Id;
                        }
                        this._rep.Insert<PointerIndex>(new PointerIndex
                        {
                            Code = string.Format("PI_{0}", maxId + 1),
                            PointerName = pointerIdx,
                            CreateTime = DateTime.Now
                        });
                        this._rep.Commit();
                    }
                }

            }
        }

        void _rtdCore_OpenSimulateQuoteHandler(YwOpenSimulateQuote openSimulateQuote)
        {
            if (_subscribeOpenSimulateQuote.ContainsKey(openSimulateQuote.Symbol) == false)
            {
                
                _subscribeOpenSimulateQuote.TryAdd(openSimulateQuote.Symbol, openSimulateQuote);
            }
            //UpdateGridView(this._symbolGV);
        }

        void _rtdCore_FoundQuoteHandler(YwFoundQuote foundQuote)
        {
            if (_subscribeFoundQuote.ContainsKey(foundQuote.Symbol) == false)
            {
                _subscribeFoundQuote.TryAdd(foundQuote.Symbol, foundQuote);
            }
            //UpdateGridView(this._symbolGV);
        }

        void _rtdCore_FOQuoteHandler(YwFOQuote foQuote)
        {
            if (_subscribeFOQuote.ContainsKey(foQuote.Symbol) == false)
            {
                _subscribeFOQuote.TryAdd(foQuote.Symbol, foQuote);
            }
            //UpdateGridView(this._symbolGV);
        }

        void _rtdCore_Best5Handler(YwBest5 best5)
        {
            if (_subscribeBest5.ContainsKey(best5.Symbol) == false)
            {
                _subscribeBest5.TryAdd(best5.Symbol, best5);
            }
            //UpdateGridView(this._symbolGV);
        }

        object _lockObj = new object();
        void _rtdCore_BasicQuoteHandler(YwBasicQuote basicQuote)
        {
            if (_subscribeBasicQuote.ContainsKey(basicQuote.Symbol) == false)
            {
                _subscribeBasicQuote.TryAdd(basicQuote.Symbol, basicQuote);
                //this._dayTradeQuoteDatas.Add(new DayTradeQuote(ref basicQuote));
                //this._unfilterRawDatas.Add(new DayTradeQuote(ref basicQuote));                               
            }
            
            //UpdateGridView(this._symbolGV);
            lock (_lockObj)
            {
                RtdRepository repo = new RtdRepository();
                var symbolList = repo.Query<Symbol>(x => x.Code == basicQuote.Symbol);
                foreach (var symbol in symbolList)
                {
                    YwBasicQuote commodity = null;
                    if (this._subscribeBasicQuote.TryGetValue(symbol.Code, out commodity))
                    {
                        if (commodity != null &&
                            string.IsNullOrEmpty(commodity.Name) == false &&
                            string.IsNullOrEmpty(symbol.SymbolName) == true)
                        {
                            symbol.SymbolName = commodity.Name;
                            repo.Update<Symbol>(symbol);                            
                        }
                    }
                }
                repo.Commit();
            }
        }
        
        void _rtdCore_CommodityChangeHandler(YwCommodity commodity)
        {
            if (_commodities.ContainsKey(commodity.Symbol) == false)
            {
                _commodities.TryAdd(commodity.Symbol, commodity);                                
                //UpdateGridView(this._symbolGV);
            }            
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

        private void _addSymbolBtn_Click(object sender, EventArgs e)
        {
            //if(string.IsNullOrEmpty(this._symbolTxt.Text) == false)
            //{
            //    this._rtdCore.AddSymbol(this._symbolTxt.Text.Trim());
            //}                 
            this._symbolGV.DataSource = this._commodities.Values.ToList();
            this._symbolGV.Refresh();
            this._symbolTxt.Text = "";
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this._rtdCore.Terminate();
            this._bufferRearThread.Abort();
            this._bufferFrontThread.Abort();
            this._updateEventThread.Abort();
        }

        private void _filterCeilPriceStockBtn_Click(object sender, EventArgs e)
        {
            this._dayTradeStockGV.DataSource = this._dayTradeQuoteDatas.Where(x => x.Ceil == x.Price &&
                x.Ceil != "" && x.Ceil != "無資料" &&
                x.Price != "" && x.Price != "無資料").OrderBy(x => x.Symbol).ToList();

            this._dayTradeStockGV.Refresh();
        }

        private void _filterFloorPriceStockBtn_Click(object sender, EventArgs e)
        {
            this._dayTradeStockGV.DataSource = this._dayTradeQuoteDatas.Where(x => x.Floor == x.Price &&
                x.Floor != "" && x.Floor != "無資料" &&
                x.Price != "" && x.Price != "無資料").OrderBy(x => x.Symbol).ToList();
            this._dayTradeStockGV.Refresh();
        }

        private void _filterUpMostBtn_Click(object sender, EventArgs e)
        {
            this._dayTradeStockGV.DataSource = this._dayTradeQuoteDatas.Where(x => x.ChangePercent.Value > 7.50M && x.ChangePercent.Value <= 9.69M).OrderBy(x => x.Symbol).ToList();
            this._dayTradeStockGV.Refresh();
        }

        private void _filterUpMidBtn_Click(object sender, EventArgs e)
        {
            var quoteList = this._dayTradeQuoteDatas.Where(x => x.ChangePercent.Value > 5.01M && x.ChangePercent.Value <= 7.50M).OrderBy(x => x.Symbol).ToList();
            this._dayTradeGVSymbols.Clear();
            this._dayTradeGVSymbols.AddRange(quoteList.Select(x => x.Symbol));
            this._dayTradeStockGV.DataSource = quoteList;
            this._dayTradeStockGV.Refresh();
        }

        private void _filterUpLowBtn_Click(object sender, EventArgs e)
        {
            this._dayTradeStockGV.DataSource = this._dayTradeQuoteDatas.Where(x => x.ChangePercent.Value > 2.50M && x.ChangePercent.Value <= 5.01M).OrderBy(x => x.Symbol).ToList();
            this._dayTradeStockGV.Refresh();
        }

        private void _filterDownMostBtn_Click(object sender, EventArgs e)
        {
            this._dayTradeStockGV.DataSource = this._dayTradeQuoteDatas.Where(x => x.ChangePercent.Value >= -9.69M && x.ChangePercent.Value < -7.51M).OrderBy(x => x.Symbol).ToList();
            this._dayTradeStockGV.Refresh();
        }

        private void _filterDownMidBtn_Click(object sender, EventArgs e)
        {
            this._dayTradeStockGV.DataSource = this._dayTradeQuoteDatas.Where(x => x.ChangePercent.Value >= -7.50M && x.ChangePercent.Value < -5.01M).OrderBy(x => x.Symbol).ToList();
            this._dayTradeStockGV.Refresh();
        }

        private void _filterDownLowBtn_Click(object sender, EventArgs e)
        {
            this._dayTradeStockGV.DataSource = this._dayTradeQuoteDatas.Where(x => x.ChangePercent.Value >= -5.00M && x.ChangePercent.Value < -2.50M).OrderBy(x => x.Symbol).ToList();
            this._dayTradeStockGV.Refresh();
        }

        /// <summary>
        /// 使用者按下加入Symbol的按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _manageSymbolAddBtn_Click(object sender, EventArgs e)
        {            
            if (MessageBox.Show("確定要新增嗎?", "Notice", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
            {
                string symbol = this._manageSymbolTxt.Text.Trim();
                string symbolName = this._manageSymbolNameTxt.Text.Trim();
                Symbol symbolObj = this._rep.DefaultOne<Symbol>(x => x.Code == symbol);
                if (symbolObj != null)
                {
                    MessageBox.Show("該股票已存在，無法新增");
                    return;
                }
                this._rep.Insert<Symbol>(new Symbol
                {
                    Code = symbol,
                    SymbolName = symbolName,
                    CreateTime = DateTime.Now
                });
                this._rep.Commit();

                MessageBox.Show("新增成功");
                var allSymbols = this._rep.FetchAll<Symbol>();
                this._symbolManageLV.Clear();
                this._symbolManageLV.Columns.Add("股票代碼");
                this._symbolManageLV.Columns.Add("名稱");
                foreach (var s in allSymbols)
                {
                    ListViewItem lvi = new ListViewItem(new string[] { s.Code, s.SymbolName });
                    this._symbolManageLV.Items.Add(lvi);
                }
                this._symbolManageLV.Refresh(); 
            }                       
        }

        

        private void _manageGroupAddBtn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("確定要新增嗎?", "Notice", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
            {
                string groupName = this._manageGroupTxt.Text.Trim();
                BizGroup grpObj = this._rep.DefaultOne<BizGroup>(x => x.GroupName == groupName);
                if (grpObj != null)
                {
                    MessageBox.Show("已經有此集團股分類，無法新增");
                    return;
                }
                int maxId = 0;
                BizGroup lastGroupObj = this._rep.FetchAll<BizGroup>().OrderByDescending(x => x.Id).FirstOrDefault();
                if (lastGroupObj != null)
                {
                    maxId = lastGroupObj.Id;
                }
                this._rep.Insert<BizGroup>(new BizGroup
                {
                    Code = string.Format("GP_{0}", maxId + 1),
                    GroupName = groupName,
                    CreateTime = DateTime.Now
                });
                this._rep.Commit();
                MessageBox.Show("新增成功");
                var allGroupNames = this._rep.FetchAll<BizGroup>().Select(x => x.GroupName);
                this._manageGroupLB.DataSource = allGroupNames.ToList();
                this._manageGroupLB.Refresh();
                this._manageGroupTxt.Text = ""; 
            }  

            return;
            
        }

        private void _manageIndustryAddBtn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("確定要新增嗎?", "Notice", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
            {
                string industryName = this._manageIndustryTxt.Text.Trim();
                Industry indObj = this._rep.DefaultOne<Industry>(x => x.IndustryName == industryName);
                if (indObj != null)
                {
                    MessageBox.Show("已經有此產業股分類，無法新增");
                    return;
                }
                int maxId = 0;
                Industry lastIndObj = this._rep.FetchAll<Industry>().OrderByDescending(x => x.Id).FirstOrDefault();
                if (lastIndObj != null)
                {
                    maxId = lastIndObj.Id;
                }
                this._rep.Insert<Industry>(new Industry
                {
                    Code = string.Format("IN_{0}", maxId + 1),
                    IndustryName = industryName,
                    CreateTime = DateTime.Now
                });
                this._rep.Commit();
                MessageBox.Show("新增成功");
                var allIndNames = this._rep.FetchAll<Industry>().Select(x => x.IndustryName);
                this._manageIndustryLB.DataSource = allIndNames.ToList();
                this._manageIndustryLB.Refresh();
                this._manageIndustryTxt.Text = ""; 
            }
            return;
            
        }

        private void _manageConceptAddBtn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("確定要新增嗎?", "Notice", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
            {
                string conceptName = this._manageConceptTxt.Text.Trim();
                Concept conceptObj = this._rep.DefaultOne<Concept>(x => x.ConceptName == conceptName);
                if (conceptObj != null)
                {
                    MessageBox.Show("已經有此概念股分類，無法新增");
                    return;
                }
                int maxId = 0;
                Concept lastConceptObj = this._rep.FetchAll<Concept>().OrderByDescending(x => x.Id).FirstOrDefault();
                if (lastConceptObj != null)
                {
                    maxId = lastConceptObj.Id;
                }
                this._rep.Insert<Concept>(new Concept
                {
                    Code = string.Format("CP_{0}", maxId + 1),
                    ConceptName = conceptName,
                    CreateTime = DateTime.Now
                });
                this._rep.Commit();
                MessageBox.Show("新增成功");
                var allConceptNames = this._rep.FetchAll<Concept>().Select(x => x.ConceptName);
                this._manageConceptLB.DataSource = allConceptNames.ToList();
                this._manageConceptLB.Refresh();
                this._manageConceptTxt.Text = "";
            }
            return;
            
        }

        private void _industrySelectCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            string industryCode = ((Industry)this._industrySelectCmb.SelectedItem).Code;
            
            var selectedIndustry = this._rep.Query<Industry>(x => x.Code == industryCode);
            var allIndustrySymbolMap = this._rep.FetchAll<IndustrySymbol>();
            var symbolIdList = (from industries in selectedIndustry
                             join symbols in allIndustrySymbolMap
                             on industries.Id equals symbols.IndustryId
                             select symbols.SymbolId).ToList();

            this._selectedIndustrySymbols.Clear();            
            List<string> symbolCodes = this._rep.Query<Symbol>(x => symbolIdList.Contains(x.Id)).Select(x => x.Code).ToList();
            this._selectedIndustrySymbols.AddRange(symbolCodes);
            List<YwCommodity> filteredCommodities = this._commodities.Values.Where(x => symbolCodes.Contains(x.Symbol)).ToList();
            this._selectedIndustryQuote.Clear();
            for(int i = 0 ;i < filteredCommodities.Count;i ++)
            {
                YwCommodity c = filteredCommodities[i];
                this._selectedIndustryQuote.Add(new IndustryQuote(ref c));
            }
            
            this._filteredIndustryGV.DataSource = null;
            this._filteredIndustryGV.DataSource = this._selectedIndustryQuote;
            this._filteredIndustryGV.Refresh();
        }

        private void _bizGroupSelectCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            string bizGroupCode = ((BizGroup)this._bizGroupSelectCmb.SelectedItem).Code;
            var selectedBizGroup = this._rep.Query<BizGroup>(x => x.Code == bizGroupCode);
            var allBizGroupSymbolMap = this._rep.FetchAll<BizGroupSymbol>();
            var symbolIdList = (from bizGroups in selectedBizGroup
                                join symbols in allBizGroupSymbolMap
                                on bizGroups.Id equals symbols.BizGroupId
                                select symbols.SymbolId).ToList();

            this._selectedBizGroupSymbols.Clear();
            List<string> symbolCodes = this._rep.Query<Symbol>(x => symbolIdList.Contains(x.Id)).Select(x => x.Code).ToList();
            this._selectedBizGroupSymbols.AddRange(symbolCodes);

            this._selectedBizGroupQuote.Clear();
            this._selectedBizGroupQuote.AddRange(this._unfilterRawDatas.Where(x => symbolCodes.Contains(x.Symbol)).ToList());
            this._filteredBizGroupGV.DataSource = null;
            this._filteredBizGroupGV.DataSource = this._selectedBizGroupQuote;
            this._filteredBizGroupGV.Refresh();
        }

        private void _conceptSelectCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            string conceptCode = ((Concept)this._conceptSelectCmb.SelectedItem).Code;
            var selectedConcept = this._rep.Query<Concept>(x => x.Code == conceptCode);
            var allConceptSymbolMap = this._rep.FetchAll<ConceptSymbol>();
            var symbolIdList = (from concepts in selectedConcept
                                join symbols in allConceptSymbolMap
                                on concepts.Id equals symbols.ConceptId
                                select symbols.SymbolId).ToList();

            this._selectedConceptSymbols.Clear();
            List<string> symbolCodes = this._rep.Query<Symbol>(x => symbolIdList.Contains(x.Id)).Select(x => x.Code).ToList();
            this._selectedConceptSymbols.AddRange(symbolCodes);

            this._selectedConceptQuote.Clear();
            this._selectedConceptQuote.AddRange(this._unfilterRawDatas.Where(x => symbolCodes.Contains(x.Symbol)).ToList());
            this._filteredConceptGV.DataSource = null;
            this._filteredConceptGV.DataSource = this._selectedConceptQuote;
            this._filteredConceptGV.Refresh();
        }

        private void _filteredIndustryGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this._filteredIndustryGV.DataSource == null)
            {
                e.FormattingApplied = true;
                return;
            }
            if (this._filteredIndustryGV.Columns[e.ColumnIndex].Name == "Change")
            {
                decimal upOrDown = Convert.ToDecimal(this._filteredIndustryGV.Rows[e.RowIndex].Cells[8].Value);
                if (upOrDown > 0)
                {
                    this._filteredIndustryGV.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    e.FormattingApplied = true;
                    return;
                }
                if (upOrDown < 0)
                {
                    this._filteredIndustryGV.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Green;
                    e.FormattingApplied = true;
                    return;
                }
                else
                { 
                    return;
                }
            }
        }

        private void _filteredBizGroupGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this._filteredBizGroupGV.DataSource == null)
            {
                e.FormattingApplied = true;
                return;
            }
            if (this._filteredBizGroupGV.Columns[e.ColumnIndex].Name == "Change")
            {
                decimal upOrDown = Convert.ToDecimal(this._filteredBizGroupGV.Rows[e.RowIndex].Cells[8].Value);
                if (upOrDown > 0)
                {
                    this._filteredBizGroupGV.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    e.FormattingApplied = true;
                    return;
                }
                if (upOrDown < 0)
                {
                    this._filteredBizGroupGV.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Green;
                    e.FormattingApplied = true;
                    return;
                }
                else
                {
                    return;
                }
            }
        }

        private void _filteredConceptGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this._filteredConceptGV.DataSource == null)
            {
                e.FormattingApplied = true;
                return;
            }
            if (this._filteredConceptGV.Columns[e.ColumnIndex].Name == "Change")
            {
                decimal upOrDown = Convert.ToDecimal(this._filteredConceptGV.Rows[e.RowIndex].Cells[8].Value);
                if (upOrDown > 0)
                {
                    this._filteredConceptGV.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    e.FormattingApplied = true;
                    return;
                }
                if (upOrDown < 0)
                {
                    this._filteredConceptGV.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Green;
                    e.FormattingApplied = true;
                    return;
                }
                else
                {
                    return;
                }
            }
        }

        private void _managePIAddBtn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("確定要新增嗎?", "Notice", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
            {
                string piName = this._managePINameTxt.Text.Trim();
                PointerIndex piObj = this._rep.DefaultOne<PointerIndex>(x => x.PointerName == piName);

                if (piObj != null)
                {
                    MessageBox.Show("該指標股已存在，無法新增");
                    return;
                }
                int maxId = this._rep.FetchAll<PointerIndex>().OrderByDescending(x => x.Id).FirstOrDefault().Id;
                this._rep.Insert<PointerIndex>(new PointerIndex
                {
                    Code = string.Format("PI_{0}", maxId + 1),
                    PointerName = piName,
                    CreateTime = DateTime.Now
                });
                this._rep.Commit();

                MessageBox.Show("新增成功");
                var allPointerIndexs = this._rep.FetchAll<PointerIndex>().Select(x => x.PointerName);
                this._managePILB.DataSource = allPointerIndexs.ToList();
                this._managePILB.Refresh();
            }  
        }

        private void _managePIQtyBtn_Click(object sender, EventArgs e)
        {
            string pointerIndexName = this._managePINameTxt.Text.Trim();
            var allPointerIndexs = this._rep.FetchAll<PointerIndex>().Select(x => x.PointerName);

            if (string.IsNullOrEmpty(pointerIndexName) == false)
            {
                allPointerIndexs = allPointerIndexs.Where(x => x.Contains(pointerIndexName));
            }
            
            this._managePILB.DataSource = allPointerIndexs.ToList();
            this._managePILB.Refresh();            
        }

        private void _manageGroupQryBtn_Click(object sender, EventArgs e)
        {
            string groupName = this._manageGroupTxt.Text.Trim();
            var allGroups = this._rep.FetchAll<BizGroup>().Select(x => x.GroupName);

            if (string.IsNullOrEmpty(groupName) == false)
            {
                allGroups = allGroups.Where(x => x.Contains(groupName));
            }

            this._manageGroupLB.DataSource = allGroups.ToList();
            this._manageGroupLB.Refresh();            
        }

        private void _manageIndustryQryBtn_Click(object sender, EventArgs e)
        {
            string industryName = this._manageIndustryTxt.Text.Trim();
            var allIndustries = this._rep.FetchAll<Industry>().Select(x => x.IndustryName);

            if (string.IsNullOrEmpty(industryName) == false)
            {
                allIndustries = allIndustries.Where(x => x.Contains(industryName));
            }

            this._manageIndustryLB.DataSource = allIndustries.ToList();
            this._manageIndustryLB.Refresh();            
        }

        private void _manageConceptQryBtn_Click(object sender, EventArgs e)
        {
            string conceptName = this._manageConceptTxt.Text.Trim();
            var allConcepts = this._rep.FetchAll<Concept>().Select(x => x.ConceptName);

            if (string.IsNullOrEmpty(conceptName) == false)
            {
                allConcepts = allConcepts.Where(x => x.Contains(conceptName));
            }

            this._manageConceptLB.DataSource = allConcepts.ToList();
            this._manageConceptLB.Refresh();        
        }

        private void _manageSymbolQryBtn_Click(object sender, EventArgs e)
        {
            string symbolCode = this._manageSymbolTxt.Text.Trim();
            string symbolName = this._manageSymbolNameTxt.Text.Trim();
            var allSymbols = this._rep.FetchAll<Symbol>();
            if (string.IsNullOrEmpty(symbolCode) == false)
            {
                allSymbols = allSymbols.Where(x => x.Code.Contains(symbolCode));
            }
            if (string.IsNullOrEmpty(symbolName) == false)
            {
                allSymbols = allSymbols.Where(x => x.SymbolName.Contains(symbolName));
            }

            this._symbolManageLV.Clear();
            this._symbolManageLV.Columns.Add("股票代碼");
            this._symbolManageLV.Columns.Add("名稱");

            foreach (var s in allSymbols)
            {
                ListViewItem lvi = new ListViewItem(new string[] { s.Code, s.SymbolName });
                this._symbolManageLV.Items.Add(lvi);
            }
            this._symbolManageLV.Refresh();  
        }        
    }
}
