using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using DataProvider_API.Enums;
using log4net;

/// <summary>
/// Summary description for EngineDataBase
/// </summary>
public class EngineDataBase :IEngine
{
    private static readonly ILog log = LogManager.GetLogger(typeof(EngineDataBase).Name);


    public byte[] GetData(OraWCI oraWciParams, SupportTypes.ResponseTypes returnType)
    {
        byte[] lres=null;
        
        DBConn.DBParam[] oip = new DBConn.DBParam[5];

        oip[0] = new DBConn.DBParam();
        oip[0].ParameterName = "inSQL";
        oip[0].DbType = DBConn.DBTypeCustom.String;
        oip[0].Value = oraWciParams.InSQL;

        oip[1] = new DBConn.DBParam();
        oip[1].ParameterName = "inParams";
        oip[1].DbType = DBConn.DBTypeCustom.String;
        oip[1].Value = oraWciParams.InParams;

        oip[2] = new DBConn.DBParam();
        oip[2].ParameterName = "inType";
        oip[2].DbType = DBConn.DBTypeCustom.String;
        oip[2].Value = oraWciParams.InType;

        oip[3] = new DBConn.DBParam();
        oip[3].ParameterName = "isCompress";
        oip[3].DbType = DBConn.DBTypeCustom.String;
        oip[3].Value = oraWciParams.IsCompress;

        oip[4] = new DBConn.DBParam();
        oip[4].ParameterName = "result";
        oip[4].DbType = DBConn.DBTypeCustom.Blob;
        oip[4].Direction = ParameterDirection.ReturnValue;

        DbConnAuth dbConnAuth = new DbConnAuth();
        try
        {
            DBConn.Conn dbconn = dbConnAuth.connOra();
            lres = dbconn.ExecuteQuery<byte[]>("gmt_xml.owci.getxmlbysql", oip);
        }
        catch (Exception e)
        {
            log.Error(e);
            throw;
        }
        return lres;
    }
}