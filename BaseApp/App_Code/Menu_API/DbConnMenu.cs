using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using log4net;
using System.Data;
using System.Web.UI.WebControls;
using System.Globalization;

public struct ThreadMG
{
    string _idMG;
    string _nameThread;
    string _nameThreadShorten;

    public string IdMG { get { return _idMG; } }
    public string NameThread { get { return _nameThread; } }
    public string NameThreadShorten { get { return _nameThreadShorten; } }

    public ThreadMG(string idMG, string nameThread, string nameThreadShorten)
    {
        _idMG = idMG;
        _nameThread = nameThread;
        _nameThreadShorten = nameThreadShorten;
    }
}

/// <summary>
/// Summary description for DbConnMenu
/// </summary>
public class DbConnMenu
{
    private static readonly ILog log = LogManager.GetLogger(typeof(DataProvider).Name);

    public DbConnMenu()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    //загрузить всё меню
    public static void LoadLeftMenuItems(ref Telerik.Web.UI.RadMenu pnlMain)
    {
        DbConnAuth dbConnAuth = new DbConnAuth();
        DBConn.Conn connOra = dbConnAuth.connOra();
        connOra.ConnectionString(WebConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);
        DataTable dt = connOra.ExecuteQuery<DataTable>(Menu_querys.GetapplicationsQuery);
        //надо всё занести в нормальную иерархию
        List<LeftMenuItem> lstLeftMenuItem = new List<LeftMenuItem>();
        foreach (DataRow dr in dt.Rows)
        {
            lstLeftMenuItem.Add(new LeftMenuItem(dr["CDESCRIPTION"].ToString(), 
                                                 dr["CAPPNAME"].ToString(), 
                                                 dr["CSTYLE"].ToString(), 
                                                 dr["NKEY"].ToString(), 
                                                 dr["NPARENTKEY"].ToString()));
        }
        pnlMain.Items.AddRange((new HierarchicalDataSource(lstLeftMenuItem)).TopMenuItems());
    }

    //загрузить всё меню
    public static List<Telerik.Web.UI.RadMenuItem> LoadMainMenuItems()
    {
        DbConnAuth dbConnAuth = new DbConnAuth();
        DBConn.Conn connOra = dbConnAuth.connOra();
        connOra.ConnectionString(WebConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);
        DataTable dt = connOra.ExecuteQuery<DataTable>(Menu_querys.GetfullmenuQuery);
        List<LeftMenuItem> lstLeftMenuItem = new List<LeftMenuItem>();
        foreach (DataRow dr in dt.Rows)
        {
            lstLeftMenuItem.Add(new LeftMenuItem(dr["CNAME"].ToString(), 
                                                 dr["CACTION"].ToString(), 
                                                 dr["CSTYLE"].ToString(), 
                                                 dr["NID"].ToString(), 
                                                 dr["NPARENTID"].ToString()));
        }
        return (new HierarchicalDataSource(lstLeftMenuItem)).exampleMenuList;
        //lstFilteredItems.Items.AddRange(hDS.exampleMenuList);
    }

    public static List<Telerik.Web.UI.RadMenuItem> LoadTopLevelMenu()
    {
       // DbConnAuth dbConnAuth = new DbConnAuth();
      //  DBConn.Conn connOra = dbConnAuth.connOra();
      //  connOra.ConnectionString(WebConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);
      //  DataTable dt = connOra.ExecuteQuery<DataTable>(Menu_querys.GetmainmenuQuery);
        List<LeftMenuItem> lstLeftMenuItem = new List<LeftMenuItem>();
        //foreach (DataRow dr in dt.Rows)
        //{
        //    //"~/Modules/Passport/Passport.aspx"
        //    lstLeftMenuItem.Add(new LeftMenuItem(dr["CNAME"].ToString(), dr["CACTION"].ToString(), dr["CSTYLE"].ToString(), dr["NID"].ToString(), dr["NPARENTID"].ToString()));
        //    //lstLeftMenuItem.Add(new LeftMenuItem(dr["CNAME"].ToString(), dr["CACTION"].ToString(), dr["CSTYLE"].ToString(), dr["NID"].ToString(), dr["NPARENTID"].ToString()));
        //}
        return (new HierarchicalDataSource(lstLeftMenuItem)).exampleMenuList;
    }

