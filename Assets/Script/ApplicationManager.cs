using System;
using UnityEngine;

public class ApplicationManager
{
    private static string locUserId;

    public static string LocUserId
    {
        get
        {
            if (locUserId != null)
                return locUserId;
            //else throw new Exception("locUserId == null");
            else return "17414";
        }

        set
        {
            locUserId = value;
        }
    }
}