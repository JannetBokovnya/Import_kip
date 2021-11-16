using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataProvider_API;

/// <summary>
/// Summary description for ParceQueryDb
/// </summary>
public class ParceQueryDb :IParceQuery
{


    public EngineApplication.QueryObject DoParceQuery(string inSQL)
    {
        EngineApplication.QueryObject query = new EngineApplication.QueryObject();

        string callStatement = string.Empty;
        string arguments = string.Empty;

        string[] inSplit = inSQL.Split('(');
        callStatement = inSplit[0];
        if (inSplit.Length == 2)
        {
            arguments = inSplit[1];
        }
        else if (inSplit.Length > 2)
        {
            throw new NotImplementedException();
        }

        int iSchemaEnd = callStatement.IndexOf('.', 0);

        query.Owner = callStatement.Substring(0, iSchemaEnd);

        int dotDelim = callStatement.Length - (callStatement.Replace(".", "")).Length;
        // parce call statement
        if (dotDelim == 2)
        {
            int packEnd = callStatement.IndexOf('.', iSchemaEnd + 1);
            query.PackageName = callStatement.Substring(iSchemaEnd + 1, (packEnd - iSchemaEnd) - 1);
            query.ObjectName = callStatement.Substring(packEnd + 1);
        }
        else if (dotDelim == 1)
        {
            query.PackageName = "";
            query.ObjectName = callStatement.Substring(iSchemaEnd + 1);
        }
        else
        {
            throw new NotImplementedException();
        }
        // parce arguments
        if (arguments.Length > 0)
        {
            //query.Arguments = (arguments.Substring(0, (arguments.Length - 1))).Split(',');
            query.Arguments = ParserWithIgnoreJsObject(arguments.Substring(0, (arguments.Length - 1)));
        }
        return query;
    }

    private string[] ParserWithIgnoreJsObject (string str)
    {
        List <string> arr = new List <string> ();
        int index = 0;
        int firstChar = 0;
        for (int i = 0; i < str.Length; i++ )
        {
            char c = str [i];
            if (c == '{')
                index++;
            if (c == '}')
                index--;
            if (c == ',' && index == 0)
            {
                arr.Add (str.Substring (firstChar, i - firstChar));
                firstChar = i + 1; // отнимаю запятую
                if (firstChar < str.Length && str[firstChar].ToString() == " ")
                    firstChar = firstChar + 1; //отнимаю пробел, если он есть
            }
        }
        arr.Add(str.Substring(firstChar, str.Length - firstChar));
        return arr.ToArray();
    }
}