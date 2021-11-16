using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataProvider_API;

/// <summary>
/// Summary description for IParceQuery
/// </summary>
public interface  IParceQuery
{
    EngineApplication.QueryObject DoParceQuery(string inSQL);
}