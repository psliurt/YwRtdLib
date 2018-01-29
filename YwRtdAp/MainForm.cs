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
using YwRtdAp.Web;

namespace YwRtdAp
{
    //delegate void UpdateHandler(DataGridView gv);
    public partial class MainForm : Form
    {
        private RtCore _rtdCore = RtCore.Instance();
        private Dispatcher _dispatcher { get; set; }
        private DailyMarketUpdater _marketUpdater { get; set; }

        private ConcurrentDictionary<string, YwCommodity> _commodities { get; set; }

        private ConcurrentDictionary<string, PriorAfterMarketStatistic> _afterMarketData { get; set; }

        //private ConcurrentDictionary<string, YwBasicQuote> _subscribeBasicQuote { get; set; }
        //private ConcurrentDictionary<string, YwBest5> _subscribeBest5 { get; set; }
        //private ConcurrentDictionary<string, YwFOQuote> _subscribeFOQuote { get; set; }
        //private ConcurrentDictionary<string, YwFoundQuote> _subscribeFoundQuote { get; set; }
        //private ConcurrentDictionary<string, YwOpenSimulateQuote> _subscribeOpenSimulateQuote { get; set; }
        
        //private ConcurrentBag<DayTradeQuote> _dayTradeQuoteDatas { get; set; }

        //private ConcurrentBag<DayTradeQuote> _unfilterRawDatas { get; set; }

        private List<string> _dayTradeGVSymbols { get; set; }

        private List<string> _selectedIndustrySymbols { get; set; }
        private List<IndustryQuote> _selectedIndustryQuote { get; set; }

        private List<string> _selectedBizGroupSymbols { get; set; }
        private List<BizGroupQuote> _selectedBizGroupQuote { get; set; }

        private List<string> _selectedConceptSymbols { get; set; }
        private List<ConceptQuote> _selectedConceptQuote { get; set; }

        private List<string> _selectedPointerIndexSymbols { get; set; }
        private List<PointerIndexQuote> _selectedPointerIndexQuote { get; set; }

        private List<string> _queryOneSymbols { get; set; }
        private List<OneSymbolQuote> _queryOneSymbolQuote { get; set; }

        private List<string> _oneCategoryRelateSymbols { get; set; }
        private List<CategoryRelatedQuote> _oneCategoryRelateSymbolQuote { get; set; }

        private List<string> _dayTradeSymbols { get; set; }
        private List<DayTradeQuote> _dayTradeSymbolQuote { get; set; }

        /// <summary>
        /// symbol -> dailyMarketTradeData Object
        /// </summary>
        private Dictionary<string, DailyMarketTradeData> _dailyMarketTradeInfos { get; set; }

        private RtdRepository _rep { get; set; }        

        public MainForm()
        {
            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.DoubleBuffer, true);

            this._commodities = new ConcurrentDictionary<string, YwCommodity>();
            this._afterMarketData = new ConcurrentDictionary<string, PriorAfterMarketStatistic>();
            this._dispatcher = Dispatcher.Instance(this._rtdCore, this._commodities);
            this._dailyMarketTradeInfos = new Dictionary<string, DailyMarketTradeData>();
            DailyMarketInfoDownloader.Instance(this._dailyMarketTradeInfos);
            InitializeComponent();
            SetUpDbPath();           

            this._rep = new RtdRepository();
            this._marketUpdater = DailyMarketUpdater.Initialize(this._afterMarketData, this._rep);
            this._rtdCore.CommodityChangeHandler += _rtdCore_CommodityChangeHandler;
            //this._rtdCore.BasicQuoteHandler += _rtdCore_BasicQuoteHandler;
            //this._rtdCore.Best5Handler += _rtdCore_Best5Handler;
            //this._rtdCore.FOQuoteHandler += _rtdCore_FOQuoteHandler;
            //this._rtdCore.FoundQuoteHandler += _rtdCore_FoundQuoteHandler;
            //this._rtdCore.OpenSimulateQuoteHandler += _rtdCore_OpenSimulateQuoteHandler;
            //this._rtdCore.DataChangeHandler += _rtdCore_DataChangeHandler;
            
            //this._subscribeBasicQuote = new ConcurrentDictionary<string, YwBasicQuote>();
            //this._subscribeBest5 = new ConcurrentDictionary<string, YwBest5>();
            //this._subscribeFOQuote = new ConcurrentDictionary<string, YwFOQuote>();
            //this._subscribeFoundQuote = new ConcurrentDictionary<string, YwFoundQuote>();
            //this._subscribeOpenSimulateQuote = new ConcurrentDictionary<string, YwOpenSimulateQuote>();
            //this._dayTradeQuoteDatas = new ConcurrentBag<DayTradeQuote>();

            //this._unfilterRawDatas = new ConcurrentBag<DayTradeQuote>();

            this._selectedIndustrySymbols = new List<string>();
            this._selectedIndustryQuote = new List<IndustryQuote>();

            this._selectedBizGroupSymbols = new List<string>();
            this._selectedBizGroupQuote = new List<BizGroupQuote>();

            this._selectedConceptSymbols = new List<string>();
            this._selectedConceptQuote = new List<ConceptQuote>();

            this._selectedPointerIndexSymbols = new List<string>();
            this._selectedPointerIndexQuote = new List<PointerIndexQuote>();

            this._queryOneSymbols = new List<string>();
            this._queryOneSymbolQuote = new List<OneSymbolQuote>();

            this._oneCategoryRelateSymbols = new List<string>();
            this._oneCategoryRelateSymbolQuote = new List<CategoryRelatedQuote>();

            this._dayTradeSymbols = new List<string>();
            this._dayTradeSymbolQuote = new List<DayTradeQuote>();

            this._dayTradeGVSymbols = new List<string>();
            this._symbolGV.AutoGenerateColumns = true;
            this.DoubleBuffered = true;

            SetUpGridViewDoubleBuffered(this._symbolGV);
            SetUpGridViewDoubleBuffered(this._filteredPointerIndexGV);
            SetUpGridViewDoubleBuffered(this._filteredIndustryGV);
            SetUpGridViewDoubleBuffered(this._filteredBizGroupGV);
            SetUpGridViewDoubleBuffered(this._filteredConceptGV);
            



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
            LoadPointerIndexDropListData();

            LoadManagePageSymbolsData();

            this.InputLanguageChanged += MainForm_InputLanguageChanged;
        }

        void MainForm_InputLanguageChanged(object sender, InputLanguageChangedEventArgs e)
        {
            // 將控制項的ImeMode設為OnHalf
            this._oneSymbolQueryTxt.ImeMode = System.Windows.Forms.ImeMode.OnHalf;
        }

