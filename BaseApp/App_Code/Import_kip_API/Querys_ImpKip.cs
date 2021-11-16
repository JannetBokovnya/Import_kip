namespace App_Code.Import_kip_API
{
    /// <summary>
    /// Summary description for oracleQuerys
    /// </summary>
    public static class Querys_ImpKip
    {
        /// <summary>
        /// увязка автоматом
        /// </summary>
        private static string applyKipKeyMapQuery = "db_api.IMPORT_KIP_API.ApplyKIPKeyMap ";
        public static string ApplyKipKeyMapQuery
        {
            get { return applyKipKeyMapQuery; }
        }

        /// <summary>
        /// сам импорт
        /// </summary>
        private static string importKipMeasurement = "db_api.IMPORT_KIP_API.ImportKipMeasurement ";

        public static string ImportKipMeasurement
        {
            get { return importKipMeasurement; }
        }

       private static string CreateNewImportKipQuery = "db_api.IMPORT_KIP_API.CreateNewImportKip";

       

        private static string GetKipMeasJournalParamsQuery = "db_api.IMPORT_KIP_API.GetKipMeasJournalParams";

        /// <summary>
        /// по ключу нити дата для кипа
        /// </summary>
        private static string getKipmeasJournalsQuery = "db_api.IMPORT_KIP_API.get_kipmeas_journals";

        public static string GetKipmeasJournalsQuery
        {
            get { return getKipmeasJournalsQuery; }
        }


        /// <summary>
        /// cписок газопроводов
        /// </summary>
        private static string getMgQuery = "db_api.IMPORT_KIP_API.GetMG";

        public static string GetMgQuery
        {
            get { return getMgQuery; }
        }

        /// <summary>
        /// список нитей
        /// </summary>
        private static string getThreadsForMgAllQuery = "db_api.IMPORT_KIP_API.getThreadsForMG";

        public static string GetThreadsForMgAllQuery
        {
            get { return getThreadsForMgAllQuery; }
        }

        /// <summary>
        /// параметры участка
        /// </summary>
        private static string getParamSecQuery = "db_api.IMPORT_KIP_API.Get_KipMeas_JournalParams";

        public static string GetParamSecQuery
        {
            get { return getParamSecQuery; }
        }

        /// <summary>
        /// используется в xls парсере
        /// </summary>
        private static string inportGetTableNameQuery = "db_api.IMPORT_API.GetTableName";

        public static string InportGetTableNameQuery
        {
            get { return inportGetTableNameQuery; }
        }

        /// <summary>
        /// логирование при импорте файла в базу
        /// </summary>
        private static readonly string inportWrite2LogQuery = "db_api.IMPORT_API.Write2Log";

        public static string InportWrite2LogQuery
        {
            get { return inportWrite2LogQuery; }
        }


        /// <summary>
        /// используется в xls парсере
        /// </summary>
        private static string inportGetColumnsQuery = "db_api.IMPORT_API.GetColumns";

        public static string InportGetColumnsQuery
        {
            get { return inportGetColumnsQuery; }
        }

        /// <summary>
        /// лог импорта
        /// </summary>
        private static string getImpLogQuery = "db_api.IMPORT_KIP_API.GetImpLog"; //Возвращает лог по импорту

        public static string GetImpLogQuery
        {
            get { return getImpLogQuery; }
        }

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
        private static string getImpAnalyzeFileLogQuery = "db_api.IMPORT_KIP_API.LogFileAnalysis"; //Возвращает лог по результатам анализа файла импорта КИП

        public static string GetImpAnalyzeFileLogQuery
        {
            get { return getImpAnalyzeFileLogQuery; }
        }

        private static string ImportKipMeasurementQuery = "db_api.IMPORT_KIP_API.ImportKipMeasurement";

        /// <summary>
        /// список кипов из базы
        /// </summary>
        private static string getKipListForJournalQuery = "db_api.IMPORT_KIP_API.GetKipListForJournal";

        public static string GetKipListForJournalQuery
        {
            get { return getKipListForJournalQuery; }
        }

        /// <summary>
        /// список кипов из файла
        /// </summary>
        private static string getKipListFromFileQuery = "db_api.IMPORT_KIP_API.GetKipListFromFile";

        public static string GetKipListFromFileQuery
        {
            get { return getKipListFromFileQuery; }
        }

        /// <summary>
        /// ручная увязка кипов
        /// </summary>
        private static string importTubeMatchingQuery = "DB_API.IMPORT_KIP_API.addKipMapping";

        public static string ImportTubeMatchingQuery
        {
            get { return importTubeMatchingQuery; }
        }

        /// <summary>
        /// удалить связанные кипы
        /// </summary>
        private static string rollbackTemMappingQuery = "db_api.IMPORT_KIP_API.rollbackKipMapping";

        public static string RollbackTemMappingQuery
        {
            get { return rollbackTemMappingQuery; }
        }

        /// <summary>
        /// возвращает список всех увязанных кипов
        /// </summary>
        private static string getMappedKipListgQuery = "DB_API.IMPORT_KIP_API.getMappedKipList";

        public static string GetMappedKipListgQuery
        {
            get { return getMappedKipListgQuery; }
        }

        /// <summary>
        /// удаление предыдущего импорта
        /// </summary>
        private static string deleteKipMeasurementQuery = "DB_API.IMPORT_KIP_API.DeleteKipMeasurement";

        public static string DeleteKipMeasurementQuery
        {
            get { return deleteKipMeasurementQuery; }
        }

        /// <summary>
        /// скрыть выбранные ктп из файла
        /// </summary>
        private static string hideKipQuery = "DB_API.IMPORT_KIP_API.manageUseOfFileMeasurement";

        public static string HideKipQuery
        {
            get { return hideKipQuery; }
        }

        /// <summary>
        /// скрыть выбранные кип из бд
        /// </summary>
        private static string hideKipBDQuery = "DB_API.IMPORT_KIP_API.manageUseOfDbKip";

        public static string HideKipBDQuery
        {
            get { return hideKipBDQuery; }
        }
    }
}