using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ImportKIP.Resources;
using ImportKIP.WCFService_ImportKIP;


namespace ImportKIP.Business
{
    public class MainViewModel : MainModel
    {
        public ObservableCollection<string> Reports { get; private set; }

        private WCFService_ImportKIPClient _service_ImpKIP;
        private List<DataMGListKIP> _mGList;
        private ObservableCollection<DataThreadsList> _threadList;
        private ObservableCollection<WCFService_ImportKIP.KipDataList> _kipDataList;
        private string _getTxtLog;
        public string KeyJornalKip = string.Empty;
        public string FILE_NAME = string.Empty;


        protected WCFService_ImportKIPClient Service_ImpKIP
        {
            get
            {
                if (_service_ImpKIP == null)
                    _service_ImpKIP = new WCFService_ImportKIPClient();
                return _service_ImpKIP;
            }
        }

        public MainViewModel()
        {
            Reports = new ObservableCollection<string>();
        }

        public void Report(string message)
        {
            Reports.Add(string.Format("{0}, {1}", DateTime.Now.ToLongTimeString(), message));
        }

        public bool IsShowBusyUserControl
        {
            get { return _isShowBusyUserControl; }
            set
            {
                _isShowBusyUserControl = value;
                IsShowBusy = _isShowBusyUserControl;
            }
        }

        

        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }

        /// <summary>
        /// список всех магистралей
        /// </summary>
        public List<DataMGListKIP> MGList
        {
            get { return _mGList; }
            set
            {
                _mGList = value;
                FirePropertyChanged("MGList");
            }
        }

        public List<ParamSecList> ParamSec
        {
            get { return _paramSec; }
            set
            {
                _paramSec = value;
                FirePropertyChanged("GetParamSec");
            }
        }

        public ObservableCollection<DataThreadsList> ThreadList
        {
            get { return _threadList; }
            set
            {
                _threadList = value;
                FirePropertyChanged("ThreadList");
            }
        }

        public ObservableCollection<KipDataList> KipDataList
        {
            get { return _kipDataList; }
            set
            {
                _kipDataList = value;
                FirePropertyChanged("KipDataList");
            }
        }

        public string GetTxtLog
        {
            get { return _getTxtLog; }
            set
            {
                _getTxtLog = value;
                FirePropertyChanged("GetTxtLog");
            }
        }

        private string TxtLogAll = "";
        /// <summary>
        /// список кипов из базы
        /// </summary>
        public List<KipDataListBD> KipDataListBD
        {
            get { return _kipDataListBD; }
            set
            {
                _kipDataListBD = value;
                FirePropertyChanged("GetKipDataListBD");
            }
        }

        /// <summary>
        /// список кипов из базы эталон
        /// </summary>
        public List<KipDataListBD> KipDataListBDEtalon { get; private set; }

        /// <summary>
        /// список кипов из файла
        /// </summary>
        public List<KipDataListFile> KipDataListFile
        {
            get { return _kipDataListFile; }
            set
            {
                _kipDataListFile = value;
                FirePropertyChanged("GetKipDataListFile");
            }
        }

        /// <summary>
        /// список кипов из файла
        /// </summary>
        public List<KipDataListFile> KipDataListFileEtalon { get; private set; }


        public List<KeyBoundKIP> ListKeyBounds
        {
            get { return _listKeyBounds; }
            set
            {
                _listKeyBounds = value;
                FirePropertyChanged("ImportKipMatchingOneRow");
            }
        }

        public int RemoveItem
        {
            get { return _removeItem; }
            set
            {
                _removeItem = value;
                FirePropertyChanged("RemoveItem");
            }
        }

        public List<KeyBoundKIP> KipMappingList
        {
            get { return _kipMappingList; }
            set
            {
                _kipMappingList = value;
                FirePropertyChanged("GetKipMapping");
            }
        }

        public string AutoLinkStop
        {
            get { return _autoLinkStop; }
            set
            {
                _autoLinkStop = value;
                FirePropertyChanged("AutoLinkStop");
            }
        }

        public bool IsHideOk
        {
            get { return _isHide; }
            set
            {
                _isHide = value;
                FirePropertyChanged("IsHideOk");
            }
        }

        public bool IsImportOk
        {
            get { return _isImportOk; }
            set
            {
                _isImportOk = value;
                FirePropertyChanged("IsImportOk");
            }
        }

        public string IsHide = string.Empty;

