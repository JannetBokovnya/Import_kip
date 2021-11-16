using System;
using System.Data;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using App_Code.Admin_module_API;
using App_Code.SessionStorage;
using App_Code.System_API;
using log4net;
using Telerik.Web.UI;

public partial class System_TopMasterPage : System.Web.UI.MasterPage, IBaseClass
{
    protected string lang = "";
    protected Admin_module_API.Connection conn = new Admin_module_API.Connection();
    protected string showContextMenu = "false";
    protected string showOpenedModulesMenu = "false";
    protected string showBookmarksMenu = "false";
    private static readonly ILog Log = LogManager.GetLogger(typeof(System_TopMasterPage).Name);
    private string _moduleName = string.Empty;
    private string _bookmarkStarPage = string.Empty;

    protected void Page_Init(object sender, EventArgs e)
    {
        // Проверка есть ли авторизация AD или через форму ЛогинПейдж
        // Если авторизации нет - пройти.
        if (LoginSession.UserLogin == "-1")
        {
            string msg = string.Empty;
            if (!AD_Auhorization(out msg))
            {
                Response.Redirect("~/" + System.Configuration.ConfigurationManager.AppSettings["LoginPage"]);
            }
            else
            {
                Response.Redirect("~/" + System.Configuration.ConfigurationManager.AppSettings["startPage"]);
            }
        }


        lang = Thread.CurrentThread.CurrentUICulture.ToString();
        RefreshSession();

        AddListeners();
        AddJSONJS();

        if (Toolbar.FindControl("BookmarkMenu") != null)
            divBookMark.Controls.Add(LoadControl("~/UserControls/Toolbar/BookmarksMenu/BookmarkPanel.ascx"));

        if (Toolbar.FindControl("contextNavigation") != null)
            divContextNavigation.Controls.Add(LoadControl("~/UserControls/Toolbar/ContextNavigationMenu/ContextNavigationPanel.ascx"));
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Toolbar.ModuleName = _moduleName;

            hdnUserFIO.Value = LoginSession.UserFIO;

            CheckIsInstanceStart();
           
            // проверка на то, может открывать данный УРЛ
            CheckModuleAccess();

            //инициализация информации о сессии
            UserSession.SetSessionParamsIfNull(Session, Request);

            SetTelerikUserSkin();
        }
        //добавить листенер
        AddListeners();
    }

    private void SetTelerikUserSkin()
    {
        string userSkin = WebConfig.GetTelerikUserSkin();
        if (!String.IsNullOrEmpty(userSkin))
        {
            ControlTypeToApplySkin[] arrUserControls = { ControlTypeToApplySkin.RadButton,
                                                         ControlTypeToApplySkin.RadComboBox,
                                                         ControlTypeToApplySkin.RadNumericTextBox,
                                                         ControlTypeToApplySkin.RadTextBox,
                                                         ControlTypeToApplySkin.RadListBox,
                                                         ControlTypeToApplySkin.RadMenu};
            RadSkinManager1.TargetControls.Clear();
            foreach (ControlTypeToApplySkin targetControl in arrUserControls)
            {
                RadSkinManager1.TargetControls.Add(targetControl, userSkin);
            }
        }
    }

    private bool AD_Auhorization(out string msg)
    {
        msg = "HttpContext.Current.User.Identity.Name.ToString()=" + HttpContext.Current.User.Identity.Name;
        if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
        {
            var currentUserName = HttpContext.Current.User.Identity.Name;
            msg = currentUserName;
            if (currentUserName.IndexOf("\\") != -1)// доменное имя пользователя имеет вид domainName\UserName
            {
                var userNameArray = currentUserName.Split('\\');

                if (userNameArray.Length >= 2)
                {
                    if (Authenticate(userNameArray[1])) //Если пользователь существует и если он не заблокирован.
                    {
                        HttpContext.Current.Session["lang"] = "ru-RU"; // Установка языка по умолчанию рус.
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private bool Authenticate(string strLogin)
    {
        bool isUserAuthenticated = false;

        if (!String.IsNullOrEmpty(strLogin))
        {
            //Запись пользователя Анонимус в сессии. 
            //ADUserLogin используется для нахождения ключа пользователя.
            LoginSession.UserLogin = "AD_USER";
            LoginSession.UserPassword = "KYQYFTf9fmqW$mJ";
            LoginSession.ADUserLogin = strLogin;

            //проверка на существование логина в базе
            Auth auth = new Auth();
            //int checkUser = auth.CheckUserLogin(strLogin);

            //LogonBasa logon = new LogonBasa();
            // string checkUser = logon.checkUserLogin(txtLogin.Text);
            //-1-такого логина нет;
            // 0 - есть логин но не задан пароль;
            // 1- все ок.
            //switch (checkUser)
            //{
            //    case 0:
            //        break;
            //    case -2:
            //       break;
            //    case -1:
            //    case 1:
            //       Log.Info("Обращение к функцие. Тут происходит ошибка. auth.GetUserId. " + LoginSession.UserLogin + " "
            //                                                                               + LoginSession.UserPassword + " "
            //                                                                               + LoginSession.UserLang + " "
            //                                                                               + LoginSession.ADUserLogin + " "
            //                                                                               + UserParams.IpAdress + " "
            //                                                                               + UserParams.CompName + " "
            //                                                                               + UserParams.SessionKey + " "
            //                                                                               + UserParams.BrowserName + " ");
            int userKey = auth.GetUserId(strLogin);
            if (userKey > 0)
            {
                string strUserFIO = auth.GetFIO();
                // Initialize FormsAuthentication, for what it's worth
                FormsAuthentication.Initialize();
                FormsAuthentication.SetAuthCookie(userKey.ToString(), false);

                LoginSession loginSession = new LoginSession();
                loginSession.SetUserSession("Authorized", userKey.ToString(), strLogin, strUserFIO);

                var httpCookie = Response.Cookies["Auth"];
                if (httpCookie != null)
                {
                    httpCookie["Status"] = "Authorized";
                    httpCookie["GUID"] = Admin_module_API.Cryptography.Encrypt(userKey.ToString());
                    httpCookie["UserName"] = strLogin;
                    var name = strUserFIO;
                    var bytes = Encoding.GetEncoding("Windows-1251").GetBytes(name);
                    httpCookie["UserFIO"] = Convert.ToBase64String(bytes);//strUserFIO;
                    httpCookie.Expires = DateTime.Now.AddDays(1);
                }

                //Забисать в базу событие авторизации пользователя   
                DateTime dateNow = DateTime.Now;
                const string eventName = "ADM_LOGIN";
                string errMsg;
                conn.CreateEvent(eventName, dateNow.ToString(), "", out errMsg);
                isUserAuthenticated = true;
            }

            //break;
            //}
        }
        return isUserAuthenticated;
    }

    public string ModuleName
    {
        get { return _moduleName; }
    }

    public string BookmarkStarPage
    {
        get { return _bookmarkStarPage; }
        set { _bookmarkStarPage = value; }
    }


    public HiddenField HiddenFieldMaster
    {
        get { return hdnWindowOpen; }
        set { hdnWindowOpen = value; }
    }

    public HtmlGenericControl body
    {
        get { return this.MasterBody; }
    }

    /// <summary>
    /// добавить библиотеку JSON2, так как не во все браузеры поддерживают нативную
    /// </summary>
    private void AddJSONJS()
    {
        Page.ClientScript.RegisterClientScriptInclude(this.GetType(), "JSON2", ResolveClientUrl("~/Scripts/JSON2/JSON2.min.js"));
        Page.ClientScript.RegisterClientScriptInclude(this.GetType(), "Observer", ResolveClientUrl("~/Scripts/Observer/Observer.js"));
        Page.ClientScript.RegisterClientScriptInclude(this.GetType(), "Common", ResolveClientUrl("~/Scripts/Common/script.js"));
    }



    private void CheckIsInstanceStart()
    {
        string l_instanceNam = string.Empty;
        l_instanceNam = Request["instanceName"];
        if (!string.IsNullOrEmpty(l_instanceNam))
        {
            SetModuleName(l_instanceNam);
        }
    }

    void Modules_StartPage_index_HiddenFieldChanged(object sender, EventArgs e)
    {
    }

    private void AddListeners()
    {
        body.Attributes.Add("onload", "javascript: windowLoad('" + ModuleName + "');" + _bookmarkStarPage);
        body.Attributes.Add("onunload", "javascript: windowClosed('" + ModuleName + "');");
    }



    /// <summary>
    /// возвращает предыдущий УРЛ 
    /// </summary>
    public string ReturnURL()
    {
        string returnURL = string.Empty;
        returnURL = Request["ReturnUrl"];
        return returnURL;
    }

    /// <summary>
    /// текущий УРЛ пользователя
    /// </summary>
    /// <returns></returns>
    private string GetReturnPath()
    {
        return "?ReturnUrl=" + Request.Path;
    }


    public string TransfetToPage(string p_cAction)
    {
        string res = string.Empty;
        if (p_cAction == "LoginFail")
        {
            res = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/" + WebConfig.GetLoginPage() + GetReturnPath();
        }
        else if (p_cAction == "unAuthorized")
        {
            res = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/" + WebConfig.GetLoginPage() + GetReturnPath();
        }
        else if (p_cAction == "AuthorizedFail")
        {
            res = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/" + WebConfig.GetLoginPage() + GetReturnPath();
        }
        return res;
    }

    /// <summary>
    /// проверка на то, что модуль может быть запущен
    /// </summary>
    public void CheckModuleAccess()
    {

        //    if (_moduleName != "Login" && HttpContext.Current.Request.IsAuthenticated)
        if (_moduleName != "Login")
        {
            if (_moduleName != "STARTPAGE")
            {
                //нужно проанализировать на имя - если есть в имени цифры - то берем только имя
                int res;
                //string redirectTo = string.Empty;
                string moduleNameParce = string.Empty;
                string[] arr = _moduleName.Split('_');

                if (arr.Length > 1)
                {
                    int valInt;

                    if (int.TryParse(arr[1], out valInt))
                    {
                        moduleNameParce = arr[0];
                    }
                    else
                    {
                        moduleNameParce = _moduleName;
                    }
                }
                else
                {
                    moduleNameParce = _moduleName;
                }

                Connection con = new Connection();
                res = con.CheckAccess(moduleNameParce);

                if (res != 1)
                {
                    string redirect = WebConfig.GetErrorAuthPage();
                    Response.Redirect(redirect);
                }


            }

        }
    }


    //if (!HttpContext.Current.Request.IsAuthenticated)
    //{
    //    int res;
    //    string redirectTo = string.Empty;
    //    //Auth auth = new Auth();
    //    Connection con = new Connection();

    //    Log.Error("Проверка вызов функции самой проверки в базе CheckModuleAccessData, _moduleName = " + _moduleName);
    //    //res = auth.CheckModuleAccess(_moduleName);
    //    res = CheckModuleAccessData(_moduleName);
    //    //res = con.CheckAccess(_moduleName);

    //    Log.Error("Проверка, возврат фуекции значение res = " + res.ToString());

    //    if (res != 1)
    //    {
    //        string redirect = WebConfig.GetErrorAuthPage();
    //        Response.Redirect(redirect);
    //    }
    //}

    /// <summary>
    /// права доступа на просмотр модуля
    /// </summary>
    /// <param name="p_cModuleName">название Модуля</param>
    /// <returns></returns>
    public void SetModuleName(string p_cModuleName)
    {

        _moduleName = p_cModuleName;

    }

    //TODO: добавить вместо стринга moduleEvents объект
    /// <summary>
    /// Модуль фиксирует событие которое произошло в модулях
    /// </summary>
    /// <param name="moduleEvents">Название события, startEvent - модуль запущен, endEvent - модуль остановлен, errorEvent -ошибка в модуле, _othersEvent - остальные события </param>
    /// <param name="p_cModuleName"></param>
    /// <param name="p_cValue"></param>
    public void moduleEvent(string moduleEvents, string p_cModuleName, string p_cValue)
    {
    }

    private void RefreshSession()
    {
        if (HttpContext.Current.Session["UserGUID"] == null && Request.Cookies["Auth"] != null)
        {
            string status = "", userKey = "", userName = "", userFIO = "";
            if (Request.Cookies["Auth"]["Status"] != null)
                status = Request.Cookies["Auth"]["Status"];
            if (Request.Cookies["Auth"]["GUID"] != null)
                userKey = Admin_module_API.Cryptography.Decrypt(Request.Cookies["Auth"]["GUID"]);
            if (Request.Cookies["Auth"]["UserName"] != null)
                userName = Request.Cookies["Auth"]["UserName"];
            if (Request.Cookies["Auth"]["UserFIO"] != null)
                userFIO = Request.Cookies["Auth"]["UserFIO"];
            LoginSession loginSession = new LoginSession();
            loginSession.SetUserSession(status, userKey, userName, userFIO);
        }
    }

    protected string GetServiceName()
    {
        string l_res = string.Empty;
        string l_appPath = string.Empty;
        l_appPath = Request.ApplicationPath ?? "";
        l_res = string.IsNullOrEmpty(l_appPath) ? WebConfig.GetDataProvider()
                                                : l_appPath + WebConfig.GetDataProvider();
        return l_res;
    }


}