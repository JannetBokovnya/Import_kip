using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class index : System.Web.UI.Page
{
    //public string userKey;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (HttpContext.Current.Session["lang"] != null)
            id.Text = HttpContext.Current.Session["lang"].ToString();
        SetModuleName("IMPORTKIP");
    }

    public void SetModuleName(string p_cModuleName)
    {
        System_TopMasterPage master = (System_TopMasterPage)this.Master;
        master.SetModuleName(p_cModuleName);
    }

    public void moduleEvent(string moduleEvents, string p_cModuleName, string p_cValue)
    {
        throw new NotImplementedException();
    }

    //Перегружаем "Культуру" для данной страницы (этот метод вызвается самым первым, раньше всех других)
    protected override void InitializeCulture()
    {
        //Выставляем "Культуру" для данной страницы, в зависимости выбранного ранее пользователем (берем из сессионной переменной Session["lang"])
        if (HttpContext.Current.Session["lang"] != null)
        {
            String selectedLanguage = HttpContext.Current.Session["lang"].ToString();
            UICulture = selectedLanguage;
            Culture = selectedLanguage;

            Thread.CurrentThread.CurrentCulture =
                CultureInfo.CreateSpecificCulture(selectedLanguage);
            Thread.CurrentThread.CurrentUICulture = new
                CultureInfo(selectedLanguage);

        }
        base.InitializeCulture();
    }
}


//public partial class ImportKipForm : System.Web.UI.Page
//{
//    readonly OracleLayer_ImpKip _ol = new OracleLayer_ImpKip();
//    readonly OracleImport_ImpKip _oi = new OracleImport_ImpKip();

//    protected void Page_Load(object sender, EventArgs e)
//    {
//        SetModuleName("IMPORTKIP");
//        if (!Page.IsPostBack)
//        {
//            FillMgList();
//        }
//    }

//    #region Methods
//    private void SetModuleName(string moduleName)
//    {
//        System_TopMasterPage master = (System_TopMasterPage)this.Master;
//        master.SetModuleName(moduleName);
//    }

//    private void FillMgList()
//    {
//        string errStr;
//        DataSet ds = _ol.GetMg(out errStr);
//        if (!string.IsNullOrEmpty(errStr))
//            ShowError(errStr);
//        else
//            PageOperation_ImpKip.FillDdls(ref DDLMG, ds, "CNAME", "NKEY", "Выбрать", "-1");
//    }

//    /// <summary>
//    /// создаёт массив связанных ключей из двух таблиц
//    /// </summary>
//    /// <returns></returns>
//    private string CreateLinkKeys()
//    {
//        string res = "";

//        for (int i = 0; i < grdKIPdb.Items.Count; i++)
//        {
//            if (grdKIPdb.Items[i].Visible)
//            {
//                res = grdKIPdb.Items[i].Cells[4].Text + "," +
//                      grdKIPfile.Items[i].Cells[6].Text + ";";
//            }
//        }

//        return res;
//    }

//    /// <summary>
//    /// сравнивает кол-во строк в двух таблицах,
//    /// и если кол-во одинаковое -   открывает кнопку
//    /// "начать импорт
//    /// </summary>
//    private void CompareRowsCount()
//    {
//        int visibleFromDb = GetVisibleRowFromGridView(grdKIPdb);
//        int visibleFromFile = GetVisibleRowFromGridView(grdKIPfile);

//        lblDBKipCount.Text = visibleFromDb.ToString();
//        lblFileKipCount.Text = visibleFromFile.ToString();

//        btnMainImport.Enabled = (visibleFromDb == visibleFromFile && visibleFromDb != 0);
//    }

//    /// <summary>
//    /// ВОзвращает количество отображаемых строк в Гриде
//    /// </summary>
//    /// <param name="gv">RadGrid</param>
//    /// <returns></returns>
//    private int GetVisibleRowFromGridView(RadGrid gv)
//    {
//        int count = 0;
//        for (int i = 0; i < gv.Items.Count; i++)
//            if (gv.Items[i].Visible)
//                count++;

//        return count;
//    }

