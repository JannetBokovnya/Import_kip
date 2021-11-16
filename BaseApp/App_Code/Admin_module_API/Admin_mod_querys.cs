/// <summary>
/// Summary description for Querys
/// </summary>
public static class Admin_mod_querys
{
    //public static string CheckUserLoginQuery { get { return "db_api.admin_api.checkUserLogin"; } }

    public static string LoginQuery { get { return "db_api.wrapper_api.login"; } }

    public static string GetUserIdQuery { get { return "db_api.wrapper_api.getUserId"; } }

    public static string GetfioQuery { get { return "db_api.wrapper_api.getfio"; } }

    public static string ChangePasswordQuery { get { return "db_api.wrapper_api.change_password"; } }

    public static string CheckmoduleaccessQuery { get { return "db_api.wrapper_api.checkmoduleaccess"; } }

    public static string AdduserQuery { get { return "db_api.admin_api.adduser"; } }

    public static string CreateeventQuery { get { return "DB_API.wrapper_api.createevent"; } }
}