using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataProvider_API.Enums;

/// <summary>
/// Summary description for IEngine
/// </summary>
public interface IEngine
{
    byte[] GetData(OraWCI oraWciParams, SupportTypes.ResponseTypes returnType);
}