//    /// <summary>
//    /// Отбработка сообщения с ошибкой
//    /// </summary>
//    /// <param name="errMsg">Текст ошибки</param>
//    private void ShowError(string errMsg)
//    {
//        //lblWarning.Text = errMsg;
//        //lblWarning.Visible = true;
//        txtImportlog.Text += errMsg + "'\n'";
//    }

//    /// <summary>
//    /// Очистка таблиц и значение
//    /// </summary>
//    private void ClearControls()
//    {
//        //Очищаем таблицы КИП(ов)
//        grdKIPdb.DataSource = null;
//        grdKIPdb.DataBind();
//        grdKIPfile.DataSource = null;
//        grdKIPfile.DataBind();
//        grdKipCommon.DataSource = null;
//        grdKipCommon.DataBind();

//        //Очищаем и делаем невидимыми строки с количеством КИП(ов)
//        lblDBKipCount.Text = "";
//        divDbKipCount.Visible = false;
//        lblFileKipCount.Text = "";
//        divFileKipCount.Visible = false;
//    }

//    /// <summary>
//    /// Автоматическое сравнение двух таблиц
//    /// </summary>
//    private void AutoCompareTables()
//    {
//        try
//        {
//            XlsParse_ImpKip cleaner = new XlsParse_ImpKip();
//            for (int dbCount = 0; dbCount < grdKIPdb.Items.Count; dbCount++)//проходим по таблице из БД
//            {
//                if (!String.IsNullOrEmpty(grdKIPdb.Items[dbCount].Cells[1].Text))
//                {
//                    bool isFound = false;
//                    for (int fileCount = 0; fileCount < grdKIPfile.Items.Count; fileCount++)//проходим по таблице из  файла
//                    {
//                        if (!String.IsNullOrEmpty(grdKIPfile.Items[fileCount].Cells[1].Text))
//                        {
//                            if (cleaner.ClearString((chkLR.SelectedValue == "1" ? grdKIPfile.Items[fileCount].Cells[1].Text + txtAddName.Text : txtAddName.Text + grdKIPfile.Items[fileCount].Cells[1].Text)).ToLower() == cleaner.ClearString(grdKIPdb.Items[dbCount].Cells[1].Text).ToLower())
//                            {
//                                isFound = true;
//                                break;
//                            }
//                        }
//                    }

//                    if (!isFound)//если не нашли совпадения
//                    {
//                        grdKIPdb.Items[dbCount].Visible = false;
//                    }
//                }
//            }

//            //проходим   обратное сравнение 
//            for (int fileCount = 0; fileCount < grdKIPfile.Items.Count; fileCount++)//проходим по таблице из  файлаХ
//            {
//                if (!string.IsNullOrEmpty(grdKIPfile.Items[fileCount].Cells[1].Text))
//                {
//                    bool isFound = false;
//                    for (int dbCount = 0; dbCount < grdKIPdb.Items.Count; dbCount++)//проходим по таблице из БД
//                    {
//                        if (cleaner.ClearString((chkLR.SelectedValue == "1" ? grdKIPfile.Items[fileCount].Cells[1].Text + txtAddName.Text : txtAddName.Text + grdKIPfile.Items[fileCount].Cells[1].Text).ToLower()) == cleaner.ClearString(grdKIPdb.Items[dbCount].Cells[1].Text).ToLower())
//                        {
//                            isFound = true;
//                            break;
//                        }
//                    }

//                    if (!isFound)//если не нашли совпадения
//                    {
//                        grdKIPfile.Items[fileCount].Visible = false;
//                    }
//                }
//            }
//        }

//        catch (Exception ex)
//        {
//            ShowError("Ошибка автосравнения: " + ex.Message);
//        }

//    }

//    /// <summary>
//    /// Проверка коррекстности расширения
//    /// </summary>
//    /// <param name="fileName"></param>
//    /// <returns></returns>
//    private bool IsExcelExtension(string fileName)
//    {
//        bool isCorrectExtension = true;
//        var extension = System.IO.Path.GetExtension(fileName);
//        if (extension != null && extension.ToLower() != ".xls")
//        {
//            isCorrectExtension = false;
//            ShowError("Загруженный файл имеет неправильный формат, \n допускается загружать  файлы xls");
//        }
//        else if (extension == null)
//            isCorrectExtension = false; 

