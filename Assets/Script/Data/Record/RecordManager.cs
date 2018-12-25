using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;

public class RecordManager : MonoBehaviour
{
    static List<Record> Records;
    static string RecordJsonFileName;

    private void Start()
    {
        init();
    }

    static void init()
    {
        RecordJsonFileName = Application.dataPath + "/Resources/RecordData.json";

        Records = new List<Record>();

        LoadRecords();
    }

    static void LoadRecords()
    {
        object o = Resources.Load("RecordData");

        if (o == null)
        {
            SaveAll();
            return;
        }

        string str = o.ToString();

        Records = JsonConvert.DeserializeObject<List<Record>>(str);
    }

    static public bool IdIsHave(int id)
    {
        foreach (var Record in Records)
        {
            if (string.IsNullOrEmpty(Record.m_ID)) continue;

            if (int.Parse(Record.m_ID) == id)
            {
                return true;
            }
        }
        return false;
    }

    static public void SaveRecord(Record Record)
    {
        Record tRecord = GetRecordById(Record.m_ID);

        if (tRecord != null)
        {
            //tRecord.m_RecordName = Record.m_RecordName;
        }
        else
        {
            int id = int.Parse(Record.m_ID);
            if (id == -1) do id = UnityEngine.Random.Range(10000, 100000); while (RecordManager.IdIsHave(id));
            Record.m_ID = id + "";
            Records.Add(Record);
        }

        SaveAll();
    }

    static public void DelRecord(string id)
    {
        DelRecord(int.Parse(id));
    }
    static public void DelRecord(int id)
    {
        Record tRecord = GetRecordById(id);

        if (tRecord != null)
        {
            Records.Remove(tRecord);
        }

        SaveAll();
    }

    static public void SaveAll()
    {
        string str = JsonConvert.SerializeObject(Records);

        ResourcesManager.WriteToJsonFile(RecordJsonFileName, str);
    }

    static public Record GetRecordById(string id)
    {
        return GetRecordById(int.Parse(id));
    }

    static public Record GetRecordById(int id)
    {
        foreach (var Record in Records)
        {
            if (Record.m_ID == id + "")
            {
                return Record;
            }
        }
        return null;
    }

    static public List<Record> GetRecords()
    {
        if (Records == null) init();

        return Records;
    }


    static public List<Record> GetRecordsByReaderId(string id)
    {
        List<Record> result = new List<Record>();
        foreach (var Record in Records)
        {
            if (Record.m_ReaderId == id + "")
            {
                result.Add(Record);
            }
        }
        return result;
    }

    static public List<Record> GetRecordsByGmId(string id)
    {
        List<Record> result = new List<Record>();
        foreach (var Record in Records)
        {
            if (Record.m_GmId == id + "")
            {
                result.Add(Record);
            }
        }
        return result;
    }

    static public List<Record> GetRecordsByBookId(string id)
    {
        List<Record> result = new List<Record>();
        foreach (var Record in GetRecords())
        {
            if (Record.m_BookId == id + "")
            {
                result.Add(Record);
            }
        }
        return result;
    }
}