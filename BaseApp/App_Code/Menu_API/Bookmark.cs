using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Bookmark
/// </summary>
public class Bookmark
{
    private Int32 _linkKey;
    private object _stateID;
    private string _appName;
    private string _appTitle;
    private string _user;

    public Bookmark()
    {
    }

    public Bookmark(string stateID, string appName, string appTitle, Int32 linkKey)
    {
        _stateID = stateID;
        _appName = appName;
        _appTitle = appTitle;
        _user = string.Empty;
        _linkKey = linkKey;
    }

    public Int32 LinkKey { get { return _linkKey; } set { _linkKey = value; } }
    public object StateID { get { return _stateID; } set { _stateID = value; } }
    public string AppName { get { return _appName; } set { _appName = value; } }
    public string AppTitle { get { return _appTitle; } set { _appTitle = value; } }
    public string User { get { return _user; } set { _user = value; } }

    public static string ToJSON(Bookmark bookmark)
    {
        var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
        return jss.Serialize(bookmark);
    }

    public static Bookmark FromJSON(string strBookmark)
    {
        Bookmark bookmark = new Bookmark();

        Newtonsoft.Json.Linq.JObject obj = Newtonsoft.Json.Linq.JObject.Parse(strBookmark);

        bookmark.StateID = obj["StateID"];
        bookmark.AppName = (string)obj["AppName"];
        bookmark.AppTitle = (string)obj["AppTitle"];
        bookmark.User = (string)obj["User"];
        //var jss = new System.Web.Script.Serialization.JavaScriptSerializer();

        return bookmark;
    }
}