    public static List<Bookmark> GetBookmarks(string strUserKey)
    {
        DbConnAuth dbConnAuth = new DbConnAuth();
        DBConn.Conn connOra = dbConnAuth.connOra();
        connOra.ConnectionString(WebConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);

        //DBConn.DBParam[] oip = new DBConn.DBParam[1];

        //oip[0] = new DBConn.DBParam();
        //oip[0].ParameterName = "p_nuserkey";
        //oip[0].DbType = DBConn.DBTypeCustom.Int32;
        //oip[0].Value = App_Code.SessionStorage.LoginSession.UserGuid;

        //DataTable dt = connOra.ExecuteQuery<DataTable>(Menu_querys.GetState4User_rsQuery, oip);
        DataTable dt = connOra.ExecuteQuery<DataTable>(Menu_querys.GetState4User_rsQuery);

        List<Bookmark> lstBookmarks = new List<Bookmark>();
        foreach (DataRow dr in dt.Rows)
        {
            if (!string.IsNullOrEmpty(dr["cMessage"].ToString()))
            {
                lstBookmarks.Add(new Bookmark(dr["CSTATE"].ToString(), dr["CAPPNAME"].ToString(), dr["cMessage"].ToString(), Convert.ToInt32(dr["nStateId"])));
            }
        }
        return lstBookmarks;
    }

    public static List<Telerik.Web.UI.RadMenuItem> LoadDocItems()
    {
        DbConnAuth dbConnAuth = new DbConnAuth();
        DBConn.Conn connOra = dbConnAuth.connOra();
        connOra.ConnectionString(WebConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);

        DBConn.DBParam[] oip = new DBConn.DBParam[0];

        DataTable dt = connOra.ExecuteQuery<DataTable>(Menu_querys.GetHelpMenuQuery, oip);

        //List<Bookmark> lstBookmarks = new List<Bookmark>();
        //foreach (DataRow dr in dt.Rows)
        //{
        //    lstBookmarks.Add(new Bookmark(dr["CACTION"].ToString(), dr["CACTION"].ToString(), dr["cName"].ToString(), Convert.ToInt32(dr["nNodeKey"])));
        //}



        List<LeftMenuItem> lstLeftMenuItem = new List<LeftMenuItem>();
        foreach (DataRow dr in dt.Rows)
        {
            lstLeftMenuItem.Add(new LeftMenuItem(dr["CNAME"].ToString(), dr["CACTION"].ToString(), dr["CSTYLE"].ToString(), dr["NID"].ToString(), dr["NPARENTID"].ToString()));
        }
        return (new HierarchicalDataSource(lstLeftMenuItem)).exampleMenuList;




        //return lstBookmarks;
    }

    public static void AddBookmark(Bookmark bookmark)
    {
        DbConnAuth dbConnAuth = new DbConnAuth();
        DBConn.Conn connOra = dbConnAuth.connOra();
        connOra.ConnectionString(WebConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);

        DBConn.DBParam[] oip = new DBConn.DBParam[5];

        oip[0] = new DBConn.DBParam();
        oip[0].ParameterName = "p_cState";
        oip[0].DbType = DBConn.DBTypeCustom.String;
        oip[0].Value = bookmark.StateID.ToString();

        oip[1] = new DBConn.DBParam();
        oip[1].ParameterName = "p_cAppName";
        oip[1].DbType = DBConn.DBTypeCustom.String;
        oip[1].Value = bookmark.AppName;

        oip[2] = new DBConn.DBParam();
        oip[2].ParameterName = "p_cMessage";
        oip[2].DbType = DBConn.DBTypeCustom.String;
        oip[2].Value = bookmark.AppTitle;

        oip[3] = new DBConn.DBParam();
        oip[3].ParameterName = "p_nUserKey";
        oip[3].DbType = DBConn.DBTypeCustom.Int32;
        oip[3].Value = bookmark.User;

        oip[4] = new DBConn.DBParam();
        oip[4].ParameterName = "p_nisshare";
        oip[4].DbType = DBConn.DBTypeCustom.Int32;
        oip[4].Value = 0;

        int i = connOra.ExecuteQuery<int>(Menu_querys.AddNewStateQuery, oip);
    }

