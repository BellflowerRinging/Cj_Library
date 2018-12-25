using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Page : MonoBehaviour
{
    public GameObject NoHaveResuleLabel;
    public Button m_FirstPageButton;
    public Button m_LastPageButton;
    public Dropdown m_PageDropdown;
    public Button m_NextPageButton;
    public Button m_EndPageButton;
    public Button m_RefreshButton;
    public Button m_NameSearch;
    public InputField m_SearchName;

    public string[] m_TittleNames;
    public const int PageRowCount = 10;

    public TableObj m_table;
    public ArrayList m_locList { get; protected set; }
    public ArrayList m_AllList { get; protected set; }
    public int m_PageCount { get; protected set; }
    public int m_locPage { get; protected set; }
    
    private void Start()
    {
        /* User 
        m_TittleNames = new string[] { "ID", "用户名", "账号", "密码" };
        
        Action<User> action = delegate (User t)
        {
            User u = (User)(object)t;

            string[] list = new string[]
            {
                u.m_ID,
                u.m_UserName,
                u.m_LoginName,
                u.m_Password
            };

            m_table.AddRow(list);
        };

        Action<User> actionS = delegate (User u)
        {
            string sname = m_SearchName.text;

            if (u.m_UserName == sname) m_AllList.Add(u);
        };

        //ArrayList AllUsers = new ArrayList(UserManager.GetUsers());
        
        Setting(new ArrayList(UserManager.GetUsers()), action, actionS);
        */
    }

    ///S查找 E显示 list=AllList list会new一份新的
    protected void Setting<T>(Func<List<T>> GetRefreshList, Action<T> forE, Action<T> forS)
    {
        List<T> list = GetRefreshList();

        m_AllList = new ArrayList(list);

        RefreshTable(1, forE);

        initButton(GetRefreshList, forE, forS);
    }

    ///S查找 E显示
    protected void initButton<T>(Func<List<T>> GetRefreshList, Action<T> forE, Action<T> forE_Search)
    {
        /*m_RefreshButton.onClick.AddListener(() =>
        {
            m_AllList = new ArrayList(list);
            RefreshTable(m_locPage, forE);
        });*/
        m_RefreshButton.onClick.AddListener(() =>
        {
            m_AllList = new ArrayList(GetRefreshList());
            RefreshTable(m_locPage, forE);
        });

        m_FirstPageButton.onClick.AddListener(() => RefreshTable(1, forE));

        m_EndPageButton.onClick.AddListener(() => RefreshTable(m_PageCount, forE));

        m_EndPageButton.onClick.AddListener(() => RefreshTable(m_PageCount, forE));

        m_LastPageButton.onClick.AddListener(() => RefreshTable(m_locPage > 1 ? m_locPage - 1 : 1, forE));

        m_NextPageButton.onClick.AddListener(() => RefreshTable(m_locPage < m_PageCount ? m_locPage + 1 : m_PageCount, forE));

        m_PageDropdown.onValueChanged.AddListener((e) => { if (m_PageDropdown.value + 1 != m_locPage) RefreshTable(m_PageDropdown.value + 1, forE); });

        m_NameSearch.onClick.AddListener(() => NameSearchButtonOnclick(forE_Search, forE));
    }

    ///S查找 E显示
    public void NameSearchButtonOnclick<T>(Action<T> forS, Action<T> forE)
    {
        m_table.ClreanTable(false);

        List<T> list = new List<T>();

        foreach (var item in m_AllList) list.Add((T)item);

        m_AllList.Clear();

        list.ForEach(forS);

        RefreshTable(1, forE);

        if (m_table.m_TableRows.Count == 0) NoHaveResuleLabel.SetActive(true);
    }

    public void RefreshTable()
    {
        m_RefreshButton.onClick.Invoke();
    }
    /// -1 = 刷新后的最后一页
    /// Action=ForEach
    public void RefreshTable<T>(int page, Action<T> Action)
    {
        NoHaveResuleLabel.SetActive(false);

        m_table.ClreanTable(true);

        m_table.SetTittleRow(m_TittleNames);

        m_PageCount = ListForPage.GetPageCount(m_AllList, PageRowCount);

        if (m_PageCount == 0)
        {
            NoHaveResuleLabel.SetActive(true);
            m_PageDropdown.options.Clear();
            return;
        }

        m_locList = ListForPage.GetPage(m_AllList, page, PageRowCount);

        List<T> list = new List<T>();

        foreach (var item in m_locList)
        {
            list.Add((T)item);
        }

        list.ForEach(Action);

        RefreshPageButton(page);
    }

    protected void RefreshPageButton(int page)
    {
        m_locPage = page;

        m_PageDropdown.options.Clear();

        List<string> options = new List<string>();

        for (int i = 1; i <= m_PageCount; i++)
        {
            options.Add(string.Format("第{0}页", i));
        }

        m_PageDropdown.AddOptions(options);

        m_PageDropdown.value = m_locPage - 1;
    }

    public void TaggleRegistPage(GameObject go)
    {
        go.SetActive(!go.activeSelf);
    }

}