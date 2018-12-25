using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;

public class BookManager : MonoBehaviour
{
    static List<Book> Books;
    static string BookJsonFileName;

    private void Start()
    {
        init();

        //Book r = new Book("100", "200", "300", "400", BookType.借书, new System.DateTime());
        //FSaveBook(r);
    }

    static void init()
    {
        BookJsonFileName = Application.dataPath + "/Resources/BookData.json";

        Books = new List<Book>();

        LoadBooks();
    }

    static void LoadBooks()
    {
        object o = Resources.Load("BookData");

        if (o == null)
        {
            SaveAll();
            return;
        }

        string str = o.ToString();

        Books = JsonConvert.DeserializeObject<List<Book>>(str);
    }

    static public bool IdIsHave(int id)
    {
        foreach (var Book in Books)
        {
            if (string.IsNullOrEmpty(Book.m_ID)) continue;

            if (int.Parse(Book.m_ID) == id)
            {
                return true;
            }
        }
        return false;
    }

    static public void SaveBook(Book Book)
    {
        Book tBook = GetBookById(Book.m_ID);

        if (tBook != null)
        {
            tBook.m_BookName = Book.m_BookName;
            tBook.m_Author=Book.m_Author;
            tBook.m_Date=Book.m_Date;
            tBook.m_ISBN=Book.m_ISBN;
            tBook.m_keyword=Book.m_keyword;
            tBook.m_Type=Book.m_Type;
        }
        else
        {
            int id = int.Parse(Book.m_ID);
            if (id == -1) do id = UnityEngine.Random.Range(10000, 100000); while (BookManager.IdIsHave(id));
            Book.m_ID = id + "";
            Books.Add(Book);
        }

        SaveAll();
    }

    static public void DelBook(string id)
    {
        DelBook(int.Parse(id));
    }

    static public void DelBook(int id)
    {
        Book tBook = GetBookById(id);

        if (tBook != null)
        {
            Books.Remove(tBook);
        }

        SaveAll();
    }

    static public void SaveAll()
    {
        string str = JsonConvert.SerializeObject(Books);

        ResourcesManager.WriteToJsonFile(BookJsonFileName, str);
    }

    static public Book GetBookById(string id)
    {
        return GetBookById(int.Parse(id));
    }

    static public Book GetBookById(int id)
    {
        foreach (var Book in Books)
        {
            if (Book.m_ID == id + "")
            {
                return Book;
            }
        }
        return null;
    }

    static public List<Book> GetBooks()
    {
        if (Books == null) init();

        return Books;
    }


    static public List<Book> GetBooksByType(BookType type)
    {
        List<Book> result = new List<Book>();
        foreach (var Book in Books)
        {
            if (Book.m_Type == type)
            {
                result.Add(Book);
            }
        }
        return result;
    }
}