    public static void DeleteBookmark(int strStateLinkKey)
    {
        DbConnAuth dbConnAuth = new DbConnAuth();
        DBConn.Conn connOra = dbConnAuth.connOra();
        connOra.ConnectionString(WebConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);

        DBConn.DBParam[] oip = new DBConn.DBParam[1];

        oip[0] = new DBConn.DBParam();
        oip[0].ParameterName = "p_NSTATELINKKEY";
        oip[0].DbType = DBConn.DBTypeCustom.Int32;
        oip[0].Value = strStateLinkKey;

        connOra.ExecuteNonQuery(Menu_querys.DeleteStateQuery, oip);
    }

    public static void DeliveredNotification(string keyNotification)
    {
        DbConnAuth dbConnAuth = new DbConnAuth();
        DBConn.Conn connOra = dbConnAuth.connOra();
        connOra.ConnectionString(WebConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);

        DBConn.DBParam[] oip = new DBConn.DBParam[1];

        oip[0] = new DBConn.DBParam();
        oip[0].ParameterName = "p_nnotification";
        oip[0].DbType = DBConn.DBTypeCustom.Number;
        oip[0].Value = keyNotification;

        connOra.ExecuteNonQuery(Menu_querys.DeliveredNotification, oip);
    }

    public static List<Notification> GetNotifications(string strUserKey, double curDate)
    {
        List<Notification> lstNtf = new List<Notification>();

        DbConnAuth dbConnAuth = new DbConnAuth();
        DBConn.Conn connOra = dbConnAuth.connOra();
        connOra.ConnectionString(WebConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);

        DBConn.DBParam[] oip = new DBConn.DBParam[1];

        //oip[0] = new DBConn.DBParam();
        //oip[0].ParameterName = "p_nUserKey";
        //oip[0].DbType = DBConn.DBTypeCustom.Int32;
        //oip[0].Value = App_Code.SessionStorage.LoginSession.UserGuid;

        oip[0] = new DBConn.DBParam();
        oip[0].ParameterName = "in_nDateUnixTime";
        oip[0].DbType = DBConn.DBTypeCustom.Int32;
        oip[0].Value = curDate;

        DataTable dt = connOra.ExecuteQuery<DataTable>(Menu_querys.Getnotification4user_timeQuery, oip);

        foreach (DataRow dr in dt.Rows)
        {
            if (!string.IsNullOrEmpty(dr["cMessage"].ToString()))
            { 
                lstNtf.Add(new Notification(Convert.ToDouble(dr["nnotiticationkey"]), dr["cmessage"].ToString(), dr["cappname"].ToString(), dr["cstate"].ToString(), strUserKey, Convert.ToDouble(dr["dcreatedate"]), Convert.ToBoolean(0)));
            }
        }

        return lstNtf;
    }

    public static List<Notification> GetNotificationsAfterCurDate(string strUserKey, double curDate)
    {
        List<Notification> lstNtf = new List<Notification>();

        DbConnAuth dbConnAuth = new DbConnAuth();
        DBConn.Conn connOra = dbConnAuth.connOra();
        connOra.ConnectionString(WebConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);

        DBConn.DBParam[] oip = new DBConn.DBParam[2];

        //oip[0] = new DBConn.DBParam();
        //oip[0].ParameterName = "p_nUserKey";
        //oip[0].DbType = DBConn.DBTypeCustom.Int32;
        //oip[0].Value = App_Code.SessionStorage.LoginSession.UserGuid;

        oip[0] = new DBConn.DBParam();
        oip[0].ParameterName = "in_nDateUnixTime";
        oip[0].DbType = DBConn.DBTypeCustom.Int32;
        oip[0].Value = curDate;

        DataTable dt = connOra.ExecuteQuery<DataTable>(Menu_querys.Getnotification4user_timeQuery, oip);

        foreach (DataRow dr in dt.Rows)
        {
            if (!string.IsNullOrEmpty(dr["cMessage"].ToString()))
            {
                if (String.IsNullOrEmpty(dr["isdelivered"].ToString()))
                    dr["isdelivered"] = 0;
                
                lstNtf.Add(new Notification(Convert.ToDouble(dr["nnotification"]), dr["cmessage"].ToString(), dr["cappname"].ToString(), dr["cstate"].ToString(), strUserKey, Convert.ToDouble(dr["dcreatedate"]), Convert.ToBoolean(dr["isdelivered"])));
            }
        }

        return lstNtf;
    }

