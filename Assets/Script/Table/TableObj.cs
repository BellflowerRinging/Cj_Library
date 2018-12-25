using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TableObj : MonoBehaviour
{
    public float Width = 590;
    //public float Height;
    [HideInInspector] public int m_ColumnCount;
    public int m_MaxRowColunt;

    private string[] m_TittleName;

    private GameObject m_TableTittle;
    private GameObject m_TableList;

    [HideInInspector] public TittleCell[] m_TittleCells;
    [HideInInspector] public List<TableRow> m_TableRows;

    private GameObject m_DefaultTittleTextCell;
    private GameObject m_DefalutTableCell;
    private GameObject m_DefalutTableButtonCell;
    private GameObject m_DefalutTableRow;

    public Vector2 m_CellSize { get; private set; }
    // Use this for initialization
    void Start()
    {
        //InitTableTittle();
        //InitTableList();
    }

    void Awake()
    {
        m_CellSize = Vector2.zero;
    }

    void Init()
    {
        m_DefaultTittleTextCell = Resources.Load<GameObject>("Defalut/TittleCell");
        m_DefalutTableRow = Resources.Load<GameObject>("Defalut/TableRow");
        m_DefalutTableCell = Resources.Load<GameObject>("Defalut/TableInputFieldCell");
        m_DefalutTableButtonCell = Resources.Load<GameObject>("Defalut/TableButtonCell");

        m_TableTittle = transform.Find("TableTittle").gameObject;
        m_TableList = transform.Find("TableList").gameObject;
    }


    void InitTableTittle()
    {
        if (m_TableTittle == null || m_TableList == null) Init();

        if (m_TittleName == null)
        {
            return;
        }

        m_ColumnCount = m_TittleName.Length;
        m_TittleCells = new TittleCell[m_ColumnCount];
        m_CellSize = new Vector2((Width + 1.6f * m_ColumnCount) / m_ColumnCount, 40.4f);

        GridLayoutGroup layout = m_TableTittle.GetComponent<GridLayoutGroup>();
        layout.constraintCount = m_ColumnCount;
        layout.cellSize = m_CellSize;

        for (int i = 0; i < m_TittleName.Length; i++)
        {
            GameObject go = GameObject.Instantiate<GameObject>(m_DefaultTittleTextCell);
            go.transform.SetParent(m_TableTittle.transform);
            go.SetActive(true);
            TittleCell cell = go.GetComponent<TittleCell>();
            cell.Value = m_TittleName[i];
            m_TittleCells[i] = cell;
        }
    }

    void InitTableList()
    {
        if (m_TableTittle == null || m_TableList == null || m_CellSize == Vector2.zero) InitTableTittle();

        m_TableRows = new List<TableRow>();

        GridLayoutGroup layout = m_TableList.GetComponent<GridLayoutGroup>();
        layout.constraintCount = m_MaxRowColunt;
        layout.cellSize = new Vector2(Width, 40.4f);

        layout = m_DefalutTableRow.GetComponent<GridLayoutGroup>();
        layout.constraintCount = m_ColumnCount;
        layout.cellSize = m_CellSize;
    }

    public void SetTittleRow(string[] names)
    {
        m_TittleName = names;
        InitTableTittle();
        InitTableList();
    }


    ///<returns> 添加成功返回新行 </returns>
    public TableRow AddRow(TableCell[] cells)
    {
        if (m_TableRows.Count == m_MaxRowColunt)
        {
            foreach (var item in cells)
            {
                Destroy(item.gameObject);
            }
            return null;
        }

        GameObject grow = GameObject.Instantiate<GameObject>(m_DefalutTableRow);
        grow.layer = 5;
        grow.transform.SetParent(m_TableList.transform);

        TableRow row = grow.GetComponent<TableRow>();
        row.Index = m_TableRows.Count;

        foreach (var item in cells)
        {
            item.gameObject.transform.SetParent(grow.transform);
            item.IndexRow = row.Index;
        }

        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].gameObject.transform.SetParent(grow.transform);
            cells[i].IndexRow = row.Index;
            cells[i].IndexColumn = i;
            cells[i].Row = row;
        }

        row.Cells = cells;

        row.TableObj = this;

        m_TableRows.Add(row);

        return row;
    }

    public TableRow AddRow(string[] cellStrings, Action<TableCell> onEndEditAction = null)
    {
        TableCell[] cells = new TableCell[cellStrings.Length];
        for (int i = 0; i < cellStrings.Length; i++) cells[i] = CreateInputFieldTableCell(cellStrings[i], onEndEditAction);
        return AddRow(cells);
    }

    public TableCell CreateInputFieldTableCell(string value, Action<TableCell> onEndEditAction = null)
    {
        GameObject gcell = GameObject.Instantiate(m_DefalutTableCell);
        TableInputFieldCell cell = gcell.GetComponent<TableInputFieldCell>();
        cell.SetOnEndEditAction(onEndEditAction);
        cell.Value = value;
        return cell;
    }

    public TableCell CreateTableButtonCell(string value, Action<TableCell> action = null)
    {
        GameObject gcell = GameObject.Instantiate(m_DefalutTableButtonCell);
        TableButtonCell cell = gcell.GetComponent<TableButtonCell>();
        cell.Value = value;
        if (action != null)
            gcell.GetComponent<Button>().onClick.AddListener(() => action(cell));
        return cell;
    }

    ///resetTittle 是否重置表头
    public void ClreanTable(bool resetTittle)
    {
        if (resetTittle)
        {
            foreach (var item in m_TittleCells)
            {
                GameObject.Destroy(item.gameObject);
            }

            m_TittleCells = null;

            m_TittleName = null;

            InitTableTittle();
        }

        foreach (var item in m_TableRows)
        {
            GameObject.Destroy(item.gameObject);
        }

        m_TableRows = null;

        InitTableList();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
