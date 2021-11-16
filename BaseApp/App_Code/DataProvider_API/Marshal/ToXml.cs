using System;
using System.Data;
using System.Text;
using DataProvider_API.Marshal;

/// <summary>
/// Summary description for ToXml
/// </summary>
public static class ToXml
{
    public static byte[] DoConvert(DataTable dt, OraWCI oraWciParams)
    {
        string query = oraWciParams.InSQL.Replace("'", "");

        byte[] bytes = null;
        DataSet dataSet = new DataSet();

        foreach (DataColumn c in dt.Columns)
            c.ColumnName = c.ColumnName.ToUpper();

        dataSet.DataSetName = "ROWSET";
        dataSet.Tables.Add(dt);

        string result = dataSet.GetXml().Replace("<ROWSET>\r\n", "<ROWSET>\r\n <Result>True</Result> \r\n ");

        if (oraWciParams.IsCheckXsd == 1)
        {
            string errMsg = (new XmlValidator(oraWciParams)).CheckValid(result);
            if (!String.IsNullOrEmpty(errMsg))
            {
                result = "<ROWSET>\r\n <Result>False</Result> \r\n <R><ERROR>Результат не соответствует XSD\n"
                                        + errMsg + "</ERROR></R></ROWSET>";
            } 
        }

        bytes  = Encoding.UTF8.GetBytes(result);

        return bytes;
    }

    private static byte[] GetBytes(string str)
    {
        byte[] bytes = new byte[str.Length * sizeof(char)];
        System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
        return bytes;
    }
}