        /// <summary>
        /// список мг
        /// </summary>
        public void GetAllMG()
        {


            IsShowBusy = true;
            IsBusyContent = ProjectResources.LoadingListOfMG;//"Загрузка данных - список МГ";
            Report("Загрузка списка МГ");
            Service_ImpKIP.GetDataMGListKIPCompleted += Service_ImpKIP_GetDataMGListKIPCompleted;
            Service_ImpKIP.GetDataMGListKIPAsync();

        }

        void Service_ImpKIP_GetDataMGListKIPCompleted(object sender, GetDataMGListKIPCompletedEventArgs e)
        {

            IsShowBusy = false;
            Service_ImpKIP.GetDataMGListKIPCompleted -= Service_ImpKIP_GetDataMGListKIPCompleted;
            if (e.Result.IsValid)
            {
                MGList = new List<DataMGListKIP>(e.Result.DataMGLists);
            }
            else
            {
                Report("Ошибка GetDataMGList +" + e.Result.ErrorMessage);
            }

        }

        /// <summary>
        /// список нитей
        /// </summary>
        /// <param name="keyMg"></param>
        public void GetThreadsForMG(string keyMg)
        {
            IsShowBusy = true;
            IsBusyContent = ProjectResources.LoadingListOfThreads;//"Загрузка данных - список нитей";
            Report("Загрузка списка нитей");
            Service_ImpKIP.GetDataThreadsListCompleted += Service_ImpKIP_GetDataThreadsListCompleted;
            Service_ImpKIP.GetDataThreadsListAsync(keyMg);
        }

        void Service_ImpKIP_GetDataThreadsListCompleted(object sender, GetDataThreadsListCompletedEventArgs e)
        {
            IsShowBusy = false;
            Service_ImpKIP.GetDataThreadsListCompleted -= Service_ImpKIP_GetDataThreadsListCompleted;
            if (e.Result.IsValid)
            {
                ThreadList = new ObservableCollection<DataThreadsList>(e.Result.DataThreadsLists);
                //new List<DataThreadsList>(e.Result.DataThreadsLists);
            }
            else
            {
                Report("Ошибка GetDataThreadsList +" + e.Result.ErrorMessage);
            }
        }

        public void GetParamSec()
        {
            IsShowBusy = true;
            IsBusyContent = ProjectResources.LoadingSettingsOfSite;//"Загрузка данных - параметры участка";
            Report("Загрузка параметры участка");

            Service_ImpKIP.GetSec_ParamCompleted += Service_ImpKIP_GetSec_ParamCompleted;
            Service_ImpKIP.GetSec_ParamAsync(KeyJornalKip);

        }

        void Service_ImpKIP_GetSec_ParamCompleted(object sender, GetSec_ParamCompletedEventArgs e)
        {
            IsShowBusy = false;
            IsBusyContent = "Загрузка файла на сервер";
            Service_ImpKIP.GetSec_ParamCompleted -= Service_ImpKIP_GetSec_ParamCompleted;

            if (e.Result.IsValid)
            {
                if (e.Result.ParamSecLists.Count > 0)
                {
                    ParamSec = new List<ParamSecList>(e.Result.ParamSecLists);
                }

            }
            else
            {
                Report("Ошибка GetSec_Param +" + e.Result.ErrorMessage);
            }
        }

        public void GetDataKip(string keyThread)
        {
            IsShowBusy = true;
            IsBusyContent = ProjectResources.LoadingListOfJournalsKIP;//"Загрузка данных - список журналов КИП";
            Report("Загрузка журнала кип");
            Service_ImpKIP.GetDataKipListCompleted += Service_ImpKIP_GetDataKipListCompleted;
            Service_ImpKIP.GetDataKipListAsync(keyThread);
        }

        void Service_ImpKIP_GetDataKipListCompleted(object sender, GetDataKipListCompletedEventArgs e)
        {
            IsShowBusy = false;
            Service_ImpKIP.GetDataKipListCompleted -= Service_ImpKIP_GetDataKipListCompleted;
            if (e.Result.IsValid)
            {
                KipDataList = new ObservableCollection<KipDataList>(e.Result.KipDataLists);
            }
            else
            {
                Report("Ошибка GetDataKipList +" + e.Result.ErrorMessage);

            }

        }