//        return isCorrectExtension;
//    }

//    /// <summary>
//    /// Попытаться сохранить файл
//    /// </summary>
//    /// <param name="fullFileName">Имя файла</param>
//    private void TrySaveAsFile(string fullFileName)
//    {
//        try
//        {
//            //FileField.PostedFile.SaveAs(AppDomain.CurrentDomain.BaseDirectory + "Upload\\UploadedImportKIP\\" + fullFileName);
//            upldKipFromFile.UploadedFiles[0].SaveAs(AppDomain.CurrentDomain.BaseDirectory + "Upload\\UploadedImportKIP\\" + fullFileName);
//            btnMainImport.Enabled = true;
//            lblWarning.Text = (string) GetLocalResourceObject("cSuccessDownload");
//        }
//        catch (Exception ex)
//        {
//            ShowError(GetLocalResourceObject("cErrorImportingFile") + ex.Message);
//        }
//    }

//    /// <summary>
//    /// Попытаться 
//    /// </summary>
//    /// <param name="fileName"></param>
//    /// <param name="kipJournalKey"></param>
//    /// <returns>Отработало ли без ошибок?</returns>
//    private bool TryCreateNewKip(string fileName, double kipJournalKey)
//    {
//        string errStr;
//        string fullFileName = PageOperation_ImpKip.GetFileName() + System.IO.Path.GetExtension(fileName);
//        double userKey = Convert.ToDouble("14056267");

//        //создаём новый импорт кип
//        _oi.CreateNewImportKip(userKey, fullFileName, fileName, kipJournalKey, out errStr);
//        if (!string.IsNullOrEmpty(errStr))
//        {
//            ShowError(errStr);
//            return false;
//        }

//        return true;
//    }

//    /// <summary>
//    /// Попытаться импортировать файл Excel в XlsParse. 
//    /// Как вы, наверное, и догадались с названия метода
//    /// </summary>
//    /// <param name="fullFileName">Имя файла</param>
//    /// <param name="kipJournalKey">Ключ журнала КИП</param>
//    /// <returns></returns>
//    private XlsParse_ImpKip TryImportFileToXlsParse(string fullFileName, double kipJournalKey)
//    {
//        string errStr = "";
//        XlsParse_ImpKip xls = new XlsParse_ImpKip();
//        try
//        {
//            xls.ImportFile(kipJournalKey, "kip", AppDomain.CurrentDomain.BaseDirectory + "Upload\\UploadedImportKIP\\" + fullFileName,
//                           out errStr);
//        }
//        catch (Exception ex)
//        {
//            ShowError(GetLocalResourceObject("cError") + ": " + ex.Message);
//        }
//        if (!string.IsNullOrEmpty(errStr))
//        {
//            ShowError(errStr);
//            xls = null;
//        }
//        return xls;
//    }

//    /// <summary>
//    /// Заполнить grdKipFile результатами из базы
//    /// </summary>
//    /// <param name="kipJournalKey">ключ журнала КИП</param>
//    private void TryFillGrdKipFileFromDb(double kipJournalKey)
//    {
//        string errStr;
//        DataSet ds = _oi.GetKipListFromFile(kipJournalKey, out errStr);
//        if (!string.IsNullOrEmpty(errStr))
//        {
//            ShowError(errStr);
//            return;
//        }

//        lblFileKipCount.Text = ds.Tables[0].Rows.Count.ToString();
//        grdKIPfile.DataSource = ds;
//        grdKIPfile.DataBind();

//        //Делаем видимой строку с количеством КИП из файла
//        divFileKipCount.Visible = true;

//        //проходим по всем строкам заполняем поля
//        foreach (DataRow row in ds.Tables[0].Rows)
//        {
//            txtImportlog.Text += row["clogmsg"] + "'\n'";
//        }
//    }

