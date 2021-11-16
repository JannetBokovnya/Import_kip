using System.Runtime.Serialization;
using System.Collections.Generic;


    /// <summary>
    /// список газопроводов
    /// </summary>
    [DataContract]
public class DataMGList_KIP : StatusAnswer_ImportKIP
    {
        [DataMember]
        public List<DataMGListKIP> DataMGLists { get; set; }
    }


    [DataContract]
    public class DataMGListKIP
    {
        /// <summary>
        /// Ключ газопровода
        /// </summary>
        [DataMember]
        public string KEYMG { get; set; }

        /// <summary>
        /// Название газопровода
        /// </summary>
        [DataMember]
        public string NAMEMG { get; set; }
    }

    /// <summary>
    /// списое нитей на газопроводе
    /// </summary>
    [DataContract]
    public class DataThreadsList_result : StatusAnswer_ImportKIP
    {
        [DataMember]
        public List<DataThreadsList> DataThreadsLists { get; set; }
    }


    [DataContract]
    public class DataThreadsList
    {
        /// <summary>
        /// Ключ газопровода
        /// </summary>
        [DataMember]
        public string KEYTHREADS { get; set; }

        /// <summary>
        /// Название газопровода
        /// </summary>
        [DataMember]
        public string NAMEHREADS { get; set; }
    }

//getKipmeasJournals
    /// <summary>
    /// список журналов  для кипов
    /// </summary>
    [DataContract]
    public class KipDataList_result : StatusAnswer_ImportKIP
    {
        [DataMember]
        public List<KipDataList> KipDataLists { get; set; }
    }


    [DataContract]
    public class KipDataList
    {
        /// <summary>
        /// Ключ даты
        /// </summary>
        [DataMember]
        public string nJourKey { get; set; }

        /// <summary>
        /// Дата Кипа
        /// </summary>
        [DataMember]
        public string dMeasDate { get; set; }
    }

/// <summary>
/// параметры участка
/// </summary>
    [DataContract]
    public class ParamSecList_result : StatusAnswer_ImportKIP
    {
        [DataMember]
        public List<ParamSecList> ParamSecLists { get; set; }
    }


    [DataContract]
    public class ParamSecList
    {
        /// <summary>
        /// км начало
        /// </summary>
        [DataMember]
        public string NKM_BEGIN { get; set; }

        /// <summary>
        /// км конец
        /// </summary>
        [DataMember]
        public string NKM_END { get; set; }

        /// <summary>
        /// длина
        /// </summary>
        [DataMember]
        public string NLENGTH { get; set; }

        /// <summary>
        /// количество кип 
        /// </summary>
        [DataMember]
        public string NUMBERKIP { get; set; }
    }


/// <summary>
/// лог по импорту
/// </summary>
    [DataContract]
    public class ImpLogKIP : StatusAnswer_ImportKIP
    {
        [DataMember]
        public ImpLog_resultKIP ImpLogKIP_result { get; set; }
    }

    [DataContract]
    public class ImpLog_resultKIP
    {
        [DataMember]
        public string ImpLogKIP_result_ret { get; set; }
    }

/// <summary>
///by Gaitov 07.05.2015 г.
/// Лог по результатам анализа файла импорта КИП.
/// Возвращает данные в формате:
///     Статистика по количеству строк в файле:
///     Импортировано	xxx
///     Не заполнено: 
///         «Километраж КИП»	xxx
///         «Потенциал Uт-з (В)»	xxx
///     Будут пропущены	xxx
///  </summary>
    [DataContract]
    public class ImpLog_resulAnalyzetKipFile : StatusAnswer_ImportKIP
    {
        [DataMember]
        public ImpLogAnaliz_resultKIP ImpLogAnalizKIP_result { get; set; }
    }

    [DataContract]
    public class ImpLogAnaliz_resultKIP
    {
        [DataMember]
        public string ImpLogAnalizKIP_result_ret { get; set; }
    }
/// <summary>
/// кип из базы
/// </summary>
    [DataContract]
    public class KipDataListBD_result : StatusAnswer_ImportKIP
    {
        [DataMember]
        public List<KipDataListBD> KipDataListsBD { get; set; }
    }


    [DataContract]
    public class KipDataListBD
    {
        /// <summary>
        /// Ключ KIP
        /// </summary>
        [DataMember]
        public string NKIP_KEY { get; set; }

        /// <summary>
        /// эксплуатационный километраж
        /// </summary>
        [DataMember]
        public string NKM { get; set; }

        /// <summary>
        /// тип
        /// </summary>
        [DataMember]
        public string cType { get; set; }

        /// <summary>
        /// название
        /// </summary>
        [DataMember]
        public string CNAME { get; set; }
    }

    [DataContract]
    public class KipDataListFile_result : StatusAnswer_ImportKIP
    {
        [DataMember]
        public List<KipDataListFile> KipDataListsFile { get; set; }
    }


    [DataContract]
    public class KipDataListFile
    {
        /// <summary>
        /// Ключ KIP
        /// </summary>
        [DataMember]
        public string nrawkey { get; set; }

        /// <summary>
        /// эксплуатационный километраж
        /// </summary>
        [DataMember]
        public string nkm { get; set; }

        /// <summary>
        ///U т-з
        /// </summary>
        [DataMember]
        public string nu_tz { get; set; }

        /// <summary>
        /// U пол
        /// </summary>
        [DataMember]
        public string nu_pol { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        [DataMember]
        public string ckipnum { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        [DataMember]
        public string ccomment { get; set; }
    }

    /// <summary>
    /// получаем список ключей при связывании 
    /// </summary>

    [DataContract]
    public class KeyBoundResultKIP : StatusAnswer_ImportKIP
    {
        [DataMember]
        public List<KeyBoundKIP> GetKeyBoundList { get; set; }
    }

    [DataContract]
    public class KeyBoundKIP
    {
        [DataMember]
        public string KeyBD;
        [DataMember]
        public string KeyFile;
    }

    /// <summary>
    /// удаляет увязанные трубы
    /// </summary>
    [DataContract]
    public class RemoveBoundKIP : StatusAnswer_ImportKIP
    {
        [DataMember]
        public RemoveBoundResultKIP RemoveBoundKIP_Result { get; set; }
    }
    [DataContract]
    public class RemoveBoundResultKIP
    {
        [DataMember]
        public int RemoveBoundKIP { get; set; }
    }
