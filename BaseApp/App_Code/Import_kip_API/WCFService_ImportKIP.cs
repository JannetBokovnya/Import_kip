using System;
using System.Collections.Generic;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OracleClient;
using System;
using System.Data;
using System.ServiceModel.Activation;
using System.Web;
using App_Code.Import_kip_API;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "oraWCFService_ImportKIP" in code, svc and config file together.
[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
public partial class WCFService_ImportKIP : IWCFService_ImportKIP
{
    /// <summary>
    /// список газопроводов
    /// </summary>
    /// <returns></returns>
    public DataMGList_KIP GetDataMGListKIP()
    {
        var result = new DataMGList_KIP();
        result.DataMGLists = new List<DataMGListKIP>();

        try
        {

            DataSet ds = new DataSet();
            DataHelper.ExecQuery(Querys_ImpKip.GetMgQuery, ref ds);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    result.DataMGLists.Add(
                        new DataMGListKIP { KEYMG = row["nkey"].ToString(), NAMEMG = row["cname"].ToString() });
                }
            }
            else
            {
                result.IsValid = false;
                result.ErrorMessage = string.Format("Нет данных...");
            }
        }
        catch (Exception ex)
        {
            result.IsValid = false;
            result.ErrorMessage = ex.Message;
        }

        return result;
    }

    /// <summary>
    /// список нитей
    /// </summary>
    /// <param name="inKeyMg"></param>
    /// <returns></returns>
    public DataThreadsList_result GetDataThreadsList(string inKeyMg)
    {
        var result = new DataThreadsList_result();
        result.DataThreadsLists = new List<DataThreadsList>();

        try
        {
            DataSet ds = new DataSet();
            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "inkey";
            oip[0].DbType = DBConn.DBTypeCustom.Int64;
            oip[0].Direction = ParameterDirection.Input;
            oip[0].Value = Convert.ToInt64(inKeyMg);


            DataHelper.ExecQuery(Querys_ImpKip.GetThreadsForMgAllQuery, ref oip, ref ds);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    result.DataThreadsLists.Add(
                        new DataThreadsList { KEYTHREADS = row["nkey"].ToString(), NAMEHREADS = row["cname"].ToString() });
                }
            }
            else
            {
                result.IsValid = false;
                result.ErrorMessage = string.Format("Нет данных...");
            }

        }
        catch (Exception ex)
        {

            result.IsValid = false;
            result.ErrorMessage = ex.Message;
        }

        return result;
    }

    public KipDataList_result GetDataKipList(string inKeyThread)
    {
        //in_nPipeKey
        var result = new KipDataList_result();
        result.KipDataLists = new List<KipDataList>();

        try
        {

            DataSet ds = new DataSet();
            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "in_nPipeKey";
            oip[0].DbType = DBConn.DBTypeCustom.Int64;
            oip[0].Direction = ParameterDirection.Input;
            oip[0].Value = Convert.ToInt64(inKeyThread);


            DataHelper.ExecQuery(Querys_ImpKip.GetKipmeasJournalsQuery, ref oip, ref ds);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    result.KipDataLists.Add(
                        new KipDataList { nJourKey = row["nJourKey"].ToString(), dMeasDate = row["dMeasDate"].ToString() });
                }
            }
            else
            {

                result.ErrorMessage = string.Format("Нет данных...");
            }

        }
        catch (Exception ex)
        {

            result.IsValid = false;
            result.ErrorMessage = ex.Message;
        }


        return result;
    }

    /// <summary>
    /// парметры выбранного участка
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public ParamSecList_result GetSec_Param(string key)
    {
        var result = new ParamSecList_result();
        result.ParamSecLists = new List<ParamSecList>();

        try
        {
            double newImportKey;
            double.TryParse(key, out newImportKey);
            string ret = string.Empty;

            DataSet ds = new DataSet();
            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "in_nJournalKey";
            oip[0].DbType = DBConn.DBTypeCustom.Double;
            oip[0].Value = newImportKey;

            DataHelper.ExecQuery(Querys_ImpKip.GetParamSecQuery, ref oip, ref ds);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    result.ParamSecLists.Add(
                        new ParamSecList
                        {
                            NKM_BEGIN = row["NKM_BEGIN"].ToString(),
                            NKM_END = row["NKM_END"].ToString(),
                            NLENGTH = row["NLENGTH"].ToString(),
                            NUMBERKIP = row["NNUMBER_IZMER_KIP"].ToString()
                        });
                }
            }
            else
            {
                result.IsValid = true;
                result.ErrorMessage = string.Format("Нет данных...");
            }

        }
        catch (Exception ex)
        {
            result.IsValid = false;
            result.ErrorMessage = ex.Message;
        }


        return result;
    }

    /// <summary>
    /// лог загрузки в файл 
    /// </summary>
    /// <param name="keyJornal"></param>
    /// <returns></returns>
    public ImpLogKIP GetImpLog(string keyJornal)
    {
        var result = new ImpLogKIP();
        result.ImpLogKIP_result = new ImpLog_resultKIP();

        try
        {
            // OracleLayer_ImpVtd ol = new OracleLayer_ImpVtd();
            double newImportKey;
            double.TryParse(keyJornal, out newImportKey);
            string ret = string.Empty;

            DataSet ds = new DataSet();
            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "in_nImp_Making_key";
            oip[0].DbType = DBConn.DBTypeCustom.Double;
            oip[0].Value = newImportKey;

            //DbConnAuth dbConnAuth = new DbConnAuth();
            //DBConn.Conn connOra = dbConnAuth.connOra();
            //connOra.ConnectionString(GetConnectionString());
            //DataSet ds = connOra.ExecuteQuery(GetImpLogQuery, false, oip);

            //ds = ol.GetImpLog(newImportKey, out errStr);
            
            //Закоментировано, т.к. эти данные для вывода на форму не актуальны. 
            //Нужные данные вернутся функцией GetImpAnalyzeFileLog
            //DataHelper.ExecQuery(Querys_ImpKip.GetImpLogQuery, ref oip, ref ds);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //проходим по всем строкам заполняем поля
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    foreach (DataColumn column in ds.Tables[0].Columns)
                    {
                        if (row[column] != null)
                        {
                            if (column.Caption.ToLower() == "clogmsg".ToLower())
                            {
                                ret += row[column].ToString() + "'\n'";
                            } //end of if (row[column] != null)
                        } // end of foreach (DataColumn column in ds.Tables[0].Columns)           
                    } //end of foreach (DataRow row in ds.Tables[0].Rows)

                }

                result.ImpLogKIP_result.ImpLogKIP_result_ret = ret;
            }
        }
        catch (Exception ex)
        {

            result.IsValid = false;
            result.ErrorMessage = ex.Message;
        }


        return result;
    }

    /// <summary>
    /// by Gaitov 07.05.2015 г.
    /// Лог по результатам анализа файла импорта КИП.
    /// Возвращает данные в формате:
    ///     Статистика по количеству строк в файле:
    ///     Импортировано	xxx
    ///     Не заполнено: 
    ///         «Километраж КИП»	xxx
    ///         «Потенциал Uт-з (В)»	xxx
    ///     Будут пропущены	xxx
    ///  </summary>
    public ImpLog_resulAnalyzetKipFile GetImpAnalyzeFileLog(string keyJornal)
    {
        var result = new ImpLog_resulAnalyzetKipFile();
        result.ImpLogAnalizKIP_result = new ImpLogAnaliz_resultKIP();
        result.IsValid = true;

        try
        {
            // OracleLayer_ImpVtd ol = new OracleLayer_ImpVtd();
            double newImportKey;
            double.TryParse(keyJornal, out newImportKey);
            string ret = string.Empty;
            DataSet ds = new DataSet();

            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "in_nJournalKey";
            oip[0].DbType = DBConn.DBTypeCustom.Double;
            oip[0].Value = newImportKey;

           // DataHelper.ExecQuery(Querys_ImpKip.GetImpAnalyzeFileLogQuery, ref oip, ref ds);

            string str1 = "Вызываем " + Querys_ImpKip.GetImpAnalyzeFileLogQuery + "  " ;

            DataHelper.ExecQuery(Querys_ImpKip.GetImpAnalyzeFileLogQuery, ref oip, ref ret);

            string str2 = Querys_ImpKip.GetImpAnalyzeFileLogQuery + "  отработал ";
            string str3 = " вернул результат " + ret;

            result.ErrorMessage = str1 + str2 + str3;

            if (!String.IsNullOrEmpty(ret))
            {
                result.ImpLogAnalizKIP_result.ImpLogAnalizKIP_result_ret = ret;
            }
            else
            {
                result.IsValid = false;
                result.ErrorMessage = "Нет данных для отображения.";
            }
        }
        catch (Exception ex)
        {

            result.IsValid = false;
            result.ErrorMessage = ex.Message;
        }
        
        return result;
    }

    /// <summary>
    /// список кип из бд
    /// </summary>
    /// <param name="keyJornal"></param>
    /// <returns></returns>
    public KipDataListBD_result GetDataKipListBD(string keyJornal)
    {
        var result = new KipDataListBD_result();
        result.KipDataListsBD = new List<KipDataListBD>();

        try
        {

            DataSet ds = new DataSet();
            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "in_nJournalKey";
            oip[0].DbType = DBConn.DBTypeCustom.Int64;
            oip[0].Direction = ParameterDirection.Input;
            oip[0].Value = Convert.ToInt64(keyJornal);


            DataHelper.ExecQuery(Querys_ImpKip.GetKipListForJournalQuery, ref oip, ref ds);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    result.KipDataListsBD.Add(
                        new KipDataListBD
                            {
                                NKIP_KEY = row["NKIP_KEY"].ToString(),
                                NKM = row["NKM"].ToString(),
                                cType = row["cType"].ToString(),
                                CNAME = row["CNAME"].ToString()
                            });
                }
            }
            else
            {
                result.IsValid = true;
                result.ErrorMessage = string.Format("Нет данных...");
            }

        }
        catch (Exception ex)
        {

            result.IsValid = false;
            result.ErrorMessage = ex.Message;
        }
        return result;
    }

    /// <summary>
    /// список кип из файла
    /// </summary>
    /// <param name="keyJornal"></param>
    /// <returns></returns>
    public KipDataListFile_result GetDataKipListFile(string keyJornal)
    {
        var result = new KipDataListFile_result();
        result.KipDataListsFile = new List<KipDataListFile>();

        try
        {

            DataSet ds = new DataSet();
            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "in_nJournalKey";
            oip[0].DbType = DBConn.DBTypeCustom.Int64;
            oip[0].Direction = ParameterDirection.Input;
            oip[0].Value = Convert.ToInt64(keyJornal);


            DataHelper.ExecQuery(Querys_ImpKip.GetKipListFromFileQuery, ref oip, ref ds);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    result.KipDataListsFile.Add(
                        new KipDataListFile
                        {
                            nrawkey = row["nrawkey"].ToString(),
                            nkm = row["nkm"].ToString(),
                            nu_tz = row["nu_tz"].ToString(),
                            nu_pol = row["nu_pol"].ToString(),
                            ckipnum = row["ckipnum"].ToString(),
                            ccomment = row["ccomment"].ToString()
                        });
                }
            }
            else
            {
                result.IsValid = true;
                result.ErrorMessage = string.Format("Нет данных...");
            }

        }
        catch (Exception ex)
        {

            result.IsValid = false;
            result.ErrorMessage = ex.Message;
        }


        return result;
    }

    /// <summary>
    /// увязка кипов
    /// </summary>
    /// <param name="in_nVTDMakingKey"></param>
    /// <param name="in_Filekey"></param>
    /// <param name="in_DBKey"></param>
    /// <param name="typeLink"></param>
    /// <returns></returns>
    public KeyBoundResultKIP ImportKipMatching(string in_nVTDMakingKey, string in_Filekey, string in_DBKey, int typeLink)
    {

        //typeLink
        //1 - ручная увязка пары кипов
        //2 - автоматическая
        //3 - без увязки (только кип из файла)
        var result = new KeyBoundResultKIP();
        result.GetKeyBoundList = new List<KeyBoundKIP>();
        result.ErrorMessage = ("ImportTubeMatching DB_API.vtd_import_api.addTemMapping  in_nVTDMakingKey = " + in_nVTDMakingKey +
                                         "  in_nRawJourKey =  " + in_Filekey + "  in_nDbTemKey = " + in_DBKey);

        DataSet ds = new DataSet();

        try
        {
            switch (typeLink)
            {
                case 1:
                    {
                        DBConn.DBParam[] oip = new DBConn.DBParam[3];
                        oip[0] = new DBConn.DBParam();
                        oip[0].ParameterName = "in_nJournalKey";
                        oip[0].DbType = DBConn.DBTypeCustom.Double;
                        oip[0].Value = in_nVTDMakingKey;

                        oip[1] = new DBConn.DBParam();
                        oip[1].ParameterName = "in_сRawKipKeyList";
                        oip[1].DbType = DBConn.DBTypeCustom.String;
                        oip[1].Value = in_Filekey;


                        oip[2] = new DBConn.DBParam();
                        oip[2].ParameterName = "in_nDbKipKey";
                        oip[2].DbType = DBConn.DBTypeCustom.String;
                        oip[2].Value = in_DBKey;

                        DataHelper.ExecQuery(Querys_ImpKip.ImportTubeMatchingQuery, ref oip, ref ds);

                        break;
                    }
                case 2:
                    {
                        DBConn.DBParam[] oip = new DBConn.DBParam[2];
                        oip[0] = new DBConn.DBParam();
                        oip[0].ParameterName = "in_nJournalKey";
                        oip[0].DbType = DBConn.DBTypeCustom.Double;
                        oip[0].Value = in_nVTDMakingKey;

                        oip[1] = new DBConn.DBParam();
                        oip[1].ParameterName = "in_cKeyPairs";
                        oip[1].DbType = DBConn.DBTypeCustom.String;
                        oip[1].Value = "";


                        // ds = connOra.ExecuteQuery(Querys_ImpKip.ApplyKipKeyMapQuery, false, oip);
                        DataHelper.ExecNonQuery(Querys_ImpKip.ImportTubeMatchingQuery, ref oip);

                        break;
                    }

                case 3:
                    {

                        DBConn.DBParam[] oip = new DBConn.DBParam[3];
                        oip[0] = new DBConn.DBParam();
                        oip[0].ParameterName = "in_nJournalKey";
                        oip[0].DbType = DBConn.DBTypeCustom.Double;
                        oip[0].Value = in_nVTDMakingKey;

                        oip[1] = new DBConn.DBParam();
                        oip[1].ParameterName = "in_сRawKipKeyList";
                        oip[1].DbType = DBConn.DBTypeCustom.String;
                        oip[1].Value = in_Filekey;


                        oip[2] = new DBConn.DBParam();
                        oip[2].ParameterName = "in_nDbKipKey";
                        oip[2].DbType = DBConn.DBTypeCustom.String;
                        oip[2].Value = in_DBKey;

                        DataHelper.ExecQuery(Querys_ImpKip.ImportTubeMatchingQuery, ref oip, ref ds);

                        break;
                    }
            }

            if (typeLink != 2)
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {

                        result.GetKeyBoundList.Add(new KeyBoundKIP()
                        {
                            KeyBD = row["nDbKipKey"].ToString(),
                            KeyFile = row["nRawKipKey"].ToString(),


                        });
                    }
                }
            }

            //if (typeLink == 2)
            //{
            //    if (ds != null && ds.Tables.Count > 0 && ds.Tables["Parameters"].Rows.Count > 0)
            //    {
            //        foreach (DataRow row in ds.Tables["Parameters"].Rows)
            //        {
            //            if (row["Name"].ToString() == "out_cStopReason")
            //            {
            //                result.ErrorMessage = row["Value"].ToString();
            //            }
            //        }
            //    }
            //}




        }
        catch (Exception e)
        {

            result.IsValid = false;
            result.ErrorMessage = (" DB_API.IMPORT_KIP_API.addKipMapping  in_nJournalKey = " + in_nVTDMakingKey +
                "  in_сRawKipKeyList =  " + in_Filekey + "  in_nDbKipKey = " + in_DBKey + "  err = " + e.Message);
        }



        ////для теста возвращаем данные которые передали

        //result.GetKeyBoundList.Add(new KeyBound() { KeyBD = in_DBKey, KeyFile = in_Filekey });

        ////string[] arr = in_bMatchTable.Split(';');
        ////for (int i = 0; i < arr.Length - 1; i++)
        ////{
        ////    string[] arr1 = arr[i].Split(',');

        ////    for (int ii = 0; ii < arr1.Length - 1; ii++)
        ////    {
        ////        result.GetKeyBoundList.Add(new KeyBound() { KeyBD = arr1[0], KeyFile = arr1[1] });
        ////    }



        return result;

    }



    /// <summary>
    /// автоматическая увязка
    /// </summary>
    /// <param name="in_nVTDMakingKey"></param>
    /// <returns></returns>
    public StatusAnswer_ImportKIP ImportKipMatchingAuto(string in_nVTDMakingKey)
    {
        var result = new StatusAnswer_ImportKIP();
        result.IsValid = true;

        // DataSet ds = new DataSet();

        try
        {

            DBConn.DBParam[] oip = new DBConn.DBParam[2];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "in_nJournalKey";
            oip[0].DbType = DBConn.DBTypeCustom.Double;
            oip[0].Value = in_nVTDMakingKey;


            oip[1] = new DBConn.DBParam();
            oip[1].ParameterName = "out_cStopReason";
            oip[1].DbType = DBConn.DBTypeCustom.String;
            oip[1].Size = 1000;
            oip[1].Direction = ParameterDirection.Output;


            // DataHelper.ExecNonQuery(Querys_ImpKip.ApplyKipKeyMapQuery, ref oip);
            // DataHelper.ExecQuery(Querys_ImpKip.ApplyKipKeyMapQuery, ref oip, ref ds);


            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn dbConn = dbConnAuth.connOra();

            DataSet ds = dbConn.ExecuteQuery(Querys_ImpKip.ApplyKipKeyMapQuery, true, oip);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables["Parameters"].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables["Parameters"].Rows)
                {
                    if (row["Name"].ToString() == "out_cStopReason")
                    {
                        result.ErrorMessage = row["Value"].ToString();
                    }
                }
            }

        }
        catch (Exception e)
        {

            result.IsValid = false;
            result.ErrorMessage = "Ошибка автоматической увязки ApplyKipKeyMapQuery key =  " + in_nVTDMakingKey + e.Message;
        }

        return result;
    }


    /// <summary>
    /// возвращает списоек увязанных кипов
    /// </summary>
    /// <param name="in_nVTDMakingKey"></param>
    /// <returns></returns>
    public KeyBoundResultKIP GetKIPMapping(string in_nVTDMakingKey)
    {
        var result = new KeyBoundResultKIP();
        result.GetKeyBoundList = new List<KeyBoundKIP>();
        DataSet ds = new DataSet();

        try
        {
            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "in_nJournalKey ";
            oip[0].DbType = DBConn.DBTypeCustom.Double;
            oip[0].Value = in_nVTDMakingKey;

            DataHelper.ExecQuery(Querys_ImpKip.GetMappedKipListgQuery, ref oip, ref ds);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    result.GetKeyBoundList.Add(new KeyBoundKIP()
                    {
                        KeyBD = row["nDbKipKey"].ToString(),
                        KeyFile = row["nRawKipKey"].ToString(),


                    });
                }
            }
        }
        catch (Exception e)
        {

            result.IsValid = false;
            result.ErrorMessage = ("ImportTubeMatching DB_API.vtd_import_api.getTemMapping  in_nVTDMakingKey = " + in_nVTDMakingKey + "  err = " + e.Message);
        }
        return result;
    }

    /// <summary>
    /// удаление связей
    /// </summary>
    /// <param name="keyImport"></param>
    /// <param name="in_Filekey"></param>
    /// <returns></returns>
    public RemoveBoundKIP RemoveBound(string keyImport, string in_Filekey)
    {
        var result = new RemoveBoundKIP();
        result.RemoveBoundKIP_Result = new RemoveBoundResultKIP();
        DataSet ds = new DataSet();

        try
        {

            DBConn.DBParam[] oip = new DBConn.DBParam[2];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "in_nJournalKey";
            oip[0].DbType = DBConn.DBTypeCustom.Double;
            oip[0].Direction = ParameterDirection.Input;
            oip[0].Value = keyImport;


            oip[1] = new DBConn.DBParam();
            oip[1].ParameterName = "in_nRawKipKey";
            oip[1].DbType = DBConn.DBTypeCustom.String;
            oip[1].Value = in_Filekey;

            DataHelper.ExecQuery(Querys_ImpKip.RollbackTemMappingQuery, ref oip, ref ds);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string res = row["nRowCount"].ToString();
                    result.RemoveBoundKIP_Result.RemoveBoundKIP = Convert.ToInt32(res);
                }
            }


            result.ErrorMessage = "RemoveBoundKIP  =  " +
                              result.RemoveBoundKIP_Result.RemoveBoundKIP.ToString();

        }
        catch (Exception e)
        {
            result.IsValid = false;
            result.ErrorMessage = "Ошибка удаления связи err = " + e.Message;

        }

        return result;
    }

    /// <summary>
    /// скрытый/отображенный кип из файла
    /// </summary>
    /// <param name="in_nKipKey"></param>
    /// <param name="isHide"> 1=использовать, 0=не использовать измерение </param>
    /// <returns></returns>
    public StatusAnswer_ImportKIP KipIsHide(string in_nJournalKey, string in_nKipKey, int isHide)
    {
        var result = new StatusAnswer_ImportKIP();
        result.IsValid = true;

        try
        {

            DBConn.DBParam[] oip = new DBConn.DBParam[3];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "in_nJournalKey";
            oip[0].DbType = DBConn.DBTypeCustom.Double;
            oip[0].Value = in_nJournalKey;

            oip[1] = new DBConn.DBParam();
            oip[1].ParameterName = "in_cKipMeasurementList";
            oip[1].DbType = DBConn.DBTypeCustom.String;
            oip[1].Value = in_nKipKey;

            oip[2] = new DBConn.DBParam();
            oip[2].ParameterName = "in_nIsUsedMeasurement";
            oip[2].DbType = DBConn.DBTypeCustom.Double;
            oip[2].Value = isHide;

            DataHelper.ExecNonQuery(Querys_ImpKip.HideKipQuery, ref oip);

            result.ErrorMessage =
                "Процедура выполнилась DB_API.IMPORT_KIP_API.manageUseOfFileMeasurement in_nJournalKey= " +
                in_nJournalKey +
                " in_cKipMeasurementList = " + in_nKipKey + " in_nIsUsedMeasurement = " + isHide;

        }
        catch (Exception e)
        {

            result.IsValid = false;
            result.ErrorMessage = "Ошибка  KipIsHide key =  " + in_nKipKey + e.Message;
        }


        return result;
    }

    /// <summary>
    /// скрытый/отображенный кип из bd
    /// </summary>
    /// <param name="in_nJournalKey"></param>
    /// <param name="in_nKipKeyBD"></param>
    /// <param name="isHide"></param>
    /// <returns></returns>
    public StatusAnswer_ImportKIP KipIsHideBD(string in_nJournalKey, string in_nKipKeyBD, int isHide)
    {
        //  1=использовать, 0=не использовать измерение

        var result = new StatusAnswer_ImportKIP();
        result.IsValid = true;

        try
        {

            DBConn.DBParam[] oip = new DBConn.DBParam[3];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "in_nJournalKey";
            oip[0].DbType = DBConn.DBTypeCustom.Double;
            oip[0].Value = in_nJournalKey;

            oip[1] = new DBConn.DBParam();
            oip[1].ParameterName = "in_cDbKipList";
            oip[1].DbType = DBConn.DBTypeCustom.String;
            oip[1].Value = in_nKipKeyBD;

            oip[2] = new DBConn.DBParam();
            oip[2].ParameterName = "in_nIsUsedKip";
            oip[2].DbType = DBConn.DBTypeCustom.Double;
            oip[2].Value = isHide;

            DataHelper.ExecNonQuery(Querys_ImpKip.HideKipBDQuery, ref oip);

            result.ErrorMessage =
               "Процедура выполнилась DB_API.IMPORT_KIP_API.manageUseOfDbKip in_nJournalKey= " +
               in_nJournalKey +
               " in_cDbKipList = " + in_nKipKeyBD + " in_nIsUsedKip = " + isHide;
        }
        catch (Exception e)
        {

            result.IsValid = false;
            result.ErrorMessage = "Ошибка  KipIsHideBD key =  " + in_nKipKeyBD + " in_nJournalKey = " + in_nJournalKey +
                "  in_nIsUsedKip = " + isHide + e.Message;
        }


        return result;
    }


    /// <summary>
    /// запуск самого импорта
    /// </summary>
    /// <param name="in_nJournalKey"></param>
    /// <returns></returns>
    public StatusAnswer_ImportKIP ImportKipMeasurement(string in_nJournalKey)
    {
        var result = new StatusAnswer_ImportKIP();
        result.IsValid = true;

        try
        {

            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "in_nJournalKey";
            oip[0].DbType = DBConn.DBTypeCustom.Double;
            oip[0].Value = in_nJournalKey;

            DataHelper.ExecNonQuery(Querys_ImpKip.ImportKipMeasurement, ref oip);

            result.ErrorMessage = " Процедкра импорта запустилась db_api.IMPORT_KIP_API.ImportKipMeasurement";

        }
        catch (Exception e)
        {

            result.IsValid = false;
            result.ErrorMessage = "Ошибка ошибка импорта Кип ImportKipMeasurement key =  " + in_nJournalKey + e.Message;
        }

        return result;

    }

    /// <summary>
    /// удаление предыдущий не законченные импорт
    /// </summary>
    /// <param name="jornalKey"></param>
    /// <returns></returns>
    public StatusAnswer_ImportKIP DeleteKipMeasurement(string jornalKey)
    {
        var result = new StatusAnswer_ImportKIP();
        result.IsValid = true;

        try
        {

            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "in_nJournalKey";
            oip[0].DbType = DBConn.DBTypeCustom.Double;
            oip[0].Value = jornalKey;

            DataHelper.ExecNonQuery(Querys_ImpKip.DeleteKipMeasurementQuery, ref oip);
            result.ErrorMessage = " Удаление импорта DB_API.IMPORT_KIP_API.DeleteKipMeasurement";

        }
        catch (Exception e)
        {

            result.IsValid = false;
            result.ErrorMessage = "Ошибка удаления записей RollbackTemMapping key =  " + jornalKey + e.Message;
        }



        return result;
    }

}


