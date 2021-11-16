using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ImportKIP.Business;
using ImportKIP.WCFService_ImportKIP;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using System.Threading;
using System.Globalization;
using SelectionChangedEventArgs = Telerik.Windows.Controls.SelectionChangedEventArgs;
using ImportKIP.Resources;

namespace ImportKIP
{
    public partial class MainPage : UserControl
    {
        private bool Isloaded = true;
        private string FILE_NAME = string.Empty;
        private string keyDataKip = string.Empty;
        private string keyPressed = "";
        string CR = "\r\n";
        private string keyVisibleLogPressed = "";
        //для вывода версии
        private RadListBox rlb = new RadListBox();
        private TextBox tb = new TextBox();
        /// <summary>
        /// последний привязаный ключ в таблице БД
        /// </summary>
        private int LastRelatedKeyBD;
        private int LastRelatedKeyFile;

        private List<string> HideBD = new List<string>();
        private List<string> HideFile = new List<string>();
        private bool isHideBD = true;

        private List<object> itemsToCollapse = new List<object>();
        private List<object> itemsToCollapseBD = new List<object>();



        private string unboundKeyFile = string.Empty; //ключ трубы их файла из таблицы увязанные кипы
        private string unboundKeyDB = string.Empty; //ключ трубы из бд из таблицы увязанные кипы


        public BoundedKip RelatedKIP;
        public ObservableCollection<BoundedKip> RelatedKIPList { get; private set; }

        public MainViewModel Model
        {
            get { return DataContext as MainViewModel; }
        }

        public MainPage()
        {
            InitializeComponent();

        }

        private void MainPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            Model.PropertyChanged -= MainModelPropertyChanged;
            Model.PropertyChanged += MainModelPropertyChanged;

            this.KeyDown -= OnKeyDown;
            this.KeyDown += OnKeyDown;

