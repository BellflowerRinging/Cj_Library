using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class User
{
    public string m_ID { get; set; }
    public string m_UserName { get; set; }
    public string m_LoginName { get; set; }
    public string m_Password { get; set; }
    public string m_Type { get; set; }
    public int m_BookCount { get; set; }
    public SafePassword m_SafePassword { get; set; }
    public bool m_Error { get; set; }
    
    public User(string m_LoginName, string m_Password, string m_Type, SafePassword m_SafePassword)
    {
        this.m_LoginName = m_LoginName;
        this.m_Password = m_Password;
        this.m_SafePassword = m_SafePassword;
        this.m_Type = m_Type;
        m_BookCount = 0;
        m_Error = false;
    }

}

public struct SafePassword
{
    public string Question;
    public string Answer;
};