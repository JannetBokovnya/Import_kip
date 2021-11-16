using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;



[DataContract]
public class VALUES
{
    private string _name;
    private string _type;
    private string _in_out;
    private int _sequence;
    [DataMember]
    public int SEQUENCE
    {
        get { return _sequence; }
        set { _sequence = value; }
    }
    [DataMember]
    public string IN_OUT
    {
        get { return _in_out; }
        set { _in_out = value; }
    }
    [DataMember]
    public string TYPE
    {
        get { return _type; }
        set { _type = value; }
    }
    [DataMember]
    public string NAME
    {
        get { return _name; }
        set { _name = value; }
    }
}


[DataContract]
public class TYPE
{
    private string _OBJECTNAME;
    private string _RETURN;

    [DataMember]
    public string RETURN
    {
        get { return _RETURN; }
        set { _RETURN = value; }
    }

    [DataMember]
    public string OBJECTNAME
    {
        get { return _OBJECTNAME; }
        set { _OBJECTNAME = value; }
    }
}

/// <summary>
/// Summary description for CallSignature
/// </summary>
[DataContract]
public class CallSignature
{

    
    private  TYPE _retvalue;
    private  List<VALUES> _inParams;


     [DataMember]
    public List<VALUES> VALUES
    {
        get { return _inParams; }
        set { _inParams = value; }
    }

     [DataMember]
     public TYPE TYPE
    {
        get { return _retvalue; }
        set { _retvalue = value; }
    }

     [DataMember]
     public string value2 { get; set; }
}