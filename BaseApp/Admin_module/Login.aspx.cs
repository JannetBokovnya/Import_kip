using System;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;
using App_Code.Admin_module_API;
using App_Code.SessionStorage;
//using Admin_module_API;

public partial class Admin_module_Default : System.Web.UI.Page
{
    protected Admin_module_API.Connection conn = new Admin_module_API.Connection();
    protected string LogoTitle { get; set; }
    protected string LogoLink { get; set; }

    protected void Page_Init(object sender, EventArgs e)
    {
        System.Configuration.Configuration imageConfig =
                System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/images/");
        if (imageConfig.AppSettings.Settings.Count > 0)
        {
            System.Configuration.KeyValueConfigurationElement logoTitleKey =
                                                    imageConfig.AppSettings.Settings["logo_title"];
            System.Configuration.KeyValueConfigurationElement logoLinkKey =
                                                    imageConfig.AppSettings.Settings["logo_link"];
            if (logoTitleKey != null)
                LogoTitle = logoTitleKey.Value;
            if (logoLinkKey != null)
                LogoLink = logoLinkKey.Value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (WebConfig.GetIsShowMessageForGuest())
            ((Literal)pnlLogin.FindControl("FailureText")).Text = GetLocalResourceObject("cMessageGuestPassword").ToString();

        ((RadioButtonList)pnlLogin.FindControl("rblLanguage")).Visible = WebConfig.GetMultiLangParam();

        if (HttpContext.Current.User.Identity.IsAuthenticated)
        {
            //Забисать в базу событие выхода пользователя 
            //const string eventName = "ADM_LOGOUT";
            //DateTime dateNow = DateTime.Now;
            //string errMsg;
            //conn.CreateEvent(eventName, dateNow.ToString(), "", out errMsg);
            FormsAuthentication.SignOut();
            if (Request.Cookies["Auth"] != null)
            {
                HttpCookie myCookie = new HttpCookie("Auth") { Expires = DateTime.Now.AddDays(-1d) };
                Response.Cookies.Add(myCookie);
            }
        }

        //Выставляем выпадающий список выбора языка в необходимое положение, в зависимости выбранного ранее пользователем
        if (Session["lang"] != null)
        {
            RadioButtonList rblLanguage = (RadioButtonList)pnlLogin.FindControl("rblLanguage");
            switch (Session["lang"].ToString())
            {
                case "ru-RU":
                    rblLanguage.SelectedIndex = 0;
                    break;
                case "en-US":
                    rblLanguage.SelectedIndex = 1;
                    break;
            }
        }

        if (!IsPostBack)
        {
            //Добавление параметров сессии пользователя
            UserSession.SetSessionParamsIfNull(Session, Request);
        }
    }

    //Перегружаем "Культуру" для данной страницы (этот метод вызвается самым первым, раньше всех других)
    protected override void InitializeCulture()
    {

        if (Request.Form["ctl00$Content$pnlLogin$rblLanguage"] != null)//Если пользователь выбрал язык в окне Логина
        {
            String selectedLanguage = Request.Form["ctl00$Content$pnlLogin$rblLanguage"];
            UICulture = selectedLanguage;
            Culture = selectedLanguage;

            Thread.CurrentThread.CurrentCulture =
                CultureInfo.CreateSpecificCulture(selectedLanguage);
            Thread.CurrentThread.CurrentUICulture = new
                CultureInfo(selectedLanguage);

            //Сохраняем выбранную пользователем культуру в сессионной переменной Session["lang"]
            HttpContext.Current.Session["lang"] = (selectedLanguage == "ru-RU") ? "ru-RU" : "en-US";
        }
        else if (HttpContext.Current.Session["lang"] != null)//Если выполнен переход на страницу Логина (нажата кнопка "Выйти")
        {
            String selectedLanguage = Session["lang"].ToString();
            UICulture = selectedLanguage;
            Culture = selectedLanguage;

            Thread.CurrentThread.CurrentCulture =
                CultureInfo.CreateSpecificCulture(selectedLanguage);
            Thread.CurrentThread.CurrentUICulture = new
                CultureInfo(selectedLanguage);

        }
        else//Во всех других случаях, когда сессионная переменная Session["lang"] - пуста
        {
            //Сохраняем в сессии культуру "русского языка" (по-умолчанию)
            HttpContext.Current.Session["lang"] = "ru-RU";
        }
        base.InitializeCulture();
    }

    /// <summary>
    /// возвращает предыдущий УРЛ 
    /// </summary>
    public string ReturnUrl()
    {
        string returnUrl = string.Empty;
        if (!String.IsNullOrEmpty(Request["ReturnUrl"]) && Request["ReturnUrl"] != "/")
        {
            returnUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request["ReturnUrl"];
        }
        return returnUrl;
    }


    //Клик на кнопке "Отмена".
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //просто очистить
    }

