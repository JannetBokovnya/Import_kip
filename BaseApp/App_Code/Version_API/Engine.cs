using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace App_Code.Version_API
{
    public class Engine
    {
        public string GetAppName(string p_csystemname)
        {
            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(WebConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);
            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "p_cSystemName";
            oip[0].DbType = DBConn.DBTypeCustom.VarChar;
            oip[0].Value = p_csystemname;

            string l_res = connOra.ExecuteQuery<string>("gis_meta_api.ver_system_api.getsystemnamebyname", oip);
            return l_res;
        }

        public string GetAppVersion(string p_csystemname)
        {
            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(WebConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);
            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "p_cSystemName";
            oip[0].DbType = DBConn.DBTypeCustom.VarChar;
            oip[0].Value = p_csystemname;

            string l_res = connOra.ExecuteQuery<string>("gis_meta_api.ver_system_api.getsystemversion", oip);
            return l_res;
        }

        public string GetAppCopyright(string p_csystemname)
        {
            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(WebConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);
            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "p_cSystemName";
            oip[0].DbType = DBConn.DBTypeCustom.VarChar;
            oip[0].Value = p_csystemname;

            string l_res = connOra.ExecuteQuery<string>("gis_meta_api.ver_system_api.getcopyright", oip);
            return l_res;
        }
    }
}