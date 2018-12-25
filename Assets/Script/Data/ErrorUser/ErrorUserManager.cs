using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;

public class ErrorUserManager : MonoBehaviour
{
    static List<ErrorUser> ErrorUsers;
    static string ErrorUserJsonFileName;

    private void Start()
    {
        init();
    }

    static void init()
    {
        ErrorUserJsonFileName = Application.dataPath + "/Resources/ErrorUserData.json";

        ErrorUsers = new List<ErrorUser>();

        LoadErrorUsers();
    }

    static void LoadErrorUsers()
    {
        object o = Resources.Load("ErrorUserData");

        if (o == null)
        {
            SaveAll();
            return;
        }

        string str = o.ToString();

        ErrorUsers = JsonConvert.DeserializeObject<List<ErrorUser>>(str);
    }

    static public bool IdIsHave(int id)
    {
        foreach (var ErrorUser in ErrorUsers)
        {
            if (string.IsNullOrEmpty(ErrorUser.RecordId)) continue;

            if (int.Parse(ErrorUser.RecordId) == id)
            {
                return true;
            }
        }
        return false;
    }

    static public void SaveErrorUser(ErrorUser ErrorUser)
    {
        ErrorUser tErrorUser = GetErrorUserById(ErrorUser.RecordId);

        if (tErrorUser != null)
        {
            //tErrorUser.m_ErrorUserName = ErrorUser.m_ErrorUserName;
        }
        else
        {
            ErrorUsers.Add(ErrorUser);
            UserManager.GetUserById(ErrorUser.ReaderId).m_Error = true;
            UserManager.SaveAll();
        }

        SaveAll();
    }

    static public void DelErrorUser(string id)
    {
        DelErrorUser(int.Parse(id));
    }
    static public void DelErrorUser(int id)
    {
        ErrorUser tErrorUser = GetErrorUserById(id);

        if (tErrorUser != null)
        {
            ErrorUsers.Remove(tErrorUser);
        }

        SaveAll();
    }

    static public void SaveAll()
    {
        string str = JsonConvert.SerializeObject(ErrorUsers);

        ResourcesManager.WriteToJsonFile(ErrorUserJsonFileName, str);
    }

    static public ErrorUser GetErrorUserById(string id)
    {
        return GetErrorUserById(int.Parse(id));
    }

    static public ErrorUser GetErrorUserById(int id)
    {
        foreach (var ErrorUser in ErrorUsers)
        {
            if (ErrorUser.RecordId == id + "")
            {
                return ErrorUser;
            }
        }
        return null;
    }

    static public List<ErrorUser> GetErrorUsers()
    {
        if (ErrorUsers == null) init();

        return ErrorUsers;
    }


    static public List<ErrorUser> GetErrorUsersByReaderId(string id)
    {
        List<ErrorUser> result = new List<ErrorUser>();
        foreach (var ErrorUser in ErrorUsers)
        {
            if (ErrorUser.ReaderId == id + "")
            {
                result.Add(ErrorUser);
            }
        }
        return result;
    }
    /* 
    static public List<ErrorUser> GetErrorUsersByGmId(string id)
    {
        List<ErrorUser> result = new List<ErrorUser>();
        foreach (var ErrorUser in ErrorUsers)
        {
            if (ErrorUser.m_GmId == id + "")
            {
                result.Add(ErrorUser);
            }
        }
        return result;
    }

    static public List<ErrorUser> GetErrorUsersByBookId(string id)
    {
        List<ErrorUser> result = new List<ErrorUser>();
        foreach (var ErrorUser in ErrorUsers)
        {
            if (ErrorUser.m_BookId == id + "")
            {
                result.Add(ErrorUser);
            }
        }
        return result;
    }*/
}