        private List<WCFService_ImportKIP.KipDataListBD> _kipDataListBD;
        private List<WCFService_ImportKIP.KipDataListFile> _kipDataListFile;
        private List<KeyBoundKIP> _listKeyBounds;
        private int _removeItem;
        private int typeLog = 1;
        private List<KeyBoundKIP> _kipMappingList;
        private bool _isShowBusyUserControl;
        private string _version;
        private List<ParamSecList> _paramSec;

        private List<WCFService_ImportKIP.KipDataListBD> _kipDataListBDEtalon;
        private List<WCFService_ImportKIP.KipDataListFile> _kipDataListFileEtalon;
        private string _autoLinkStop;
        private bool _isHide;
        private bool _isImportOk;


        public void Load_KIPXLSfile(string keyJornalKip, string fileName)
        {
            IsShowBusy = true;
            IsBusyContent = ProjectResources.DownloadFileInDatabase;// "Загрузка файла в базу данных";
            Report("Загрузка файла в базу данных");
            Service_ImpKIP.ImportFileCompleted += Service_ImpKIP_ImportFileCompleted;
            Service_ImpKIP.ImportFileAsync(Convert.ToDouble(keyJornalKip), "KIP", fileName);

        }

        void Service_ImpKIP_ImportFileCompleted(object sender, ImportFileCompletedEventArgs e)
        {
            IsShowBusy = false;
            Report("Загрузка файла завершена"); 
            Service_ImpKIP.ImportFileCompleted -= Service_ImpKIP_ImportFileCompleted;
            if (e.Result.IsValid)
            {
                //Получаем Лог анализа файла
                Report("Вызываем процедуру GetImpLog KeyJornalKip =  " + KeyJornalKip); 
                typeLog = 1;
                GetImpLog(KeyJornalKip);

            }
            else
            {

                //Получаем Лог анализа файла
                typeLog = 2;
                GetImpLog(KeyJornalKip);

                MessageBox.Show("Ошибка!" + "Ошибка " + e.Result.ErrorMessage, "Ошибка", MessageBoxButton.OK);
                Report("Ошибка ImportFile загрузка файла в базу" + e.Result.ErrorMessage);
            }
        }

        /// <summary>
        /// лог анализа файла
        /// </summary>
        /// <param name="keyImport"></param>
        public void GetImpLog(string keyImport)
        {
            IsShowBusy = true;
            IsBusyContent = ProjectResources.LoadingDataLog;//"Загрузка  данных лога";
            Report("Загрузка лога анализа файла данных");
            Report("typeLog = " + typeLog);
            Report(" Вызываем GetImpLog");
            Service_ImpKIP.GetImpLogCompleted += GetImpLogCompleted;
            Service_ImpKIP.GetImpLogAsync(keyImport);
        
        }

        void GetImpLogCompleted(object sender, GetImpLogCompletedEventArgs e)
        {
            IsShowBusy = false;
            Report("Сервис  GetImpLog отработал");
            Service_ImpKIP.GetImpLogCompleted -= GetImpLogCompleted;
            if (e.Result.IsValid)
            {
                
                //GetTxtLog += "\n" + e.Result.ImpLogKIP_result.ImpLogKIP_result_ret;
                TxtLogAll += "\n" + e.Result.ImpLogKIP_result.ImpLogKIP_result_ret;

                Report("Вернулись данные " + TxtLogAll.Length + " записей");
                Report(" typeLog = " + typeLog);
                if (typeLog == 1)
                {

                   

                    IsShowBusy = true;
                    IsBusyContent = ProjectResources.LoadingDataLog;//"Загрузка  данных лога";

                    Report("Вызываем GetImpAnalyzeFileLog  - загрузка данных лога анализ файла");

                    Service_ImpKIP.GetImpAnalyzeFileLogCompleted += GetImpAnalyzeFileLogCompleted;
                    Service_ImpKIP.GetImpAnalyzeFileLogAsync(KeyJornalKip);


                    //IsShowBusy = true;
                    //IsBusyContent = ProjectResources.LoadingListOfKipFromDatabase;//"Загрузка  данных - список КИП из БД";
                    ////получаем для таблицы список кипов из бд
                    //GetKipListBD(KeyJornalKip);
                }
            }
            else
            {
                Report("Ошибка получения лога = " + e.Result.ErrorMessage);
            }
        }


