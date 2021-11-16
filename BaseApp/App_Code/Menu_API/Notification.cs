using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Notification
/// </summary>
public class Notification
{
    private double key;
    private string message;
    private string appName;
    private string state;
    private string userKey;
    private double curDate;
    private bool isDelivered;

	public Notification()
	{
	}

    public Notification(double Key, string Message, string AppName, string State, string UserKey, double CurDate, bool IsDelivered)
    {
        key = Key;
        message = Message;
        appName = AppName;
        state = State;
        userKey = UserKey;
        curDate = CurDate;
        isDelivered = IsDelivered;
    }

    public double Key { get { return key; } set { key = value; } }
    public string Message { get { return message; } set { message = value; } }
    public string AppName { get { return appName; } set { appName = value; } }
    public string State { get { return state; } set { state = value; } }
    public string UserKey { get { return userKey; } set { userKey = value; } }
    public double CurDate { get { return curDate; } set { curDate = value; } }
    public bool IsDelivered { get { return isDelivered; } set { isDelivered = value; } }

    public static double GetMaxDate(List<Notification> lstNotification)
    {
        if (lstNotification.Count > 0)
        {
            return lstNotification.Max(ntf => ntf.CurDate);
        }
        else
        {
            return 0;
        }
    }

}