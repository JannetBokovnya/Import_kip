using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

/// <summary>
/// Summary description for ContextNavigation
/// </summary>
public class ContextNavigation
{
    private string idMg;
    private string nameMg;

    private string idThread;
    private string nameThread;
    private string nameThreadShorten;

    private string kmStart;
    private string kmEnd;

	public ContextNavigation()
	{
	}

    public ContextNavigation(string IdMg, string NameMg, string IdThread, string NameThread, string NameThreadShorten, string KmStart, string KmEnd)
    {
        idMg = IdMg;
        nameMg = NameMg;
        idThread = IdThread;
        nameThread = NameThread;
        nameThreadShorten = NameThreadShorten;
        kmStart = KmStart;
        kmEnd = KmEnd;
    }

    public string IdMg { get { return idMg; } set { idMg = value; } }
    public string NameMg { get { return nameMg; } set { nameMg = value; } }
    public string IdThread { get { return idThread; } set { idThread = value; } }
    public string NameThread { get { return nameThread; } set { nameThread = value; } }
    public string NameThreadShorten { get { return nameThreadShorten; } set { nameThreadShorten = value; } }
    public string KmStart { get { return kmStart; } set { kmStart = value.Replace(".", ","); } }
    public string KmEnd { get { return kmEnd; } set { kmEnd = value.Replace(".", ","); } }

    public string GetContextNavigation() 
    {
        double kmS, kmE;
        bool res = true;
        if (double.TryParse(KmStart, NumberStyles.AllowDecimalPoint, CultureInfo.GetCultureInfo("ru-RU").NumberFormat, out kmS))
        {
            kmS = Math.Round(kmS, 2);
        }
        else
        {
            res = false;
        }
        if (double.TryParse(KmEnd, NumberStyles.AllowDecimalPoint, CultureInfo.GetCultureInfo("ru-RU").NumberFormat, out kmE))
        {
            kmE = Math.Round(kmE, 2);
        }
        else
        {
            res = false;
        }

        if (res)
        {
            return nameThreadShorten + "\t " + kmS.ToString() + " - " + kmE.ToString() + " м";
        }
        else
        {
            return nameThreadShorten + "\t " + kmS.ToString() + " - " + kmE.ToString() + " м";
        }
    } 

    public static string ToJSON(ContextNavigation contextNavigation)
    {
        var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
        return jss.Serialize(contextNavigation);
    }

    public string ToJSON()
    {
        var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
        return jss.Serialize(this);
    }

    public static ContextNavigation FromJSON(string strContextNavigation)
    {
        var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
        return jss.Deserialize<ContextNavigation>(HttpUtility.UrlDecode(strContextNavigation));

    }
}