    protected void pnlLogin_LoggingIn(object sender, LoginCancelEventArgs e)
    {
    }

    protected void LoginControl_Authenticate(object sender, AuthenticateEventArgs e)
    {
        var login = sender as Login;
        if (login != null) Authenticate(login.UserName, login.Password);
    }

    private void Authenticate(string strLogin, string strPassword)
    {
        if (!String.IsNullOrEmpty(strLogin))
        {
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
            //        LoginSession.UserName = strLogin;
            //        string setNewPassword = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath +
            //                               "/Admin_module/ResetPassword.aspx";

            //        Response.Redirect(setNewPassword);
            //        break;
            //    case -2:
            //        var localResourceObject = GetLocalResourceObject ("pnlLoginResource1.ErrorConn");
            //        if (localResourceObject != null)
            //            pnlLogin.FailureText = localResourceObject.ToString();
            //        break;
            //    case -1:
            //    case 1:
            Int64 userKey = auth.AuthUser(strLogin, strPassword);
            //проверяем на совпадение пароля -1 - ошибка не совпадают пароли
            if (userKey == -1 | userKey == -3)
            {
                var resourceObject = GetLocalResourceObject("pnlLoginResource1.ErrorEnterData");
                if (resourceObject != null)
                    pnlLogin.FailureText = resourceObject.ToString();
            }
            else if (userKey == -2)
            {
                var resourceObject = GetLocalResourceObject("pnlLoginResource1.Blocked");
                if (resourceObject != null)
                    pnlLogin.FailureText = resourceObject.ToString();
            }
            else if (userKey > 0)
            {
                string strUserFIO = "";
                strUserFIO = auth.GetFIO();
                // Initialize FormsAuthentication, for what it's worth
                FormsAuthentication.Initialize();
                FormsAuthentication.SetAuthCookie(userKey.ToString(), false);

                LoginSession loginSession = new LoginSession();
                loginSession.SetUserSession("Authorized", userKey.ToString(), strLogin, strUserFIO);

                HttpCookie myCookie = new HttpCookie("Auth");
                if (myCookie != null)
                {
                    myCookie.Values.Add("Status", "Authorized");
                    myCookie.Values.Add("GUID", Admin_module_API.Cryptography.Encrypt(userKey.ToString()));
                    myCookie.Values.Add("UserName", strLogin);
                    var name = strUserFIO;
                    var bytes = Encoding.GetEncoding("Windows-1251").GetBytes(name);
                    myCookie.Values.Add("UserFIO", Convert.ToBase64String(bytes));//strUserFIO);
                    myCookie.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(myCookie);
                }


                //обавить вычитку пути из  мастер окна
                string redirectTo = ReturnUrl();
                if (string.IsNullOrEmpty(redirectTo))
                {
                    redirectTo = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath +
                                 "/" + WebConfig.GetStartPage();
                }

                //Забисать в базу событие авторизации пользователя   
                const string eventName = "ADM_LOGIN";
                string errMsg;
                string date = XmlForDbCreator.GetDateTimeNow();
                conn.CreateEvent(eventName, date, "", out errMsg);
                Response.Redirect(redirectTo);
            }
            //break;
            //}
        }
    }

}