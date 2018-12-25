using UnityEngine.UI;

using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Globalization;

public class BookUIContorl : MonoBehaviour
{
    public InputField BookNameField;
    public InputField AuthorField;
    public Dropdown TypeDropDown;
    public InputField ISBNField;
    public InputField DateField;
    public InputField KeyValueField;

    public Text Hint;

    public Action CallBack;

    string BookId;

    public void Awake()
    {
        CleanInputField();
        TypeDropDown.options.Clear();
        foreach (var item in Enum.GetNames(typeof(BookType)))
        {
            TypeDropDown.options.Add(new Dropdown.OptionData(item));
        }
        TypeDropDown.value = 1;
        TypeDropDown.value = 0;
    }

    public void EnterAddBook()
    {
        if (string.IsNullOrEmpty(BookNameField.text))
        {
            ShowHint("书名不能为空！");
            return;
        }
        else if (string.IsNullOrEmpty(AuthorField.text))
        {
            ShowHint("作者不能为空！");
            return;
        }
        else if (TypeDropDown.value == -1)
        {
            ShowHint("类别不能为空！");
            return;
        }
        else if (string.IsNullOrEmpty(ISBNField.text))
        {
            ShowHint("ISBN不能为空！");
            return;
        }
        else if (string.IsNullOrEmpty(DateField.text))
        {
            ShowHint("发售日期不能为空！");
            return;
        }
        else if (string.IsNullOrEmpty(KeyValueField.text))
        {
            ShowHint("关键词不能为空！");
            return;
        }
        else if (GetDate() == new DateTime(0))
        {
            ShowHint("日期错误,例:2018-01-01");
            return;
        }

        Book book = new Book(
            BookId,
            BookNameField.text,
            AuthorField.text,
            (BookType)TypeDropDown.value,
            ISBNField.text,
            GetKeyWord(),
            GetDate()
        );

        BookManager.SaveBook(book);

        if (CallBack != null)
            CallBack();

        gameObject.SetActive(false);
    }

    private DateTime GetDate()
    {
        string datastr = DateField.text;
        DateTimeFormatInfo fomat = new DateTimeFormatInfo();
        fomat.ShortDatePattern = "yyyy-MM-dd";
        DateTime time = new DateTime(0);
        try
        {
            time = Convert.ToDateTime(datastr, fomat);
        }
        catch (System.Exception)
        {
            // Debug.Log("Date Format Error!");
        }
        return time;
    }

    public void ShowBook(Book book)
    {
        Open();
        BookId = book.m_ID;
        BookNameField.text = book.m_BookName;
        AuthorField.text = book.m_Author;
        TypeDropDown.value = (int)book.m_Type;
        ISBNField.text = book.m_ISBN;
        DateField.text = book.m_Date.ToString("yyyy-MM-dd");
        string kw = "";
        foreach (var item in book.m_keyword)
        {
            kw += item + ";";
        }
        KeyValueField.text = kw;
    }

    public void CleanInputField()
    {
        BookId = "-1";
        BookNameField.text = "";
        AuthorField.text = "";
        TypeDropDown.value = 0;
        ISBNField.text = "";
        DateField.text = "2018-01-01";
        KeyValueField.text = "";
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Open()
    {
        CleanInputField();

        gameObject.SetActive(true);
    }

    String[] GetKeyWord()
    {

        List<string> kw = new List<string>();
        string[] kws = null;
        string str = KeyValueField.text;
        int i;
        if ((i = str.IndexOf(";")) != -1)
        {
            string temp = str;
            do
            {
                if (i == 0) break;
                kw.Add(temp.Substring(0, i));
                temp = temp.Substring(i + 1, temp.Length - i - 1);
            } while ((i = temp.IndexOf(";")) != -1);
            if (!string.IsNullOrEmpty(temp) && temp.IndexOf(";") == -1)
                kw.Add(temp);
            kws = kw.ToArray();
        }
        else
        {
            kws = new string[] { str };
        }
        return kws;
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