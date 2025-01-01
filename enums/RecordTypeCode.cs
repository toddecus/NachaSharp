namespace NachaSharp;
public enum RecordTypeCode
{
    /* Each line of the file should begin with the RecordTypeCode  Header and Control like HTML open/close tags*/
    FileHeader = 1,
    BatchHeader = 5,
    EntryDetail = 6,
    Addendum = 7,
    BatchControl = 8,
    FileControl = 9
}

public static class RecordTypeCodeExtensions
{
    public static string ToStringValue(this RecordTypeCode recordTypeCode)
    {
        return recordTypeCode switch
        {
            RecordTypeCode.FileHeader => "1",
            RecordTypeCode.BatchHeader => "5",
            RecordTypeCode.EntryDetail => "6",
            RecordTypeCode.Addendum => "7",
            RecordTypeCode.BatchControl => "8",
            RecordTypeCode.FileControl => "9",
            _ => throw new ArgumentOutOfRangeException(nameof(recordTypeCode), recordTypeCode, "Somehow tyring to ToStringValue an unknown enum value on RecordTypeCode.")
        };
    }
}