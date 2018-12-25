using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginUser : MonoBehaviour
{
    public InputField m_LoginNameInput;
    public InputField m_PasswordInput;
    public Text Hint;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Login()
    {
        if (string.IsNullOrEmpty(m_LoginNameInput.text))
        {
            ShowHint("账号不能为空！");
            return;
        }
        else if (string.IsNullOrEmpty(m_PasswordInput.text))
        {
            ShowHint("密码不能为空！");
            return;
        }

        User user = UserManager.GetUserByLoginName(m_LoginNameInput.text);

        if (user == null)
        {
            ShowHint("账号不存在！");
            return;
        }

        if (user.m_Password != m_PasswordInput.text)
        {
            ShowHint("账号或密码错误！");
            return;
        }

        m_LoginNameInput.text = "";
        m_PasswordInput.text = "";

        ShowHint("登陆成功！");
        ApplicationManager.LocUserId = user.m_ID;
        SceneManager.LoadScene("MainWindow");
        SceneManager.UnloadSceneAsync("LoginWindow");
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
