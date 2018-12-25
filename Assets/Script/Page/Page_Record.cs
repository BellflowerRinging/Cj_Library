using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Page_Record : Page
{
    public InputField BookIDField;
    public InputField ReaderIDField;
    public Dropdown StateDropdowm;
    public Button EnterButton;
    public Button ToggleNameIdButton;
    public Dropdown SeachTypeDrop;
    bool IsShowID = true;

    void Start()
    {
        //m_TittleNames = new string[] { "ID", "用户名", "账号", "密码" };
        Action<TableCell> DelButtonOnckick = (cell) =>
        {
            RecordManager.DelRecord(((TableInputFieldCell)cell.Row.Cells[0]).Value);
            RefreshTable();
        };

        Action<Record> action = delegate (Record t)
        {
            Record u = (Record)(object)t;

            List<TableCell> list = new List<TableCell>();
            list.Add(m_table.CreateInputFieldTableCell(u.m_ID));

            string bookid, readerid, gmid;

            if (IsShowID)
            {
                bookid = u.m_BookId;
                readerid = u.m_ReaderId;
                gmid = u.m_GmId;
            }
            else
            {
                Book book = BookManager.GetBookById(u.m_BookId);
                if (book != null) bookid = book.m_BookName;
                else bookid = "未知的";

                User user = UserManager.GetUserById(u.m_ReaderId);
                if (user != null) readerid = user.m_UserName;
                else readerid = "未知的";

                user = UserManager.GetUserById(u.m_GmId);
                if (user != null) gmid = user.m_UserName;
                else gmid = "未知的";
            }

            list.Add(m_table.CreateInputFieldTableCell(bookid));
            list.Add(m_table.CreateInputFieldTableCell(readerid));
            list.Add(m_table.CreateInputFieldTableCell(gmid));

            list.Add(m_table.CreateInputFieldTableCell(u.m_Type.ToString()));
            list.Add(m_table.CreateInputFieldTableCell(u.m_Date.ToString("MM月dd日")));
            list.Add(m_table.CreateTableButtonCell("删除", DelButtonOnckick));

            TableRow row = m_table.AddRow(list.ToArray());

            row.Cells[0].GetComponent<InputField>().interactable = false;
            row.Cells[1].GetComponent<InputField>().interactable = false;
            row.Cells[2].GetComponent<InputField>().interactable = false;
            row.Cells[3].GetComponent<InputField>().interactable = false;
            row.Cells[4].GetComponent<InputField>().interactable = false;
            row.Cells[5].GetComponent<InputField>().interactable = false;
        };

        Action<Record> actionS = delegate (Record u)
        {
            string sname = m_SearchName.text;
            switch (SeachTypeDrop.value)
            {
                case 0:
                    if (u.m_ID == sname) m_AllList.Add(u);
                    break;
                case 1:
                    if (u.m_BookId == sname) m_AllList.Add(u);
                    break;
                case 2:
                    if (u.m_ReaderId == sname) m_AllList.Add(u);
                    break;
                case 3:
                    if (u.m_GmId == sname) m_AllList.Add(u);
                    break;
            }
        };

        Func<List<Record>> GetRefreshList = () =>
        {
            return RecordManager.GetRecords();
        };

        Setting(GetRefreshList, action, actionS);

        //-
        EnterButton.onClick.AddListener(EnterButtonOnclick);

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

        });
    }

    public void EnterButtonOnclick()
    {
        string rids = ReaderIDField.text;
        string bids = BookIDField.text;

        if (string.IsNullOrEmpty(rids))
        {
            //Debug.Log("ReaderIDField.text == null");
            HintWindowManager.Contorl.Show("读者ID不能为空！");
            return;
        }

        if (string.IsNullOrEmpty(bids))
        {
            //Debug.Log("BookIDField.text == null");
            HintWindowManager.Contorl.Show("书本ID不能为空！");
            return;
        }

        int bid = Int32.Parse(bids);
        int rid = Int32.Parse(rids);

        Book book = BookManager.GetBookById(bid);
        if (book == null)
        {
            //Debug.Log("Book == null");
            HintWindowManager.Contorl.Show("书本不存在或已被删除！");
            return;
        }

        User user = UserManager.GetUserById(rid);
        if (user == null)
        {
            //Debug.Log("User == null");
            HintWindowManager.Contorl.Show("管理员不存在或已被删除！");
            return;
        }


        BookState state;
        RecordType type;

        if (StateDropdowm.value == 0)
        {
            if (book.m_State != BookState.已借)
                state = BookState.已借;
            else
            {
                //Debug.Log("书本已被借走");
                HintWindowManager.Contorl.Show("书本已被借走！");
                return;
            }

            if (user.m_BookCount == 5)
            {
                HintWindowManager.Contorl.Show("该读者已经借出5本书！");
                return;
            }

            if (user.m_Error)
            {
                HintWindowManager.Contorl.Show("该读者尚未进行处罚！");
                return;
            }

            type = RecordType.借书;
            user.m_BookCount = user.m_BookCount + 1;
        }
        else
        {
            if (book.m_State != BookState.已还)
                state = BookState.已还;
            else
            {
                //Debug.Log("书本已被还回");
                HintWindowManager.Contorl.Show("书本已被还回！");
                return;
            }

            type = RecordType.还书;

            List<Record> list = RecordManager.GetRecordsByBookId(bids);   //未测试bug
            Record re = list[list.Count - 1];
            User bu = UserManager.GetUserById(re.m_ReaderId);
            bu.m_BookCount = bu.m_BookCount - 1;
            UserManager.SaveUser(bu);

            /* -- */
            TimeSpan span = DateTime.Now - re.m_Date;
            if (span.Days >= 30)
            {
                ErrorUser euser = new ErrorUser(
                    re.m_ID,
                    re.m_ReaderId,
                    UserManager.GetUserById(re.m_ReaderId).m_UserName,
                    bids,
                    BookManager.GetBookById(bids).m_BookName,
                    span.Days
                );
                ErrorUserManager.SaveErrorUser(euser);
                HintWindowManager.Contorl.Show("该读者逾期" + span.Days + "天归还书本，需要进行处罚！");
            }

            //user.m_BookCount = user.m_BookCount - 1;   //替别人还书情况处理
        }

        book.m_State = state;
        BookManager.SaveBook(book);
        UserManager.SaveUser(user);
        Record record = new Record(rids, bids, ApplicationManager.LocUserId, type, DateTime.Now);
        RecordManager.SaveRecord(record);

        RefreshTable();
    }
}