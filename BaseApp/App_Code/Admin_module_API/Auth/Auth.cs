using System;
using System.Data;
using App_Code.SessionStorage;
using log4net;

namespace App_Code.Admin_module_API
{
    /// <summary>
    /// Summary description for Auth
    /// </summary>
    public class Auth : IAuth
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Auth).Name);

        //public int CheckAuth(string login, string psw)
        //{
        //    int res;
        //    string hashPassword = Hashing(psw);

        //    DBConn.DBParam[] oip = new DBConn.DBParam[1];
        //    oip[0] = new DBConn.DBParam
        //        {
        //            ParameterName = "in_cLogin",
        //            DbType = DBConn.DBTypeCustom.String,
        //            Value = login
        //        };

        //    DbConnAuth dbConnAuth = new DbConnAuth();
        //    try
        //    {
        //        DBConn.Conn dbconn = dbConnAuth.connOra();
        //        res = dbconn.ExecuteQuery<int>(Admin_mod_querys.CheckUserLoginQuery, oip);
        //    }
        //    catch (Exception e)
        //    {

        //        Log.Error(e);
        //        res = -1;
        //    }
        //    return res;
        //}

        public int GetUserId(string pCUser)
        {
            int res;
            //DBConn.DBParam[] oip = new DBConn.DBParam[1];
            //oip[0] = new DBConn.DBParam
            //{
            //    ParameterName = "in_clogin",
            //    DbType = DBConn.DBTypeCustom.String,
            //    Value = pCUser
            //};
            DbConnAuth dbConnAuth = new DbConnAuth();
            try
            {
                DBConn.Conn dbconn = dbConnAuth.connOra();
                res = dbconn.ExecuteQuery<int>(Admin_mod_querys.GetUserIdQuery);
            }
            catch (Exception e)
            {

                Log.Error(e);
                res = -1;
            }

            return res;
        }

        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        /// <param name="pCUser"></param>
        /// <param name="pCPassword"></param>
        /// <returns></returns>
        public Int64 AuthUser(string pCUser, string pCPassword)
        {
            Int64 res = 0;
            string hashPassword = Hashing(pCPassword);

            DBConn.DBParam[] oip = new DBConn.DBParam[10];
            oip[0] = new DBConn.DBParam
                {
                    ParameterName = "in_clogin",
                    DbType = DBConn.DBTypeCustom.String,
                    Value = pCUser
                };

            oip[1] = new DBConn.DBParam
                {
                    ParameterName = "in_cpwd",
                    DbType = DBConn.DBTypeCustom.String,
                    Value = hashPassword
                };

            oip[2] = new DBConn.DBParam
                {
                    ParameterName = "in_cLang",
                    DbType = DBConn.DBTypeCustom.String,
                    Value = (System.Web.HttpContext.Current.Session["lang"] != null) ? System.Web.HttpContext.Current.Session ["lang"].ToString() : "Ru-ru"
                };

            oip[3] = new DBConn.DBParam
            {
                ParameterName = "in_cLoginAD",
                DbType = DBConn.DBTypeCustom.String,
                Value = ""
            };

            oip[4] = new DBConn.DBParam
            {
                ParameterName = "in_cIpAdress",
                DbType = DBConn.DBTypeCustom.String,
                Value = ""
            };

            oip[5] = new DBConn.DBParam
            {
                ParameterName = "in_cCompName",
                DbType = DBConn.DBTypeCustom.String,
                Value = ""
            };

            oip[6] = new DBConn.DBParam
            {
                ParameterName = "in_cSessionKey",
                DbType = DBConn.DBTypeCustom.String,
                Value = ""
            };

            oip[7] = new DBConn.DBParam
            {
                ParameterName = "in_cBrowserName",
                DbType = DBConn.DBTypeCustom.String,
                Value = ""
            };

            oip[8] = new DBConn.DBParam
            {
                ParameterName = "out_cResult",
                DbType = DBConn.DBTypeCustom.String,
                Size = 1000,
                Direction = ParameterDirection.Output,
            };

            oip[9] = new DBConn.DBParam
            {
                ParameterName = "out_nUserKey",
                DbType = DBConn.DBTypeCustom.Int64,
                Size = 1000,
                Direction = ParameterDirection.Output,
            };

            LoginSession.UserLogin = pCUser;
            LoginSession.UserPassword = hashPassword;
            LoginSession.ADUserLogin = "";

            DbConnAuth dbConnAuth = new DbConnAuth();
            try
            {
                DBConn.Conn dbconn = dbConnAuth.connOra();
                DataSet ds = dbconn.ExecuteQuery(Admin_mod_querys.LoginQuery, false, oip);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables["Parameters"].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables["Parameters"].Rows)
                    {
                        if (row["Name"].ToString() == "out_nUserKey")
                        {
                            res = Convert.ToInt64(row["Value"]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                
                Log.Error(e);
                res = -1;
            }
            return res;
        }

        /// <summary>
        /// получить ФИО
        /// </summary>
        /// <returns></returns>
        public string GetFIO()
        {
            string res = "";

            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn dbconn = dbConnAuth.connOra();
            try
            {
                //DBConn.DBParam[] oip = null;
                DBConn.DBParam[] oip = new DBConn.DBParam[1];
                oip[0] = new DBConn.DBParam();
                oip[0].ParameterName = "out_cFio";
                oip[0].DbType = DBConn.DBTypeCustom.String;
                oip[0].Size = 1000;
                oip[0].Direction = ParameterDirection.Output;

                DataSet ds = dbconn.ExecuteQuery(Admin_mod_querys.GetfioQuery, false, oip);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables["Parameters"].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables["Parameters"].Rows)
                    {
                        if (row["Name"].ToString() == "out_cFio")
                        {
                            res = row["Value"].ToString();
                        }
                    }
                }
                //res = dbconn.ExecuteQuery<string>(Admin_mod_querys.GetfioQuery, oip);
                //ExecuteQuery(OracleQuerys.GetFullNameParentObjQuery, false, oip);
            }
            catch (Exception e)
            {
                Log.Error(e);
                res = "";
            }

            return res;
        }

        /// <summary>
        /// получить ФИО по ключу пользователя
        /// </summary>
        /// <param name="pCLogin"></param>
        /// <returns></returns>
        //public string GetFIO(string pCLogin)
        //{
        //    string res;
        //    DBConn.DBParam[] oip = new DBConn.DBParam[1];
        //    oip[0] = new DBConn.DBParam
        //        {
        //            ParameterName = "in_clogin",
        //            DbType = DBConn.DBTypeCustom.String,
        //            Value = pCLogin
        //        };

        //    DbConnAuth dbConnAuth = new DbConnAuth();
        //    DBConn.Conn dbconn = dbConnAuth.connOra();
        //    try
        //    {
        //        res = dbconn.ExecuteQuery<string>(Admin_mod_querys.GetfioQuery, oip);
        //    }
        //    catch (Exception e)
        //    {
        //        Log.Error(e);
        //        res = "";
        //    }
        //    return res;
        //}

        /// <summary>
        /// проверка на существование пользователя в системе
        /// </summary>
        /// <param name="pCLogin"></param>
        /// <returns></returns>
        //public int CheckUserLogin(string pCLogin)
        //{

        //    int res;
        //    DBConn.DBParam[] oip = new DBConn.DBParam[1];
        //    oip[0] = new DBConn.DBParam
        //        {
        //            ParameterName = "in_clogin",
        //            DbType = DBConn.DBTypeCustom.String,
        //            Value = pCLogin
        //        };

        //    DbConnAuth dbConnAuth = new DbConnAuth();
        //    DBConn.Conn dbconn = dbConnAuth.connOra();

        //    try
        //    {
        //        res = dbconn.ExecuteQuery<int>(Admin_mod_querys.CheckUserLoginQuery, oip);
        //    }
        //    catch (Exception e)
        //    {
        //        Log.Error(e);
        //        res = -2;
        //    }
        //    return res;
        //}

        public int ChangePassword(string pCPassword)
        {
            int res;
            string hashPassword = Hashing(pCPassword);

            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            //oip[0] = new DBConn.DBParam
            //    {
            //        ParameterName = "in_cUserLogin",
            //        DbType = DBConn.DBTypeCustom.String,
            //        Value = pCUser
            //    };

            oip[0] = new DBConn.DBParam
                {
                    ParameterName = "in_cPassword",
                    DbType = DBConn.DBTypeCustom.String,
                    Value = hashPassword
                };

            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn dbconn = dbConnAuth.connOra();
            try
            {
                res = dbconn.ExecuteQuery<int>(Admin_mod_querys.ChangePasswordQuery, oip);
            }
            catch (Exception e)
            {
                Log.Error(e);
                res = -1;
            }
            return res;
        }


        /// <summary>
        /// проверка на авторизацию
        /// </summary>
        /// <returns></returns>
        public string CheckAuth()
        {
            string res;
            string authMode = WebConfig.AuthMode();
            switch (authMode)
            {
                case "Enable":
                    switch (LoginSession.IsLogin)
                    {
                        case "Authorized":
                            res = "Authorized";
                            break;
                        case "UnAuthorized":
                            res = "unAuthorized";
                            break;
                        default:
                            res = "AuthorizedFail";
                            break;
                    }
                    break;
                case "Disable":
                    res = "OK";
                    break;
                default:
                    //Добавить в лог запись
                    res = "FailReadConfig";
                    break;
            }
            return res;
        }

        /// <summary>
        ///  проверка на доступность модуля
        /// </summary>
        /// <param name="pCModuleName"></param>
        /// <returns></returns>
        public int CheckModuleAccess(string pCModuleName)
        {
            Log.Error("Проверка на то, может открывать данный УРЛ - Вызов базейной функции _moduleName = " + pCModuleName);
            int res = 1;
            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            oip[0] = new DBConn.DBParam
                {
                    ParameterName = "in_cModuleName",  //"p_cmodulename",
                    DbType = DBConn.DBTypeCustom.VarChar2,
                    Direction = ParameterDirection.Input,
                    Value = pCModuleName
                };

            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn dbconn = dbConnAuth.connOra();
            try
            {
                res = dbconn.ExecuteQuery<int>("db_api.WRAPPER_API.checkModuleAccess", oip); //WRAPPER_API].[checkModuleAccess
            }
            catch (Exception e)
            {
                Log.Error("Проверка на то, может открывать данный УРЛ Ошибка =" + e.ToString());
                res = -1;
                throw e;
            }

            Log.Error("Проверка на то, может открывать данный УРЛ результат =" + res.ToString());
            return res;
        }

        
        /// <summary>
        /// создать нового пользователя
        /// </summary>
        /// <param name="pCUser"></param>
        /// <param name="pCPassword"></param>
        /// <param name="pCName"></param>
        /// <returns></returns>
        public double CreateUser(string pCUser, string pCPassword, string pCName)
        {
            double res;
            string hashPassword = Hashing(pCPassword);
            DBConn.DBParam[] oip = new DBConn.DBParam[3];

            oip[0] = new DBConn.DBParam
                {
                    ParameterName = "in_cName",
                    DbType = DBConn.DBTypeCustom.String,
                    Value = pCName
                };

            oip[1] = new DBConn.DBParam
                {
                    ParameterName = "in_cLogin",
                    DbType = DBConn.DBTypeCustom.String,
                    Value = pCUser
                };

            oip[2] = new DBConn.DBParam
                {
                    ParameterName = "in_cpwd",
                    DbType = DBConn.DBTypeCustom.String,
                    Value = hashPassword
                };

            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn dbconn = dbConnAuth.connOra();

            try
            {
                res = dbconn.ExecuteQuery<int>(Admin_mod_querys.AdduserQuery, oip);
            }
            catch (Exception e)
            {

                Log.Error(e);
                res = -1;
            }
            return res;
        }

        /// <summary>
        /// хеширование пароля
        /// </summary>
        /// <param name="pCValue"></param>
        /// <returns></returns>
        public static string Hashing(string pCValue)
        {
            string lRes;
            try
            {
                System.Security.Cryptography.SHA1 hash = System.Security.Cryptography.SHA1.Create();
                System.Text.ASCIIEncoding encoder = new System.Text.ASCIIEncoding();
                byte[] combined = encoder.GetBytes(pCValue);
                hash.ComputeHash(combined);
                lRes = Convert.ToBase64String(hash.Hash);
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
            return lRes;
        }
    }
}