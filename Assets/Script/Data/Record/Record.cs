using System;

public class Record
{
    public string m_ID;
    public string m_ReaderId;
    public string m_BookId;
    public string m_GmId;
    public RecordType m_Type;
    public DateTime m_Date;

    public Record(string p_ReaderId, string p_BookId, string p_GmId, RecordType p_Type, DateTime p_Date,string p_Id="-1")
    {
        m_ID = p_Id;
        m_ReaderId = p_ReaderId;
        m_BookId = p_BookId;
        m_GmId = p_GmId;
        m_Type = p_Type;
        m_Date = p_Date;
    }
}

public enum RecordType
{
    借书,
    还书,
}