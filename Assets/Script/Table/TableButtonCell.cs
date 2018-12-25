using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class TableButtonCell : TableCell
{
    public Text text;
    public string Value
    {
        get { return text.text; }
        set { text.text = value; }
    }

    void Awake()
    {
        //InputField = GetComponent<InputField>();
        text = GetComponentInChildren<Text>();
    }

    void Start()
    {
        /*InputField.onEndEdit.AddListener((tvalue) =>
        {
            User user = UserManager.GetUserByLoginName(((TableInputFieldCell)Row.Cells[2]).Value);
            if (tvalue != user.m_UserName)
            {
                user.m_UserName = tvalue;
                UserManager.SaveAll();
            }
        });*/
    }

}