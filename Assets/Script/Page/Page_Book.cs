using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Page_Book : Page
{

    public BookUIContorl UiContorl;

    public Dropdown SeachTypeDrop;


    void Start()
    {
        //m_TittleNames = new string[] { "ID", "用户名", "账号", "密码" };
        Action<TableCell> DelButtonOnckick = (cell) =>
        {
            BookManager.DelBook(((TableInputFieldCell)cell.Row.Cells[0]).Value);
            RefreshTable();
        };

        Action<TableCell> InfoOnclick = (cell) =>
        {
            UiContorl.ShowBook(BookManager.GetBookById(((TableInputFieldCell)cell.Row.Cells[0]).Value));
        };

        Action<Book> action = delegate (Book t)
        {
            Book u = (Book)(object)t;

            List<TableCell> list = new List<TableCell>();
            list.Add(m_table.CreateInputFieldTableCell(u.m_ID));
            list.Add(m_table.CreateInputFieldTableCell(u.m_BookName));
            list.Add(m_table.CreateInputFieldTableCell(u.m_Type.ToString()));
            list.Add(m_table.CreateInputFieldTableCell(u.m_State.ToString()));
            list.Add(m_table.CreateTableButtonCell("详情", InfoOnclick));
            list.Add(m_table.CreateTableButtonCell("删除", DelButtonOnckick));

            TableRow row = m_table.AddRow(list.ToArray());

            row.Cells[0].GetComponent<InputField>().interactable = false;
            row.Cells[1].GetComponent<InputField>().interactable = false;
            row.Cells[2].GetComponent<InputField>().interactable = false;
            row.Cells[3].GetComponent<InputField>().interactable = false;
            //  row.Cells[4].GetComponent<InputField>().interactable = false;
            //            row.Cells[5].GetComponent<InputField>().interactable = false;
        };

        Action<Book> actionS = delegate (Book u)
        {
            string sname = m_SearchName.text;

            switch (SeachTypeDrop.value)
            {
                case 0:
                    if (u.m_ID == sname) m_AllList.Add(u);
                    break;
                case 1:
                    if (u.m_BookName.Contains(sname)) m_AllList.Add(u);
                    break;
                case 2:
                    if (u.m_Type.ToString() == sname) m_AllList.Add(u);
                    break;
                case 3:
                    if (u.m_Author == sname) m_AllList.Add(u);
                    break;
                case 4:
                    int year;
                    try
                    {
                        year = int.Parse(sname);
                        if (u.m_Date.Year == year) m_AllList.Add(u);
                    }
                    catch (System.Exception)
                    {
                        HintWindowManager.Contorl.Show("请输入正确年份");
                        RefreshTable();
                        return;
                    }
                    break;
                case 5:
                    foreach (var item in u.m_keyword)
                    {
                        if (item == sname) m_AllList.Add(u);
                    }
                    break;
            }
        };

        Func<List<Book>> GetRefreshList = () =>
        {
            return BookManager.GetBooks();
        };

        Setting(GetRefreshList, action, actionS);

        UiContorl.CallBack = () =>
        {
            RefreshTable();
        };
    }
}