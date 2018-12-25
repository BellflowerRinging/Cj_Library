public class ErrorUser
{
    public string RecordId;
    public string ReaderId;
    public string ReaderName;
    public string BookId;
    public string BookName;
    public int date;

    public ErrorUser(string rid, string uid, string un, string bid, string bn, int da)
    {
        RecordId = rid;
        ReaderId = uid;
        ReaderName = un;
        BookId = bid;
        BookName = bn;
        date = da;
        
    }
}