    #region context navigation

    public static Dictionary<string, string> GetListMG()
    {
        Dictionary<string, string> lstRes = new Dictionary<string, string>();

        DbConnAuth dbConnAuth = new DbConnAuth();
        DBConn.Conn connOra = dbConnAuth.connOra();
        connOra.ConnectionString(WebConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);

        DataTable dt = connOra.ExecuteQuery<DataTable>(Menu_querys.GetmgQuery);

        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                if (!string.IsNullOrEmpty(dr["CNAME"].ToString()))
                {
                    lstRes.Add(dr["NKEY"].ToString(), dr["CNAME"].ToString());
                }
            }
        }
        return lstRes;
    }

    public static List<ThreadMG> GetListThread(string idMG)
    {

        List<ThreadMG> lstRes = new List<ThreadMG>();

        DbConnAuth dbConnAuth = new DbConnAuth();
        DBConn.Conn connOra = dbConnAuth.connOra();
        connOra.ConnectionString(WebConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);

        DBConn.DBParam[] oip = new DBConn.DBParam[1];

        oip[0] = new DBConn.DBParam();
        oip[0].ParameterName = "in_nMgKey";
        oip[0].DbType = DBConn.DBTypeCustom.Int64;
        oip[0].Value = idMG;
        //oip[0].DbType = DBConn.DBTypeCustom.Number;
        //oip[0].Value = Convert.ToDouble(idMG);
        


        DataTable dt = connOra.ExecuteQuery<DataTable>(Menu_querys.GetthreadsbymgQuery, oip);
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                if (!string.IsNullOrEmpty(dr["CNAME"].ToString()))
                {
                    lstRes.Add(new ThreadMG(dr["NKEY"].ToString(), dr["CNAME"].ToString(), dr["CSHORTNAME"].ToString()));
                }
            }
        }

        return lstRes;
    }

    public static Dictionary<string, string> GetListKmStartEnd(string idThread)
    {
        Dictionary<string, string> lstRes = new Dictionary<string, string>();

        DbConnAuth dbConnAuth = new DbConnAuth();
        DBConn.Conn connOra = dbConnAuth.connOra();
        connOra.ConnectionString(WebConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);

        DBConn.DBParam[] oip = new DBConn.DBParam[1];

        oip[0] = new DBConn.DBParam();
        oip[0].ParameterName = "in_nThreadKey";
        oip[0].DbType = DBConn.DBTypeCustom.Int64;
        oip[0].Value = idThread;
        //oip[0].DbType = DBConn.DBTypeCustom.Number;
        //oip[0].Value = Convert.ToDouble(idThread);
        

        DataTable dt = connOra.ExecuteQuery<DataTable>(Menu_querys.GetthreadkmQuery, oip);

        foreach (DataRow dr in dt.Rows)
        {
            if (!String.IsNullOrEmpty (dr ["NKM_BEG"].ToString()))
                lstRes.Add("KmStart", Math.Round(Convert.ToDouble(dr["NKM_BEG"], CultureInfo.GetCultureInfo("ru-RU").NumberFormat), 2).ToString());
            else
                lstRes.Add("KmStart", "-1");
            if (!String.IsNullOrEmpty(dr["NKM_END"].ToString()))
                lstRes.Add("KmEnd", Math.Round(Convert.ToDouble(dr["NKM_END"], CultureInfo.GetCultureInfo("ru-RU").NumberFormat), 2).ToString());
            else
                lstRes.Add("KmEnd", "-1");
        }

        return lstRes;
    }

    #endregion
}