        private void SetUpGridViewDoubleBuffered(DataGridView gv)
        {
            Type dgvType = gv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                  BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(gv, true, null);
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
        

        //void _rtdCore_DataChangeHandler(DataChangeInfo dci)
        //{
        //    if (this._dayTradeGVSymbols.Exists(x=>dci.Symbols.Contains(x)))
        //    {
        //        //UpdateGridView(this._dayTradeStockGV);
                
        //    }
            
        //}

        private void LoadPointerIndexDropListData()
        {
            var pointerIndexList = this._rep.FetchAll<PointerIndex>().ToList();
            this._pointerIndexSelectCmb.ValueMember = "Code";
            this._pointerIndexSelectCmb.DisplayMember = "PointerName";
            this._pointerIndexSelectCmb.DataSource = pointerIndexList;

            this._symbolCatPICmb.ValueMember = "Code";
            this._symbolCatPICmb.DisplayMember = "PointerName";
            this._symbolCatPICmb.DataSource = pointerIndexList;
        }

        private void LoadIndustryDropListData()
        {
            var industryList = this._rep.FetchAll<Industry>().ToList();
            this._industrySelectCmb.ValueMember = "Code";
            this._industrySelectCmb.DisplayMember = "IndustryName";
            this._industrySelectCmb.DataSource = industryList;

            this._symbolCatIndustryCmb.ValueMember = "Code";
            this._symbolCatIndustryCmb.DisplayMember = "IndustryName";
            this._symbolCatIndustryCmb.DataSource = industryList;        
        }

        private void LoadBizGroupDropListData()
        {
            var bizGroupList = this._rep.FetchAll<BizGroup>().ToList();
            this._bizGroupSelectCmb.ValueMember = "Code";
            this._bizGroupSelectCmb.DisplayMember = "GroupName";
            this._bizGroupSelectCmb.DataSource = bizGroupList;

            this._symbolCatBizGroupCmb.ValueMember = "Code";
            this._symbolCatBizGroupCmb.DisplayMember = "GroupName";
            this._symbolCatBizGroupCmb.DataSource = bizGroupList;
        }

        private void LoadConceptDropListData()
        {
            var conceptList = this._rep.FetchAll<Concept>().ToList();
            this._conceptSelectCmb.ValueMember = "Code";
            this._conceptSelectCmb.DisplayMember = "ConceptName";
            this._conceptSelectCmb.DataSource = conceptList;

            this._symbolCatConceptCmb.ValueMember = "Code";
            this._symbolCatConceptCmb.DisplayMember = "ConceptName";
            this._symbolCatConceptCmb.DataSource = conceptList;
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
                    //string pointerIdx = s.Trim();
                    //if (string.IsNullOrEmpty(pointerIdx) == false)
                    //{
                    //    int maxId = 0;
                    //    PointerIndex lastPiObj = this._rep.FetchAll<PointerIndex>().OrderByDescending(x => x.Id).FirstOrDefault();
                    //    if (lastPiObj != null)
                    //    {
                    //        maxId = lastPiObj.Id;
                    //    }
                    //    this._rep.Insert<PointerIndex>(new PointerIndex
                    //    {
                    //        Code = string.Format("PI_{0}", maxId + 1),
                    //        PointerName = pointerIdx,
                    //        CreateTime = DateTime.Now
                    //    });
                    //    this._rep.Commit();
                    //}
                }

            }
        }

        //void _rtdCore_OpenSimulateQuoteHandler(YwOpenSimulateQuote openSimulateQuote)
        //{
        //    if (_subscribeOpenSimulateQuote.ContainsKey(openSimulateQuote.Symbol) == false)
        //    {
                
        //        _subscribeOpenSimulateQuote.TryAdd(openSimulateQuote.Symbol, openSimulateQuote);
        //    }
        //    //UpdateGridView(this._symbolGV);
        //}

        //void _rtdCore_FoundQuoteHandler(YwFoundQuote foundQuote)
        //{
        //    if (_subscribeFoundQuote.ContainsKey(foundQuote.Symbol) == false)
        //    {
        //        _subscribeFoundQuote.TryAdd(foundQuote.Symbol, foundQuote);
        //    }
        //    //UpdateGridView(this._symbolGV);
        //}

        //void _rtdCore_FOQuoteHandler(YwFOQuote foQuote)
        //{
        //    if (_subscribeFOQuote.ContainsKey(foQuote.Symbol) == false)
        //    {
        //        _subscribeFOQuote.TryAdd(foQuote.Symbol, foQuote);
        //    }
        //    //UpdateGridView(this._symbolGV);
        //}

        //void _rtdCore_Best5Handler(YwBest5 best5)
        //{
        //    if (_subscribeBest5.ContainsKey(best5.Symbol) == false)
        //    {
        //        _subscribeBest5.TryAdd(best5.Symbol, best5);
        //    }
        //    //UpdateGridView(this._symbolGV);
        //}

        //object _lockObj = new object();
        //void _rtdCore_BasicQuoteHandler(YwBasicQuote basicQuote)
        //{
        //    if (_subscribeBasicQuote.ContainsKey(basicQuote.Symbol) == false)
        //    {
        //        _subscribeBasicQuote.TryAdd(basicQuote.Symbol, basicQuote);
        //        //this._dayTradeQuoteDatas.Add(new DayTradeQuote(ref basicQuote));
        //        //this._unfilterRawDatas.Add(new DayTradeQuote(ref basicQuote));                               
        //    }
            
        //    //UpdateGridView(this._symbolGV);
        //    lock (_lockObj)
        //    {
        //        RtdRepository repo = new RtdRepository();
        //        var symbolList = repo.Query<Symbol>(x => x.Code == basicQuote.Symbol);
        //        foreach (var symbol in symbolList)
        //        {
        //            YwBasicQuote commodity = null;
        //            if (this._subscribeBasicQuote.TryGetValue(symbol.Code, out commodity))
        //            {
        //                if (commodity != null &&
        //                    string.IsNullOrEmpty(commodity.Name) == false &&
        //                    string.IsNullOrEmpty(symbol.SymbolName) == true)
        //                {
        //                    symbol.SymbolName = commodity.Name;
        //                    repo.Update<Symbol>(symbol);                            
        //                }
        //            }
        //        }
        //        repo.Commit();
        //    }
        //}
        
        void _rtdCore_CommodityChangeHandler(YwCommodity commodity)
        {
            if (_commodities.ContainsKey(commodity.Symbol) == false)
            {
                _commodities.TryAdd(commodity.Symbol, commodity);                                
                //UpdateGridView(this._symbolGV);
            }            
        }

        
        //private void UpdateGridView(DataGridView gv)
        //{
        //    if (gv.InvokeRequired)
        //    {
        //        UpdateHandler handler = new UpdateHandler(UpdateGridView);
        //        gv.Invoke(handler, gv);
        //    }
        //    else
        //    {
        //        //gv.DataSource = this._dayTradeQuoteDatas;
        //        gv.Refresh();                
        //    }
        //}

        private void _addSymbolBtn_Click(object sender, EventArgs e)
        {
            //if(string.IsNullOrEmpty(this._symbolTxt.Text) == false)
            //{
            //    this._rtdCore.AddSymbol(this._symbolTxt.Text.Trim());
            //}                 
            //this._symbolGV.DataSource = this._commodities.Values.ToList();
            //this._symbolGV.Refresh();
            //this._symbolTxt.Text = "";


            this._dispatcher.RemoveSymbolGridMap(this._dayTradeSymbols, this._symbolGV);
            this._dayTradeSymbols.Clear();
            List<YwCommodity> filteredCommodities = this._commodities.Values.Where(x => x.ChangePercent.HasValue &&
                x.ChangePercent.Value > 5.01M && x.ChangePercent.Value <= 7.50M &&
                x.CumulativeVolume.HasValue).OrderByDescending(x => x.Volume).ToList();

            this._dayTradeSymbolQuote.Clear();

            for (int i = 0; i < filteredCommodities.Count; i++)
            {
                YwCommodity c = filteredCommodities[i];
                this._dayTradeSymbolQuote.Add(new DayTradeQuote(ref c));
                this._dispatcher.AddSymbolGridMap(c.Symbol, this._symbolGV, typeof(DayTradeQuote));
                this._dayTradeSymbols.Add(c.Symbol);
            }

            this._symbolGV.DataSource = null;
            this._symbolGV.DataSource = this._dayTradeSymbolQuote;
            this._symbolGV.Refresh(); 
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this._rtdCore.Terminate();
            this._dispatcher.Terminate();
            //this._bufferRearThread.Abort();
            //this._bufferFrontThread.Abort();
            //this._updateEventThread.Abort();
        }

        private void DisplayDayTradeBtnColor(Button clickedBtn)
        {
            this._filterCeilPriceStockBtn.BackColor = Button.DefaultBackColor;
            this._filterFloorPriceStockBtn.BackColor = Button.DefaultBackColor;
            this._filterUpLowBtn.BackColor = Button.DefaultBackColor;
            this._filterUpMidBtn.BackColor = Button.DefaultBackColor;
            this._filterUpMostBtn.BackColor = Button.DefaultBackColor;
            this._filterDownLowBtn.BackColor = Button.DefaultBackColor;
            this._filterDownMidBtn.BackColor = Button.DefaultBackColor;
            this._filterDownMostBtn.BackColor = Button.DefaultBackColor;
            this._filterUpVolMostBtn.BackColor = Button.DefaultBackColor;
            this._filterUpVolMidBtn.BackColor = Button.DefaultBackColor;
            this._filterUpVolMinBtn.BackColor = Button.DefaultBackColor;
            this._filterDownVolMostBtn.BackColor = Button.DefaultBackColor;
            this._filterDownVolMidBtn.BackColor = Button.DefaultBackColor;
            this._filterDownVolMinBtn.BackColor = Button.DefaultBackColor;
            


            clickedBtn.BackColor = Color.LightYellow;

        }

        private void _filterCeilPriceStockBtn_Click(object sender, EventArgs e)
        {
            DisplayDayTradeBtnColor(this._filterCeilPriceStockBtn);
            this._dispatcher.RemoveSymbolGridMap(this._dayTradeSymbols, this._dayTradeStockGV);
            this._dayTradeSymbols.Clear();
            List<YwCommodity> filteredCommodities = this._commodities.Values.Where(x => x.Ceil == x.Price &&
                x.Ceil != "" && x.Ceil != "無資料" &&
                x.Price != "" && x.Price != "無資料" &&
                x.CumulativeVolume.HasValue).OrderByDescending(x => x.ChangePercent).ToList();

            this._dayTradeSymbolQuote.Clear();
            
            for (int i = 0; i < filteredCommodities.Count; i++)
            {
                YwCommodity c = filteredCommodities[i];
                this._dayTradeSymbolQuote.Add(new DayTradeQuote(ref c));
                this._dispatcher.AddSymbolGridMap(c.Symbol, this._dayTradeStockGV, typeof(DayTradeQuote));
                this._dayTradeSymbols.Add(c.Symbol);
            }

            this._dayTradeStockGV.DataSource = null;
            this._dayTradeStockGV.DataSource = this._dayTradeSymbolQuote;
            this._dayTradeStockGV.Refresh();

        }

        private void _filterFloorPriceStockBtn_Click(object sender, EventArgs e)
        {
            DisplayDayTradeBtnColor(this._filterFloorPriceStockBtn);
            this._dispatcher.RemoveSymbolGridMap(this._dayTradeSymbols, this._dayTradeStockGV);
            this._dayTradeSymbols.Clear();
            List<YwCommodity> filteredCommodities = this._commodities.Values.Where(x => x.Floor == x.Price &&
                x.Floor != "" && x.Floor != "無資料" &&
                x.Price != "" && x.Price != "無資料" &&
                x.CumulativeVolume.HasValue).OrderByDescending(x => x.ChangePercent).ToList();

            this._dayTradeSymbolQuote.Clear();

            for (int i = 0; i < filteredCommodities.Count; i++)
            {
                YwCommodity c = filteredCommodities[i];
                this._dayTradeSymbolQuote.Add(new DayTradeQuote(ref c));
                this._dispatcher.AddSymbolGridMap(c.Symbol, this._dayTradeStockGV, typeof(DayTradeQuote));
                this._dayTradeSymbols.Add(c.Symbol);
            }

            this._dayTradeStockGV.DataSource = null;
            this._dayTradeStockGV.DataSource = this._dayTradeSymbolQuote;
            this._dayTradeStockGV.Refresh();            
        }

        private void _filterUpMostBtn_Click(object sender, EventArgs e)
        {
            DisplayDayTradeBtnColor(this._filterUpMostBtn);
            this._dispatcher.RemoveSymbolGridMap(this._dayTradeSymbols, this._dayTradeStockGV);
            this._dayTradeSymbols.Clear();
            List<YwCommodity> filteredCommodities = this._commodities.Values.Where(x => x.ChangePercent.HasValue &&
                x.ChangePercent.Value > 7.50M && x.ChangePercent.Value <= 9.69M &&
                x.CumulativeVolume.HasValue).OrderByDescending(x => x.ChangePercent).ToList();

            this._dayTradeSymbolQuote.Clear();

            for (int i = 0; i < filteredCommodities.Count; i++)
            {
                YwCommodity c = filteredCommodities[i];
                this._dayTradeSymbolQuote.Add(new DayTradeQuote(ref c));
                this._dispatcher.AddSymbolGridMap(c.Symbol, this._dayTradeStockGV, typeof(DayTradeQuote));
                this._dayTradeSymbols.Add(c.Symbol);
            }

            this._dayTradeStockGV.DataSource = null;
            this._dayTradeStockGV.DataSource = this._dayTradeSymbolQuote;
            this._dayTradeStockGV.Refresh(); 

            //this._dayTradeStockGV.DataSource = this._dayTradeQuoteDatas.Where(x => x.ChangePercent.Value > 7.50M && x.ChangePercent.Value <= 9.69M).OrderBy(x => x.Symbol).ToList();
            //this._dayTradeStockGV.Refresh();
        }

        private void _filterUpMidBtn_Click(object sender, EventArgs e)
        {
            DisplayDayTradeBtnColor(this._filterUpMidBtn);
            this._dispatcher.RemoveSymbolGridMap(this._dayTradeSymbols, this._dayTradeStockGV);
            this._dayTradeSymbols.Clear();
            List<YwCommodity> filteredCommodities = this._commodities.Values.Where(x => x.ChangePercent.HasValue &&
                x.ChangePercent.Value > 5.01M && x.ChangePercent.Value <= 7.50M &&
                x.CumulativeVolume.HasValue).OrderByDescending(x => x.ChangePercent).ToList();

            this._dayTradeSymbolQuote.Clear();

            for (int i = 0; i < filteredCommodities.Count; i++)
            {
                YwCommodity c = filteredCommodities[i];
                this._dayTradeSymbolQuote.Add(new DayTradeQuote(ref c));
                this._dispatcher.AddSymbolGridMap(c.Symbol, this._dayTradeStockGV, typeof(DayTradeQuote));
                this._dayTradeSymbols.Add(c.Symbol);
            }

            this._dayTradeStockGV.DataSource = null;
            this._dayTradeStockGV.DataSource = this._dayTradeSymbolQuote;
            this._dayTradeStockGV.Refresh(); 

            //var quoteList = this._dayTradeQuoteDatas.Where(x => x.ChangePercent.Value > 5.01M && x.ChangePercent.Value <= 7.50M).OrderBy(x => x.Symbol).ToList();
            //this._dayTradeGVSymbols.Clear();
            //this._dayTradeGVSymbols.AddRange(quoteList.Select(x => x.Symbol));
            //this._dayTradeStockGV.DataSource = quoteList;
            //this._dayTradeStockGV.Refresh();
        }

        private void _filterUpLowBtn_Click(object sender, EventArgs e)
        {
            DisplayDayTradeBtnColor(this._filterUpLowBtn);
            this._dispatcher.RemoveSymbolGridMap(this._dayTradeSymbols, this._dayTradeStockGV);

            this._dayTradeSymbols.Clear();
            List<YwCommodity> filteredCommodities = this._commodities.Values.Where(x => x.ChangePercent.HasValue &&
                x.ChangePercent.Value > 2.50M && x.ChangePercent.Value <= 5.01M &&
                x.CumulativeVolume.HasValue).OrderByDescending(x => x.ChangePercent).ToList();

            this._dayTradeSymbolQuote.Clear();

            for (int i = 0; i < filteredCommodities.Count; i++)
            {
                YwCommodity c = filteredCommodities[i];
                this._dayTradeSymbolQuote.Add(new DayTradeQuote(ref c));
                this._dispatcher.AddSymbolGridMap(c.Symbol, this._dayTradeStockGV, typeof(DayTradeQuote));
                this._dayTradeSymbols.Add(c.Symbol);
            }

            this._dayTradeStockGV.DataSource = null;
            this._dayTradeStockGV.DataSource = this._dayTradeSymbolQuote;
            this._dayTradeStockGV.Refresh(); 

            //this._dayTradeStockGV.DataSource = this._dayTradeQuoteDatas.Where(x => x.ChangePercent.Value > 2.50M && x.ChangePercent.Value <= 5.01M).OrderBy(x => x.Symbol).ToList();
            //this._dayTradeStockGV.Refresh();
        }

        private void _filterDownMostBtn_Click(object sender, EventArgs e)
        {
            DisplayDayTradeBtnColor(this._filterDownMostBtn);
            this._dispatcher.RemoveSymbolGridMap(this._dayTradeSymbols, this._dayTradeStockGV);

            this._dayTradeSymbols.Clear();
            List<YwCommodity> filteredCommodities = this._commodities.Values.Where(x => x.ChangePercent.HasValue &&
                x.ChangePercent.Value >= -9.69M && x.ChangePercent.Value < -7.51M &&
                x.CumulativeVolume.HasValue).OrderByDescending(x => x.ChangePercent).ToList();

            this._dayTradeSymbolQuote.Clear();

            for (int i = 0; i < filteredCommodities.Count; i++)
            {
                YwCommodity c = filteredCommodities[i];
                this._dayTradeSymbolQuote.Add(new DayTradeQuote(ref c));
                this._dispatcher.AddSymbolGridMap(c.Symbol, this._dayTradeStockGV, typeof(DayTradeQuote));
                this._dayTradeSymbols.Add(c.Symbol);
            }

            this._dayTradeStockGV.DataSource = null;
            this._dayTradeStockGV.DataSource = this._dayTradeSymbolQuote;
            this._dayTradeStockGV.Refresh(); 

            //this._dayTradeStockGV.DataSource = this._dayTradeQuoteDatas.Where(x => x.ChangePercent.Value >= -9.69M && x.ChangePercent.Value < -7.51M).OrderBy(x => x.Symbol).ToList();
            //this._dayTradeStockGV.Refresh();
        }

        private void _filterDownMidBtn_Click(object sender, EventArgs e)
        {
            DisplayDayTradeBtnColor(this._filterDownMidBtn);
            this._dispatcher.RemoveSymbolGridMap(this._dayTradeSymbols, this._dayTradeStockGV);
            this._dayTradeSymbols.Clear();
            List<YwCommodity> filteredCommodities = this._commodities.Values.Where(x => x.ChangePercent.HasValue &&
                x.ChangePercent.Value >= -7.50M && x.ChangePercent.Value < -5.01M &&
                x.CumulativeVolume.HasValue).OrderByDescending(x => x.ChangePercent).ToList();

            this._dayTradeSymbolQuote.Clear();

            for (int i = 0; i < filteredCommodities.Count; i++)
            {
                YwCommodity c = filteredCommodities[i];
                this._dayTradeSymbolQuote.Add(new DayTradeQuote(ref c));
                this._dispatcher.AddSymbolGridMap(c.Symbol, this._dayTradeStockGV, typeof(DayTradeQuote));
                this._dayTradeSymbols.Add(c.Symbol);
            }

            this._dayTradeStockGV.DataSource = null;
            this._dayTradeStockGV.DataSource = this._dayTradeSymbolQuote;
            this._dayTradeStockGV.Refresh(); 

            //this._dayTradeStockGV.DataSource = this._dayTradeQuoteDatas.Where(x => x.ChangePercent.Value >= -7.50M && x.ChangePercent.Value < -5.01M).OrderBy(x => x.Symbol).ToList();
            //this._dayTradeStockGV.Refresh();
        }

        private void _filterDownLowBtn_Click(object sender, EventArgs e)
        {
            DisplayDayTradeBtnColor(this._filterDownLowBtn);
            this._dispatcher.RemoveSymbolGridMap(this._dayTradeSymbols, this._dayTradeStockGV);
            this._dayTradeSymbols.Clear();
            List<YwCommodity> filteredCommodities = this._commodities.Values.Where(x => x.ChangePercent.HasValue &&
                x.ChangePercent.Value >= -5.00M && x.ChangePercent.Value < -2.50M &&
                x.CumulativeVolume.HasValue).OrderByDescending(x => x.ChangePercent).ToList();

            this._dayTradeSymbolQuote.Clear();

            for (int i = 0; i < filteredCommodities.Count; i++)
            {
                YwCommodity c = filteredCommodities[i];
                this._dayTradeSymbolQuote.Add(new DayTradeQuote(ref c));
                this._dispatcher.AddSymbolGridMap(c.Symbol, this._dayTradeStockGV, typeof(DayTradeQuote));
                this._dayTradeSymbols.Add(c.Symbol);
            }

            this._dayTradeStockGV.DataSource = null;
            this._dayTradeStockGV.DataSource = this._dayTradeSymbolQuote;
            this._dayTradeStockGV.Refresh(); 

            //this._dayTradeStockGV.DataSource = this._dayTradeQuoteDatas.Where(x => x.ChangePercent.Value >= -5.00M && x.ChangePercent.Value < -2.50M).OrderBy(x => x.Symbol).ToList();
            //this._dayTradeStockGV.Refresh();
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

            this._dispatcher.RemoveSymbolGridMap(this._selectedIndustrySymbols, this._filteredIndustryGV);
            this._selectedIndustrySymbols.Clear();            
            List<string> symbolCodes = this._rep.Query<Symbol>(x => symbolIdList.Contains(x.Id)).Select(x => x.Code).ToList();
            this._selectedIndustrySymbols.AddRange(symbolCodes);
            List<YwCommodity> filteredCommodities = this._commodities.Values.Where(x => symbolCodes.Contains(x.Symbol)).OrderByDescending(x => x.ChangePercent).ToList();
            this._selectedIndustryQuote.Clear();
            for(int i = 0 ;i < filteredCommodities.Count;i ++)
            {
                YwCommodity c = filteredCommodities[i];
                this._selectedIndustryQuote.Add(new IndustryQuote(ref c));
                this._dispatcher.AddSymbolGridMap(c.Symbol, this._filteredIndustryGV, typeof(IndustryQuote));
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

            this._dispatcher.RemoveSymbolGridMap(this._selectedBizGroupSymbols, this._filteredBizGroupGV);
            this._selectedBizGroupSymbols.Clear();
            List<string> symbolCodes = this._rep.Query<Symbol>(x => symbolIdList.Contains(x.Id)).Select(x => x.Code).ToList();
            this._selectedBizGroupSymbols.AddRange(symbolCodes);

            List<YwCommodity> filteredCommodities = this._commodities.Values.Where(x => symbolCodes.Contains(x.Symbol)).OrderByDescending(x => x.ChangePercent).ToList();
            this._selectedBizGroupQuote.Clear();
            for (int i = 0; i < filteredCommodities.Count; i++)
            {
                YwCommodity c = filteredCommodities[i];
                this._selectedBizGroupQuote.Add(new BizGroupQuote(ref c));
                this._dispatcher.AddSymbolGridMap(c.Symbol, this._filteredBizGroupGV, typeof(BizGroupQuote));
            }           
            
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

            this._dispatcher.RemoveSymbolGridMap(this._selectedConceptSymbols, this._filteredConceptGV);
            this._selectedConceptSymbols.Clear();
            List<string> symbolCodes = this._rep.Query<Symbol>(x => symbolIdList.Contains(x.Id)).Select(x => x.Code).ToList();
            this._selectedConceptSymbols.AddRange(symbolCodes);

            List<YwCommodity> filteredCommodities = this._commodities.Values.Where(x => symbolCodes.Contains(x.Symbol)).OrderByDescending(x => x.ChangePercent).ToList();
            this._selectedConceptQuote.Clear();
            for (int i = 0; i < filteredCommodities.Count; i++)
            {
                YwCommodity c = filteredCommodities[i];
                this._selectedConceptQuote.Add(new ConceptQuote(ref c));
                this._dispatcher.AddSymbolGridMap(c.Symbol, this._filteredConceptGV, typeof(ConceptQuote));
            }                  

            this._filteredConceptGV.DataSource = null;
            this._filteredConceptGV.DataSource = this._selectedConceptQuote;
            this._filteredConceptGV.Refresh();
        }

        private void _filteredIndustryGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this._filteredIndustryGV.DataSource == null)
            {
                //e.FormattingApplied = true;
                return;
            }
            if (this._filteredIndustryGV.Columns[e.ColumnIndex].Name == "Change")
            {
                decimal upOrDown = Convert.ToDecimal(this._filteredIndustryGV.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                if (upOrDown > 0)
                {
                    this._filteredIndustryGV.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    //e.FormattingApplied = true;
                    return;
                }
                if (upOrDown < 0)
                {
                    this._filteredIndustryGV.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Green;
                    //e.FormattingApplied = true;
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
                //e.FormattingApplied = true;
                return;
            }
            if (this._filteredBizGroupGV.Columns[e.ColumnIndex].Name == "Change")
            {
                decimal upOrDown = Convert.ToDecimal(this._filteredBizGroupGV.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                if (upOrDown > 0)
                {
                    this._filteredBizGroupGV.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    //e.FormattingApplied = true;
                    return;
                }
                if (upOrDown < 0)
                {
                    this._filteredBizGroupGV.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Green;
                    //e.FormattingApplied = true;
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
                //e.FormattingApplied = true;
                return;
            }
            if (this._filteredConceptGV.Columns[e.ColumnIndex].Name == "Change")
            {
                decimal upOrDown = Convert.ToDecimal(this._filteredConceptGV.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                if (upOrDown > 0)
                {
                    this._filteredConceptGV.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    //e.FormattingApplied = true;
                    return;
                }
                if (upOrDown < 0)
                {
                    this._filteredConceptGV.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Green;
                    //e.FormattingApplied = true;
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

        private void _pointerIndexSelectCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            string pointerIndexCode = ((PointerIndex)this._pointerIndexSelectCmb.SelectedItem).Code;

            var selectedPointerIndex = this._rep.Query<PointerIndex>(x => x.Code == pointerIndexCode);
            var allPointerIndexSymbolMap = this._rep.FetchAll<PointerIndexSymbol>();
            var symbolIdList = (from pointerIndexes in selectedPointerIndex
                                join symbols in allPointerIndexSymbolMap
                                on pointerIndexes.Id equals symbols.PointerIndexId
                                select symbols.SymbolId).ToList();

            this._dispatcher.RemoveSymbolGridMap(this._selectedPointerIndexSymbols, this._filteredPointerIndexGV);
            this._selectedPointerIndexSymbols.Clear();
            List<string> symbolCodes = this._rep.Query<Symbol>(x => symbolIdList.Contains(x.Id)).Select(x => x.Code).ToList();
            this._selectedPointerIndexSymbols.AddRange(symbolCodes);
            List<YwCommodity> filteredCommodities = this._commodities.Values.Where(x => symbolCodes.Contains(x.Symbol)).OrderByDescending(x => x.ChangePercent).ToList();
            this._selectedPointerIndexQuote.Clear();
            for (int i = 0; i < filteredCommodities.Count; i++)
            {
                YwCommodity c = filteredCommodities[i];
                this._selectedPointerIndexQuote.Add(new PointerIndexQuote(ref c));
                this._dispatcher.AddSymbolGridMap(c.Symbol, this._filteredPointerIndexGV, typeof(PointerIndexQuote));
            }

            this._filteredPointerIndexGV.DataSource = null;
            this._filteredPointerIndexGV.DataSource = this._selectedPointerIndexQuote;
            this._filteredPointerIndexGV.Refresh();
        }

        private void _filteredPointerIndexGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this._filteredPointerIndexGV.DataSource == null)
            {
                //e.FormattingApplied = true;
                return;
            }
            if (this._filteredPointerIndexGV.Columns[e.ColumnIndex].Name == "Change")
            {
                decimal upOrDown = Convert.ToDecimal(this._filteredPointerIndexGV.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                if (upOrDown > 0)
                {
                    this._filteredPointerIndexGV.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    //e.FormattingApplied = true;
                    return;
                }
                if (upOrDown < 0)
                {
                    this._filteredPointerIndexGV.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Green;
                    //e.FormattingApplied = true;
                    return;
                }
                else
                {
                    return;
                }
            }
        }

        private void _symbolCatQueryBtn_Click(object sender, EventArgs e)
        {
            string symbolCodeOrName = this._symbolCatQueryTxt.Text.Trim();
            Symbol symbolObj =
                this._rep.DefaultOne<Symbol>(x => x.Code == symbolCodeOrName || x.SymbolName == symbolCodeOrName);

            this._symbolNameCatLbl.Text = symbolObj.SymbolName;
            this._symbolCatQueryTxt.Text = symbolObj.Code;

            this._symbolCatPILB.DataSource = null;
            var allPointerIndexName = from sym in this._rep.Query<PointerIndexSymbol>(x => x.SymbolId == symbolObj.Id)
                                      join pi in this._rep.FetchAll<PointerIndex>()
                                      on sym.PointerIndexId equals pi.Id
                                      select pi.PointerName;

            this._symbolCatPILB.DataSource = allPointerIndexName.ToList();
            this._symbolCatPILB.Refresh();

            this._symbolCatIndustryLB.DataSource = null;
            var allIndustryName = from sym in this._rep.Query<IndustrySymbol>(x => x.SymbolId == symbolObj.Id)
                                      join ind in this._rep.FetchAll<Industry>()
                                      on sym.IndustryId equals ind.Id
                                      select ind.IndustryName;

            this._symbolCatIndustryLB.DataSource = allIndustryName.ToList();
            this._symbolCatIndustryLB.Refresh();


            this._symbolCatBizGroupLB.DataSource = null;
            var allBizGroupName = from sym in this._rep.Query<BizGroupSymbol>(x => x.SymbolId == symbolObj.Id)
                                      join biz in this._rep.FetchAll<BizGroup>()
                                      on sym.BizGroupId equals biz.Id
                                      select biz.GroupName;

            this._symbolCatBizGroupLB.DataSource = allBizGroupName.ToList();
            this._symbolCatBizGroupLB.Refresh();


            this._symbolCatConceptLB.DataSource = null;
            var allConceptName = from sym in this._rep.Query<ConceptSymbol>(x => x.SymbolId == symbolObj.Id)
                                      join con in this._rep.FetchAll<Concept>()
                                      on sym.ConceptId equals con.Id
                                      select con.ConceptName;

            this._symbolCatConceptLB.DataSource = allConceptName.ToList();
            this._symbolCatConceptLB.Refresh();
        }

        private void _symbolCatClearBtn_Click(object sender, EventArgs e)
        {
            this._symbolNameCatLbl.Text = "";
            this._symbolCatQueryTxt.Text = "";
            this._symbolCatConceptLB.DataSource = null;
            this._symbolCatBizGroupLB.DataSource = null;
            this._symbolCatIndustryLB.DataSource = null;
            this._symbolCatPILB.DataSource = null;
        }

        private void _symbolCatPIAddBtn_Click(object sender, EventArgs e)
        {            
            string symbolCodeOrName = this._symbolCatQueryTxt.Text.Trim();
            Symbol symbolObj =
                this._rep.DefaultOne<Symbol>(x => x.Code == symbolCodeOrName || x.SymbolName == symbolCodeOrName);

            PointerIndex piObj = this._symbolCatPICmb.SelectedItem as PointerIndex;            

            if (MessageBox.Show(string.Format("確定要把 {0} 跟指標類 [{1}] 作關連?", symbolObj.SymbolName, piObj.PointerName), "Warning", MessageBoxButtons.YesNoCancel) != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }

            PointerIndexSymbol existRelation = this._rep.DefaultOne<PointerIndexSymbol>(x => x.PointerIndexId == piObj.Id && x.SymbolId == symbolObj.Id);
            if (existRelation == null)
            {
                this._rep.Insert<PointerIndexSymbol>(new PointerIndexSymbol
                {
                    CreateTime = DateTime.Now,
                    PointerIndexId = piObj.Id,
                    SymbolId = symbolObj.Id
                });
                this._rep.Commit();
            }

            MessageBox.Show("新增完畢");

            this._symbolCatPILB.DataSource = null;
            var allPointerIndexName = from sym in this._rep.Query<PointerIndexSymbol>(x => x.SymbolId == symbolObj.Id)
                                      join pi in this._rep.FetchAll<PointerIndex>()
                                      on sym.PointerIndexId equals pi.Id
                                      select pi.PointerName;

            this._symbolCatPILB.DataSource = allPointerIndexName.ToList();
            this._symbolCatPILB.Refresh();
        }

        private void _symbolCatPIDeleteBtn_Click(object sender, EventArgs e)
        {
            string symbolCodeOrName = this._symbolCatQueryTxt.Text.Trim();
            Symbol symbolObj =
                this._rep.DefaultOne<Symbol>(x => x.Code == symbolCodeOrName || x.SymbolName == symbolCodeOrName);

            string pointerIndexName = this._symbolCatPILB.SelectedItem as string;

            PointerIndex piObj = this._rep.DefaultOne<PointerIndex>(x => x.PointerName == pointerIndexName);

            if (MessageBox.Show(string.Format("確定要把 {0} 跟指標類 [{1}] 的關連刪除?", symbolObj.SymbolName, piObj.PointerName), "Warning", MessageBoxButtons.YesNoCancel) != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }

            PointerIndexSymbol existRelation = this._rep.DefaultOne<PointerIndexSymbol>(x => x.PointerIndexId == piObj.Id && x.SymbolId == symbolObj.Id);
            if (existRelation != null)
            {
                this._rep.Delete<PointerIndexSymbol>(existRelation);
                this._rep.Commit();
            }

            MessageBox.Show("刪除完畢");
            //重新reload

            this._symbolCatPILB.DataSource = null;
            var allPointerIndexName = from sym in this._rep.Query<PointerIndexSymbol>(x => x.SymbolId == symbolObj.Id)
                                      join pi in this._rep.FetchAll<PointerIndex>()
                                      on sym.PointerIndexId equals pi.Id
                                      select pi.PointerName;

            this._symbolCatPILB.DataSource = allPointerIndexName.ToList();
            this._symbolCatPILB.Refresh();
        }

        private void _symbolCatIndustryAddBtn_Click(object sender, EventArgs e)
        {
            string symbolCodeOrName = this._symbolCatQueryTxt.Text.Trim();
            Symbol symbolObj =
                this._rep.DefaultOne<Symbol>(x => x.Code == symbolCodeOrName || x.SymbolName == symbolCodeOrName);

            Industry indObj = this._symbolCatIndustryCmb.SelectedItem as Industry;

            if (MessageBox.Show(string.Format("確定要把 {0} 跟產業類 [{1}] 作關連?", symbolObj.SymbolName, indObj.IndustryName), "Warning", MessageBoxButtons.YesNoCancel) != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }

            IndustrySymbol existRelation = this._rep.DefaultOne<IndustrySymbol>(x => x.IndustryId == indObj.Id && x.SymbolId == symbolObj.Id);
            if (existRelation == null)
            {
                this._rep.Insert<IndustrySymbol>(new IndustrySymbol
                {
                    CreateTime = DateTime.Now,
                    IndustryId = indObj.Id,
                    SymbolId = symbolObj.Id
                });
                this._rep.Commit();
            }

            MessageBox.Show("新增完畢");

            this._symbolCatIndustryLB.DataSource = null;
            var allIndustryName = from sym in this._rep.Query<IndustrySymbol>(x => x.SymbolId == symbolObj.Id)
                                  join ind in this._rep.FetchAll<Industry>()
                                      on sym.IndustryId equals ind.Id
                                  select ind.IndustryName;

            this._symbolCatIndustryLB.DataSource = allIndustryName.ToList();
            this._symbolCatIndustryLB.Refresh();
        }

        private void _symbolCatIndustryDeleteBtn_Click(object sender, EventArgs e)
        {
            string symbolCodeOrName = this._symbolCatQueryTxt.Text.Trim();
            Symbol symbolObj =
                this._rep.DefaultOne<Symbol>(x => x.Code == symbolCodeOrName || x.SymbolName == symbolCodeOrName);

            string industryName = this._symbolCatIndustryLB.SelectedItem as string;

            Industry indObj = this._rep.DefaultOne<Industry>(x => x.IndustryName == industryName);            

            if (MessageBox.Show(string.Format("確定要把 {0} 跟產業類 [{1}] 的關連刪除?", symbolObj.SymbolName, indObj.IndustryName), "Warning", MessageBoxButtons.YesNoCancel) != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }

            IndustrySymbol existRelation = this._rep.DefaultOne<IndustrySymbol>(x => x.IndustryId == indObj.Id && x.SymbolId == symbolObj.Id);
            if (existRelation != null)
            {
                this._rep.Delete<IndustrySymbol>(existRelation);
                this._rep.Commit();
            }

            MessageBox.Show("刪除完畢");
            //重新reload

            this._symbolCatIndustryLB.DataSource = null;
            var allIndustryName = from sym in this._rep.Query<IndustrySymbol>(x => x.SymbolId == symbolObj.Id)
                                      join ind in this._rep.FetchAll<Industry>()
                                      on sym.IndustryId equals ind.Id
                                      select ind.IndustryName;

            this._symbolCatIndustryLB.DataSource = allIndustryName.ToList();
            this._symbolCatIndustryLB.Refresh();
        }

        private void _symbolCatBizGroupDeleteBtn_Click(object sender, EventArgs e)
        {
            string symbolCodeOrName = this._symbolCatQueryTxt.Text.Trim();
            Symbol symbolObj =
                this._rep.DefaultOne<Symbol>(x => x.Code == symbolCodeOrName || x.SymbolName == symbolCodeOrName);            

            string grpName = this._symbolCatBizGroupLB.SelectedItem as string;

            BizGroup grpObj = this._rep.DefaultOne<BizGroup>(x => x.GroupName == grpName);

            if (MessageBox.Show(string.Format("確定要把 {0} 跟集團類 [{1}] 的關連刪除?", symbolObj.SymbolName, grpObj.GroupName), "Warning", MessageBoxButtons.YesNoCancel) != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }

            BizGroupSymbol existRelation = this._rep.DefaultOne<BizGroupSymbol>(x => x.BizGroupId == grpObj.Id && x.SymbolId == symbolObj.Id);
            if (existRelation != null)
            {
                this._rep.Delete<BizGroupSymbol>(existRelation);
                this._rep.Commit();
            }

            MessageBox.Show("刪除完畢");
            //重新reload

            this._symbolCatBizGroupLB.DataSource = null;
            var allBizGroupName = from sym in this._rep.Query<BizGroupSymbol>(x => x.SymbolId == symbolObj.Id)
                                  join grp in this._rep.FetchAll<BizGroup>()
                                  on sym.BizGroupId equals grp.Id
                                  select grp.GroupName;

            this._symbolCatBizGroupLB.DataSource = allBizGroupName.ToList();
            this._symbolCatBizGroupLB.Refresh();
        }

        private void _symbolCatBizGroupAddBtn_Click(object sender, EventArgs e)
        {
            string symbolCodeOrName = this._symbolCatQueryTxt.Text.Trim();
            Symbol symbolObj =
                this._rep.DefaultOne<Symbol>(x => x.Code == symbolCodeOrName || x.SymbolName == symbolCodeOrName);

            BizGroup grpObj = this._symbolCatBizGroupCmb.SelectedItem as BizGroup;

            if (MessageBox.Show(string.Format("確定要把 {0} 跟集團類 [{1}] 作關連?", symbolObj.SymbolName, grpObj.GroupName), "Warning", MessageBoxButtons.YesNoCancel) != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }

            BizGroupSymbol existRelation = this._rep.DefaultOne<BizGroupSymbol>(x => x.BizGroupId == grpObj.Id && x.SymbolId == symbolObj.Id);
            if (existRelation == null)
            {
                this._rep.Insert<BizGroupSymbol>(new BizGroupSymbol
                {
                    CreateTime = DateTime.Now,
                    BizGroupId = grpObj.Id,
                    SymbolId = symbolObj.Id
                });
                this._rep.Commit();
            }

            MessageBox.Show("新增完畢");

            this._symbolCatBizGroupLB.DataSource = null;
            var allBizGroupName = from sym in this._rep.Query<BizGroupSymbol>(x => x.SymbolId == symbolObj.Id)
                                  join grp in this._rep.FetchAll<BizGroup>()
                                      on sym.BizGroupId equals grp.Id
                                  select grp.GroupName;

            this._symbolCatBizGroupLB.DataSource = allBizGroupName.ToList();
            this._symbolCatBizGroupLB.Refresh();
        }

        private void _symbolCatConceptAddBtn_Click(object sender, EventArgs e)
        {
            string symbolCodeOrName = this._symbolCatQueryTxt.Text.Trim();
            Symbol symbolObj =
                this._rep.DefaultOne<Symbol>(x => x.Code == symbolCodeOrName || x.SymbolName == symbolCodeOrName);

            Concept conObj = this._symbolCatConceptCmb.SelectedItem as Concept;

            if (MessageBox.Show(string.Format("確定要把 {0} 跟概念類 [{1}] 作關連?", symbolObj.SymbolName, conObj.ConceptName), "Warning", MessageBoxButtons.YesNoCancel) != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }

            ConceptSymbol existRelation = this._rep.DefaultOne<ConceptSymbol>(x => x.ConceptId == conObj.Id && x.SymbolId == symbolObj.Id);
            if (existRelation == null)
            {
                this._rep.Insert<ConceptSymbol>(new ConceptSymbol
                {
                    CreateTime = DateTime.Now,
                    ConceptId = conObj.Id,
                    SymbolId = symbolObj.Id
                });
                this._rep.Commit();
            }

            MessageBox.Show("新增完畢");

            this._symbolCatConceptLB.DataSource = null;
            var allConceptName = from sym in this._rep.Query<ConceptSymbol>(x => x.SymbolId == symbolObj.Id)
                                  join con in this._rep.FetchAll<Concept>()
                                      on sym.ConceptId equals con.Id
                                  select con.ConceptName;

            this._symbolCatConceptLB.DataSource = allConceptName.ToList();
            this._symbolCatConceptLB.Refresh();
        }

        private void _symbolCatConceptDeleteBtn_Click(object sender, EventArgs e)
        {
            string symbolCodeOrName = this._symbolCatQueryTxt.Text.Trim();
            Symbol symbolObj =
                this._rep.DefaultOne<Symbol>(x => x.Code == symbolCodeOrName || x.SymbolName == symbolCodeOrName);

            string conName = this._symbolCatConceptLB.SelectedItem as string;

            Concept conObj = this._rep.DefaultOne<Concept>(x => x.ConceptName == conName);            

            if (MessageBox.Show(string.Format("確定要把 {0} 跟概念類 [{1}] 的關連刪除?", symbolObj.SymbolName, conObj.ConceptName), "Warning", MessageBoxButtons.YesNoCancel) != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }

            ConceptSymbol existRelation = this._rep.DefaultOne<ConceptSymbol>(x => x.ConceptId == conObj.Id && x.SymbolId == symbolObj.Id);
            if (existRelation != null)
            {
                this._rep.Delete<ConceptSymbol>(existRelation);
                this._rep.Commit();
            }

            MessageBox.Show("刪除完畢");
            //重新reload

            this._symbolCatConceptLB.DataSource = null;
            var allConceptName = from sym in this._rep.Query<ConceptSymbol>(x => x.SymbolId == symbolObj.Id)
                                  join con in this._rep.FetchAll<Concept>()
                                  on sym.ConceptId equals con.Id
                                  select con.ConceptName;

            this._symbolCatConceptLB.DataSource = allConceptName.ToList();
            this._symbolCatConceptLB.Refresh();
        }

        private void _symbolCatPIRdoBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (this._symbolCatPIRdoBtn.Checked == false)
            { return; }

            this._symbolCategoryCmb.DataSource = null;
            var pointerIndexList = this._rep.FetchAll<PointerIndex>().ToList();            

            this._symbolCategoryCmb.ValueMember = "Code";
            this._symbolCategoryCmb.DisplayMember = "PointerName";
            this._symbolCategoryCmb.DataSource = pointerIndexList;
        }

        private void _symbolCatIndustryRdoBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (this._symbolCatIndustryRdoBtn.Checked == false)
            { return; }

            this._symbolCategoryCmb.DataSource = null;
            var industryList = this._rep.FetchAll<Industry>().ToList();

            this._symbolCategoryCmb.ValueMember = "Code";
            this._symbolCategoryCmb.DisplayMember = "IndustryName";
            this._symbolCategoryCmb.DataSource = industryList;  
        }

        private void _symbolCatBizGroupRdoBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (this._symbolCatBizGroupRdoBtn.Checked == false)
            { return; }

            this._symbolCategoryCmb.DataSource = null;
            var bizGroupList = this._rep.FetchAll<BizGroup>().ToList();

            this._symbolCategoryCmb.ValueMember = "Code";
            this._symbolCategoryCmb.DisplayMember = "GroupName";
            this._symbolCategoryCmb.DataSource = bizGroupList;
        }

        private void _symbolCatConceptRdoBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (this._symbolCatConceptRdoBtn.Checked == false)
            { return; }


            this._symbolCategoryCmb.DataSource = null;
            var conceptList = this._rep.FetchAll<Concept>().ToList();

            this._symbolCategoryCmb.ValueMember = "Code";
            this._symbolCategoryCmb.DisplayMember = "ConceptName";
            this._symbolCategoryCmb.DataSource = conceptList;
        }

        private void _symbolCatClearRdoBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (this._symbolCatClearRdoBtn.Checked == false)
            { return; }

            this._symbolCategoryCmb.DataSource = null;
        }

        private void _symbolCategoryCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadCategorySymbolData();                     
        }

        private void _categorySymbolAddBtn_Click(object sender, EventArgs e)
        {
            if (this._symbolCatClearRdoBtn.Checked)
            { return; }

            string symbolCode = this._categorySymbolCodeTxt.Text.Trim();
            string symbolName = this._categorySymbolNameTxt.Text.Trim();
            Symbol symbolExist =
                    this._rep.DefaultOne<Symbol>(x => x.Code == symbolCode && x.SymbolName == symbolName);
            DialogResult diaResult;
            if (this._symbolCatPIRdoBtn.Checked)
            {
                PointerIndex pi = this._symbolCategoryCmb.SelectedItem as PointerIndex;
                diaResult = MessageBox.Show("", "Warning", MessageBoxButtons.YesNoCancel);
                if (diaResult != System.Windows.Forms.DialogResult.Yes) 
                { return; }
                
                if (symbolExist != null)
                {
                    this._rep.Insert<PointerIndexSymbol>(new PointerIndexSymbol
                    {
                        CreateTime = DateTime.Now,
                        PointerIndexId = pi.Id,
                        SymbolId = symbolExist.Id
                    });
                    this._rep.Commit();
                }
            }

            if (this._symbolCatIndustryRdoBtn.Checked)
            {
                Industry ind = this._symbolCategoryCmb.SelectedItem as Industry;
                diaResult = MessageBox.Show("", "Warning", MessageBoxButtons.YesNoCancel);
                if (diaResult != System.Windows.Forms.DialogResult.Yes)
                { return; }

                if (symbolExist != null)
                {
                    this._rep.Insert<IndustrySymbol>(new IndustrySymbol
                    {
                        CreateTime = DateTime.Now,
                        IndustryId = ind.Id,
                        SymbolId = symbolExist.Id
                    });
                    this._rep.Commit();
                }
            }

            if (this._symbolCatBizGroupRdoBtn.Checked)
            {
                BizGroup grp = this._symbolCategoryCmb.SelectedItem as BizGroup;
                diaResult = MessageBox.Show("", "Warning", MessageBoxButtons.YesNoCancel);
                if (diaResult != System.Windows.Forms.DialogResult.Yes)
                { return; }

                if (symbolExist != null)
                {
                    this._rep.Insert<BizGroupSymbol>(new BizGroupSymbol
                    {
                        CreateTime = DateTime.Now,
                        BizGroupId = grp.Id,
                        SymbolId = symbolExist.Id
                    });
                    this._rep.Commit();
                }
            }

            if (this._symbolCatConceptRdoBtn.Checked)
            {
                Concept con = this._symbolCategoryCmb.SelectedItem as Concept;
                diaResult = MessageBox.Show("", "Warning", MessageBoxButtons.YesNoCancel);
                if (diaResult != System.Windows.Forms.DialogResult.Yes)
                { return; }

                if (symbolExist != null)
                {
                    this._rep.Insert<ConceptSymbol>(new ConceptSymbol
                    {
                        CreateTime = DateTime.Now,
                        ConceptId = con.Id,
                        SymbolId = symbolExist.Id
                    });
                    this._rep.Commit();
                }
            }

            ReloadCategorySymbolData();

        }

        private void ReloadCategorySymbolData()
        {
            if (this._symbolCatClearRdoBtn.Checked)
            {
                this._categorySymbolLV.Items.Clear();
                return;
            }
            IQueryable<Symbol> filterSymbols = null;
            List<int> symbolIds = null;
            if (this._symbolCatPIRdoBtn.Checked)
            {
                PointerIndex obj = this._symbolCategoryCmb.SelectedItem as PointerIndex;
                symbolIds =
                    this._rep.Query<PointerIndexSymbol>(x => x.PointerIndexId == obj.Id).Select(x => x.SymbolId).ToList();
            }

            if (this._symbolCatIndustryRdoBtn.Checked)
            {
                Industry obj = this._symbolCategoryCmb.SelectedItem as Industry;
                symbolIds =
                    this._rep.Query<IndustrySymbol>(x => x.IndustryId == obj.Id).Select(x => x.SymbolId).ToList();
            }

            if (this._symbolCatBizGroupRdoBtn.Checked)
            {
                BizGroup obj = this._symbolCategoryCmb.SelectedItem as BizGroup;
                symbolIds =
                    this._rep.Query<BizGroupSymbol>(x => x.BizGroupId == obj.Id).Select(x => x.SymbolId).ToList();
            }

            if (this._symbolCatConceptRdoBtn.Checked)
            {
                Concept obj = this._symbolCategoryCmb.SelectedItem as Concept;
                symbolIds =
                    this._rep.Query<ConceptSymbol>(x => x.ConceptId == obj.Id).Select(x => x.SymbolId).ToList();
            }


            filterSymbols = this._rep.Query<Symbol>(x => symbolIds.Contains(x.Id));
            if (filterSymbols != null)
            {
                this._categorySymbolLV.Clear();
                this._categorySymbolLV.Columns.Add("股票代碼");
                this._categorySymbolLV.Columns.Add("名稱");
                foreach (var s in filterSymbols)
                {
                    ListViewItem lvi = new ListViewItem(new string[] { s.Code, s.SymbolName });
                    this._categorySymbolLV.Items.Add(lvi);
                }
                this._categorySymbolLV.Refresh();
            }     
        }

        private void _categorySymbolDeleteBtn_Click(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection  items = this._categorySymbolLV.SelectedItems;

            Symbol selectedSymbol = null;
            string symbolCode = "";

            if (items.Count > 0)
            {
                symbolCode = items[0].Text;                
            }

            if (string.IsNullOrEmpty(symbolCode))
            { return; }

            selectedSymbol = this._rep.DefaultOne<Symbol>(x => x.Code == symbolCode);

            
            if (this._symbolCatPIRdoBtn.Checked)
            {
                PointerIndex obj = this._symbolCategoryCmb.SelectedItem as PointerIndex;
                PointerIndexSymbol relation=
                    this._rep.DefaultOne<PointerIndexSymbol>(x => x.SymbolId == selectedSymbol.Id && x.PointerIndexId == obj.Id);
                if (relation != null)
                {
                    this._rep.Delete<PointerIndexSymbol>(relation);
                }
                this._rep.Commit();                
            }

            if (this._symbolCatIndustryRdoBtn.Checked)
            {
                Industry obj = this._symbolCategoryCmb.SelectedItem as Industry;
                IndustrySymbol relation =
                    this._rep.DefaultOne<IndustrySymbol>(x => x.SymbolId == selectedSymbol.Id && x.IndustryId == obj.Id);
                if (relation != null)
                {
                    this._rep.Delete<IndustrySymbol>(relation);
                }
                this._rep.Commit();
            }

            if (this._symbolCatBizGroupRdoBtn.Checked)
            {
                BizGroup obj = this._symbolCategoryCmb.SelectedItem as BizGroup;
                BizGroupSymbol relation =
                    this._rep.DefaultOne<BizGroupSymbol>(x => x.SymbolId == selectedSymbol.Id && x.BizGroupId == obj.Id);
                if (relation != null)
                {
                    this._rep.Delete<BizGroupSymbol>(relation);
                }
                this._rep.Commit();
            }

            if (this._symbolCatConceptRdoBtn.Checked)
            {
                Concept obj = this._symbolCategoryCmb.SelectedItem as Concept;
                ConceptSymbol relation =
                    this._rep.DefaultOne<ConceptSymbol>(x => x.SymbolId == selectedSymbol.Id && x.ConceptId == obj.Id);
                if (relation != null)
                {
                    this._rep.Delete<ConceptSymbol>(relation);
                }
                this._rep.Commit();
            }

            ReloadCategorySymbolData();
        }

        private void _oneSymbolQueryBtn_Click(object sender, EventArgs e)
        {
            QueryOneSymbol();                 
        }

        private void QueryOneSymbol()
        {
            ResetSymbolQueryData();

            string codeOrName = this._oneSymbolQueryTxt.Text.Trim();
            Symbol symbolObj =
                this._rep.DefaultOne<Symbol>(x => x.Code == codeOrName || x.SymbolName == codeOrName);

            if (symbolObj == null)
            { return; }

            this._dispatcher.RemoveSymbolGridMap(this._queryOneSymbols, this._oneSymbolQueryGV);
            this._queryOneSymbols.Clear();
            this._queryOneSymbols.Add(symbolObj.Code);

            List<YwCommodity> filteredCommodities = this._commodities.Values.Where(x => x.Symbol == symbolObj.Code).ToList();
            this._queryOneSymbolQuote.Clear();
            for (int i = 0; i < filteredCommodities.Count; i++)
            {
                YwCommodity c = filteredCommodities[i];
                this._queryOneSymbolQuote.Add(new OneSymbolQuote(ref c));
                this._dispatcher.AddSymbolGridMap(c.Symbol, this._oneSymbolQueryGV, typeof(OneSymbolQuote));
            }

            this._oneSymbolQueryGV.DataSource = null;
            this._oneSymbolQueryGV.DataSource = this._queryOneSymbolQuote;
            this._oneSymbolQueryGV.Refresh();

            this._oneSymbolPILB.DataSource = null;
            var allPointerIndex = from sym in this._rep.Query<PointerIndexSymbol>(x => x.SymbolId == symbolObj.Id)
                                  join pi in this._rep.FetchAll<PointerIndex>()
                                  on sym.PointerIndexId equals pi.Id
                                  select pi;

            this._oneSymbolPILB.ValueMember = "Code";
            this._oneSymbolPILB.DisplayMember = "PointerName";
            this._oneSymbolPILB.DataSource = allPointerIndex.ToList();

            this._oneSymbolIndustryLB.DataSource = null;
            var allIndustry = from sym in this._rep.Query<IndustrySymbol>(x => x.SymbolId == symbolObj.Id)
                              join ind in this._rep.FetchAll<Industry>()
                              on sym.IndustryId equals ind.Id
                              select ind;

            this._oneSymbolIndustryLB.ValueMember = "Code";
            this._oneSymbolIndustryLB.DisplayMember = "IndustryName";
            this._oneSymbolIndustryLB.DataSource = allIndustry.ToList();

            this._oneSymbolBizGroupLB.DataSource = null;
            var allBizGroup = from sym in this._rep.Query<BizGroupSymbol>(x => x.SymbolId == symbolObj.Id)
                              join biz in this._rep.FetchAll<BizGroup>()
                              on sym.BizGroupId equals biz.Id
                              select biz;

            this._oneSymbolBizGroupLB.ValueMember = "Code";
            this._oneSymbolBizGroupLB.DisplayMember = "GroupName";
            this._oneSymbolBizGroupLB.DataSource = allBizGroup.ToList();

            this._oneSymbolConceptLB.DataSource = null;
            var allConcept = from sym in this._rep.Query<ConceptSymbol>(x => x.SymbolId == symbolObj.Id)
                             join con in this._rep.FetchAll<Concept>()
                             on sym.ConceptId equals con.Id
                             select con;

            this._oneSymbolConceptLB.ValueMember = "Code";
            this._oneSymbolConceptLB.DisplayMember = "ConceptName";
            this._oneSymbolConceptLB.DataSource = allConcept.ToList(); 
        }

        private void _oneSymbolQueryGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this._oneSymbolQueryGV.DataSource == null)
            {
                //e.FormattingApplied = true;
                return;
            }
            if (this._oneSymbolQueryGV.Columns[e.ColumnIndex].Name == "Change")
            {
                decimal upOrDown = Convert.ToDecimal(this._oneSymbolQueryGV.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                if (upOrDown > 0)
                {
                    this._oneSymbolQueryGV.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    //e.FormattingApplied = true;
                    return;
                }
                if (upOrDown < 0)
                {
                    this._oneSymbolQueryGV.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Green;
                    //e.FormattingApplied = true;
                    return;
                }
                else
                {
                    return;
                }
            }
        }

        private void _filterUpVolMostBtn_Click(object sender, EventArgs e)
        {
            DisplayDayTradeBtnColor(this._filterUpVolMostBtn);
            this._dispatcher.RemoveSymbolGridMap(this._dayTradeSymbols, this._dayTradeStockGV);
            this._dayTradeSymbols.Clear();
            List<YwCommodity> filteredCommodities = this._commodities.Values.Where(x => x.ChangePercent.HasValue &&
                x.ChangePercent.Value > 0M && 
                x.VolumeStrength >= 700M &&
                x.CumulativeVolume.HasValue).OrderByDescending(x => x.ChangePercent).ToList();

            this._dayTradeSymbolQuote.Clear();

            for (int i = 0; i < filteredCommodities.Count; i++)
            {
                YwCommodity c = filteredCommodities[i];
                this._dayTradeSymbolQuote.Add(new DayTradeQuote(ref c));
                this._dispatcher.AddSymbolGridMap(c.Symbol, this._dayTradeStockGV, typeof(DayTradeQuote));
                this._dayTradeSymbols.Add(c.Symbol);
            }

            this._dayTradeStockGV.DataSource = null;
            this._dayTradeStockGV.DataSource = this._dayTradeSymbolQuote;
            this._dayTradeStockGV.Refresh(); 
        }

        private void _filterUpVolMidBtn_Click(object sender, EventArgs e)
        {
            DisplayDayTradeBtnColor(this._filterUpVolMidBtn);
            this._dispatcher.RemoveSymbolGridMap(this._dayTradeSymbols, this._dayTradeStockGV);
            this._dayTradeSymbols.Clear();
            List<YwCommodity> filteredCommodities = this._commodities.Values.Where(x => x.ChangePercent.HasValue &&
                x.ChangePercent.Value > 0M &&
                x.VolumeStrength >= 500M &&
                x.CumulativeVolume.HasValue).OrderByDescending(x => x.ChangePercent).ToList();

            this._dayTradeSymbolQuote.Clear();

            for (int i = 0; i < filteredCommodities.Count; i++)
            {
                YwCommodity c = filteredCommodities[i];
                this._dayTradeSymbolQuote.Add(new DayTradeQuote(ref c));
                this._dispatcher.AddSymbolGridMap(c.Symbol, this._dayTradeStockGV, typeof(DayTradeQuote));
                this._dayTradeSymbols.Add(c.Symbol);
            }

            this._dayTradeStockGV.DataSource = null;
            this._dayTradeStockGV.DataSource = this._dayTradeSymbolQuote;
            this._dayTradeStockGV.Refresh(); 
        }

        private void _filterUpVolMinBtn_Click(object sender, EventArgs e)
        {
            DisplayDayTradeBtnColor(this._filterUpVolMinBtn);
            this._dispatcher.RemoveSymbolGridMap(this._dayTradeSymbols, this._dayTradeStockGV);
            this._dayTradeSymbols.Clear();
            List<YwCommodity> filteredCommodities = this._commodities.Values.Where(x => x.ChangePercent.HasValue &&
                x.ChangePercent.Value > 0M &&
                x.VolumeStrength >= 100M &&
                x.CumulativeVolume.HasValue).OrderByDescending(x => x.ChangePercent).ToList();

            this._dayTradeSymbolQuote.Clear();

            for (int i = 0; i < filteredCommodities.Count; i++)
            {
                YwCommodity c = filteredCommodities[i];
                this._dayTradeSymbolQuote.Add(new DayTradeQuote(ref c));
                this._dispatcher.AddSymbolGridMap(c.Symbol, this._dayTradeStockGV, typeof(DayTradeQuote));
                this._dayTradeSymbols.Add(c.Symbol);
            }

            this._dayTradeStockGV.DataSource = null;
            this._dayTradeStockGV.DataSource = this._dayTradeSymbolQuote;
            this._dayTradeStockGV.Refresh(); 
        }

        private void _filterDownVolMostBtn_Click(object sender, EventArgs e)
        {
            DisplayDayTradeBtnColor(this._filterDownVolMostBtn);
            this._dispatcher.RemoveSymbolGridMap(this._dayTradeSymbols, this._dayTradeStockGV);
            this._dayTradeSymbols.Clear();
            List<YwCommodity> filteredCommodities = this._commodities.Values.Where(x => x.ChangePercent.HasValue &&
                x.ChangePercent.Value < 0M &&
                x.VolumeStrength >= 700M &&
                x.CumulativeVolume.HasValue).OrderByDescending(x => x.ChangePercent).ToList();

            this._dayTradeSymbolQuote.Clear();

            for (int i = 0; i < filteredCommodities.Count; i++)
            {
                YwCommodity c = filteredCommodities[i];
                this._dayTradeSymbolQuote.Add(new DayTradeQuote(ref c));
                this._dispatcher.AddSymbolGridMap(c.Symbol, this._dayTradeStockGV, typeof(DayTradeQuote));
                this._dayTradeSymbols.Add(c.Symbol);
            }

            this._dayTradeStockGV.DataSource = null;
            this._dayTradeStockGV.DataSource = this._dayTradeSymbolQuote;
            this._dayTradeStockGV.Refresh(); 
        }

        private void _filterDownVolMidBtn_Click(object sender, EventArgs e)
        {
            DisplayDayTradeBtnColor(this._filterDownVolMidBtn);
            this._dispatcher.RemoveSymbolGridMap(this._dayTradeSymbols, this._dayTradeStockGV);
            this._dayTradeSymbols.Clear();
            List<YwCommodity> filteredCommodities = this._commodities.Values.Where(x => x.ChangePercent.HasValue &&
                x.ChangePercent.Value < 0M &&
                x.VolumeStrength >= 500M &&
                x.CumulativeVolume.HasValue).OrderByDescending(x => x.ChangePercent).ToList();

            this._dayTradeSymbolQuote.Clear();

            for (int i = 0; i < filteredCommodities.Count; i++)
            {
                YwCommodity c = filteredCommodities[i];
                this._dayTradeSymbolQuote.Add(new DayTradeQuote(ref c));
                this._dispatcher.AddSymbolGridMap(c.Symbol, this._dayTradeStockGV, typeof(DayTradeQuote));
                this._dayTradeSymbols.Add(c.Symbol);
            }

            this._dayTradeStockGV.DataSource = null;
            this._dayTradeStockGV.DataSource = this._dayTradeSymbolQuote;
            this._dayTradeStockGV.Refresh(); 
        }

        private void _filterDownVolMinBtn_Click(object sender, EventArgs e)
        {
            DisplayDayTradeBtnColor(this._filterDownVolMinBtn);
            this._dispatcher.RemoveSymbolGridMap(this._dayTradeSymbols, this._dayTradeStockGV);
            this._dayTradeSymbols.Clear();
            List<YwCommodity> filteredCommodities = this._commodities.Values.Where(x => x.ChangePercent.HasValue &&
                x.ChangePercent.Value < 0M &&
                x.VolumeStrength >= 100M &&
                x.CumulativeVolume.HasValue).OrderByDescending(x => x.ChangePercent).ToList();

            this._dayTradeSymbolQuote.Clear();

            for (int i = 0; i < filteredCommodities.Count; i++)
            {
                YwCommodity c = filteredCommodities[i];
                this._dayTradeSymbolQuote.Add(new DayTradeQuote(ref c));
                this._dispatcher.AddSymbolGridMap(c.Symbol, this._dayTradeStockGV, typeof(DayTradeQuote));
                this._dayTradeSymbols.Add(c.Symbol);
            }

            this._dayTradeStockGV.DataSource = null;
            this._dayTradeStockGV.DataSource = this._dayTradeSymbolQuote;
            this._dayTradeStockGV.Refresh(); 
        }

        private void _dayTradeStockGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this._dayTradeStockGV.DataSource == null)
            {
                //e.FormattingApplied = true;
                return;
            }
            if (this._dayTradeStockGV.Columns[e.ColumnIndex].Name == "VolumeStrength")
            {
                decimal upOrDown = Convert.ToDecimal(this._dayTradeStockGV.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                if (upOrDown > 0)
                {
                    this._dayTradeStockGV.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Red;                    
                    return;
                }
                if (upOrDown < 0)
                {
                    this._dayTradeStockGV.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Green;                 
                    return;
                }
                else
                {                    
                    return;
                }
            }
        }        

        private void _oneSymbolBizGroupLB_Click(object sender, EventArgs e)
        {
            BizGroup bizGrp = this._oneSymbolBizGroupLB.SelectedItem as BizGroup;
            if (bizGrp == null) { return; }
            string bizGroupCode = bizGrp.Code;
            var selectedBizGroup = this._rep.Query<BizGroup>(x => x.Code == bizGroupCode);
            var allBizGroupSymbolMap = this._rep.FetchAll<BizGroupSymbol>();
            var symbolIdList = (from bizGroups in selectedBizGroup
                                join symbols in allBizGroupSymbolMap
                                on bizGroups.Id equals symbols.BizGroupId
                                select symbols.SymbolId).ToList();
            List<string> symbolCodes = this._rep.Query<Symbol>(x => symbolIdList.Contains(x.Id)).Select(x => x.Code).ToList();

            this._dispatcher.RemoveSymbolGridMap(this._oneCategoryRelateSymbols, this._oneSymbolRelateGV);
            this._oneCategoryRelateSymbols.Clear();

            List<YwCommodity> filteredCommodities = 
                this._commodities.Values.Where(x => symbolCodes.Contains(x.Symbol)).OrderByDescending(x => x.ChangePercent).ToList();

            this._oneCategoryRelateSymbolQuote.Clear();

            for (int i = 0; i < filteredCommodities.Count; i++)
            {
                YwCommodity c = filteredCommodities[i];
                this._oneCategoryRelateSymbolQuote.Add(new CategoryRelatedQuote(ref c));
                this._dispatcher.AddSymbolGridMap(c.Symbol, this._oneSymbolRelateGV, typeof(CategoryRelatedQuote));
                this._oneCategoryRelateSymbols.Add(c.Symbol);
            }

            this._oneSymbolRelateGV.DataSource = null;
            this._oneSymbolRelateGV.DataSource = this._oneCategoryRelateSymbolQuote;
            this._oneSymbolRelateGV.Refresh();
        }

        private void _oneSymbolPILB_Click(object sender, EventArgs e)
        {
            PointerIndex pi = this._oneSymbolPILB.SelectedItem as PointerIndex;
            if (pi == null) { return; }
            string piCode = pi.Code;
            var selectedPI = this._rep.Query<PointerIndex>(x => x.Code == piCode);
            var allPISymbolMap = this._rep.FetchAll<PointerIndexSymbol>();
            var symbolIdList = (from pis in selectedPI
                                join symbols in allPISymbolMap
                                on pis.Id equals symbols.PointerIndexId
                                select symbols.SymbolId).ToList();
            List<string> symbolCodes = this._rep.Query<Symbol>(x => symbolIdList.Contains(x.Id)).Select(x => x.Code).ToList();

            this._dispatcher.RemoveSymbolGridMap(this._oneCategoryRelateSymbols, this._oneSymbolRelateGV);
            this._oneCategoryRelateSymbols.Clear();

            List<YwCommodity> filteredCommodities = 
                this._commodities.Values.Where(x => symbolCodes.Contains(x.Symbol)).OrderByDescending(x => x.ChangePercent).ToList();

            this._oneCategoryRelateSymbolQuote.Clear();

            for (int i = 0; i < filteredCommodities.Count; i++)
            {
                YwCommodity c = filteredCommodities[i];
                this._oneCategoryRelateSymbolQuote.Add(new CategoryRelatedQuote(ref c));
                this._dispatcher.AddSymbolGridMap(c.Symbol, this._oneSymbolRelateGV, typeof(CategoryRelatedQuote));
                this._oneCategoryRelateSymbols.Add(c.Symbol);
            }

            this._oneSymbolRelateGV.DataSource = null;
            this._oneSymbolRelateGV.DataSource = this._oneCategoryRelateSymbolQuote;
            this._oneSymbolRelateGV.Refresh();
        }

        private void _oneSymbolIndustryLB_Click(object sender, EventArgs e)
        {
            Industry ind = this._oneSymbolIndustryLB.SelectedItem as Industry;
            if (ind == null) { return; }
            string indCode = ind.Code;
            var selectedIndustry = this._rep.Query<Industry>(x => x.Code == indCode);
            var allIndustrySymbolMap = this._rep.FetchAll<IndustrySymbol>();
            var symbolIdList = (from inds in selectedIndustry
                                join symbols in allIndustrySymbolMap
                                on inds.Id equals symbols.IndustryId
                                select symbols.SymbolId).ToList();
            List<string> symbolCodes = this._rep.Query<Symbol>(x => symbolIdList.Contains(x.Id)).Select(x => x.Code).ToList();

            this._dispatcher.RemoveSymbolGridMap(this._oneCategoryRelateSymbols, this._oneSymbolRelateGV);
            this._oneCategoryRelateSymbols.Clear();

            List<YwCommodity> filteredCommodities = 
                this._commodities.Values.Where(x => symbolCodes.Contains(x.Symbol)).OrderByDescending(x => x.ChangePercent).ToList();

            this._oneCategoryRelateSymbolQuote.Clear();

            for (int i = 0; i < filteredCommodities.Count; i++)
            {
                YwCommodity c = filteredCommodities[i];
                this._oneCategoryRelateSymbolQuote.Add(new CategoryRelatedQuote(ref c));
                this._dispatcher.AddSymbolGridMap(c.Symbol, this._oneSymbolRelateGV, typeof(CategoryRelatedQuote));
                this._oneCategoryRelateSymbols.Add(c.Symbol);
            }

            this._oneSymbolRelateGV.DataSource = null;
            this._oneSymbolRelateGV.DataSource = this._oneCategoryRelateSymbolQuote;
            this._oneSymbolRelateGV.Refresh();
        }

        private void _oneSymbolConceptLB_Click(object sender, EventArgs e)
        {
            Concept con = this._oneSymbolConceptLB.SelectedItem as Concept;
            
            if (con == null) { return; }
            string conCode = con.Code;
            var selectedConcept = this._rep.Query<Concept>(x => x.Code == conCode);
            var allConceptSymbolMap = this._rep.FetchAll<ConceptSymbol>();
            var symbolIdList = (from cons in selectedConcept
                                join symbols in allConceptSymbolMap
                                on cons.Id equals symbols.ConceptId
                                select symbols.SymbolId).ToList();
            List<string> symbolCodes = this._rep.Query<Symbol>(x => symbolIdList.Contains(x.Id)).Select(x => x.Code).ToList();

            this._dispatcher.RemoveSymbolGridMap(this._oneCategoryRelateSymbols, this._oneSymbolRelateGV);
            this._oneCategoryRelateSymbols.Clear();

            List<YwCommodity> filteredCommodities = 
                this._commodities.Values.Where(x => symbolCodes.Contains(x.Symbol)).OrderByDescending(x => x.ChangePercent).ToList();

            this._oneCategoryRelateSymbolQuote.Clear();

            for (int i = 0; i < filteredCommodities.Count; i++)
            {
                YwCommodity c = filteredCommodities[i];
                this._oneCategoryRelateSymbolQuote.Add(new CategoryRelatedQuote(ref c));
                this._dispatcher.AddSymbolGridMap(c.Symbol, this._oneSymbolRelateGV, typeof(CategoryRelatedQuote));
                this._oneCategoryRelateSymbols.Add(c.Symbol);
            }

            this._oneSymbolRelateGV.DataSource = null;
            this._oneSymbolRelateGV.DataSource = this._oneCategoryRelateSymbolQuote;
            this._oneSymbolRelateGV.Refresh();
        }

        private void _oneSymbolQueryClearBtn_Click(object sender, EventArgs e)
        {
            this._oneSymbolQueryTxt.Text = "";
            ResetSymbolQueryData();
        }

        private void ResetSymbolQueryData()
        {            
            this._dispatcher.RemoveSymbolGridMap(this._queryOneSymbols, this._oneSymbolQueryGV);
            this._oneSymbolQueryGV.DataSource = null;
            this._queryOneSymbols.Clear();
            this._dispatcher.RemoveSymbolGridMap(this._oneCategoryRelateSymbols, this._oneSymbolRelateGV);
            this._oneSymbolRelateGV.DataSource = null;
            this._oneCategoryRelateSymbols.Clear();

            this._oneSymbolBizGroupLB.DataSource = null;
            this._oneSymbolBizGroupLB.Items.Clear();
            this._oneSymbolConceptLB.DataSource = null;
            this._oneSymbolConceptLB.Items.Clear();
            this._oneSymbolIndustryLB.DataSource = null;
            this._oneSymbolIndustryLB.Items.Clear();
            this._oneSymbolPILB.DataSource = null;
            this._oneSymbolPILB.Items.Clear();
        }

        private void _oneSymbolRelateGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this._oneSymbolRelateGV.DataSource == null)
            {
                
                return;
            }
            if (this._oneSymbolRelateGV.Columns[e.ColumnIndex].Name == "Change")
            {
                decimal upOrDown = Convert.ToDecimal(this._oneSymbolRelateGV.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                if (upOrDown > 0)
                {
                    this._oneSymbolRelateGV.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    
                    return;
                }
                if (upOrDown < 0)
                {
                    this._oneSymbolRelateGV.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Green;
                    
                    return;
                }
                else
                {
                    return;
                }
            }
        }

        private void _dayTradeStockGV_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (e.StateChanged != DataGridViewElementStates.Selected)
            { return; }
            try
            {
                if (e.Row.Index < this._dayTradeSymbols.Count)
                {
                    string symbol = this._dayTradeSymbols[e.Row.Index];
                    this._oneSymbolQueryTxt.Text = symbol;
                    QueryOneSymbol();
                }                
            }
            catch (Exception exc)
            { 
                
            }
            
        }

        
    }
}