//    /// <summary>
//    /// удаляет задвоенные значения
//    /// </summary>
//    private void DeleteDoubleData()
//    {
//        try
//        {
//            XlsParse_ImpKip cleaner = new XlsParse_ImpKip();
//            for (int dbCount = 0; dbCount < grdKIPdb.Items.Count; dbCount++)//проходим по таблице из БД
//            {
//                string firstValue = grdKIPdb.Items[dbCount].Cells[1].Text;
//                int countFound = 0;
//                for (int subdbCount = 0; subdbCount < grdKIPdb.Items.Count; subdbCount++)//проходим по таблице из БД
//                {
//                    if (cleaner.ClearString(firstValue).ToLower() == cleaner.ClearString(grdKIPdb.Items[subdbCount].Cells[1].Text).ToLower())
//                    {
//                        countFound++;
//                        if (countFound > 1)
//                        {
//                            grdKIPdb.Items[subdbCount].Visible = false;
//                        }
//                    }
//                }
//                if (countFound > 1)
//                {
//                    txtImportlog.Text += "\n [Функция удаления повторяющихся элементов сообщает ]: В таблице из БД  для " + firstValue + " найдено " + countFound.ToString() + " совпадений";
//                }

//            }

//            for (int dbCount = 0; dbCount < grdKIPfile.Items.Count; dbCount++)//проходим по таблице из файла
//            {
//                string firstValue = grdKIPfile.Items[dbCount].Cells[1].Text;
//                int countFound = 0;
//                for (int subdbCount = 0; subdbCount < grdKIPfile.Items.Count; subdbCount++)//проходим по таблице из файла
//                {
//                    if (cleaner.ClearString(firstValue).ToLower() == cleaner.ClearString(grdKIPfile.Items[subdbCount].Cells[1].Text).ToLower())
//                    {
//                        countFound++;
//                        if (countFound > 1)
//                        {
//                            grdKIPfile.Items[subdbCount].Visible = false;
//                        }
//                    }
//                }
//                if (countFound > 1)
//                {
//                    txtImportlog.Text += "\n " + GetLocalResourceObject("cMesFuncDelRepeatVal") + firstValue + GetLocalResourceObject("cFound")
//                                               + countFound + GetLocalResourceObject("cCoincidence");



//                    //(string)GetLocalResourceObject("cSuccessDownload");
//                }

//            }
//        }
//        catch (Exception ex)
//        {
//            ShowError((string)GetLocalResourceObject("cErrorDelDoubleVal") + ex.Message);
//        }

//    }
//    #endregion

//    #region Events
//    protected void DDLMG_SelectedIndexChanged(object sender, EventArgs e)
//    {
//        double tmp;
//        string errStr;

//        DDLThread.Enabled = true;
//        DDLThread.Items.Clear();
//        double.TryParse(DDLMG.SelectedValue, out tmp);
//        DataSet ds = _ol.GetThreadsForMgAll(tmp, out errStr);
//        if (string.IsNullOrEmpty(errStr))
//        {
//            DDLThread.Enabled = true;
//            PageOperation_ImpKip.FillDdls(ref DDLThread, ds, "CNAME", "NKEY", "Выбрать", "-1");

//            //Скрываем блок с данными по выбранному участку, очищаем таблицы с КИП(ами) и скрываем строки с их количеством
//            ClearControls();
//        }
//        else
//        {
//            ShowError(errStr);
//        }
//    }
    
//    protected void DDLThread_SelectedIndexChanged(object sender, EventArgs e)
//    {
//        OracleImport_ImpKip oi = new OracleImport_ImpKip();
//        double ikey;
//        string errStr;
//        double.TryParse(DDLThread.SelectedValue, out ikey);
//        DataSet ds = oi.GetKipMeasJournals(ikey, out errStr);
//        if (string.IsNullOrEmpty(errStr))
//        {
//            DDLThread.Enabled = true;
//            PageOperation_ImpKip.FillDdls(ref DDLDates, ds, "dMeasDate", "nJourKey", "Выбрать", "-1");
//            DDLDates.Enabled = true;

