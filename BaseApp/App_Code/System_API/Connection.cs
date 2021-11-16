using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for connection
/// </summary>
namespace App_Code.System_API
{
    public class Connection
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DataProvider).Name);
        private string con_meta = WebConfig.GetDBConnection();

        public int CheckAccess(string moduleName)
        {
            
            //DBConn.DBParam[] oip = new DBConn.DBParam[1];
            ////подстановка входного параметра
            //oip[0] = new DBConn.DBParam();
            //oip[0].ParameterName = "p_cModuleName";
            //oip[0].DbType = DBConn.DBTypeCustom.String;
            //oip[0].Value = moduleName;

            //DbConnAuth dbConnAuth = new DbConnAuth();
            //try
            //{
            //    DBConn.Conn connOra = dbConnAuth.connOra();
            //    access = connOra.ExecuteQuery<int>("gis_meta_api.admin_api.checkmoduleaccess", oip);
            //}
            //catch (Exception e)
            //{
            //    log.Error(e);
            //    access = 0;
            //}

            int access = 1;
            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            oip[0] = new DBConn.DBParam
            {
                ParameterName = "in_cModuleName",  //"p_cmodulename",
                DbType = DBConn.DBTypeCustom.VarChar2,
                Direction = ParameterDirection.Input,
                Value = moduleName
            };

            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn dbconn = dbConnAuth.connOra();
            try
            {
                access = dbconn.ExecuteQuery<int>("db_api.WRAPPER_API.checkModuleAccess", oip);
            }
            catch (Exception e)
            {
               // log.Error("AAAAAAAAAAAAAПроверка на то, может открывать данный УРЛ Ошибка =" + e.ToString());
                access = -1;
                throw e;
            }

         //   log.Error("AAAAAAAAAAAAAAПроверка на то, может открывать данный УРЛ результат =" + access.ToString());

            return access;
        }
    }
}