using System;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Xml.Linq;
using DataProvider_API.Enums;
using log4net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;

namespace DataProvider_API
{
    /// <summary>
    /// Summary description for EngineApplication
    /// </summary>
    public class EngineApplication : IEngine
    {

        private static readonly ILog Log = LogManager.GetLogger(typeof (EngineApplication).Name);

        public class QueryObject
        {
            public string[] Arguments { get; set; }

            public string ObjectName { get; set; }

            public string PackageName { get; set; }

            public string Owner { get; set; }
        }


        private string GetQueryMetaData(QueryObject query)
        {
          

            DBConn.DBParam[] oip = new DBConn.DBParam[4];
            oip[0] = new DBConn.DBParam
            {
                ParameterName = "in_cowner",
                DbType = DBConn.DBTypeCustom.String,
                Value = query.Owner
            };

            oip[1] = new DBConn.DBParam
            {
                ParameterName = "in_cobject_name",
                DbType = DBConn.DBTypeCustom.String,
                Value = query.ObjectName
            };

            oip[2] = new DBConn.DBParam
            {
                ParameterName = "in_cpackage_name",
                DbType = DBConn.DBTypeCustom.String,
                Value = query.PackageName
            };

            string args = (query.Arguments ==null) ? "": query.Arguments.ToString();

            oip[3] = new DBConn.DBParam
            {
                ParameterName = "in_carguments",
                DbType = DBConn.DBTypeCustom.String,
                Value = args
            };

            string lres = string.Empty;

            DbConnAuth dbConnAuth = new DbConnAuth();
            try
            {
                DBConn.Conn dbconn = dbConnAuth.connOra();
                lres = dbconn.ExecuteQuery<String>("getsignature", oip);
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }

            return lres;
        }

         
        public byte[] GetData(OraWCI oraWciParams, SupportTypes.ResponseTypes returnType)
        {
            byte[] bytes = null;
            CallSignature signature = null;
            QueryObject query = new QueryObject();
            IParceQuery querySql = new ParceQueryDb();
            query = querySql.DoParceQuery(oraWciParams.InSQL.Replace("'", ""));


            if (oraWciParams.InType == 2)
            {
                string methodSignature = GetQueryMetaData(query);
                signature = DecodeSignature(methodSignature);
                object returnParam;

                if (signature.TYPE.RETURN.ToUpper() == "CURSOR")
                {
                    returnParam = CallFunction<DataTable>(signature, query);
                    DataTable dt = (DataTable) returnParam;
                    dt.TableName = "R";
                    bytes = ToXml.DoConvert(dt, oraWciParams);
                }
                else if (signature.TYPE.RETURN.ToUpper() == "NUMBER" ||
                         signature.TYPE.RETURN.ToUpper() == "NUMERIC")
                {
                    returnParam = CallFunction<Double>(signature, query);
                    bytes = Encoding.UTF8.GetBytes(returnParam.ToString());
                }
                else if (signature.TYPE.RETURN.ToUpper() == "VARCHAR" || 
                         signature.TYPE.RETURN.ToUpper() == "CLOB")
                {
                    returnParam = CallFunction<String>(signature, query);
                    bytes = Encoding.UTF8.GetBytes(returnParam.ToString());
                }
                else if (signature.TYPE.RETURN.ToUpper() == "DATE")
                {
                    returnParam = CallFunction<DateTime>(signature, query);
                    bytes = Encoding.UTF8.GetBytes(returnParam.ToString());
                }
                else if (signature.TYPE.RETURN.ToUpper() == "VARBINARY")
                {
                    returnParam = CallFunction<byte[]>(signature, query);
                    bytes = (byte[]) returnParam;
                }
                else
                {
                    throw new ArgumentException("Функция возвращает не поддерживаемый тип: " 
                                                            + signature.TYPE.RETURN.ToUpper());
                }
            }
            else
            {
                throw new NotImplementedException();
            }
            return bytes;
        }


        private CallSignature DecodeSignature(string sjson)
        {
            CallSignature signature;
            try
            {  
                byte[] byteArray = Encoding.UTF8.GetBytes(sjson);
                MemoryStream stream1 = new MemoryStream(byteArray);

                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(CallSignature));
                signature = ser.ReadObject(stream1) as CallSignature;
 
            }
            catch (Exception e )
            {
                
                throw;
            }
            return signature;
        }



        private TypeValue CallFunction<TypeValue>(CallSignature signature, QueryObject query)
        {

            object returnParam;
            string callQuery;
            callQuery = query.Owner + "." + query.PackageName + "." + query.ObjectName;

            int queryArgCount = (query.Arguments != null) ? query.Arguments.Length : 0;
            DBConn.DBParam[] oip = new DBConn.DBParam[queryArgCount];
            int index = 0;
            foreach (VALUES value in signature.VALUES)
            {
                //Проверка на return и необязательные параметры
                if (!String.IsNullOrEmpty(value.NAME) && index < query.Arguments.Length)
                {
                    oip[index] = new DBConn.DBParam();
                    oip[index].ParameterName = value.NAME;
                    oip[index].DbType = DBConn.DBTypeCustom.String;
                    oip[index].Value = query.Arguments[index];
                    index++;
                }
            }

            DbConnAuth dbConnAuth = new DbConnAuth();
            try
            {
                DBConn.Conn dbconn = dbConnAuth.connOra();
                returnParam = dbconn.ExecuteQuery<TypeValue>(callQuery, oip);
            }

            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
            return (TypeValue)returnParam;
        }
    }
}