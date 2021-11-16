using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using DBConn;

/// <summary>
/// Summary description for DataHelper
/// </summary>
public class DataHelper
{
    public static OracleConnection GetOracleConnection()
    {

        //string conStr = WebConfigurationManager.ConnectionStrings["Ora_meta"].ConnectionString;
        string conStr = WebConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
        OracleConnection conn = new OracleConnection(conStr);
        conn.Open();
        return conn;
    }

    public static OracleCommand GetStoredProcCommand(string spName)
    {
        OracleCommand sp = new OracleCommand(spName);
        sp.CommandType = CommandType.StoredProcedure;
        return sp;
    }

    public static DataSet ExecQuery(OracleCommand cmd)
    {



        OracleConnection conn = GetOracleConnection();
        try
        {
            DBConn.DBParam[] oip = ConvertTypes(cmd);

            cmd.Connection = conn;

            #region Устанавливаем в БД сессию текущего пользователя.


            //oracleEngine.SetCurrentSession(conn, new AuthorisationAPP(UserSession.UserGuid, UserSession.IpAdress,
            //UserSession.CompName, UserSession.SessionKey, UserSession.BrowserName));

            #endregion

            DataSet ds = new DataSet();
            OracleDataAdapter adapter = new OracleDataAdapter(cmd);
            adapter.Fill(ds);
            return ds;


        }
        finally
        {
            conn.Close();
            conn.Dispose();
        }
    }

    /// <summary>
    /// ExecNonQuery() - не возвращает значения
    /// </summary>
    /// <param name="Query"></param>
    /// <param name="oip"></param>
    public static void ExecNonQuery(string Query, ref DBConn.DBParam[] oip, IDbConnection conn = null, IDbTransaction tr = null)
    {
        try
        {
            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(GetConnectionString());
            connOra.ExecuteNonQuery(Query, ref oip, conn, tr);

        }
        catch (OracleException e)
        {
            
            throw;
        }
    }

    public static string ExecNonQuery1(string query)
    {

        string ErrMsg = "";
        try
        {

            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(GetConnectionString());
            //connOra.ExecuteNonQuery(query);
            int result = connOra.ExecuteTextQuery<Int32>(query);
        }
        catch (OracleException e)
        {

            ErrMsg = "Ошибка выполнения запроса. " + e.Message;
        }
        return ErrMsg;
    }

    /// <summary>
    /// Перегруженный метод ExecQuery() - return Int (без входных параметров)
    /// </summary>
    /// <param name="Query"></param>
    /// <param name="result"></param>
    public static void ExecQuery(string Query, ref int result)
    {
        try
        {
            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(GetConnectionString());
            result = connOra.ExecuteQuery<int>(Query);

        }
        catch (OracleException e)
        {
            
            throw;
        }
    }

    /// <summary>
    /// Перегруженный метод ExecQuery() - return DataSet (без агрумента oip)
    /// </summary>
    /// <param name="Query"></param>
    /// <param name="ds"></param>
    public static void ExecQuery(string Query, ref DataSet ds)
    {
        try
        {


            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(GetConnectionString());
            DataTable dt = connOra.ExecuteQuery<DataTable>(Query);
            ds.Tables.Add(dt);


        }
        catch (OracleException e)
        {
            
            throw;
        }
    }

    /// <summary>
    /// Перегруженный метод ExecQuery() - return DataSet
    /// </summary>
    /// <param name="Query"></param>
    /// <param name="oip"></param>
    /// <param name="ds"></param>
    public static void ExecQuery(string Query, ref DBConn.DBParam[] oip, ref DataSet ds)
    {
        try
        {


            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(GetConnectionString());
            DataTable dt = connOra.ExecuteQuery<DataTable>(Query, oip);
            ds.Tables.Add(dt);


        }
        catch (OracleException e)
        {
            
            throw;
        }
    }

    /// <summary>
    /// Перегруженный метод ExecQuery() - return Int
    /// </summary>
    /// <param name="Query"></param>
    /// <param name="oip"></param>
    /// <param name="result"></param>
    public static void ExecQuery(string Query, ref DBConn.DBParam[] oip, ref int result)
    {
        try
        {
            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(GetConnectionString());
            result = connOra.ExecuteQuery<int>(Query, oip);

        }
        catch (OracleException e)
        {
           
            throw;
        }
    }



