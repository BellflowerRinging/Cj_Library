using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RePassword : MonoBehaviour
{


    public InputField m_LoginNameInput;
    public InputField m_PasswordInput;
    public InputField m_PasswordAgainInput;
    public Dropdown m_SafeQuestion;
    public InputField m_SafePassword;

    public Text Hint;

    private void Start()
    {

    }

    public void Repassword()
    {
        User user;

        if (string.IsNullOrEmpty(m_LoginNameInput.text))
        {
            ShowHint("账号不能为空！");
            return;
        }
        else if ((user = UserManager.GetUserByLoginName(m_LoginNameInput.text)) == null)
        {
            ShowHint("账号不存在！");
            return;
        }
        else if (string.IsNullOrEmpty(m_SafePassword.text))
        {
            ShowHint("请输入安全问题的答案！");
            return;
        }
        else if (user.m_SafePassword.Question != m_SafeQuestion.options[m_SafeQuestion.value].text || user.m_SafePassword.Answer != m_SafePassword.text)
        {
            ShowHint("安全问题答案错误！");
            return;
        }

        if (string.IsNullOrEmpty(m_PasswordInput.text))
        {
            ShowHint("密码不能为空！");
            return;
        }
        else if (string.IsNullOrEmpty(m_PasswordAgainInput.text))
        {
            ShowHint("请再次输入密码！");
            return;
        }
        else if (m_PasswordInput.text != m_PasswordAgainInput.text)
        {
            ShowHint("两次输入密码不相同！");
            return;
        }

        user.m_Password = m_PasswordInput.text;

        UserManager.SaveUser(user);

        ResetField();

        FindObjectOfType<UIGroundContorll>().ShowLoginGround();
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