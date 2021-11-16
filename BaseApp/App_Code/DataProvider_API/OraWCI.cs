using System;

/// <summary>
/// Summary description for OraWCI
/// </summary>
public class OraWCI
{
    public string InSQL { get; set; }
    public string ModuleName { get; set; }
    public string InParams { get; set; }
    public int InType { get; set; }
    public int IsCompress { get; set; }
    public string ContentType { get; set; }
    public int IsCheckXsd { get; set; }

    public OraWCI(string inSQL, string moduleName, string inParams, string inType,
        string isCompress, string contentType, string isCheckXsd)
    {
        InSQL = inSQL;
        ModuleName = moduleName;
        InParams = inParams;
        InType = Convert.ToInt32(inType);
        IsCompress = Convert.ToInt32(isCompress);
        ContentType = contentType;
        IsCheckXsd = Convert.ToInt32(isCheckXsd);
    }
}