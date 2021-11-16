using System.Configuration;
using System.Data;
using App_Code.SessionStorage;

/// <summary>
/// Summary description for DbConnAuth
/// </summary>
public class DbConnAuth
{
    private static DBConn.Factory<DBConn.Conn> databaseFactory = new DBConn.Factory<DBConn.Conn>();
    private static readonly string _authMethod = DbConnAuth_query.LoginQuery;
    private static readonly string _deAuthMethod = "";//DbConnAuth_query.CleanSessionParamsQuery;

    public DBConn.Conn connOra()
    {
        string dbType = ConfigurationManager.AppSettings["DBType"];
        DBConn.Conn connOra = databaseFactory.CreateObject(dbType);
        connOra.ConnectionString(WebConfig.GetDBConnection());
        DBConn.DBParam[] oipAuth = null;
        if (WebConfig.AuthMode() == "Enable")
        {
            oipAuth = DbAuth();
            connOra.Authorization(_authMethod, oipAuth, _deAuthMethod);
        }
        return connOra;
    }
 
    private DBConn.DBParam[] DbAuth()
    {
        #region

        DBConn.DBParam[] oipAuth = new DBConn.DBParam[10];

        oipAuth[0] = new DBConn.DBParam
        {
            ParameterName = "in_cLogin",
            Direction = ParameterDirection.Input,
            DbType = DBConn.DBTypeCustom.String,
            Value = LoginSession.UserLogin
        };

        oipAuth[1] = new DBConn.DBParam
        {
            ParameterName = "in_cPwd",
            Direction = ParameterDirection.Input,
            DbType = DBConn.DBTypeCustom.String,
            Value = LoginSession.UserPassword
        };

        oipAuth[2] = new DBConn.DBParam
        {
            ParameterName = "in_cLang",
            Direction = ParameterDirection.Input,
            DbType = DBConn.DBTypeCustom.String,
            Value = LoginSession.UserLang
        };

        oipAuth[3] = new DBConn.DBParam
        {
            ParameterName = "in_cLoginAD",
            Direction = ParameterDirection.Input,
            DbType = DBConn.DBTypeCustom.String,
            Value = LoginSession.ADUserLogin
        };

        oipAuth[4] = new DBConn.DBParam
        {
            ParameterName = "in_cIpAdress",
            DbType = DBConn.DBTypeCustom.String,
            Value = UserParams.IpAdress
        };

        oipAuth[5] = new DBConn.DBParam
        {
            ParameterName = "in_cCompName",
            DbType = DBConn.DBTypeCustom.String,
            Value = UserParams.CompName
        };

        oipAuth[6] = new DBConn.DBParam
        {
            ParameterName = "in_cSessionKey",
            DbType = DBConn.DBTypeCustom.String,
            Value = UserParams.SessionKey
        };

        oipAuth[7] = new DBConn.DBParam
        {
            ParameterName = "in_cBrowserName",
            DbType = DBConn.DBTypeCustom.String,
            Value = UserParams.BrowserName
        };

        oipAuth[8] = new DBConn.DBParam
        {
            ParameterName = "out_cResult",
            DbType = DBConn.DBTypeCustom.String,
            Size = 1000,
            Direction = ParameterDirection.Output,
        };

        oipAuth[9] = new DBConn.DBParam
        {
            ParameterName = "out_nUserKey",
            DbType = DBConn.DBTypeCustom.String,
            Size = 1000,
            Direction = ParameterDirection.Output,
        };
        return oipAuth;
        
        #endregion
        #region OraAuth
        //DBConn.DBParam[] oipAuth = new DBConn.DBParam[5];
        //oipAuth[0] = new DBConn.DBParam();
        //oipAuth[0].ParameterName = "in_nUserGUID";
        //oipAuth[0].Direction = ParameterDirection.Input;
        //oipAuth[0].DbType = DBConn.DBTypeCustom.String;
        //oipAuth[0].Value = LoginSession.UserGuid;

        //oipAuth[1] = new DBConn.DBParam();
        //oipAuth[1].ParameterName = "in_cIpAdress";
        //oipAuth[1].Direction = ParameterDirection.Input;
        //oipAuth[1].DbType = DBConn.DBTypeCustom.String;                         
        //oipAuth[1].Value = UserParams.IpAdress;

        //oipAuth[2] = new DBConn.DBParam();
        //oipAuth[2].ParameterName = "in_cCompName";
        //oipAuth[2].Direction = ParameterDirection.Input;
        //oipAuth[2].DbType = DBConn.DBTypeCustom.String;
        //oipAuth[2].Value = UserParams.CompName;

        //oipAuth[3] = new DBConn.DBParam();
        //oipAuth[3].ParameterName = "in_cSesstionKey";
        //oipAuth[3].Direction = ParameterDirection.Input;
        //oipAuth[3].DbType = DBConn.DBTypeCustom.String;
        //oipAuth[3].Value = UserParams.SessionKey;

        //oipAuth[4] = new DBConn.DBParam();
        //oipAuth[4].ParameterName = "in_nBrowserName";
        //oipAuth[4].Direction = ParameterDirection.Input;
        //oipAuth[4].DbType = DBConn.DBTypeCustom.String;
        //oipAuth[4].Value = UserParams.BrowserName;


        //return oipAuth;
        #endregion
    }
}