//            //Скрываем блок с данными по выбранному участку, очищаем таблицы с КИП(ами) и скрываем строки с их количеством
//            ClearControls();
//        }
//        else
//        {
//            ShowError(errStr);
//        }
//    }

//    protected void DDLDates_SelectedIndexChanged(object sender, EventArgs e)
//    {
//        OracleImport_ImpKip oi = new OracleImport_ImpKip();
//        double ikey;
//        string errStr;
//        double.TryParse(DDLDates.SelectedValue, out ikey);
//        DataSet ds = oi.GetKipMeasJournalParams(ikey, out  errStr);
//        if (string.IsNullOrEmpty(errStr))
//        {
//            foreach (DataRow row in ds.Tables[0].Rows)
//            {
//                lblStartKm.Text = row["NKM_BEGIN"].ToString();

//                lblEndKm.Text = row["NKM_END"].ToString();

//                lblLen.Text = row["NLENGTH"].ToString();

//                lblCountKip.Text = row["NNUMBER_IZMER_KIP"].ToString();
//            }

//            //вытягиваем список КИПов из БД
//            if (ikey > 0)
//            {
//                try
//                {
//                    ds = oi.GetKipListForJournal(ikey, out  errStr);
//                    grdKIPdb.DataSource = ds;
//                    grdKIPdb.DataBind();
//                    lblDBKipCount.Text = ds.Tables[0].Rows.Count.ToString();
//                    lblCountKip.Text = (lblCountKip.Text == "") ? lblDBKipCount.Text : lblCountKip.Text;

//                    //Делаем видимой строку с количеством КИП из БД
//                    divDbKipCount.Visible = true;
//                }
//                catch
//                {
//                    ShowError((string)GetLocalResourceObject("cFailReceiveTsEvent"));
//                }
//            }
//            else
//            {
//                grdKIPdb.DataSource = null;
//                grdKIPdb.DataBind();
//            }
//        }
//        else
//        {
//            ShowError(errStr);
//        }
//    }

//    protected void btnDelSelectedItems_Click(object sender, EventArgs e)
//    {
//        int i = 0;
//        int countVisible = 0;
//        while (i < grdKIPdb.Items.Count)
//        {
//            CheckBox cb = new CheckBox();
//            if (grdKIPdb.Items[i].Visible)
//            {
//                cb = (CheckBox)grdKIPdb.Items[i].FindControl("CheckBox1");

//                if (cb.Checked)
//                {
//                    grdKIPdb.Items[i].Visible = false;
//                    i = 0;
//                }
//            }
//            i++;
//        }
//        countVisible = GetVisibleRowFromGridView(grdKIPdb);

//        lblDBKipCount.Text = countVisible.ToString();
//        CompareRowsCount();
//    }

//    protected void btnViewAll_Click(object sender, EventArgs e)
//    {
//        for (int i = 0; i < grdKIPdb.Items.Count; i++)
//        {
//            grdKIPdb.Items[i].Visible = true;
//        }
//        lblDBKipCount.Text = grdKIPdb.Items.Count.ToString();
//        CompareRowsCount();
//    }

//    protected void btnDelSelectItems_Click(object sender, EventArgs e)
//    {
//        int i = 0;

//        while (i < grdKIPfile.Items.Count)
//        {
//            CheckBox cb = new CheckBox();
//            if (grdKIPfile.Items[i].Visible)
//            {
//                cb = (CheckBox)grdKIPfile.Items[i].FindControl("CheckBox2");

//                if (cb.Checked)
//                {
//                    grdKIPfile.Items[i].Visible = false;
//                    i = 0;
//                }
//            }
//            i++;
//        }

//        int countVisible = GetVisibleRowFromGridView(grdKIPdb);

//        lblFileKipCount.Text = countVisible.ToString();
//        CompareRowsCount();
//    }

//    protected void btnSelectAll_Click(object sender, EventArgs e)
//    {
//        for (int i = 0; i < grdKIPfile.Items.Count; i++)
//        {
//            grdKIPfile.Items[i].Visible = true;
//        }
//        lblFileKipCount.Text = grdKIPfile.Items.Count.ToString();
//        CompareRowsCount();
//    }
 