        //by Gaitov 07.05.2015 г.
        void GetImpAnalyzeFileLogCompleted(object sender, GetImpAnalyzeFileLogCompletedEventArgs e)
        {

            IsShowBusy = false;
            IsBusyContent = "";
            Report("Сервис  GetImpAnalyzeFileLog отработал");
            Report(e.Result.ErrorMessage);
            Report("e.Result.IsValid " + e.Result.IsValid);

            Service_ImpKIP.GetImpAnalyzeFileLogCompleted -= GetImpAnalyzeFileLogCompleted;
            if (e.Result.IsValid)
            {
                GetTxtLog = TxtLogAll + e.Result.ImpLogAnalizKIP_result.ImpLogAnalizKIP_result_ret;
                   // IsShowBusy = true;
                    //IsBusyContent = ProjectResources.LoadingListOfKipFromDatabase;//"Загрузка  данных - список КИП из БД";
                    //получаем для таблицы список кипов из бд
                    GetKipListBD(KeyJornalKip);
                
            }
            else
            {
                Report("Ошибка получения лога GetImpAnalyzeFileLog = " + e.Result.ErrorMessage);
            }
        }
        public int CoutnKipDB { get; private set; }

        public int CountKipFile { get; private set; }

        /// <summary>
        /// список кипов из файла
        /// </summary>
        /// <param name="keyJornalKip"></param>
        public void GetKipListBD(string keyJornalKip)
        {
            IsShowBusy = true;
            IsBusyContent = ProjectResources.LoadingListOfKipFromDatabase;
            Report("Список кип из бд");
            Service_ImpKIP.GetDataKipListBDCompleted += Service_ImpKIP_GetDataKipListBDCompleted;
            Service_ImpKIP.GetDataKipListBDAsync(keyJornalKip);
        }

        void Service_ImpKIP_GetDataKipListBDCompleted(object sender, GetDataKipListBDCompletedEventArgs e)
        {
            IsShowBusy = false;
            IsBusyContent = "";
            Service_ImpKIP.GetDataKipListBDCompleted -= Service_ImpKIP_GetDataKipListBDCompleted;
            if (e.Result.IsValid)
            {
                
               
                KipDataListBD = new List<KipDataListBD>(e.Result.KipDataListsBD);
                KipDataListBDEtalon = new List<KipDataListBD>(e.Result.KipDataListsBD);
                CoutnKipDB = e.Result.KipDataListsBD.Count;

                if (e.Result.KipDataListsBD.Count == 0)
                {
                    Report(e.Result.ErrorMessage);
                }
                GetKipListFile(KeyJornalKip);

            }
            else
            {
                Report("Ошибка GetDataKipListBD +" + e.Result.ErrorMessage);

            }
        }

        /// <summary>
        /// список кипов из файла
        /// </summary>
        /// <param name="keyJornalKip"></param>
        public void GetKipListFile(string keyJornalKip)
        {
            Report("Список кип из файла");
            IsShowBusy = true;
            IsBusyContent = ProjectResources.LoadingListOfKipFromFile;//"Загрузка  данных - список КИП из файла";
            Service_ImpKIP.GetDataKipListFileCompleted += Service_ImpKIP_GetDataKipListFileCompleted;
            Service_ImpKIP.GetDataKipListFileAsync(keyJornalKip);
        }

        void Service_ImpKIP_GetDataKipListFileCompleted(object sender, GetDataKipListFileCompletedEventArgs e)
        {
            IsShowBusy = false;
            IsBusyContent = "";
            Service_ImpKIP.GetDataKipListFileCompleted -= Service_ImpKIP_GetDataKipListFileCompleted;
            if (e.Result.IsValid)
            {
                KipDataListFile = new List<KipDataListFile>(e.Result.KipDataListsFile);
                KipDataListFileEtalon = new List<KipDataListFile>(e.Result.KipDataListsFile);
                CountKipFile = e.Result.KipDataListsFile.Count;

                if (e.Result.KipDataListsFile.Count == 0)
                {
                    Report(e.Result.ErrorMessage);
                }
            }
            else
            {
                Report("Ошибка GetDataKipListFile +" + e.Result.ErrorMessage);

            }
        }

        /// <summary>
        /// скрыть/отобразить кипы из файла
        /// </summary>
        /// <param name="keysKip"></param>
        /// <param name="isHide"></param>
        public void KipsIsHide(string keysKip, int isHide)
        {
            //1=использовать, 0=не использовать измерение
            IsHide = isHide.ToString();
            IsShowBusy = true;
            IsBusyContent = ProjectResources.HideOrShowKIP;//"Скрыть/отобразить КИП";
            Service_ImpKIP.KipIsHideCompleted += Service_ImpKIP_KipIsHideCompleted;
            Service_ImpKIP.KipIsHideAsync(KeyJornalKip, keysKip, isHide);

        }