            RelatedKIPList = new ObservableCollection<BoundedKip>();
            Model.GetAllMG();
        }
        private void MainModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "MGList")
            {
                DDLMG.ItemsSource = null;
                if (Model.MGList.Count > 0)
                {
                    DDLMG.ItemsSource = Model.MGList;
                    DDLMG.SelectedIndex = 0;
                }

            }
            if (e.PropertyName == "ThreadList")
            {
                DDLThread.ItemsSource = null;
                if (Model.ThreadList.Count > 0)
                {
                    DDLThread.ItemsSource = Model.ThreadList;
                }
            }
            if (e.PropertyName == "KipDataList")
            {
                DDLDataList.ItemsSource = null;
                if (Model.KipDataList.Count > 0)
                {

                    DDLDataList.ItemsSource = Model.KipDataList;
                    //btnUploadFile.IsEnabled = true;

                }
            }
            //параметры участка
            if (e.PropertyName == "GetParamSec")
            {
                txtKmStart.Text = Model.ParamSec[0].NKM_BEGIN;
                txtKmEnd.Text = Model.ParamSec[0].NKM_END;

                if (string.IsNullOrEmpty(Model.ParamSec[0].NLENGTH))
                {
                    double kme = (txtKmEnd.Text != String.Empty ? Convert.ToDouble(txtKmEnd.Text.Replace(" ", "").Replace(".", ",")) : 0d);
                    double kmb = (txtKmStart.Text != String.Empty ? Convert.ToDouble(txtKmStart.Text.Replace(" ", "").Replace(".", ",")) : 0d);
                    //Convert.ToDouble(txtKmStart.Text.Replace(" ", "").Replace(".", ","));
                    //txtKmLen.Text = (Convert.ToDouble(txtKmEnd.Text.Replace(" ", "").Replace(".", ",")) -
                    //    Convert.ToDouble(txtKmStart.Text.Replace(" ", "").Replace(".", ","))).ToString();

                    txtKmLen.Text = ((kme - kmb) == 0d ? "" : (kme - kmb).ToString());

                }
                else
                {
                    txtKmLen.Text = Model.ParamSec[0].NLENGTH;
                }
                
                // txtCountKip.Text = Model.ParamSec[0].NUMBERKIP;
                btnUploadFile.IsEnabled = true;
            }
            //получаем данные лога
            if (e.PropertyName == "GetTxtLog")
            {
                txtInfoTab2.Text = "";
                txtInfoTab2.Text += Model.GetTxtLog;
            }
            if (e.PropertyName == "GetKipDataListBD")
            {
                List<KipDataListBD> table = new List<KipDataListBD>(Model.KipDataListBD);

                lbCountKipBD.Content = table.Count ;
                lbIsVisible.Content = " 0" ;

                if (table.Count == 0)
                {

                    Model.Report("не удалось наполнить  список  трубного журнала из бд = 0");
                    //непонятно что делать если нет массива
                }
                else
                {
                    grdKipBD.ItemsSource = null;
                    grdKipBD.ItemsSource = table;
                }
            }
            if (e.PropertyName == "GetKipDataListFile")
            {
                List<KipDataListFile> table = new List<KipDataListFile>(Model.KipDataListFile);
                lblCountKipF.Content = table.Count + " шт.";
                lbIsVisibleF.Content = " 0 шт.";

                if (table.Count == 0)
                {
                    MessageBox.Show(ProjectResources.NoData, ProjectResources.NoData, MessageBoxButton.OK);
                    Model.Report("не удалось наполнить  список  трубного журнала из файла = 0");
                    //непонятно что делать если нет массива
                }
                else
                {
                    grdKipFile.ItemsSource = null;
                    grdKipFile.ItemsSource = table;
                    btnBindAuto.IsEnabled = true;
                    buImport.IsEnabled = true;
                }
            }
            if (e.PropertyName == "ImportKipMatchingOneRow")
            {
                // очищаем данные в таблице
                RelatedKIPList = new ObservableCollection<BoundedKip>();


                GetlinkKipTable(Model.ListKeyBounds);
            }

            if (e.PropertyName == "AutoLinkStop")
            {
                // снимаем подсветки с левой таблицы и невидимые строки в правой и переначитываем таблицу
                if (string.IsNullOrEmpty(Model.AutoLinkStop))
                {
                    MessageBox.Show(ProjectResources.LinkGood);
                }
                else
                {
                    MessageBox.Show(Model.AutoLinkStop);
                }

                foreach (var item in grdKipFile.Items)
                {
                    var row2 = grdKipFile.ItemContainerGenerator.ContainerFromItem(item) as GridViewRow;
                    if (row2 != null)
                    {
                        row2.Background = new SolidColorBrush(Color.FromArgb(255, 0xff, 0xff, 0xff));
                    }
                }

                //при удалении связи очищаем данные в таблице
                RelatedKIPList = new ObservableCollection<BoundedKip>();

                //нужно запустить 
                Model.GetKipMapping();
            }

            //разорвать связь
            if (e.PropertyName == "RemoveItem")
            {
                ////новое
                //List<KeyBoundKIP> _kipMappingList = new List<KeyBoundKIP>();
                //_kipMappingList = Model.KipMappingList;

                //List<KipDataListBD> tableGrdKipBD = new List<KipDataListBD>();
                //tableGrdKipBD = GetKipListBD(Model.KipDataListBDEtalon, itemsToCollapseBD, _kipMappingList);
                //grdKipBD.ItemsSource = null;
                //grdKipBD.ItemsSource = tableGrdKipBD;

                ////новое
                //List<KipDataListFile> tableGrdKipFile = new List<KipDataListFile>();
                //tableGrdKipFile = GetKipListFile(Model.KipDataListFileEtalon, itemsToCollapse, _kipMappingList);
                //grdKipFile.ItemsSource = null;
                //grdKipFile.ItemsSource = tableGrdKipFile;


                //при удалении связи очищаем данные в таблице
                RelatedKIPList = new ObservableCollection<BoundedKip>();

                //нужно запустить 
                Model.GetKipMapping();
            }
            //увязать
            if (e.PropertyName == "GetKipMapping")
            {
                if (Model.KipMappingList.Count > 0)
                {
                    GetlinkKipTable(Model.KipMappingList);
                }
                else
                {

                    //новое
                    List<KeyBoundKIP> _kipMappingList = new List<KeyBoundKIP>();
                    _kipMappingList = Model.KipMappingList;

                    List<KipDataListBD> tableGrdKipBD = new List<KipDataListBD>();
                    tableGrdKipBD = GetKipListBD(Model.KipDataListBDEtalon, itemsToCollapseBD, _kipMappingList);
                    grdKipBD.ItemsSource = null;
                    grdKipBD.ItemsSource = tableGrdKipBD;

                    //новое
                    List<KipDataListFile> tableGrdKipFile = new List<KipDataListFile>();
                    tableGrdKipFile = GetKipListFile(Model.KipDataListFileEtalon, itemsToCollapse, _kipMappingList);
                    grdKipFile.ItemsSource = null;
                    grdKipFile.ItemsSource = tableGrdKipFile;



                    grdKipBound.ItemsSource = null;

                    int t = RelatedKIPList.Count;

                    int countBD = Model.CoutnKipDB - t - countVisiblecBD;
                    lbCountKipBD.Content = countBD.ToString();


                    int countFile = Model.CountKipFile - t - countVisiblecF;
                    lblCountKipF.Content = countFile.ToString();

                    lbCountKipLink.Content = "0 ";
                    lbNoLink.Content = "0 ";
                }

            }

            //скрыть отобразить
            if (e.PropertyName == "IsHideOk")
            {
                //0- скрыть
                if (Model.IsHide == "0")
                {
                    List<KeyBoundKIP> _relatedKIPList = new List<KeyBoundKIP>();
                    foreach (var row in grdKipBound.Items)
                    {
                        BoundedKip kb = (BoundedKip)(row);
                        _relatedKIPList.Add(new KeyBoundKIP()
                        {
                            KeyBD = kb.KeyDB,
                            KeyFile = kb.KeyFile,
                        }
                            );
                    }


                    //проверка скрывать в таблице бд
                    if (isHideBD)
                    {

                        List<KipDataListBD> tableGrdKipBD = new List<KipDataListBD>();

                        tableGrdKipBD.Clear();
                        tableGrdKipBD = GetKipListBD(Model.KipDataListBDEtalon, itemsToCollapseBD, _relatedKIPList);
                        grdKipBD.ItemsSource = null;
                        grdKipBD.ItemsSource = tableGrdKipBD;

                        lbIsVisible.Content = itemsToCollapseBD.Count.ToString() ;
                        buShow1.IsEnabled = true;
                        grdKipBD.SelectedItem = null;
                        buHide1.IsEnabled = btnBind.IsEnabled = false;

                    }
                    //в таблице файл
                    else
                    {

                        List<KipDataListFile> tableGrdKipFile = new List<KipDataListFile>();

                        tableGrdKipFile.Clear();
                        tableGrdKipFile = GetKipListFile(Model.KipDataListFileEtalon, itemsToCollapse, _relatedKIPList);
                        grdKipFile.ItemsSource = null;
                        grdKipFile.ItemsSource = tableGrdKipFile;

                        lbIsVisibleF.Content = itemsToCollapse.Count.ToString() ;
                        buShow.IsEnabled = true;
                        grdKipFile.SelectedItem = null;
                        buHide.IsEnabled = false;
                        btnBind.IsEnabled = btnNoBind.IsEnabled = false;

                    }

                }
                //1- отобразить
                else
                {
                    List<KeyBoundKIP> _relatedKIPList = new List<KeyBoundKIP>();

                    foreach (var row in grdKipBound.Items)
                    {
                        BoundedKip kb = (BoundedKip)(row);
                        _relatedKIPList.Add(new KeyBoundKIP()
                        {
                            KeyBD = kb.KeyDB,
                            KeyFile = kb.KeyFile,
                        }
                            );
                    }

                    if (isHideBD)
                    {
                        itemsToCollapseBD.Clear();

                        List<KipDataListBD> tableGrdKipBD = new List<KipDataListBD>();

                        tableGrdKipBD = GetKipListBD(Model.KipDataListBDEtalon, itemsToCollapseBD, _relatedKIPList);
                        grdKipBD.ItemsSource = null;
                        grdKipBD.ItemsSource = tableGrdKipBD;

                        lbIsVisible.Content = "0 ";
                        buShow1.IsEnabled = false;
                        grdKipBD.SelectedItem = null;
                        btnBind.IsEnabled = false;


                    }
                    else
                    {
                        itemsToCollapse.Clear();

                        List<KipDataListFile> tableGrdKipFile = new List<KipDataListFile>();

                        tableGrdKipFile.Clear();
                        tableGrdKipFile = GetKipListFile(Model.KipDataListFileEtalon, itemsToCollapse, _relatedKIPList);
                        grdKipFile.ItemsSource = null;
                        grdKipFile.ItemsSource = tableGrdKipFile;


                        countVisiblecF = 0;
                        lbIsVisibleF.Content = countVisiblecF ;
                        buShow.IsEnabled = false;
                        grdKipFile.SelectedItem = null;
                    }


                }

                //int tt = (Convert.ToInt32(lbCountKipBD.Content));
                int countBD = Model.KipDataListBDEtalon.Count - countLink - itemsToCollapseBD.Count;
                lbCountKipBD.Content = countBD.ToString() ;

                int t = RelatedKIPList.Count;
                int countFile = Model.CountKipFile - t - itemsToCollapse.Count;
                lblCountKipF.Content = countFile.ToString();

            }
            if (e.PropertyName == "IsImportOk")
            {


                if (Model.IsImportOk)
                {

                    MessageBox.Show(ProjectResources.ImportEnd, ProjectResources.ImportEnd, MessageBoxButton.OK);

                }
                else
                {
                    MessageBox.Show(ProjectResources.Error, ProjectResources.ErrorImport, MessageBoxButton.OK);
                }


                grdKipBound.ItemsSource = null;
                grdKipFile.ItemsSource = null;
                grdKipBD.ItemsSource = null;
                txtInfoTab2.Text = "";
                //DDLDataList.Items.Clear();
                DDLDataList.ItemsSource = null;

                txtKmStart.Text = "";
                txtKmEnd.Text = "";
                txtKmLen.Text = "";
                // txtCountKip.Text = "";
                StatusText.Foreground = new SolidColorBrush(Color.FromArgb(255, Byte.Parse("160"), Byte.Parse("160"), Byte.Parse("160")));
                StatusText.FontStyle = FontStyles.Italic;
                StatusText.Text = ProjectResources.SelectFile;
                lbCountKipBD.Content = " 0 ";
                lbIsVisible.Content = " 0 ";
                lblCountKipF.Content = " 0 ";
                lbIsVisibleF.Content = " 0 ";

                lbCountKipLink.Content = " 0 ";
                lbNoLink.Content = " 0 ";
                lbIsVisible.Content = " 0 ";
                txtInfoTab2.Text = ProjectResources.ResultUploadFile;


                btnBind.IsEnabled = false;
                btnNoBind.IsEnabled = false;
                btnBindAuto.IsEnabled = false;
                btnUndo.IsEnabled = false;
                buHide1.IsEnabled = false;
                buHide.IsEnabled = false;
                buShow.IsEnabled = false;
                buShow1.IsEnabled = false;
                btnBindAuto.IsEnabled = false;
                buImport.IsEnabled = false;
                txtKmStart.Text = "";
                txtKmEnd.Text = "";
                txtKmLen.Text = "";

                DDLMG.IsEnabled = true;
                DDLThread.IsEnabled = true;
                DDLDataList.ItemsSource = null;
                DDLDataList.IsEnabled = true;


            }
            //скрыть или показать лог
            if (e.PropertyName == "VisibleLog")
            {
                if (gridAll.RowDefinitions[1].Height.ToString() == "0")
                {
                    gridAll.RowDefinitions[1].Height = new GridLength(50);
                }
                else
                {
                    gridAll.RowDefinitions[1].Height = new GridLength(0);
                }

            }
        }


        #region Блок выбора данных

        private void DDLMG_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            btnUploadFile.IsEnabled = false;
            btnAnalizeFile.IsEnabled = false;

            if ((DDLMG.SelectedItem != null))
            {

                string keyMg = ((DataMGListKIP)DDLMG.SelectedItem).KEYMG;
                DDLThread.ItemsSource = null;
                Model.GetThreadsForMG(keyMg);
            }

        }

        /// <summary>
        /// клик на выбор нити
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DDLThread_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnUploadFile.IsEnabled = false;
            btnAnalizeFile.IsEnabled = false;

            txtKmStart.Text = string.Empty;
            txtKmEnd.Text = string.Empty;
            txtKmLen.Text = string.Empty;

            StatusText.Foreground = new SolidColorBrush(Color.FromArgb(255, Byte.Parse("160"), Byte.Parse("160"), Byte.Parse("160")));
            StatusText.FontStyle = FontStyles.Italic;
            StatusText.Text = ProjectResources.SelectFile;


            if (DDLThread.ItemsSource != null)
            {
                DDLDataList.ItemsSource = null;
                string keyThread = ((DataThreadsList)DDLThread.SelectedItem).KEYTHREADS;
                Model.GetDataKip(keyThread);

            }
        }

        private void DDLDataList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            StatusText.Foreground = new SolidColorBrush(Color.FromArgb(255, Byte.Parse("160"), Byte.Parse("160"), Byte.Parse("160")));
            StatusText.FontStyle = FontStyles.Italic;
            StatusText.Text = ProjectResources.SelectFile;

            txtKmStart.Text = string.Empty;
            txtKmEnd.Text = string.Empty;
            txtKmLen.Text = string.Empty;

            if (DDLDataList.ItemsSource != null)
            {
                Model.KeyJornalKip = ((KipDataList)DDLDataList.SelectedItem).nJourKey;
                Model.GetParamSec();

            }
        }

        #endregion Блок выбора данных
        #region Загрузка файла на сервер

        private void RadUpload1_OnUploadStarted(object sender, UploadStartedEventArgs e)
        {

            Model.IsShowBusyUserControl = true;
            
        }

        private void RadUpload1_FileUploadFailed(object sender, FileUploadFailedEventArgs e)
        {
            Model.Report("Ошибка загрузки файла на сервер");
            Model.IsShowBusyUserControl = false;
            Isloaded = false;
        }

        private void RadUpload1_UploadResumed(object sender, RoutedEventArgs e)
        {
            Model.IsShowBusyUserControl = false;
        }

        private void RadUpload1_OnUploadFinished(object sender, RoutedEventArgs e)
        {

            Model.IsShowBusyUserControl = false;
            if (Isloaded)
            {
                btnAnalizeFile.IsEnabled = true;
            }
            else
            {
                MessageBox.Show(ProjectResources.ErrorFile);
                btnAnalizeFile.IsEnabled = false;
            }

        }

        private void RadUpload1_FileUploadStarting(object sender, Telerik.Windows.Controls.FileUploadStartingEventArgs e)
        {
            StatusText.Foreground = new SolidColorBrush(Colors.Black);
            StatusText.FontStyle = FontStyles.Normal;
            StatusText.Text = e.SelectedFile.Name;
            FILE_NAME = e.SelectedFile.Name;

            e.FileParameters.Add("Filename", e.SelectedFile.Name);

            //btnAnalizeFile.IsEnabled = true;


        }

        /// <summary>
        /// загрузка на сервер файла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnUploadFile_OnClick(object sender, RoutedEventArgs e)
        {
            ////http://localhost:43554/BaseApp/Modules/Import_vtd/oraWCFService_ImpVtd.svc  
            //UriBuilder ub = new UriBuilder("http://localhost:43554/BaseApp/Modules/Import_vtd/receiverUpload.ashx");
           

            string path1 = HtmlPage.Document.DocumentUri.AbsoluteUri;
            int indexSlash1 = path1.LastIndexOf('/');
            path1 = path1.Substring(0, indexSlash1);
            string uri1 = path1 + "/RadUploadHandler.ashx";
            RadUpload1.UploadServiceUrl = uri1;

            RadUpload1.Filter = "Exel Files (*.xls)|*.xls|All Files(*.*)|*.*";  //"All files (*.*)|*.*|PNG Images (*.png)|*.png";

            RadUpload1.ShowFileDialog();
            // Model.IsShow = true;
            RadUpload1.StartUpload();



        }

        #endregion Загрузка файла на сервер

        /// <summary>
        /// загрузка файла в базу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAnalizeFile_OnClick(object sender, RoutedEventArgs e)
        {

            grdKipBound.ItemsSource = null;
            grdKipFile.ItemsSource = null;
            grdKipBD.ItemsSource = null;

            lbCountKipBD.Content = " 0 ";
            lbIsVisible.Content = " 0 ";
            lblCountKipF.Content = " 0 ";
            lbIsVisibleF.Content = " 0 ";
            btnAnalizeFile.IsEnabled = false;
            btnUploadFile.IsEnabled = true;
            btnBind.IsEnabled = false;
            btnNoBind.IsEnabled = false;
            btnBindAuto.IsEnabled = false;
            btnUndo.IsEnabled = false;

            DDLMG.IsEnabled = false;
            DDLThread.IsEnabled = false;
            DDLDataList.IsEnabled = false;

            //вначале удаляем предыдущие данные;
            Model.FILE_NAME = FILE_NAME;
            Model.DeleteKipMeasurement();
        }

        #region Блок различных увязок кип
        /// <summary>
        /// увязать
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBind_OnClick(object sender, RoutedEventArgs e)
        {
            if (grdKipFile.SelectedItem == null | grdKipBD.SelectedItem == null)
            {
                MessageBox.Show(ProjectResources.NoSelectKIP, ProjectResources.Attantion, MessageBoxButton.OK);
                return;
            }
            StringBuilder mappedKeys = new StringBuilder();
            mappedKeys.Append(((KipDataListBD)grdKipBD.SelectedItem).NKIP_KEY + "," +
                              ((KipDataListFile)grdKipFile.SelectedItem).nrawkey + ";");

            //вызываем увязку пары ключей
            Model.AddBoundedCurrentRow(((KipDataListFile)grdKipFile.SelectedItem).nrawkey, ((KipDataListBD)grdKipBD.SelectedItem).NKIP_KEY, 1);

        }

        /// <summary>
        /// добавить без увязки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNoBind_OnClick(object sender, RoutedEventArgs e)
        {
            if (grdKipFile.SelectedItem == null)
            {
                MessageBox.Show(ProjectResources.NoSelectKIP, ProjectResources.Attantion, MessageBoxButton.OK);
                return;
            }

            StringBuilder keyKipFileIsSelect = new StringBuilder();


            foreach (var selectItem in grdKipFile.SelectedItems)
            {
                keyKipFileIsSelect.Append(((KipDataListFile)(selectItem)).nrawkey + ",");
            }


            if (!string.IsNullOrEmpty(keyKipFileIsSelect.ToString()))
            {
                //вызываем процедуру добавления ручного добавления ключей в базе  
                Model.AddBoundedCurrentRow(keyKipFileIsSelect.ToString().TrimEnd(','), "0", 3);

            }

        }

        /// <summary>
        /// увязать авто
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBindAuto_OnClick(object sender, RoutedEventArgs e)
        {
            //автоматическая увязка в любом случае!
            Model.GetKipMappingAuto();

        }

        private int countLink = 0;

        private int countNoLink = 0;

        /// <summary>
        /// поведение увязанных кипов
        /// </summary>
        /// <param name="listKeyBoundsAll"></param>
        private void GetlinkKipTable(List<KeyBoundKIP> listKeyBoundsAll)
        {
            List<KeyBoundKIP> listKeyBounds = new List<KeyBoundKIP>(listKeyBoundsAll);

            KipDataListBD bd = new KipDataListBD();
            KipDataListFile file = new KipDataListFile();


            string kmBd = "";

            foreach (var itemKeyBounds in listKeyBounds)
            {
                // выделяем цветом увязанные тзмы dв таблице кип из бд
                foreach (var itemBD in Model.KipDataListBDEtalon)
                {
                    bd = new KipDataListBD();
                    if (!String.IsNullOrEmpty(itemKeyBounds.KeyBD))
                    {
                        if (((KipDataListBD)(itemBD)).NKIP_KEY == itemKeyBounds.KeyBD)
                        {
                            bd = (KipDataListBD)(itemBD);
                            break;
                        }
                    }
                }
                // выделяем цветом увязанные кипы из файла таблице кип из файла
                foreach (var itemFile in Model.KipDataListFileEtalon)
                {
                    file = new KipDataListFile();
                    if (!String.IsNullOrEmpty(itemKeyBounds.KeyBD))
                    {
                        if (((KipDataListFile)(itemFile)).nrawkey == itemKeyBounds.KeyFile)
                        {
                            file = (KipDataListFile)(itemFile);
                            break;
                        }
                    }

                }

                RelatedKIPList.Add(
                (new BoundedKip(bd.NKIP_KEY, bd.CNAME, bd.NKM, bd.cType,
                         file.nrawkey, file.ckipnum, file.nkm, file.nu_tz, file.nu_pol, file.ccomment)));

            }

            //new
            List<KipDataListBD> tableGrdKipBD = new List<KipDataListBD>();
            tableGrdKipBD = GetKipListBD(Model.KipDataListBDEtalon, itemsToCollapseBD, listKeyBoundsAll);
            grdKipBD.ItemsSource = null;
            grdKipBD.ItemsSource = tableGrdKipBD;

            List<KipDataListFile> tableGrdKipFile = new List<KipDataListFile>();
            tableGrdKipFile = GetKipListFile(Model.KipDataListFileEtalon, itemsToCollapse, listKeyBoundsAll);
            grdKipFile.ItemsSource = null;
            grdKipFile.ItemsSource = tableGrdKipFile;

            countNoLink = 0;
            countLink = 0;

            foreach (var boundedKip in RelatedKIPList)
            {

                if (string.IsNullOrEmpty(boundedKip.KeyDB))
                {
                    countNoLink = countNoLink + 1;
                }
                else
                {
                    countLink = countLink + 1;
                }
            }


            int countBD = Model.CoutnKipDB - countLink - itemsToCollapseBD.Count;
            lbCountKipBD.Content = countBD.ToString() ;

            int t = RelatedKIPList.Count;
            int countFile = Model.CountKipFile - t - countVisiblecF;
            lblCountKipF.Content = countFile.ToString();


            lbCountKipLink.Content = countLink.ToString();
            lbNoLink.Content = countNoLink.ToString();

            grdKipBound.ItemsSource = null;
            grdKipBound.ItemsSource = RelatedKIPList;


            grdKipBD.SelectedItem = null;
            grdKipFile.SelectedItem = null;
            btnBind.IsEnabled = false;
            buHide.IsEnabled = false;
            buHide1.IsEnabled = false;
            btnNoBind.IsEnabled = false;

        }

        /// <summary>
        /// отвязкать
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnUndo_OnClick(object sender, RoutedEventArgs e)
        {
            if (grdKipBound.SelectedItems.Count != 0)
            {
                unboundKeyDB = ((BoundedKip)grdKipBound.SelectedItem).KeyDB;
                unboundKeyFile = ((BoundedKip)grdKipBound.SelectedItem).KeyFile;

                Model.DeleteBounded(unboundKeyFile);

            }
            btnUndo.IsEnabled = false;
        }

        #endregion

        #region Блок кнопок Скрыть отобразить

        private bool isVisibleGrdBd = false;
        private bool isVisibleGrdFile = false;
        private int countVisiblec = 0;
        private int countVisiblecF = 0;
        private int countVisiblecBD = 0;


        /// <summary>
        /// кнопка скрыть (в таблице файле)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BuHide_OnClick(object sender, RoutedEventArgs e)
        {
            // 1=использовать, 0=не использовать измерение
            countVisiblecF = 0;
            isHideBD = false;
            StringBuilder keyKipFileIsHide = new StringBuilder();
            itemsToCollapse.AddRange(grdKipFile.SelectedItems);


            foreach (var visibleRow in grdKipFile.Items)
            {
                if (this.itemsToCollapse.Contains(((KipDataListFile)(visibleRow))))
                {
                    GridViewRow row = grdKipFile.ItemContainerGenerator.ContainerFromItem(visibleRow) as GridViewRow;

                    keyKipFileIsHide.Append(((KipDataListFile)(visibleRow)).nrawkey + ",");
                }
            }

            if (!string.IsNullOrEmpty(keyKipFileIsHide.ToString()))
            {
                Model.KipsIsHide(keyKipFileIsHide.ToString().TrimEnd(','), 0);

            }

        }

        /// <summary>
        /// отобразить все скрытые кип из файла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BuShow_OnClick(object sender, RoutedEventArgs e)
        {
            // 1=использовать, 0=не использовать измерение

            StringBuilder keyKipFileIsHide = new StringBuilder();

            //новое
            if (itemsToCollapse.Count > 0)
            {
                foreach (var item in itemsToCollapse)
                {
                    keyKipFileIsHide.Append(((KipDataListFile)(item)).nrawkey + ",");
                }
            }

            isHideBD = false;

            if (!string.IsNullOrEmpty(keyKipFileIsHide.ToString()))
            {
                Model.KipsIsHide(keyKipFileIsHide.ToString().TrimEnd(','), 1);

            }

        }


        /// <summary>
        /// кнопка скрыть (в таблице BD)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buHide1_OnClick(object sender, RoutedEventArgs e)
        {

            // 1=использовать, 0=не использовать измерение
            countVisiblecBD = 0;
            isHideBD = true;
            StringBuilder keyKipBDIsHide = new StringBuilder();
            itemsToCollapseBD.AddRange(grdKipBD.SelectedItems);


            foreach (var visibleRow in grdKipBD.Items)
            {
                if (this.itemsToCollapseBD.Contains(((KipDataListBD)(visibleRow))))
                {
                    //  GridViewRow row = grdKipFile.ItemContainerGenerator.ContainerFromItem(visibleRow) as GridViewRow;
                    keyKipBDIsHide.Append(((KipDataListBD)(visibleRow)).NKIP_KEY + ",");
                }
            }

            if (!string.IsNullOrEmpty(keyKipBDIsHide.ToString()))
            {
                Model.KipsIsHideBD(keyKipBDIsHide.ToString().TrimEnd(','), 0);
            }
        }
        /// <summary>
        /// кнопка отобразить все в таблице кип из бд
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buShow1_OnClick(object sender, RoutedEventArgs e)
        {

            // 1=использовать, 0=не использовать измерение

            StringBuilder keyKipBDIsHide = new StringBuilder();

            //новое
            if (itemsToCollapseBD.Count > 0)
            {
                foreach (var item in itemsToCollapseBD)
                {
                    keyKipBDIsHide.Append(((KipDataListBD)(item)).NKIP_KEY + ",");
                }
            }

            isHideBD = true;

            if (!string.IsNullOrEmpty(keyKipBDIsHide.ToString()))
            {
                Model.KipsIsHideBD(keyKipBDIsHide.ToString().TrimEnd(','), 1);

            }

        }


        #endregion


        /// <summary>
        /// импорт кип
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BuImport_OnClick(object sender, RoutedEventArgs e)
        {
            Model.ImportKipMeasurement();
        }

        /// <summary>
        /// сохраняем лог
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReportListBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            if ((keyPressed + e.Key.ToString()) == "CtrlC")
            {
                string s = string.Empty;
                foreach (var _item in reportListBox.SelectedItems)
                {

                    s = s + _item.ToString() + CR;
                }

                System.Windows.Clipboard.SetText(s);
                keyPressed = "";

            }
            else
            {
                keyPressed = e.Key.ToString();
            }
        }

        /// <summary>
        /// убрать - показать лог
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if ((keyVisibleLogPressed + e.Key.ToString()) == "CtrlM")
            {
                string s = string.Empty;


                Model.FirePropertyChanged("VisibleLog");
                keyVisibleLogPressed = "";


            }
            else
            {
                keyVisibleLogPressed = e.Key.ToString();
            }
        }

        /// <summary>
        /// показ лога
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainPage_OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            if (rlb != null)
            {
                gridAll.Children.Remove(rlb);
            }

            tb = new TextBox();
            Point p = e.GetPosition(this);
            rlb.Items.Clear();
            string[] arrVersion = Model.Version.Split(';');
            for (int i = 0; i < arrVersion.Length - 1; i++)
            {
                rlb.Items.Add(arrVersion[i]);
            }
            rlb.VerticalAlignment = VerticalAlignment.Stretch;
            rlb.HorizontalAlignment = HorizontalAlignment.Stretch;
            double top = ((ActualHeight - p.Y) - (30 * (arrVersion.Length - 1)));
            double right = ((ActualWidth - p.X) - 300);
            rlb.Margin = new Thickness(p.X, p.Y, right, top);
            gridAll.Children.Add(rlb);
        }

        private void MainPage_OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            gridAll.Children.Remove(rlb);
        }


        /// <summary>
        /// выбор записи в таблице кипы из бд
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrdKipBD_OnSelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            buHide1.IsEnabled = true;
            btnNoBind.IsEnabled = false;
            btnBind.IsEnabled = false;

            if (grdKipFile.SelectedItem != null && grdKipBD.SelectedItem != null)
            {
                btnBind.IsEnabled = true;
            }

        }

        /// <summary>
        /// клик в таблице файл
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrdKipFile_OnSelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            buHide.IsEnabled = true;
            btnNoBind.IsEnabled = true;
            btnBind.IsEnabled = false;
            if (grdKipFile.SelectedItem != null && grdKipBD.SelectedItem != null)
            {
                btnBind.IsEnabled = true;
            }

        }

        /// <summary>
        /// клик в таблице увязанных кипов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrdKipBound_OnSelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            btnNoBind.IsEnabled = false;
            btnBind.IsEnabled = false;

            if (grdKipBound.SelectedItem != null)
            {
                btnUndo.IsEnabled = true;
            }

        }

        public event EventHandler CollapseButtonClicked;

        /// <summary>
        /// сплиттер - нажать на кнопочку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrsplSplitter_OnCollapseButtonClicked(object sender, EventArgs e)
        {
            if (TopGrid.Height.Value == 0)
            {
                TopGrid.Height = new GridLength(140);
            }
            else if (sender != this)
            {
                TopGrid.Height = new GridLength(0);
            }


        }

        /// <summary>
        /// перенаполняем таблицу кип из бд
        /// </summary>
        /// <param name="kipDataListBDEtalon"></param>
        /// <param name="itemsToCollapseBD"></param>
        /// <param name="kipMappingList"></param>
        /// <returns></returns>
        private List<KipDataListBD> GetKipListBD(List<KipDataListBD> kipDataListBDEtalon, List<object> itemsToCollapseBD, List<KeyBoundKIP> kipMappingList)
        {

            //эталон всех кипов из бд
            List<KipDataListBD> table = new List<KipDataListBD>();
            //результат таблицы (возвращаем)
            List<KipDataListBD> tableBD = new List<KipDataListBD>(kipDataListBDEtalon);
            //скрытые кип
            List<object> itemsCollapseBD = new List<object>(itemsToCollapseBD);
            //увязынные кипы
            List<KeyBoundKIP> kipMappingListAll = new List<KeyBoundKIP>();
            kipMappingListAll = kipMappingList;

            bool findBouded = false;
            bool findCollapced = false;

            for (int i = 0; i < tableBD.Count; i++)
            {
                string keyBD = tableBD[i].NKIP_KEY;


                if (kipMappingListAll != null)
                {
                    if (kipMappingListAll.Count > 0)
                    {
                        for (int i1 = 0; i1 < kipMappingListAll.Count; i1++)
                        {
                            findBouded = false;
                            if (keyBD == kipMappingListAll[i1].KeyBD)
                            {
                                findBouded = true;
                                break;
                            }
                        }
                    }
                }

                if (itemsCollapseBD.Count > 0)
                {
                    findCollapced = false;
                    if (itemsCollapseBD.Contains(((KipDataListBD)(tableBD[i]))))
                    {
                        findCollapced = true;
                    }
                }

                if (!findBouded)
                {
                    if (!findCollapced)
                    {
                        table.Add(tableBD[i]);
                    }
                }
            }

            return table;
        }

        /// <summary>
        /// перенаполняем таблицу кип из файла
        /// </summary>
        /// <param name="kipDataListFileEtalon"></param>
        /// <param name="itemsToCollapseFile"></param>
        /// <param name="kipMappingList"></param>
        /// <returns></returns>
        private List<KipDataListFile> GetKipListFile(List<KipDataListFile> kipDataListFileEtalon, List<object> itemsToCollapseFile, List<KeyBoundKIP> kipMappingList)
        {

            //эталон всех кипов из бд
            List<KipDataListFile> table = new List<KipDataListFile>();
            //результат таблицы (возвращаем)
            List<KipDataListFile> tableFile = new List<KipDataListFile>(kipDataListFileEtalon);
            //скрытые кип
            List<object> itemsCollapseFile = new List<object>(itemsToCollapseFile);
            //увязынные кипы
            List<KeyBoundKIP> kipMappingListAll = new List<KeyBoundKIP>();
            kipMappingListAll = kipMappingList;

            bool findBouded = false;
            bool findCollapced = false;

            for (int i = 0; i < tableFile.Count; i++)
            {
                string keyFile = tableFile[i].nrawkey;

                if (kipMappingListAll !=null)
                {
                     if (kipMappingListAll.Count > 0)
                {
                    for (int i1 = 0; i1 < kipMappingListAll.Count; i1++)
                    {
                        findBouded = false;
                        if (keyFile == kipMappingListAll[i1].KeyFile)
                        {
                            findBouded = true;
                            break;
                        }
                    }
                }
                }
               
                if (itemsCollapseFile.Count > 0)
                {
                    findCollapced = false;
                    if (itemsCollapseFile.Contains(((KipDataListFile)(tableFile[i]))))
                    {
                        findCollapced = true;
                    }
                }

                if (!findBouded)
                {
                    if (!findCollapced)
                    {
                        table.Add(tableFile[i]);
                    }
                }
            }

            return table;
        }


    }
}




