using System;

namespace App_Code.Admin_module_API
{
    /// <summary>
    /// Summary description for Ilogin
    /// </summary>
    public interface IAuth
    {
        /// <summary>
        /// аутентифицировать пользователя
        /// </summary>
        /// <param name="pCUser"></param>
        /// <param name="pCPassword"></param>
        /// <returns></returns>
        Int64 AuthUser(string pCUser, string pCPassword);

        /// <summary>
        /// получить ФИО текущего пользователя
        /// </summary>
        /// <returns></returns>
        string GetFIO();

        /// <summary>
        /// получить ФИО по ключу пользователя
        /// </summary>
        /// <param name="pCLogin"></param>
        /// <returns></returns>
        //string GetFIO(string pCLogin);

        /// <summary>
        /// создать нового пользователя
        /// </summary>
        /// <param name="pCUser"></param>
        /// <param name="pCPassword"></param>
        double CreateUser(string pCUser, string pCPassword, string pCName);

        /// <summary>
        /// проверка на существование пользователя в системе
        /// </summary>
        /// <param name="pCLogin"></param>
        /// <returns></returns>
        //int CheckUserLogin(string pCLogin);

        /// <summary>
        /// задать новый пароль
        /// </summary>
        /// <param name="pCUser"></param>
        /// <param name="pCPassword"></param>
        /// <returns></returns>
        int ChangePassword(string pCPassword);

        /// <summary>
        /// проверка на авторизацию
        /// </summary>
        /// <returns></returns>
        string CheckAuth();

        /// <summary>
        ///  проверка на доступность модуля
        /// </summary>
        /// <param name="pCModuleName"></param>
        /// <returns></returns>
        int CheckModuleAccess(string pCModuleName);


        
    }

}