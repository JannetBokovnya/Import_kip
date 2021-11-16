using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using App_Code.Admin_module_API;
using App_Code.SessionStorage;
using System.Threading;
using System.Globalization;
using log4net;

public partial class Modules_Admin_ResetPassword : System.Web.UI.Page
{
    private Literal _failureText;
    private static readonly ILog Log = LogManager.GetLogger(typeof(Auth).Name);

    protected Admin_module_API.Connection conn = new Admin_module_API.Connection();

    protected void Page_Load(object sender, EventArgs e)
    {
        //Manually register the event-handling methods.
        //chepa.ChangingPassword += new LoginCancelEventHandler(this._ChangingPassword);

        //Выставляем выпадающий список выбора языка в необходимое положение, в зависимости выбранного ранее пользователем
        if (Session["lang"] != null)
        {
            Label lblSetNewPassword = (Label)chepa.ChangePasswordTemplateContainer.FindControl("lblSetNewPassword");
            lblSetNewPassword.Text = (string)GetLocalResourceObject("chepaResource1.ChangePasswordTitleText");
        }

        if (!IsPostBack)
        {
            if (_failureText == null)
                _failureText = chepa.ChangePasswordTemplateContainer.FindControl("FailureText") as Literal;
        }
    }

    protected void Page_PreRender (object sender, EventArgs e)
    {
        //if (_failureText == null)
        //    _failureText = chepa.ChangePasswordTemplateContainer.FindControl("FailureText") as Literal;
    }


    //Перегружаем "Культуру" для данной страницы (этот метод вызвается самым первым, раньше всех других)
    protected override void InitializeCulture()
    {
        //Выставляем "Культуру" для данной страницы, в зависимости выбранного ранее пользователем (берем из сессионной переменной Session["lang"])
        if (Session["lang"] != null)
        {
            String selectedLanguage = Session["lang"].ToString();
            UICulture = selectedLanguage;
            Culture = selectedLanguage;

            Thread.CurrentThread.CurrentCulture =
                CultureInfo.CreateSpecificCulture(selectedLanguage);
            Thread.CurrentThread.CurrentUICulture = new
                CultureInfo(selectedLanguage);

        }
        base.InitializeCulture();
    }

    protected void chepa_OnChangingPassword(Object sender, LoginCancelEventArgs e)
    {
        try
        {
            _failureText = chepa.ChangePasswordTemplateContainer.FindControl("FailureText") as Literal;

            if (chepa.CurrentPassword == chepa.NewPassword)
            {
                _failureText.Text = (string)GetLocalResourceObject("MsgDifferentPasspords");
                e.Cancel = true;
            }
            else if (chepa.NewPassword.Length >= 6)
            {
                if (!Regex.Match(chepa.NewPassword, @"\p{IsCyrillic}|\p{IsCyrillicSupplement}").Success)
                {
                    Auth auth = new Auth();
                    Int64 userKey = auth.AuthUser(LoginSession.UserName, chepa.CurrentPassword);
                    if (userKey > 0)
                    {
                        int result = auth.ChangePassword(chepa.NewPassword);
                        if (result >= 0)
                        {
                            string goBack = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath +
                                            "/Admin_module/Login.aspx";
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                            e.Cancel = true;
                            Response.Redirect(goBack,false);
                        }
                        else
                        {
                            if (_failureText != null) _failureText.Text = (string)GetLocalResourceObject("MsgDidntChangePassword");
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        if (_failureText != null) _failureText.Text = (string)GetLocalResourceObject("MsgUnCorrectOldPassword");
                        e.Cancel = true;
                    }
                }
                else
                {
                    if (_failureText != null) _failureText.Text = (string)GetLocalResourceObject("MsgNoCyrillic");
                    e.Cancel = true;
                }
            }
            else
            {
                if (_failureText != null) _failureText.Text = (string)GetLocalResourceObject("MsgShortPassword");
                e.Cancel = true;
            }
        }
        catch (Exception ex)
        {
            if (_failureText != null) _failureText.Text = (string)GetLocalResourceObject("MsgError");
            Log.Error(ex);
            e.Cancel = true;
        }
       
    }
}