using System.Web;

namespace App_Code.SessionStorage
{
    /// <summary>
    /// Summary description for LoginSession
    /// </summary>
    public class LoginSession : UserSession
    {

        public void SetUserSession(string p_IsAuth, string p_cUserGuid, string p_cUserName, string p_cUserFIO)
        {
             
             HttpContext.Current.Session["IsAuth"] = p_IsAuth;
             HttpContext.Current.Session["UserGUID"] = p_cUserGuid;
             HttpContext.Current.Session["UserName"] = p_cUserName;
             HttpContext.Current.Session["UserFIO"] = p_cUserFIO;
        }

        /// <summary>
        /// Проверка на авторизацию
        /// </summary>
        public static string IsLogin
        {
            get
            {
                return HttpContext.Current.Session["IsAuth"] != null
                           ? HttpContext.Current.Session["IsAuth"].ToString()
                           : "-1";
            }  
        }

        /// <summary>
        /// ключ пользователя
        /// </summary>
        public static string UserGuid
        {
            get
            {
                return HttpContext.Current.Session["UserGUID"] != null
                           ? HttpContext.Current.Session["UserGUID"].ToString()
                           : "-1";
            }
            set
            {
                HttpContext.Current.Session["UserGUID"] = value;
            }
        }

        /// <summary>
        /// имя пользователя пользователя
        /// </summary>
        public static string UserName
        {
            get
            {
                return HttpContext.Current.Session["UserName"] != null
                           ? HttpContext.Current.Session["UserName"].ToString()
                           : "-1";
            }
            set
            {
                HttpContext.Current.Session["UserName"] = value;
            }
        }

        /// <summary>
        /// Логин пользователя
        /// </summary>
        public static string UserLogin
        {
            get
            {
                return HttpContext.Current.Session["userLogin"] != null
                           ? HttpContext.Current.Session["userLogin"].ToString()
                           : "-1";
            }
            set
            {
                HttpContext.Current.Session["userLogin"] = value;
            }
        }

        /// <summary>
        /// Логин пользователя
        /// </summary>
        public static string ADUserLogin
        {
            get
            {
                return HttpContext.Current.Session["ADuserLogin"] != null
                           ? HttpContext.Current.Session["ADuserLogin"].ToString()
                           : "-1";
            }
            set
            {
                HttpContext.Current.Session["ADuserLogin"] = value;
            }
        }

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        public static string UserPassword
        {
            get
            {
                return HttpContext.Current.Session["userPassword"] != null
                           ? HttpContext.Current.Session["userPassword"].ToString()
                           : "-1";
            }
            set
            {
                HttpContext.Current.Session["userPassword"] = value;
            }
        }

        /// <summary>
        /// Язык пользователя
        /// </summary>
        public static string UserLang
        {
            get
            {
                return HttpContext.Current.Session["lang"] != null
                           ? HttpContext.Current.Session["lang"].ToString()
                           : "-1";
            }
            set
            {
                HttpContext.Current.Session["lang"] = value;
            }
        }

        /// <summary>
        /// ФИО пользователя
        /// </summary>
        public static string UserFIO
        {
            get
            {
                return HttpContext.Current.Session["UserFIO"] != null
                           ? HttpContext.Current.Session["UserFIO"].ToString()
                           : "";
            }
            set
            {
                HttpContext.Current.Session["UserFIO"] = value;
            }
        }

    }
}