using System;

public class Book
{
    public string m_ID;
    public string m_BookName;
    public string m_Author;
    public string m_ISBN;
    public string[] m_keyword;
    public DateTime m_Date;
    public BookType m_Type;
    public BookState m_State;

    public Book(string ID, string BookName, string Author, BookType BookType, string ISBN, string[] keyword, DateTime Date)
    {
        m_ID = ID;
        m_BookName = BookName;
        m_Author = Author;
        m_ISBN = ISBN;
        m_keyword = keyword;
        m_Date = Date;
        m_Type = BookType;
        m_State = BookState.已还;
    }

    public void ToggleState()
    {
        if (m_State == BookState.已还)
        {
            m_State = BookState.已借;
        }
        else
        {
            m_State = BookState.已还;
        }
    }

}

public enum BookState
{
    已借,
    已还
}

public enum BookType
{
    人文科普,
    小说,
    漫画,
    历史
}