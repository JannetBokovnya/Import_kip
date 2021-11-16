using System;
using DataProvider_API;
using DataProvider_API.Enums;

public partial class DataProvider_OraWCI : System.Web.UI.Page
{
    protected OraWCI OraWciParams;

    protected void Page_Load(object sender, EventArgs e)
    {
        // request value
        string _cInSql = Request["inSQL"];
        string moduleName = Request["inModuleName"];
        string cInParams = Request["inParams"];
        string inType = Request["inType"];
        string isCompress = Request["isCompress"];
        string contentType = Request["ContentType"];
        string isCheckXsd = Request["isCheckXsd"] ?? "1";

        OraWciParams = new OraWCI(_cInSql, moduleName, cInParams, inType, isCompress, contentType, isCheckXsd);
    }


    protected string GetContentType ()
    {
        string l_res = string.Empty;
         if (ContentType == null)
         {
             l_res = "text/xml";
         }
         else
         {
             l_res = ContentType;
         }
        return l_res;
    }


    protected byte[] GetMainResBinary()
    {
        byte[] l_res = null;
        string errMsg = string.Empty;
        string oraWciType = string.Empty;
        IEngine engine = null;
        engine = new EngineApplication();
       
        DataProvider dataProvider = new DataProvider(engine);

        SupportTypes.ResponseTypes returnType = SupportTypes.ResponseTypes.Xml;

        l_res = dataProvider.BuildResponse(OraWciParams, returnType, "");

        if (!string.IsNullOrEmpty(errMsg))
        {
            // l_res = getExption(errMsg);
        }
        
        return l_res;
    }

    


    private string getExption(string errMsg)
    {
        string l_res = string.Empty;
        l_res = "<ROWSET><Result>False</Result><R><ERROR>" + errMsg + "</ERROR></R></ROWSET>";
        return l_res;
    }
}