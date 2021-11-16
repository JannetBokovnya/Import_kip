using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for IBaseClass
/// </summary>
public interface IBaseClass
{
    /// <summary>
    /// проверка на просмотр модуля
    /// </summary>
    /// <param name="p_cModuleName">название модуля</param>
    /// <returns>1- есть права, 0- нет прав</returns>
    void SetModuleName(string p_cModuleName);
    
    /// <summary>
    /// фиксирует событие которое произошло в модулях
    /// </summary>
    /// <param name="p_cModuleName">название модуля</param>
    /// <returns>название события</returns>
    void moduleEvent(string moduleEvents, string p_cModuleName, string p_cValue);
    
}