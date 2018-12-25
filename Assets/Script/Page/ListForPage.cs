
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListForPage
{
    static public int GetPageCount(ArrayList list, int RowCount)
    {
        return (int)Math.Ceiling((double)list.Count / (double)RowCount);
    }

    static public int GetPageCount<T>(List<T> list, int RowCount)
    {
        return (int)Math.Ceiling((double)list.Count / (double)RowCount);
    }

    static public ArrayList GetPage(ArrayList list, int page, int RowCount)
    {
        page = Mathf.Clamp(page, 1, 999);
        //if (page < 0) { throw new Exception("Error:page<0"); }
        if (RowCount < 0) { throw new Exception("Error:RowCount<0"); }

        int pageCount = GetPageCount(list, RowCount);

        if (page > pageCount) { throw new Exception("Error:page>pageCount"); }

        int index = (page - 1) * RowCount;
        int count = RowCount;

        if (index + RowCount > list.Count) count = list.Count - index;

        ArrayList rlist = list.GetRange(index, count);

        return rlist;
    }

    static public List<T> GetPage<T>(List<T> list, int page, int RowCount)
    {
        if (page < 0) { throw new Exception("Error:page<0"); }
        if (RowCount < 0) { throw new Exception("Error:RowCount<0"); }

        int pageCount = GetPageCount(list, RowCount);

        if (page > pageCount) { throw new Exception("Error:page>pageCount"); }

        int index = (page - 1) * RowCount;
        int count = RowCount;

        if (index + RowCount > list.Count) count = list.Count - index;

        List<T> rlist = list.GetRange(index, count);

        return rlist;
    }

}