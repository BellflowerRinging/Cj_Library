using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Page_ErrorUser : Page
{
    public Button ToggleNameIdButton;
    public Dropdown SeachTypeDrop;

    void Start()
    {
        //m_TittleNames = new string[] { "ID", "用户名", "账号", "密码" };
        Func<List<ErrorUser>> GetRefreshList = GetErrorUser;

        Action<ErrorUser> action = delegate (ErrorUser t)
        {
            ErrorUser u = (ErrorUser)(object)t;

            List<TableCell> list = new List<TableCell>();
            list.Add(m_table.CreateInputFieldTableCell(u.RecordId));
            list.Add(m_table.CreateInputFieldTableCell(u.ReaderId));
            list.Add(m_table.CreateInputFieldTableCell(u.ReaderName));
            list.Add(m_table.CreateInputFieldTableCell(u.BookId));
            list.Add(m_table.CreateInputFieldTableCell(u.BookName));
            list.Add(m_table.CreateInputFieldTableCell(u.date + ""));
            list.Add(m_table.CreateTableButtonCell("已处罚", (cell) =>
            {
                ErrorUserManager.DelErrorUser(((TableInputFieldCell)cell.Row.Cells[0]).Value);

                if (RecordManager.GetRecordsByBookId(((TableInputFieldCell)cell.Row.Cells[3]).Value).Count % 2 != 0)
                {
                    HintWindowManager.Contorl.Show("请先让该读者归还书籍！");
                }

                User user = UserManager.GetUserById(((TableInputFieldCell)cell.Row.Cells[1]).Value);
                if (user == null)
                {
                    Debug.Log("user==null");
                    return;
                }

                if (!ListContainsReader(ErrorUserManager.GetErrorUsers(), user.m_ID))
                {
                    user.m_Error = false;
                }

                RefreshTable();
            }));

            TableRow row = m_table.AddRow(list.ToArray());

            row.Cells[0].GetComponent<InputField>().interactable = false;
            row.Cells[1].GetComponent<InputField>().interactable = false;
            row.Cells[2].GetComponent<InputField>().interactable = false;
            row.Cells[3].GetComponent<InputField>().interactable = false;
            row.Cells[4].GetComponent<InputField>().interactable = false;
            row.Cells[5].GetComponent<InputField>().interactable = false;
        };

        Action<ErrorUser> actionS = delegate (ErrorUser u)
        {
            string sname = m_SearchName.text;
            switch (SeachTypeDrop.value)
            {
                case 0:
                    if (u.RecordId == sname) m_AllList.Add(u);
                    break;
                case 1:
                    if (u.ReaderId == sname) m_AllList.Add(u);
                    break;
                case 2:
                    if (u.BookId == sname) m_AllList.Add(u);
                    break;
            }
        };

        Setting(GetRefreshList, action, actionS);

        //-
        /*
        ToggleNameIdButton.onClick.AddListener(() =>
        {
            if (IsShowID)
            {
                ToggleNameIdButton.GetComponentInChildren<Text>().text = "显示名称";
                IsShowID = false;
            }
            else
            {
                ToggleNameIdButton.GetComponentInChildren<Text>().text = "显示ID";
                IsShowID = true;
            }
            RefreshTable();

        }); */
    }

    private List<ErrorUser> GetErrorUser()
    {
        List<ErrorUser> list = ErrorUserManager.GetErrorUsers();

        List<Book> bList = new List<Book>();
        foreach (var item in BookManager.GetBooks())
        {
            if (item.m_State == BookState.已借)
            {
                bList.Add(item);
            }
        }

        DateTime now = DateTime.Now;
        foreach (var item in bList)
        {
            List<Record> relist = RecordManager.GetRecordsByBookId(item.m_ID);
            if (relist != null)
            {
                Record nr = relist[relist.Count - 1];
                TimeSpan span = now - nr.m_Date;
                if (span.Days >= 30)
                {
                    ErrorUser user = new ErrorUser(
                        nr.m_ID,
                        nr.m_ReaderId,
                        UserManager.GetUserById(nr.m_ReaderId).m_UserName,
                        item.m_ID,
                        item.m_BookName,
                        span.Days);
                    if (!ListContainsRecorId(list, user))  //有空的时候重写compareto代替
                    {
                        list.Add(user);
                        ErrorUserManager.SaveErrorUser(user);
                    }
                }
            }
        }

        return list;
    }

    public bool ListContainsRecorId(List<ErrorUser> list, ErrorUser eu)
    {
        foreach (var item in list)
        {
            if (item.RecordId == eu.RecordId) return true;
        }
        return false;
    }

    public bool ListContainsReader(List<ErrorUser> list, string id)
    {
        foreach (var item in list)
        {
            if (item.ReaderId == id) return true;
        }
        return false;
    }
}