        void Service_ImpKIP_KipIsHideCompleted(object sender, KipIsHideCompletedEventArgs e)
        {
            IsShowBusy = false;
            Service_ImpKIP.KipIsHideCompleted -= Service_ImpKIP_KipIsHideCompleted;
            if (e.Result.IsValid)
            {
                IsHideOk = true;
                Report("Скрыть/отобразить кип из файла" + e.Result.ErrorMessage);
            }
            else
            {
                Report("Ошибка скрытия/открытия кип err = " + e.Result.ErrorMessage);
            }


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keysKipBD"></param>
        /// <param name="isHide"></param>
        public void KipsIsHideBD(string keysKipBD, int isHide)
        {
            //1=использовать, 0=не использовать измерение
            IsHide = isHide.ToString();
            IsShowBusy = true;
            IsBusyContent = ProjectResources.HideOrShowKIP;//"Скрыть/отобразить КИП";

            Service_ImpKIP.KipIsHideBDCompleted += Service_ImpKIP_KipIsHideBDCompleted;
            Service_ImpKIP.KipIsHideBDAsync(KeyJornalKip, keysKipBD, isHide);
        }

        void Service_ImpKIP_KipIsHideBDCompleted(object sender, KipIsHideBDCompletedEventArgs e)
        {
            IsShowBusy = false;
            Service_ImpKIP.KipIsHideBDCompleted -= Service_ImpKIP_KipIsHideBDCompleted;
            if (e.Result.IsValid)
            {
                Report("Скрыть/отобразить кип из бд" + e.Result.ErrorMessage);
                IsHideOk = true;
            }
            else
            {
                Report("Ошибка скрытия/открытия кип err = " + e.Result.ErrorMessage);
            }
        }


        /// <summary>
        /// ручная увязка
        /// </summary>
        /// <param name="keyImport"></param>
        /// <param name="in_Filekey"></param>
        /// <param name="in_DBKey"></param>
        /// <param name="typeLink"> </param> какого типа увязка, ручная, автоматом или просто без увязки 1 - ручная
        public void AddBoundedCurrentRow(string in_Filekey, string in_DBKey, int typeLink)
        {
            IsShowBusy = true;
            Report("Увязка кип");
            IsBusyContent = ProjectResources.LinkingKIP;//"Увязка КИП";
            Service_ImpKIP.ImportKipMatchingCompleted += Service_ImpKIP_ImportKipMatchingCompleted;
            Service_ImpKIP.ImportKipMatchingAsync(KeyJornalKip, in_Filekey, in_DBKey, typeLink);
        }
        void Service_ImpKIP_ImportKipMatchingCompleted(object sender, ImportKipMatchingCompletedEventArgs e)
        {
            IsShowBusy = false;
            Service_ImpKIP.ImportKipMatchingCompleted -= Service_ImpKIP_ImportKipMatchingCompleted;

            if (e.Result.IsValid)
            {
                Report("Данне по увязке кип " + e.Result.ErrorMessage);
                ListKeyBounds = new List<KeyBoundKIP>(e.Result.GetKeyBoundList);

            }
            else
            {
                Report("Ошибка по увязке кип ImportKipMatching : " + e.Result.ErrorMessage);
            }
        }

        /// <summary>
        /// удалить увязку
        /// </summary>
        /// <param name="in_Filekey"></param>
        public void DeleteBounded(string in_Filekey)
        {
            IsShowBusy = true;
            IsBusyContent = ProjectResources.DeletingLinkedKIP;// "Удаление увязанных КИП";
            Service_ImpKIP.RemoveBoundCompleted += Service_ImpKIP_RemoveBoundCompleted;
            Service_ImpKIP.RemoveBoundAsync(KeyJornalKip, in_Filekey);

        }

        void Service_ImpKIP_RemoveBoundCompleted(object sender, RemoveBoundCompletedEventArgs e)
        {
            IsShowBusy = false;
            Service_ImpKIP.RemoveBoundCompleted -= Service_ImpKIP_RemoveBoundCompleted;
            if (e.Result.IsValid)
            {
                RemoveItem = e.Result.RemoveBoundKIP_Result.RemoveBoundKIP;
                Report("удаление связи труб RemoveItem = " + RemoveItem.ToString());
            }
            else
            {
                Report("удаление связи труб RemoveBound = " + e.Result.ErrorMessage);
            }
        }

        /// <summary>
        /// список увязанных кипов
        /// </summary>
        public void GetKipMapping()
        {
            IsShowBusy = true;
            IsBusyContent = ProjectResources.LoadingListOfLinkedKIP;//"Загрузка  данных - список увязанных КИП";
            Service_ImpKIP.GetKIPMappingCompleted += Service_ImpKIP_GetKIPMappingCompleted;
            Service_ImpKIP.GetKIPMappingAsync(KeyJornalKip);

        }

        private void Service_ImpKIP_GetKIPMappingCompleted(object sender, GetKIPMappingCompletedEventArgs e)
        {
            IsShowBusy = false;

            Service_ImpKIP.GetKIPMappingCompleted -= Service_ImpKIP_GetKIPMappingCompleted;
            if (e.Result.IsValid)
            {
                KipMappingList = new List<KeyBoundKIP>(e.Result.GetKeyBoundList);
            }
            else
            {
                Report("Ошибка получения увязанных кипов GetKIPMapping err = " + e.Result.ErrorMessage);
            }
        }


        /// <summary>
        /// автоматическая увязка кип
        /// </summary>
        public void GetKipMappingAuto()
        {
            IsShowBusy = true;
            IsBusyContent = ProjectResources.AutomaticLinkingKIP;//"Автоматическая увязка КИП";

            Service_ImpKIP.ImportKipMatchingAutoCompleted += Service_ImpKIP_ImportKipMatchingAutoCompleted;
            Service_ImpKIP.ImportKipMatchingAutoAsync(KeyJornalKip);

        }

        void Service_ImpKIP_ImportKipMatchingAutoCompleted(object sender, ImportKipMatchingAutoCompletedEventArgs e)
        {
            Service_ImpKIP.ImportKipMatchingAutoCompleted -= Service_ImpKIP_ImportKipMatchingAutoCompleted;
            IsShowBusy = false;
            if (e.Result.IsValid)
            {

                AutoLinkStop = e.Result.ErrorMessage;
            }
            else
            {
                Report("Ошибка  увязки кип авто ImportKipMatchingAuto err = " + e.Result.ErrorMessage);
            }

        }


        public void ImportKipMeasurement()
        {
            IsShowBusy = true;
            IsBusyContent = ProjectResources.ImportKIP;//"Импорт КИП";

            Service_ImpKIP.ImportKipMeasurementCompleted += Service_ImpKIP_ImportKipMeasurementCompleted;
            Service_ImpKIP.ImportKipMeasurementAsync(KeyJornalKip);

        }

        void Service_ImpKIP_ImportKipMeasurementCompleted(object sender, ImportKipMeasurementCompletedEventArgs e)
        {
            Service_ImpKIP.ImportKipMeasurementCompleted -= Service_ImpKIP_ImportKipMeasurementCompleted;
            IsShowBusy = false;

            if (e.Result.IsValid)
            {
                IsImportOk = true;

            }
            else
            {
                IsImportOk = false;
                Report("Ошибка  импорта кип ImportKipMeasurement err = " + e.Result.ErrorMessage);
            }

        }

        private bool deleteImport = false;
        /// <summary>
        /// удалить данные предыдущего импорта
        /// </summary>
        public void DeleteKipMeasurement()
        {
            IsShowBusy = true;
            IsBusyContent = ProjectResources.DeletingPreviousImport;//"Удаление данных предыдущего импорта";
            deleteImport = true;
            Service_ImpKIP.DeleteKipMeasurementCompleted += Service_ImpKIP_DeleteKipMeasurementCompleted;
            Service_ImpKIP.DeleteKipMeasurementAsync(KeyJornalKip);


        }

        void Service_ImpKIP_DeleteKipMeasurementCompleted(object sender, DeleteKipMeasurementCompletedEventArgs e)
        {
            Service_ImpKIP.DeleteKipMeasurementCompleted -= Service_ImpKIP_DeleteKipMeasurementCompleted;
            if (e.Result.IsValid)
            {
                if (deleteImport)
                {
                    //загружаем файл в базу
                    Load_KIPXLSfile(KeyJornalKip, FILE_NAME);
                    deleteImport = false;
                }

            }
            else
            {
                Report("Ошибка получения удаления данных предыдущего импорта DeleteKipMeasurement err = " + e.Result.ErrorMessage);
            }

        }


    }
}
