using System.Collections.Generic;
using System.ServiceModel;
using System.Runtime.Serialization;
using App_Code.Import_kip_API;


[ServiceContract]
public interface IWCFService_ImportKIP
{
    //загрузка мг в комбобокс
    [OperationContract]
    DataMGList_KIP GetDataMGListKIP();

     //загрузка нитей по ключу мг
    [OperationContract]
    DataThreadsList_result GetDataThreadsList(string inKeyMg);

    //дата для Кипа по ключу нити
    [OperationContract]
    KipDataList_result GetDataKipList(string inKeyThread);

    /// <summary>
    /// импорт файла в базу
    /// </summary>
    /// <param name="impKey"></param>
    /// <param name="typeOfImport"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    [OperationContract]
    StatusAnswer_ImportKIP ImportFile(double impKey, string typeOfImport, string fileName);

    /// <summary>
    /// лог
    /// </summary>
    /// <param name="keyJornal"></param>
    /// <returns></returns>
    [OperationContract]
    ImpLogKIP GetImpLog(string keyJornal);

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
    [OperationContract]
    ImpLog_resulAnalyzetKipFile GetImpAnalyzeFileLog(string keyJornal);

    /// <summary>
    /// кип из базы
    /// </summary>
    /// <param name="keyJornal"></param>
    /// <returns></returns>
    [OperationContract]
    KipDataListBD_result GetDataKipListBD(string keyJornal);

    /// <summary>
    /// кип из файла
    /// </summary>
    /// <param name="keyJornal"></param>
    /// <returns></returns>
    [OperationContract]
    KipDataListFile_result GetDataKipListFile(string keyJornal);

    /// <summary>
    /// увязка кипов
    /// </summary>
    /// <param name="in_nVTDMakingKey"></param>
    /// <param name="in_Filekey"></param>
    /// <param name="in_DBKey"></param>
    /// <param name="typeLink"></param>
    /// <returns></returns>
    [OperationContract]
    KeyBoundResultKIP ImportKipMatching(string in_nVTDMakingKey, string in_Filekey, string in_DBKey, int typeLink);

    /// <summary>
    /// увязывание автоматом
    /// </summary>
    /// <param name="in_nVTDMakingKey"></param>
    /// <returns></returns>
    [OperationContract]
    StatusAnswer_ImportKIP ImportKipMatchingAuto(string in_nVTDMakingKey);

    /// <summary>
    /// импорт кип
    /// </summary>
    /// <param name="in_nJournalKey"></param>
    /// <returns></returns>
    [OperationContract]
    StatusAnswer_ImportKIP ImportKipMeasurement(string in_nJournalKey);

    /// <summary>
    /// скрыть кип из файла
    /// </summary>
    /// <param name="in_nJournalKey"></param>
    /// <param name="in_nKipKey"></param>
    /// <param name="isHide"></param>
    /// <returns></returns>
    [OperationContract]
    StatusAnswer_ImportKIP KipIsHide(string in_nJournalKey, string in_nKipKey, int isHide);

    /// <summary>
    /// скрыть кип из бд
    /// </summary>
    /// <param name="in_nJournalKey"></param>
    /// <param name="in_nKipKeyBD"></param>
    /// <param name="isHide"></param>
    /// <returns></returns>
    [OperationContract]
    StatusAnswer_ImportKIP KipIsHideBD(string in_nJournalKey, string in_nKipKeyBD, int isHide);

    /// <summary>
    /// удалить увязанные кипы
    /// </summary>
    /// <param name="keyImport"></param>
    /// <param name="in_Filekey"></param>
    /// <returns></returns>
    [OperationContract]
    RemoveBoundKIP RemoveBound(string keyImport, string in_Filekey);

    /// <summary>
    /// возвращает все увязанные темы
    /// </summary>
    /// <param name="in_nVTDMakingKey"></param>
    /// <returns></returns>
    [OperationContract]
    KeyBoundResultKIP GetKIPMapping(string in_nVTDMakingKey);

    [OperationContract]
    StatusAnswer_ImportKIP DeleteKipMeasurement(string jornalKey);

    /// <summary>
    /// параметры выбранного участка
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    [OperationContract]
    ParamSecList_result GetSec_Param(string key);
}
