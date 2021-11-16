using System;

public partial class System_Admin : System.Web.UI.MasterPage
{
    protected string enterLogin;
    protected string enterPassword;
    protected string logo_title { get; set; }
    protected string logo_link { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        enterLogin = GetLocalResourceObject("enterLiginResource1.Text").ToString();
        enterPassword = GetLocalResourceObject("enterPasswordResource1.Text").ToString();

        if (!IsPostBack)
        {
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
                    if (!String.IsNullOrEmpty(logo_title) && !String.IsNullOrEmpty(logo_link))
                    {
                        Image1.ToolTip = logo_title;
                        Image1.Attributes.Add("onclick", "window.open('" + logo_link + "');");
                    }
                }
            }
        }
    }
}
