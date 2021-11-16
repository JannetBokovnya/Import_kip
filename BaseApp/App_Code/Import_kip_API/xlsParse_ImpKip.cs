using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Data.OleDb;
using System.Collections;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading;
using App_Code.Admin_module_API;
using App_Code.Import_kip_API;
using log4net;


    public partial class WCFService_ImportKIP : IWCFService_ImportKIP

        //public class XlsParse_ImpKip : OracleEngine_ImpKip
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (Auth).Name);

        public class statusEnum
        {
            public string D = "D";//done
            public string E = "E";//error
            public string W = "W";//warning
        }

        statusEnum _statusEnum = new statusEnum();
        /// <summary>
        /// структура описывающая данные о сопоставленных таблицах и листах
        /// </summary>
        private struct TableNameStruct
        {
            public double nsheetkey;
            public string sheets_name;
            public string raw_table_name;
        }

        /// <summary>
        /// структура описывающая данные о сопоставленных столбцах
        /// </summary>
        private struct ColumnNameStruct
        {
            public string ColName;
            public string DataType;
            public int EnableNull;

            /// TODO: почему int   
            public string DbColName;
        }

        /// <summary>
        /// получает список сопоставлений листов excel   таблицам оракла
        /// </summary>
        /// <param name="importType">название типа импорта</param>
        /// <param name="errMsg">сообщение об ошибке получения данных</param>
        /// <returns>возвращает датасет с одной таблице</returns>
        private DataSet GetTableName(string importType, out string errMsg)
        {
            DataSet ds = new DataSet();
            string err = "";

            try
            {
               
                //OraInputParam[] oip = new OraInputParam[1];
                //oip[0].InputFieldName = "in_ctype";
                //oip[0].InputType = OracleType.VarChar;
                //oip[0].InputValue = importType;
                //ConstructEngine(Querys_ImpKip.InportGetTableNameQuery, oip, GetConnectionString(), ref ds, true, out errMsg);

                DBConn.DBParam[] oip = new DBConn.DBParam[1];
                oip[0] = new DBConn.DBParam();
                oip[0].ParameterName = "in_ctype";
                oip[0].DbType = DBConn.DBTypeCustom.VarChar;
                oip[0].Direction = ParameterDirection.Input;
                oip[0].Value = importType;
                DataHelper.ExecQuery(Querys_ImpKip.InportGetTableNameQuery, ref oip, ref ds);
              
            }
            catch (Exception ex)
            {

                err  = ex.ToString();
            }

            errMsg = err;
            return ds;
        }

        /// <summary>
        /// получаем список полей  таблицы по ключу названия листа excel
        /// </summary>
        /// <param name="sheetkey">ключ названия листа excel</param>
        /// <param name="errMsg">сообщение об ошибке получения данных</param>
        /// <returns>датасет</returns>
        private DataSet GetColumns(double sheetkey, out string errMsg)
        {
            DataSet ds = new DataSet();
            string err = "";

            try
            {

                //OraInputParam[] oip = new OraInputParam[1];
                //oip[0].InputFieldName = "in_sheetkey";
                //oip[0].InputType = OracleType.Number;
                //oip[0].InputValue = sheetkey;
                //ConstructEngine(Querys_ImpKip.InportGetColumnsQuery, oip, GetConnectionString(), ref ds, true, out errMsg);

                DBConn.DBParam[] oip = new DBConn.DBParam[1];
                oip[0] = new DBConn.DBParam();
                oip[0].ParameterName = "in_sheetkey";
                oip[0].DbType = DBConn.DBTypeCustom.Number;
                oip[0].Direction = ParameterDirection.Input;
                oip[0].Value = sheetkey;

                DataHelper.ExecQuery(Querys_ImpKip.InportGetColumnsQuery, ref oip, ref ds);
            }
            catch (Exception ex)
            {

                err = ex.ToString();
            }

            errMsg = err;
            return ds;
        }


        /// <summary>
        /// логирование  процесса импорта в базу
        /// </summary>
        /// <param name="impKey">ключ импорта</param>
        /// <param name="msg">сообщение</param>
        /// <param name="status">тип сообщения</param>
        /// <param name="errMsg">сообщение об ошибке получения данных</param>
        protected void LogMessage(double impKey, string msg, string status, out string errMsg)
        {
            DataSet ds = new DataSet();
            string err = string.Empty;

            try
            {
                DBConn.DBParam[] oip = new DBConn.DBParam[3];
                oip[0] = new DBConn.DBParam();
                oip[0].ParameterName = "in_nPK";
                oip[0].DbType = DBConn.DBTypeCustom.Number;
                oip[0].Direction = ParameterDirection.Input;
                oip[0].Value = impKey;

                oip[1] = new DBConn.DBParam();
                oip[1].ParameterName = "in_cMSG";
                oip[1].DbType = DBConn.DBTypeCustom.VarChar;
                oip[1].Direction = ParameterDirection.Input;
                oip[1].Value = msg;

                oip[2] = new DBConn.DBParam();
                oip[2].ParameterName = "in_cStatus";
                oip[2].DbType = DBConn.DBTypeCustom.VarChar;
                oip[2].Direction = ParameterDirection.Input;
                oip[2].Value = status;

                DataHelper.ExecNonQuery(Querys_ImpKip.InportWrite2LogQuery, ref oip);
                
            }
            catch (Exception ex)
            {
                err = ex.ToString();
            }

            errMsg = err;
        }


        private DataSet GetDataFromXLSSheet(string filePath, string sheetName, out string errMsg)
        {
            errMsg = string.Empty;
            DataSet ds = new DataSet();

            OleDbConnection con =
                new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath +
                                    ";Extended Properties=Excel 8.0");
            con.Open();
            try
            {
                OleDbDataAdapter odb = new OleDbDataAdapter("select * from [" + sheetName + "$]", con);
                odb.Fill(ds);
                con.Close();
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return null;
            }
            finally
            {
                con.Dispose();
            }

            return ds;
        }

        public StatusAnswer_ImportKIP ImportFile(double impKey, string typeOfImport, string fileName)
        {
            var result = new StatusAnswer_ImportKIP();
            result.IsValid = true;

            //string internalError;
            //bool isFirstRow = true;
            //StringBuilder templateColumnPartQuery = new StringBuilder(); //содержит шаблонную часть  запроса с колонками

            fileName = AppDomain.CurrentDomain.BaseDirectory + "Upload\\UploadedImportKIP\\" + fileName;
            string errMsg = string.Empty;
            string internalError = string.Empty;
            bool isFirstRow = true;
            StringBuilder templateColumnPartQuery = new StringBuilder(); //содержит шаблонную часть  запроса с колонками

            //проверяем, существует ли указанный файл
            if (!System.IO.File.Exists(fileName))
            {
                //errMsg = string.Format("Файл {0} не найден", fileName);
                //return false;

                errMsg = string.Format("Файл {0} не найден", fileName);
                LogMessage(impKey, errMsg, _statusEnum.E, out internalError);
                result.IsValid = false;
                result.ErrorMessage = errMsg;
                return result;

            }

            #region пытаемся получить  сопоставление таблиц БД с названиями листов excel

            DataSet tableNameDs;
            try
            {
                tableNameDs = GetTableName(typeOfImport, out errMsg);
            }
            catch (Exception e)
            {
                errMsg = string.Format("Ошибка получения сопоставления названий таблиц, {0}", e.Message.ToString());
                LogMessage(impKey, errMsg, _statusEnum.E, out internalError);
                result.IsValid = false;
                result.ErrorMessage = errMsg;
                return result;
            }
            if (!String.IsNullOrEmpty(errMsg))
            {
                LogMessage(impKey, string.Format("Невозможно получить сопоставление названий таблиц,{0}", errMsg), _statusEnum.E, out internalError);
                result.IsValid = false;
                result.ErrorMessage = "Невозможно получить сопоставление названий таблиц";
                return result;
            }
            if (tableNameDs.Tables[0].Rows.Count == 0)
            {
                errMsg = "Не найдено сопоставление названий таблиц для текущего импорта";
                LogMessage(impKey, errMsg, _statusEnum.E, out internalError);
                result.IsValid = false;
                result.ErrorMessage = errMsg;
                return result;
            }
            else
            {
                LogMessage(impKey, "Получено сопоставление названий таблиц для текущего импорта", _statusEnum.D, out internalError);
            }

            #endregion

            #region полученные сопоставления  таблиц переносим во внутренние структуры

            List<TableNameStruct> tableNameList = new List<TableNameStruct>();
            IEnumerable<DataRow> query = from t in tableNameDs.Tables[0].AsEnumerable()
                                         select t;
            try
            {
                foreach (DataRow p in query)
                    tableNameList.Add(new TableNameStruct()
                        {
                            nsheetkey = Convert.ToDouble(p.Field<object>("nsheetkey")),
                            raw_table_name = p.Field<string>("raw_table_name"),
                            sheets_name = p.Field<string>("sheets_name")
                        });
            }
            catch (Exception e)
            {
                errMsg = string.Format("Ошибка получения данных о таблицах: {0}", e.Message);
                LogMessage(impKey, errMsg, _statusEnum.E, out internalError);
                result.IsValid = false;
                result.ErrorMessage = errMsg;
                return result;
            }

            #endregion

            #region завершаем работу метода,  если не удалось успешно перенести сопоставления во внутренние  структуры

            if (tableNameList.Count == 0)
            {
                errMsg = "Ошибка преобразования сопоставленых названий таблиц";
                LogMessage(impKey, errMsg, _statusEnum.E, out internalError);
                result.IsValid = false;
                result.ErrorMessage = errMsg;
                return result; 
            }

            #endregion

            for (int i = 0; i < tableNameList.Count(); i++)
            {
                isFirstRow = true;

                #region получаем сопоставление полей  для текущей таблицы

                DataSet columnsNameDs;
                try
                {
                    columnsNameDs = GetColumns(tableNameList[i].nsheetkey, out errMsg);
                }
                catch (Exception e)
                {
                    errMsg = string.Format("Ошибка  получения полей для {0}. {1}", tableNameList[i].nsheetkey, e.Message.ToString());
                    LogMessage(impKey, errMsg, _statusEnum.E, out internalError);
                    result.IsValid = false;
                    result.ErrorMessage = errMsg;
                    return result;
                }

                #endregion

                //проверяем удалось ли получить  сопставленные поля
                if (!string.IsNullOrEmpty(errMsg))
                {
                    LogMessage(impKey, string.Format("Не удалось получить сопоставление полей для листа {0} ", tableNameList[i].sheets_name), _statusEnum.E, out internalError);
                    result.IsValid = false;
                    result.ErrorMessage = errMsg;
                    return result;
                }

                #region полученные сопоставления колонок переносим во внутренние структуры

                List<ColumnNameStruct> columnNameList = new List<ColumnNameStruct>();
                IEnumerable<DataRow> queryColumns = from t in columnsNameDs.Tables[0].AsEnumerable()
                                                    select t;
                try
                {
                    bool isAddComa = false;
                    templateColumnPartQuery = new StringBuilder();
                    templateColumnPartQuery.Append(" nImpKey, ");
                    foreach (DataRow p in queryColumns)
                    {
                        columnNameList.Add(new ColumnNameStruct()
                            {
                                EnableNull = Convert.ToInt32(p.Field<object>("CanBeNull")),
                                ColName = p.Field<string>("cColName"),
                                DataType = p.Field<string>("cDataType"),
                                DbColName = p.Field<string>("cDbColName")
                            });
                        //формируем часть запроса с колонками для дальнейшего  использования
                        if (isAddComa)
                            templateColumnPartQuery.Append(", ");

                        templateColumnPartQuery.Append(p.Field<string>("cDbColName"));
                        isAddComa = true;
                    }
                }
                catch (Exception e)
                {
                    errMsg = string.Format("Ошибка получения данных о таблицах: {0}", e.Message);
                    LogMessage(impKey, errMsg, _statusEnum.E, out internalError);
                    result.IsValid = false;
                    result.ErrorMessage = errMsg;
                    return result;
                }

                #endregion

                #region получаем данные с указанного  листка

                DataSet sheetWithDataDS;
                //т.к. excel имеет ограничение на кол-во данных на одном листе, то необходимо сделать
                //чтение много постранично для этого дабавлена переменная  multyPage
                //и все соответствующее

                int multyPage = 0;
                while (multyPage != -1)
                {
                    try
                    {
                        if (multyPage == 0)
                        {
                            //читаем настоящий лист
                            sheetWithDataDS = GetDataFromXLSSheet(fileName, tableNameList[i].sheets_name,
                                                                  out internalError);
                        }
                        else
                        {
                            try
                            {
                                //читаем  возможно существующий лист многостраничности
                                sheetWithDataDS = GetDataFromXLSSheet(fileName,
                                                                      tableNameList[i].sheets_name + "_" +
                                                                      multyPage.ToString(), out internalError);
                                isFirstRow = true; //должно здесь быть
                                if (sheetWithDataDS == null)
                                {
                                    multyPage = -1;
                                    break;
                                }
                                if (sheetWithDataDS.Tables[0].Rows.Count == 0)
                                {
                                    multyPage = -1;
                                    break;
                                }
                            }
                            catch
                            {
                                multyPage = -1;
                                break;
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        LogMessage(impKey, string.Format("Ошибка чтения листа {0} в excel файле. {1}", tableNameList[i].sheets_name, e.Message.ToString()), _statusEnum.E, out internalError);
                        errMsg = e.Message.ToString();
                        result.IsValid = false;
                        result.ErrorMessage = errMsg;
                        return result;
                    }

                    #endregion

                    #region проверяем удалось ли получить данные с указанного листка

                    if (sheetWithDataDS == null)
                    {
                        errMsg = string.Format("Ошибка формата файла, лист {0} не найден", tableNameList[i].sheets_name);
                        LogMessage(impKey, errMsg, _statusEnum.E, out internalError);
                        result.IsValid = false;
                        result.ErrorMessage = errMsg;
                        return result;
                    }

                    if (!string.IsNullOrEmpty(internalError) | sheetWithDataDS.Tables.Count == 0)
                    {
                        errMsg = string.Format("Ошибка формата файла, лист {0} не найден", tableNameList[i].sheets_name);
                        LogMessage(impKey, errMsg, _statusEnum.E, out internalError);
                        result.IsValid = false;
                        result.ErrorMessage = errMsg;
                        return result;
                    }

                    #endregion

                    Hashtable checkColumnName = new Hashtable();
                    Hashtable columnData = new Hashtable();
                    int counter, currentRowNum = 0;
                    counter = currentRowNum = 0;
                    int countTest = 0;

                    foreach (DataRow row in sheetWithDataDS.Tables[0].Rows)
                    {
                        countTest = countTest + 1;
                        columnData = new Hashtable();
                        foreach (DataColumn column in sheetWithDataDS.Tables[0].Columns)
                        {

                            if (isFirstRow)
                            {
                                #region читаем названия колонок

                                string tmp = ClearString(column.Caption).ToLower();
                                if (checkColumnName.ContainsValue(tmp))
                                {
                                    errMsg = string.Format("Ошибка, для листа {0}, дублируются колонки {1}", (multyPage == 0 ? tableNameList[i].sheets_name : tableNameList[i].sheets_name + "_" + multyPage.ToString()), tmp);
                                    LogMessage(impKey, errMsg, _statusEnum.E, out internalError);
                                    result.IsValid = false;
                                    result.ErrorMessage = errMsg;
                                    return result;
                                }
                                //храним следующую связку
                                //key=индексу поля на листе(далее в цикле по индексу мы сможем определить к какому полю 
                                //относится текущее значение цикла
                                //value=обработанное название столбца ( обрабатываем т.к. выборка данных из excel 
                                //получается с некоторыми артефактами)
                                checkColumnName.Add(counter, tmp);
                                columnData.Add(counter, row[column].ToString());

                                counter++;

                                #endregion
                            }
                            else
                            {
                                columnData.Add(counter, row[column].ToString());
                                counter++;
                            }

                        }

                        counter = 0; //сбрасываем счетчик

                        if (isFirstRow)
                        {
                            #region проверяем наличие всех необходимых колонок на текущем листе

                            isFirstRow = false;

                            bool isFoundCriticalError = false;
                            for (int j = 0; j < columnNameList.Count(); j++)
                            {
                                string tmp = ClearString(columnNameList[j].ColName).ToLower();

                                if (!checkColumnName.ContainsValue(tmp))
                                {
                                    if (columnNameList[j].EnableNull == 0)
                                    {
                                        LogMessage(impKey, string.Format("Отсутствует  обязательная колонка {0} на листе {1}", columnNameList[j].ColName, (multyPage == 0 ? tableNameList[i].sheets_name : tableNameList[i].sheets_name + "_" + multyPage.ToString())), _statusEnum.E, out internalError);
                                        isFoundCriticalError = true;
                                    }
                                    else if (columnNameList[j].EnableNull == 1)
                                    {
                                        LogMessage(impKey, string.Format("Отсутствует  необязательная колонка {0} на листе {1}", columnNameList[j].ColName, (multyPage == 0 ? tableNameList[i].sheets_name : tableNameList[i].sheets_name + "_" + multyPage.ToString())), _statusEnum.W, out internalError);
                                    }
                                }
                            }
                            if (isFoundCriticalError)
                            {
                               errMsg = string.Format("Отсутствует обязательная колонка на листе {0}, продолжение импорта невозможно", (multyPage == 0 ? tableNameList[i].sheets_name : tableNameList[i].sheets_name + "_" + multyPage.ToString()));
                                LogMessage(impKey, errMsg, _statusEnum.E, out internalError);
                                result.IsValid = false;
                                result.ErrorMessage = errMsg;
                                return result;
                            }

                            #endregion
                        }
                        // else
                        // {
                        // прочитали строку данных из листа и имеем маппинг названий полей с положением на листе
                        //надо генерировать  инсерты, но вначале проверить   то заполнены обязательные поля
                        /* сравниваем одновременно три массива
                            * 1. columnNameList- содержит маппинг из базы соответствий названий полей
                            * 2. checkColumnName- содержит key- индекс поля  на листке, value- обработанное название поля
                            * 3. columnData- содержит key- индекс поля, value значение
                            * пытаемся найти совпадения для обязательных полей  в следующем порядке
                            * columnNameList=checkColumnName по названию поля
                            * checkColumnName= columnData по индексу
                            */
                        bool isSkeepThisRow = true;
                        string columnNameNotFilled = string.Empty;
                        for (int j = 0; j < columnNameList.Count(); j++)
                        {
                            if (columnNameList[j].EnableNull == 0)
                            {
                                isSkeepThisRow = true;
                                columnNameNotFilled = columnNameList[j].ColName;
                                for (int k = 0; k < columnNameList.Count(); k++)
                                    if (ClearString(columnNameList[j].ColName.ToLower()) ==
                                        ClearString(checkColumnName[k].ToString().ToLower()))
                                    {
                                        if (ClearString(columnData[k].ToString()) != "")
                                        {
                                            isSkeepThisRow = false;
                                            columnNameNotFilled = string.Empty;
                                            break;
                                        }
                                        else
                                        {
                                            isSkeepThisRow = true;
                                            break;
                                        }
                                    }
                                if (isSkeepThisRow) break;
                            }
                        }

                        if (!isSkeepThisRow)
                        {
                            StringBuilder queryBulder = new StringBuilder();
                            queryBulder.Append("insert into ");
                            queryBulder.Append("DB_API.dbo." + tableNameList[i].raw_table_name);
                            queryBulder.Append(" ( " + templateColumnPartQuery);
                            //queryBulder.Append(" ) values ( GIS_META.util.GetNewKey, 1 , " + impKey + " ");
                            queryBulder.Append(" ) values ( " + impKey + " ");

                            for (int j = 0; j < columnNameList.Count(); j++)
                            {
                                //for (int k = 0; k < columnNameList.Count(); k++)//By Gaitov///////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                for (int k = 0; k < checkColumnName.Count; k++)
                                    if (ClearString(columnNameList[j].ColName.ToLower()) ==
                                        ClearString(checkColumnName[k].ToString().ToLower()))
                                    {
                                        //нужно проверять тип вставляемого объекта ( и если нужно добавлять ковычки
                                        //нужно написать  в базовом классе оракла, возможность вставки sql
                                        //потом вызывать вставку каждой строки\
                                        //не забыть про много страничноть
                                        try
                                        {
                                            switch (columnNameList[j].DataType.ToUpper())
                                            {
                                                case "N":
                                                    //queryBulder.Append("," + Convert.ToDouble(columnData[k]));
                                                    queryBulder.Append("," +
                                                                       (columnData[k] != ""
                                                                            ? columnData[k].ToString()
                                                                                           .Replace(" ", "")
                                                                                           .Replace(",", ".")
                                                                            : " null "));
                                                    break;
                                                case "I":
                                                    queryBulder.Append("," +
                                                                       (columnData[k] != "" ? columnData[k] : " null "));
                                                    break;
                                                case "D":
                                                    DateTime tmpDate = Convert.ToDateTime(columnData[k]);
                                                    if (columnData[k] != null)
                                                        queryBulder.Append(",to_date('" + tmpDate.Day.ToString() + "/" +
                                                                           tmpDate.Month.ToString() + "/" +
                                                                           tmpDate.Year.ToString() + "', 'DD/MM/YYYY') ");
                                                    else
                                                        queryBulder.Append(", null");
                                                    break;
                                                case "S":
                                                    //queryBulder.Append(",'" + columnData[k] + "'");
                                                    if (columnData[k] != null)
                                                        queryBulder.Append(",'" +
                                                                           columnData[k].ToString()
                                                                                        .TrimStart(' ')
                                                                                        .TrimEnd(' ') + "'");
                                                    else
                                                        queryBulder.Append(", null");
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            LogMessage(impKey, string.Format("Ошибка данных в файле. На листе {0} в строке № {1}, неправильный формат данных. Строка будет пропущена" + e.Message,
                                                                   (multyPage == 0 ? tableNameList[i].sheets_name : tableNameList[i].sheets_name + "_" + multyPage.ToString()), (currentRowNum + 2).ToString()), _statusEnum.E, out internalError);
                                        }

                                        break;
                                    }
                            }

                            queryBulder.Append(" ) ");
                            DataHelper.ExecNonQuery1(queryBulder.ToString()); //отправляем строку данных
                            Thread.Sleep(5);
                        }
                        else
                        {
                            int tt = countTest;
                            LogMessage(impKey, string.Format("Ошибка данных в файле. На листе {0} в строке № {1}, не заполнена обязательная колонка {2}. Строка будет пропущена",
                                (multyPage == 0 ? tableNameList[i].sheets_name : tableNameList[i].sheets_name + "_" + multyPage.ToString()), (currentRowNum + 2).ToString(), columnNameNotFilled), _statusEnum.E, out internalError);
                            //сообщение о том, что будет пропущена строка, т.к. не заполнена  обязательная ячейка
                        }
                        currentRowNum++; //держим индекс текущей строки

                    }
                    multyPage++;
                }
            }

            return result;
        }


        public string ClearString(string sourse)
        {
            string s =
                Regex.Replace(sourse, @"[^\w\.@-]", "")
                     .Replace(".", "")
                     .Replace("@", "")
                     .Replace("_", "")
                     .Replace("\n", "")
                     .Replace("Δ", "")
                     .Replace("δ", "");
            if (s.Length >= 57)
                s = s.Substring(0, 56);
            return s;
        }
    
}