    /// <summary>
    /// Перегруженный метод ExecQuery() - return Double
    /// </summary>
    /// <param name="Query"></param>
    /// <param name="oip"></param>
    /// <param name="result"></param>
    public static void ExecQuery(string Query, ref DBConn.DBParam[] oip, ref double result)
    {
        try
        {
            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(GetConnectionString());
            result = connOra.ExecuteQuery<double>(Query, oip);

        }
        catch (OracleException e)
        {
           
            throw;
        }
    }

    /// <summary>
    /// Перегруженный метод ExecQuery() - return String
    /// </summary>
    /// <param name="Query"></param>
    /// <param name="oip"></param>
    /// <param name="result"></param>
    public static void ExecQuery(string Query, ref DBConn.DBParam[] oip, ref string result)
    {
        try
        {
            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();

            connOra.ConnectionString(GetConnectionString());
            result = connOra.ExecuteQuery<string>(Query, oip);

        }
        catch (OracleException e)
        {
           
            throw;
        }
    }

    /// <summary>
    /// Преобразовываем массив параметров OraInputParam[] в DBConn.DBParam[]
    /// </summary>
    /// <param name="InputParams"></param>
    /// <returns></returns>
    public static DBConn.DBParam[] ConvertTypes(OracleCommand cmd)
    {
        OracleParameterCollection oracleParameterCollection = cmd.Parameters;
        DBConn.DBParam[] oip = (oracleParameterCollection.Count != 0) ? new DBConn.DBParam[oracleParameterCollection.Count] : null;
        if (oracleParameterCollection.Count != 0)
        {
            for (int i = 0; i < oracleParameterCollection.Count; i++)
            {
                oip[i] = new DBConn.DBParam();
                oip[i].ParameterName = oracleParameterCollection[i].ParameterName;
                if (oracleParameterCollection[i].OracleType == OracleType.VarChar)
                {
                    oip[i].DbType = DBConn.DBTypeCustom.String;
                }
                else if (oracleParameterCollection[i].OracleType == OracleType.Number)
                {
                    oip[i].DbType = DBConn.DBTypeCustom.Int32;
                }
                else if (oracleParameterCollection[i].OracleType == OracleType.Double)
                {
                    oip[i].DbType = DBConn.DBTypeCustom.Double;
                }
                else
                {
                    oip[i].DbType = new DBTypeCustom(Convert.ToInt16(oracleParameterCollection[i].OracleType), 0);
                    //oip[i].DbType = (DBTypeCustom)oracleParameterCollection[i].OracleType;
                }
                oip[i].Size = oracleParameterCollection[i].Size;
                oip[i].Direction = oracleParameterCollection[i].Direction;
                oip[i].Value = oracleParameterCollection[i].Value;
            }
        }
        return oip;
    }

    ///// <summary>
    ///// выполнение запросов с произольным кол-вом входных параметров
    ///// </summary>
    ///// <param name="Query">запрос(серверный)</param>
    ///// <param name="InputParams">список параметров</param>
    ///// <param name="conString">строка подключения к бд</param>
    ///// <param name="ds">сылка на датасет</param>
    ///// <param name="needReturnValue">нужно ли возвращать значения</param>
    ///// <param name="ErrMsg">ошибка выполнения</param>
    //public static constructEngine(string Query, DBConn.DBParam[] oip, string conString, ref DataSet ds,
    //                            bool needReturnValue, out string ErrMsg)
    //{
    //    ErrMsg = "";

    //    try
    //    {
    //        DbConnAuth dbConnAuth = new DbConnAuth();
    //        DBConn.Conn connOra = dbConnAuth.connOra();
    //        connOra.ConnectionString(GetConnectionString());
    //        if (needReturnValue)
    //        {
    //            DataTable dt = connOra.ExecuteQuery<DataTable>(Query, oip);
    //            ds.Tables.Add(dt);
    //        }
    //        else
    //        {
    //            connOra.ExecuteNonQuery(Query, oip);
    //        }

    //    }
    //    catch
    //    {
    //        throw;
    //    }

    //}



    public static string GetConnectionString()
    {
        return WebConfig.GetDBConnection();
    }
}