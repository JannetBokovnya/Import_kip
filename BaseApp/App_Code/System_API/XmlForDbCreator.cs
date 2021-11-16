using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

/// <summary>
/// Класс для создания строки в многоязычием перед отправкой в БД
/// </summary>
public static class XmlForDbCreator
{
    /// <summary>
    /// Возвращает DateTime.Now Ru и En
    /// </summary>
    /// <returns></returns>
    public static string GetDateTimeNow()
    {
        DateTime dateNow = DateTime.Now;
        return Wrap(dateNow.ToString(CultureInfo.CreateSpecificCulture("ru-RU")),
                        dateNow.ToString(CultureInfo.CreateSpecificCulture("en-US")));
    }

    /// <summary>
    /// Возвращает DateTime.UtcNow Ru и En
    /// </summary>
    /// <returns></returns>
    public static string GetDateTimeUtcNow()
    {
        DateTime dateNow = DateTime.UtcNow;
        return Wrap(dateNow.ToString(CultureInfo.CreateSpecificCulture("ru-RU")),
                    dateNow.ToString(CultureInfo.CreateSpecificCulture("en-US")));
    }

    /// <summary>
    /// Возвращает обертку из двух строк в Ru и En
    /// </summary>
    /// <returns></returns>
    public static string Wrap(string ruStr, string enStr)
    {
        return "<ru-RU>" + ruStr + "</ru-RU>" +
                "<en-US>" + enStr + "</en-US>";

    }

    /// <summary>
    /// Возвращает обертку язык-значение из IEnumerable
    /// </summary>
    /// <returns></returns>
    public static string Wrap(IEnumerable<Tuple<string, string>> lstStr)
    {
        return lstStr.Aggregate("", (current, str) => current + ("<" + str.Item1 + ">" + str.Item2 + "</" + str.Item1 + ">"));
    }
}