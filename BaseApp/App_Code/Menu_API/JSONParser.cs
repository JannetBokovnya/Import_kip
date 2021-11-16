using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for JSONParser
/// </summary>
public class JSONParser
{
    public JSONParser()
    {
    }

    public static void ParseOpenWindows(string strJSON, ref List<string> lstWindows, ref List<string> lstTitles)
    {
        var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
        var lst = jss.Deserialize<dynamic>(strJSON);
        if (lst != null)
        {
            int count = (lst["WindowStatus"] as System.Array).Length;
            for (int i = 0; i < count; i++)
            {
                if (lst["WindowStatus"][i]["WindowName"] != "STARTPAGE")
                {
                    lstWindows.Add(lst["WindowStatus"][i]["WindowName"]);
                    lstTitles.Add(lst["WindowStatus"][i]["WindowTitle"]);
                }
            }
        }
    }

    public static List<InfoOpenModule> ParseOpenWindows(string strJSON)
    {
        var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
        var lst = jss.Deserialize<dynamic>(strJSON);
        List<InfoOpenModule> res = new List<InfoOpenModule>();
        if (lst != null)
        {
            int count = (lst["WindowStatus"] as System.Array).Length;
            for (int i = 0; i < count; i++)
            {
                if (lst["WindowStatus"][i]["WindowName"] != "STARTPAGE")
                {
                    res.Add(new InfoOpenModule(lst["WindowStatus"][i]["WindowName"], lst["WindowStatus"][i]["WindowTitle"]));
                }
            }
        }
        return res;
    }

    public static List<string> ParseAllWindows(string strJSON, string strOpenModule)
    {
        List<string> lstWindows = new List<string>();
        var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
        var lst = jss.Deserialize<dynamic>(strJSON);
        if (lst != null)
        {
            int count = (lst["WindowStatus"] as System.Array).Length;
            for (int i = 0; i < count; i++)
            {
                if (lst["WindowStatus"][i]["WindowName"] != strOpenModule)
                {
                    lstWindows.Add(lst["WindowStatus"][i]["WindowName"]);
                }
            }
        }
        return lstWindows;
    }
}