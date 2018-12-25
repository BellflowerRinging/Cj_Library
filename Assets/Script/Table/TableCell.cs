using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class TableCell : MonoBehaviour
{
    //所在位置 默认为null 仅可赋值一次
    private int? indexRow;
    private int? indexColumn;

    // public int r;
    // public int c;
    public int? IndexRow
    {
        get { return indexRow; }
        set
        {
            if (indexRow == null)
            {
                indexRow = value;
                // r = (int)indexRow;
            }
        }
    }

    public int? IndexColumn
    {
        get { return indexColumn; }
        set
        {
            if (indexColumn == null)
            {
                indexColumn = value;
                // c = (int)value;
            }
        }
    }

    private TableRow row;
    public TableRow Row
    {
        get { return row; }
        set { if (row == null) row = value; }
    }

}