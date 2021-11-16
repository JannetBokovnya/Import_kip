using System.Web;

namespace App_Code.SessionStorage
{
    /// <summary>
    /// Summary description for UserSession
    /// </summary>
    public class UserSession
    { 
        /// <summary>
        /// отчистить все сессионные переменные
        /// </summary>
        public static void DisposeStorage()
        {
            HttpContext.Current.Session.Clear(); 
            // TODO: добавить удаление информации о пользователе
            // FormsAuthentication.SignOut();
        }

        /// <summary>
        /// идентификатор что сессия запущена
        /// </summary>
        public static string SessionStart
        {
            get
            {
                return HttpContext.Current.Session["SessionStart"] != null
                           ? HttpContext.Current.Session["SessionStart"].ToString()
                           : "-1";
            }
            set { HttpContext.Current.Session["SessionStart"] = value; }
        }

        public static void SetSessionParamsIfNull(System.Web.SessionState.HttpSessionState session, HttpRequest request)
        {
            if (SessionStart != "up")
            {
                //инициализация информации о сессии
                SetUserSessionParams(session, request);
                SessionStart = "up";
            }
        }

        /// <summary>
        /// задать сессионные параметры
        /// </summary>
        private static void SetUserSessionParams(System.Web.SessionState.HttpSessionState session, HttpRequest request)
        {
            string sessionKey = session.SessionID;
            string ipAdress = request.UserHostAddress;

            string compName = (!string.IsNullOrEmpty(ipAdress)) ? System.Net.Dns.Resolve(ipAdress).HostName : "";
            string browserName = request.Browser.Browser + " " + request.Browser.Version;

            UserParams userParams = new UserParams();
            userParams.SetUserParams(sessionKey, ipAdress, compName, browserName);
        }
    }
}