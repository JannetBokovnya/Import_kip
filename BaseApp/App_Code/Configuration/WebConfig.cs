using System;
using System.Configuration;

/// <summary>
/// Summary description for WebConfig
/// </summary>
public class WebConfig
{
    /// <summary>
    /// получить коннекшн в базу
    /// </summary>
    /// <returns></returns>
	public static string GetDBConnection()
    {
        return ConfigurationManager.ConnectionStrings["DBConn"].ToString();
	}

    public static string GetDBProvider()
    {
        return ConfigurationManager.ConnectionStrings["DBConn"].ProviderName;
    }
    /// <summary>
    /// Конфигурация авторизации (включена\выключена)
    /// </summary>
    /// <returns></returns>
    public static string AuthMode()
    {
        return ConfigurationManager.AppSettings["AuthMode"];
    }
    /// <summary>
    /// стартовая страница
    /// </summary>
    /// <returns></returns>
    public static string GetStartPage ()
    {
        return ConfigurationManager.AppSettings["startPage"];
    }

    /// <summary>
    /// стартовая страница
    /// </summary>
    /// <returns></returns>
    public static string GetRedmineUrl(out bool isProd)
    {
        const string projStr = "projects";
        string prod = ConfigurationManager.AppSettings["production"];
        string retVal = ConfigurationManager.AppSettings["redmineUrl"];
        
        if (!String.IsNullOrEmpty(retVal) && 
            !String.IsNullOrEmpty(prod) && 
            prod.ToLower() == "false")
        {
            int index = retVal.IndexOf(projStr, StringComparison.Ordinal);
            if (index > 0 && retVal.Length > (index + projStr.Length))
                retVal = retVal.Remove(index + projStr.Length);

            isProd = false;
        }
        else
            isProd = true;

        return retVal;
    }

    /// <summary>
    /// ссылка на страницу с ошибкой авторизации
    /// </summary>
    /// <returns></returns>
    public static string GetErrorAuthPage()
    {
        return ConfigurationManager.AppSettings["ErrorAuthPage"];
    } 

    /// <summary>
    /// страница логона
    /// </summary>
    /// <returns></returns>
    public static string GetLoginPage()
    {
        return ConfigurationManager.AppSettings["loginPage"];
    }

    /// <summary>
    /// страница логона
    /// </summary>
    /// <returns></returns>
    public static string GetDataProvider()
    {
        return ConfigurationManager.AppSettings["DataProvider"];
    }

    /// <summary>
    /// Имя Web-сервиса для загрузки файлов на сервер
    /// </summary>
    /// <returns></returns>
    public static string GetFileUploadServiceName()
    {
        return ConfigurationManager.AppSettings["FileUploadServiceName"];
    }

    public static string GetTelerikUserSkin()
    {
        return ConfigurationManager.AppSettings["TelerikUserSkin"];
    }

    /// <summary>
    /// Установлено ли многоязычие в системе
    /// </summary>
    /// <returns></returns>
    public static bool GetMultiLangParam()
    {
        string multiLang = ConfigurationManager.AppSettings["multiLang"];

        return (!String.IsNullOrEmpty(multiLang) && multiLang == "Enable");
    }

    /// <summary>
    /// Установлено ли многоязычие в системе
    /// </summary>
    /// <returns></returns>
    public static bool GetIsShowMessageForGuest()
    {
        string multiLang = ConfigurationManager.AppSettings["showMessageForGuest"];

        return (!String.IsNullOrEmpty(multiLang) && multiLang == "Enable");
    }


}