using UnityEngine;
using UnityEngine.UI;

public class TableRow : MonoBehaviour
{
    [HideInInspector]

    //所在位置 默认为null 仅可赋值一次
    private int? index;

    public int? Index
    {
        get { return index; }
        set
        {
            if (index == null)
            {
                index = value;
            }
        }
    }

    private TableCell[] cells;
    public TableCell[] Cells
    {
        get
        {
            return cells;
        }
        set
        {
            if (cells == null)
            {
                cells = value;
            }

        }
    }

    private TableObj table;
    public TableObj TableObj
    {
        get
        {
            return table;
        }
        set
        {
            if (table == null)
            {
                table = value;
            }
        }
    }


}