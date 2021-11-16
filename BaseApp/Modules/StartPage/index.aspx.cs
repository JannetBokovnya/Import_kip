using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App_Code.SessionStorage;

public partial class Modules_StartPage_index : System.Web.UI.Page, IBaseClass
{
    string p_cModuleName = string.Empty;
    protected string logo_title { get; set; }
    protected string logo_link { get; set; }

    protected void Page_Init(object sender, EventArgs e)
    {
        SetModuleName("STARTPAGE");
        ((Master as System_TopMasterPage).FindControl("hdnWindowOpen") as HiddenField).ValueChanged += new EventHandler(Modules_StartPage_index_ValueChanged);
        (((Master as System_TopMasterPage).FindControl("Toolbar") as UserControls_Toolbar_Toolbar).FindControl("BookmarkMenu") as UserControls_Menu_BookmarksMenu_Bookmarks).HiddenFieldChanged += new UserControls_Menu_BookmarksMenu_Bookmarks.BookmarkChangedEventHandler(Modules_StartPage_index_HiddenFieldChanged);

        (Master as System_TopMasterPage).BookmarkStarPage = "applicationLoaded();";

        System.Configuration.Configuration imageConfig =
            System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/images/");
        if (imageConfig.AppSettings.Settings.Count > 0)
        {
            System.Configuration.KeyValueConfigurationElement logo_title_key =
                imageConfig.AppSettings.Settings["logo_title"];
            System.Configuration.KeyValueConfigurationElement logo_link_key =
                imageConfig.AppSettings.Settings["logo_link"];
            if (logo_title_key != null && logo_link_key != null)
            {
                logo_title = logo_title_key.Value;
                logo_link = logo_link_key.Value;
                if (!String.IsNullOrEmpty (logo_title) && !String.IsNullOrEmpty (logo_link))
                {
                    Image1.ToolTip = logo_title;
                    Image1.Attributes.Add ("onclick", "window.open('" + logo_link + "');");
                }
            }
        }
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

    protected void Modules_StartPage_index_HiddenFieldChanged(object sender, EventArgs e)
    {
        pnlBookMark.GetUserBookmarks(LoginSession.UserName);
        upnlBookmark.Update();  
    }

    void Modules_StartPage_index_ValueChanged(object sender, EventArgs e)
    {
        List<string> lstWindows = new List<string>();
        List<string> lstTitles = new List<string>();

        JSONParser.ParseOpenWindows((this.Master.FindControl("hdnWindowOpen") as HiddenField).Value, ref lstWindows, ref lstTitles);

        pnlOpenWindows.CheckActiveWindows(lstWindows, lstTitles);
        upnlWindows.Update();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ((Master as System_TopMasterPage).FindControl("MasterBody") as System.Web.UI.HtmlControls.HtmlGenericControl).Attributes.Add("onload", "alert('Hi')");

            List<string> lstWindows = new List<string>();
            List<string> lstTitles = new List<string>();

            JSONParser.ParseOpenWindows((this.Master.FindControl("hdnWindowOpen") as HiddenField).Value, ref lstWindows, ref lstTitles);

            pnlOpenWindows.CheckActiveWindows(lstWindows, lstTitles);
            pnlBookMark.GetUserBookmarks(LoginSession.UserName);
            //fio.Text = "ФИО пользователя : " + LoginSession.UserFIO;

        }
    }

    protected void upnlBookmark_Load(object sender, EventArgs e)
    {
        pnlBookMark.GetUserBookmarks(LoginSession.UserName);
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
}