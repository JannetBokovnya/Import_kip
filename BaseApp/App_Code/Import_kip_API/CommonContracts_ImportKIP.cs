using System.Runtime.Serialization;


/// <summary>
/// Базовый класс для "[DataContract]" классов, которым необходимо возвращать статус 
/// выполнения сервисной функции и сообщение об ошибке
/// </summary>

[DataContract]
public class StatusAnswer_ImportKIP
{
    [DataMember]
    public bool IsValid { get; set; }

    [DataMember]
    public string ErrorMessage { get; set; }

    #region Ctor

    public StatusAnswer_ImportKIP()
    {
        IsValid = true;
        ErrorMessage = "OK";
    }
    #endregion Ctor
}