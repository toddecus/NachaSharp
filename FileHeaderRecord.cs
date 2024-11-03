namespace NachaFile; 
public class FileHeaderRecord
{
    public string RecordTypeCode = "1";  // Fixed value for file header
    public string PriorityCode = "01";   // Default priority
    public string ImmediateDestination;  // Destination routing number
    public string ImmediateOrigin;       // Origin routing number
    public DateTime FileCreationDate;
    public TimeSpan FileCreationTime;
    public string FileIdModifier = "A";  // File version
    public string RecordSize = "094";    // Fixed size
    public string BlockingFactor = "10"; // Fixed blocking factor
    public string FormatCode = "1";      // Fixed format code
    public string ImmediateDestinationName;
    public string ImmediateOriginName;

    public string GenerateRecord()
    {
        return $"{RecordTypeCode}{PriorityCode}{ImmediateDestination.PadLeft(10, ' ')}{ImmediateOrigin.PadLeft(10, ' ')}{FileCreationDate:yyMMdd}{FileCreationTime:hhmm}{FileIdModifier}{RecordSize}{BlockingFactor}{FormatCode}{ImmediateDestinationName.PadRight(23)}{ImmediateOriginName.PadRight(23)}";
    }
}
