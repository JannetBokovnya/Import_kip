using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using DataProvider_API.Enums;
using log4net;

/// <summary>
/// Summary description for DataProvider
/// </summary>
public class DataProvider  
{
    private static readonly ILog Log = LogManager.GetLogger(typeof(DataProvider).Name);
    private IEngine _engine;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dataEngine"></param>
    public DataProvider(IEngine dataEngine)
    {
        this._engine = dataEngine;
    }

    public byte[] ConvertToJson(byte[] date)
    {
        throw new  NotImplementedException ();
    }

    public byte[] ConvertToXml(byte[] date)
    {
        throw new NotImplementedException();
    }


    public byte[] BuildResponse(OraWCI oraWciParams, SupportTypes.ResponseTypes returnType, string cVersion)
    {

        byte[] bytes = null;

        bytes = this._engine.GetData(oraWciParams, returnType);


        
        return bytes;
    }

}