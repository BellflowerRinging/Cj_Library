using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.Events;

public class RegistUser : MonoBehaviour
{
    public InputField m_LoginNameInput;
    public InputField m_PasswordInput;
    public InputField m_PasswordAgainInput;
    public Dropdown m_TypeInput;
    public Dropdown m_SafeQuestion;
    public InputField m_SafePassword;


    public Text Hint;

    private void Start()
    {

    }

    ///成功返回id 否则返回 -1
    public int Regist()
    {
        if (string.IsNullOrEmpty(m_LoginNameInput.text))
        {
            ShowHint("账号不能为空！");
            return -1;
        }
        else if (UserManager.GetUserByLoginName(m_LoginNameInput.text) != null)
        {
            ShowHint("账号已经存在！");
            return -1;
        }
        else if (string.IsNullOrEmpty(m_PasswordInput.text))
        {
            ShowHint("密码不能为空！");
            return -1;
        }
        else if (string.IsNullOrEmpty(m_PasswordAgainInput.text))
        {
            ShowHint("请再次输入密码！");
            return -1;
        }
        else if (string.IsNullOrEmpty(m_SafePassword.text))
        {
            ShowHint("请输入安全问题的答案！");
            return -1;
        }
        else if (m_PasswordInput.text != m_PasswordAgainInput.text)
        {
            ShowHint("两次输入密码不相同！");
            return -1;
        }

        string type = m_TypeInput.options[m_TypeInput.value].text;
        User user = new User(
            m_LoginNameInput.text,
            m_PasswordInput.text,
            type,
            new SafePassword
            {
                Question = m_SafeQuestion.options[m_SafeQuestion.value].text,
                Answer = m_SafePassword.text
            }
            );

        int id;
        do id = UnityEngine.Random.Range(10000, 100000); while (UserManager.IdIsHave(id));

        user.m_ID = id + "";
        user.m_UserName = type + id;

        UserManager.SaveUser(user);

        ResetField();

        if (FindObjectOfType<UIGroundContorll>() != null)
            FindObjectOfType<UIGroundContorll>().ShowLoginGround();

        return id;
    }

    [SerializeField]
    public void Regist(Page_User page)
    {
        if (Regist() != -1)
            page.RegistCallBack(this);
    }


    void ResetField()
    {
        m_LoginNameInput.text = "";
        m_PasswordInput.text = "";
        m_PasswordAgainInput.text = "";
        m_SafePassword.text = "";
    }

    public void ShowHint(string str)
    {
        //StopCoroutine(IeHint);
        StopAllCoroutines();
        Hint.enabled = true;
        Hint.text = str;
        StartCoroutine(HintHiden());
    }

    IEnumerator HintHiden()
    {
        yield return new WaitForSeconds(3);
        Hint.enabled = false;
    }

}