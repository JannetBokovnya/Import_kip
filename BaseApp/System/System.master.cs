using System;

public partial class System_System : System.Web.UI.MasterPage
{
    protected string logo_title { get; set; }
    protected string logo_link { get; set; }

    protected void Page_Init(object sender, EventArgs e)
    {
        System.Configuration.Configuration imageConfig =
            System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/images/");
        if (imageConfig.AppSettings.Settings.Count > 0)
        {
            System.Configuration.KeyValueConfigurationElement logo_title_key =
                imageConfig.AppSettings.Settings["logo_title"];
            System.Configuration.KeyValueConfigurationElement logo_link_key =
                imageConfig.AppSettings.Settings["logo_link"];
            logo_title = logo_title_key.Value;
            logo_link = logo_link_key.Value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}
