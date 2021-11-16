using System;
using log4net;

namespace Admin_module_API
{
    public class Connection
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DataProvider).Name);

        public void CreateEvent(string inCInnerName, string inCUserInfo, string inCAdmInfo, out string errMsg)
        {
            errMsg = string.Empty;
            DBConn.DBParam[] cmd = new DBConn.DBParam[3];
            //подстановка входного параметра
            cmd[0] = new DBConn.DBParam
            {
                ParameterName = "in_cInnerName",
                DbType = DBConn.DBTypeCustom.String,
                Value = inCInnerName
            };

            cmd[1] = new DBConn.DBParam
            {
                ParameterName = "in_cUserInfo",
                DbType = DBConn.DBTypeCustom.String,
                Value = inCUserInfo
            };

            cmd[2] = new DBConn.DBParam
            {
                ParameterName = "in_cAdmInfo",
                DbType = DBConn.DBTypeCustom.String,
                Value = inCAdmInfo
            };

            DbConnAuth dbConnAuth = new DbConnAuth();
            try
            {
                DBConn.Conn dbconn = dbConnAuth.connOra();
                dbconn.ExecuteNonQuery(Admin_mod_querys.CreateeventQuery, cmd);
            }
            catch (Exception e)
            {
                Log.Error(e);
                errMsg = e.ToString();
            }
        }
    }
}