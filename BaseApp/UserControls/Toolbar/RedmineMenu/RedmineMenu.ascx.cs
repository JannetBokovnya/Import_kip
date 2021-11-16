using System;

public partial class UserControls_Toolbar_RedmineMenu_RedmineMenu : System.Web.UI.UserControl
{
    public string ModuleName { private get; set; }
    protected string RedmineUrl = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bool isProd;
            RedmineUrl = WebConfig.GetRedmineUrl(out isProd);

            if (String.IsNullOrEmpty(RedmineUrl))
                FiddenRedmineMenu();
            else if (!isProd)
                FormUrlForModule();

            rmRedmine.OnClientItemClicked = "openRedmine";
        }
    }

    private void FiddenRedmineMenu()
    {
        this.Visible = false;
    }

    private void FormUrlForModule()
    {
        var localResourceObject = GetLocalResourceObject(ModuleName);
        if (localResourceObject != null)
            RedmineUrl += "/" + localResourceObject + "/issues/new";
    }
}