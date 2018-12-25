using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;

public class UserManager : MonoBehaviour
{
    static List<User> Users;
    static string UserJsonFileName;

    private void Start()
    {
        init();
    }

    static void init()
    {
        UserJsonFileName = Application.dataPath + "/Resources/UserData.json";

        Users = new List<User>();

        LoadUsers();
    }

    static void LoadUsers()
    {
        string users = Resources.Load("UserData").ToString();

        Users = JsonConvert.DeserializeObject<List<User>>(users);
    }

    static public bool IdIsHave(int id)
    {
        foreach (var user in Users)
        {
            if (string.IsNullOrEmpty(user.m_ID)) continue;

            if (int.Parse(user.m_ID) == id)
            {
                return true;
            }
        }
        return false;
    }

    static public void SaveUser(User user)
    {
        User tuser = GetUserByLoginName(user.m_LoginName);

        if (tuser != null)
        {
            tuser.m_UserName = user.m_UserName;
            tuser.m_Type = user.m_Type;
            tuser.m_BookCount = user.m_BookCount;
        }
        else
        {
            Users.Add(user);
        }

        SaveAll();
    }

    static public void DelUser(string loginName)
    {
        User tuser = GetUserByLoginName(loginName);

        if (tuser != null)
        {
            Users.Remove(tuser);
        }

        SaveAll();
    }

    static public void SaveAll()
    {
        string str = JsonConvert.SerializeObject(Users);

        ResourcesManager.WriteToJsonFile(UserJsonFileName, str);
    }

    static public User GetUserByLoginName(string name)
    {
        foreach (var user in Users)
        {
            if (user.m_LoginName == name)
            {
                return user;
            }
        }
        return null;
    }

    static public User GetUserById(string id)
    {
        return GetUserById(Int32.Parse(id));
    }
    static public User GetUserById(int id)
    {
        foreach (var user in GetUsers())
        {
            if (user.m_ID == id + "")
            {
                return user;
            }
        }
        return null;
    }

    static public List<User> GetUsers(string type = "所有用户")
    {
        if (Users == null) init();

        if (type != "所有用户")
        {
            List<User> list = new List<User>();
            Users.ForEach((user) =>
            {
                if (user.m_Type == type)
                {
                    list.Add(user);
                }
            });
            return list;
        }
        else return Users;
    }
}