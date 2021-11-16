<%@ Application Language="C#" %>
<%@ Import Namespace="System.Web.Routing" %>

<script RunAt="server">

    void Application_Start(object sender, EventArgs e)
    {

        log4net.Config.XmlConfigurator.Configure();

        //Роутинг кастомарных адресов (их маппинг на реальные адреса)
        RouteTable.Routes.MapPageRoute("StoreRouteXlsExport", "XLSExporter/XLS/export", "~/Modules/XLSCreator/index.aspx");
        RouteTable.Routes.MapPageRoute("StoreRouteXlsVerifyFile", "XLSExporter/XLS/verifyfile", "~/Modules/XLSCreator/index.aspx");
        RouteTable.Routes.MapPageRoute("StoreRouteXlsImport", "XLSExporter/XLS/import", "~/Modules/XLSCreator/index.aspx");
    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs
        try
        {
            HttpException ex = (HttpException)Server.GetLastError();
            if (ex.GetHttpCode() == 403)
            {
                System.Configuration.Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("");
                if (config.AppSettings.Settings.Count > 0)
                {
                    string startPage = config.AppSettings.Settings["startPage"].ToString();
                    Response.Redirect(startPage);
                }
            }
        }
        catch
        {
            Exception ex = Server.GetLastError();
        }

    }

    void Session_Start(object sender, EventArgs e)
    {
        // Administrator will only be allowed a certain number of login attempts
        Session["MaxLoginAttempts"] = 3;
        Session["LoginCount"] = 0;

        // Track whether they're logged in or not
        Session["LoggedIn"] = "No";/**/

    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>
