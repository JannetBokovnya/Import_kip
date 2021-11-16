using System.Web;

namespace App_Code.SessionStorage
{
    /// <summary>
    /// Summary description for UserParams
    /// </summary>
    public class UserParams :UserSession
    {
        /// <summary>
        /// Конструктор старта сессии
        /// </summary>
        /// <param name="p_cSessionKey">ключ сессии</param> 
        /// <param name="p_cIpAdress">АйПи адрес</param>
        /// <param name="p_cCompName">имя компьютера</param>
        /// <param name="p_cBrowserName">название браузера</param>
        public void SetUserParams(string p_cSessionKey,  string p_cIpAdress, string p_cCompName,
                          string p_cBrowserName)
        {
            HttpContext.Current.Session["sessionId"] = p_cSessionKey; 
            HttpContext.Current.Session["IpAdress"] = p_cIpAdress;
            HttpContext.Current.Session["CompName"] = p_cCompName;
            HttpContext.Current.Session["BrowserName"] = p_cBrowserName;
        }

        /// <summary>
        /// Имя браузера
        /// </summary>
        public static string BrowserName
        {
            get
            {
                return HttpContext.Current.Session["BrowserName"] != null
                           ? HttpContext.Current.Session["BrowserName"].ToString()
                           : "-1";
            }
        }

        /// <summary>
        /// Название Комьютера
        /// </summary>
        public static string CompName
        {
            get
            {
                return HttpContext.Current.Session["CompName"] != null
                    && HttpContext.Current.Session["CompName"].ToString() != "::1" 
                           ? HttpContext.Current.Session["CompName"].ToString()
                           : "-1";
            }
        }

        /// <summary>
        /// Ip адрес
        /// </summary>
        public static string IpAdress
        {
            get
            {
                return HttpContext.Current.Session["IpAdress"] != null
                    && HttpContext.Current.Session["IpAdress"].ToString() != "::1" 
                           ? HttpContext.Current.Session["IpAdress"].ToString()
                           : "-1";
            }
        }

        /// <summary>
        /// ключ сессии
        /// </summary>
        public static string SessionKey
        {
            get
            {
                return HttpContext.Current.Session["sessionId"] != null
                           ? HttpContext.Current.Session["sessionId"].ToString()
                           : "-1";
            }
        }

    }
}