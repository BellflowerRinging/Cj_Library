using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Page_User : Page
{
    public string ListType;
    void Start()
    {
        //m_TittleNames = new string[] { "ID", "用户名", "账号", "密码" };
        Action<TableCell> DelButtonOnckick = (cell) =>
        {
            UserManager.DelUser(((TableInputFieldCell)cell.Row.Cells[2]).Value);
            RefreshTable();
        };

        Action<TableCell> onEndEditAction = (cell) =>
        {
            User user = UserManager.GetUserByLoginName(((TableInputFieldCell)cell.Row.Cells[2]).Value);
            string tvalue = ((TableInputFieldCell)cell.Row.Cells[1]).Value;
            string type = ((TableInputFieldCell)cell.Row.Cells[4]).Value;

            if (tvalue != user.m_UserName || type != user.m_Type)
            {
                user.m_UserName = tvalue;
                user.m_Type = type;
                UserManager.SaveAll();
            }

        };

        Action<User> action = delegate (User t)
        {
            User u = (User)(object)t;

            List<TableCell> list = new List<TableCell>();
            list.Add(m_table.CreateInputFieldTableCell(u.m_ID));
            list.Add(m_table.CreateInputFieldTableCell(u.m_UserName, onEndEditAction));
            list.Add(m_table.CreateInputFieldTableCell(u.m_LoginName));
            list.Add(m_table.CreateInputFieldTableCell(u.m_Password));
            list.Add(m_table.CreateInputFieldTableCell(u.m_Type, onEndEditAction));
            list.Add(m_table.CreateTableButtonCell("删除", DelButtonOnckick));

            TableRow row = m_table.AddRow(list.ToArray());

            row.Cells[0].GetComponent<InputField>().interactable = false;
            row.Cells[2].GetComponent<InputField>().interactable = false;
            row.Cells[3].GetComponent<InputField>().interactable = false;
        };

        Action<User> actionS = delegate (User u)
        {
            string sname = m_SearchName.text;
            if (u.m_UserName == sname) m_AllList.Add(u);
        };

        Func<List<User>> GetRefreshList = () =>
        {
            return UserManager.GetUsers(ListType);
        };

        Setting(GetRefreshList, action, actionS);
        //
        fore = action;
    }

    Action<User> fore;
    public void RegistCallBack(RegistUser registG)
    {
        registG.gameObject.SetActive(false);
        if (!gameObject.activeSelf) return;
        m_AllList = new ArrayList(UserManager.GetUsers(ListType));
        RefreshTable(ListForPage.GetPageCount(UserManager.GetUsers(ListType), PageRowCount), fore);
    }
}