//    protected void btnMainImport_Click(object sender, EventArgs e)
//    {
//        try
//        {
//            double key;
//            string errStr;
//            double.TryParse(DDLDates.SelectedValue, out key);
//            OracleImport_ImpKip oi = new OracleImport_ImpKip();
//            //oi.ApplyKipKeyMap(key, CreateLinkKeys(), out errStr);
//            oi.ApplyKipKeyMap(key, out errStr);
//            if (!string.IsNullOrEmpty(errStr))
//            {
//                ShowError(errStr);
//                return;
//            }

//            oi.ImportKipMeasurement(key, out errStr);
//            if (!string.IsNullOrEmpty(errStr))
//            {
//                ShowError(errStr);
//                return;
//            }

//            lblWarning.Text = (string)GetLocalResourceObject("cSuccessImport");
//        }
//        catch(Exception ex)
//        {
//            ShowError(GetLocalResourceObject("cErrorImporting") + ex.Message);
//        }
//    }

//    protected void btnDownload_Click(object sender, EventArgs e)
//    {
//        if (DDLMG.SelectedValue != "-1" && DDLThread.SelectedValue != "-1" && DDLDates.SelectedValue != "-1")
//        {
//            if (!IsFileSelected())
//                return;

//            //string strFileName = FileField.PostedFile.FileName;
//            string strFileName = upldKipFromFile.UploadedFiles[0].FileName;
//            string fullFileName = PageOperation_ImpKip.GetFileName() + System.IO.Path.GetExtension(strFileName);

//            if (!IsExcelExtension(strFileName))
//                return;

//            TrySaveAsFile(fullFileName);

//            double kipJournalKey;
//            double.TryParse(DDLDates.SelectedValue, out kipJournalKey);

//            //Коммент для упрощенной версии
//            //bool isCorrent = TryCreateNewKip(strFileName, kipJournalKey);
//            //if (!isCorrent)
//            //    return;

//            //Временное решение от 01.10.2014 г. - удаляем данные из RAW-таблицы перед ее наполнением
//            string errMsg = String.Empty;
//            OracleImport_ImpKip oi = new OracleImport_ImpKip();
//            oi.DeleteKipImportByImportKey(kipJournalKey, out errMsg);

//            //Пишем данные в RAW-таблицу
//            XlsParse_ImpKip xls = TryImportFileToXlsParse(fullFileName, kipJournalKey);
//            if (xls == null)
//                return;

//            //Временное решение от 01.10.2014 г., т.к. пока временно реперы увязываются в БД
//            //TryFillGrdKipFileFromDb(kipJournalKey);
//        }
//        else
//        {
//            lblWarning.Text = (string)GetLocalResourceObject("cNotSelectedParams");
//        }
//    }

//    private bool IsFileSelected()
//    {
//        bool isFileDownloaded =  upldKipFromFile.UploadedFiles.Count > 0;
//        if(!isFileDownloaded)
//            lblWarning.Text = (string)GetLocalResourceObject("cFileNotUploaded");

//        return isFileDownloaded;
//    }

//    protected void btnCompare_Click(object sender, EventArgs e)
//    {
//        AutoCompareTables();
//        DeleteDoubleData();
//        CompareRowsCount();
//    }
//    #endregion

//    //Перегружаем "Культуру" для данной страницы (этот метод вызвается самым первым, раньше всех других)
//    protected override void InitializeCulture()
//    {
//        //Выставляем "Культуру" для данной страницы, в зависимости выбранного ранее пользователем (берем из сессионной переменной Session["lang"])
//        if (Session["lang"] != null)
//        {
//            String selectedLanguage = Session["lang"].ToString();
//            UICulture = selectedLanguage;
//            Culture = selectedLanguage;

//            Thread.CurrentThread.CurrentCulture =
//                CultureInfo.CreateSpecificCulture(selectedLanguage);
//            Thread.CurrentThread.CurrentUICulture = new
//                CultureInfo(selectedLanguage);

//        }
//        base.InitializeCulture();
//    }

//}