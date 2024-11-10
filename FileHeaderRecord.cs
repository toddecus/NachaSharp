using System.Diagnostics.CodeAnalysis;
using System.Runtime.ConstrainedExecution;

namespace NachaSharp; 
public class FileHeaderRecord
{
    public RecordTypeCode RecordTypeCode = RecordTypeCode.FileHeader;  // Fixed value for file header
    public string PriorityCode = "01";   // Default priority 01-99 01 is the highest
    public required string ImmediateDestination;  // Destination routing number a " " + 9 digit number bank routing number
    public required string ImmediateOrigin;       // Origin routing number 10 digit number usually IRS federal tax ID or Bank assigned number.
    public DateTime FileCreationDate = DateTime.Now; //yyMMdd
    public TimeSpan FileCreationTime = DateTime.Now.TimeOfDay; //hhmm
    /* File version The File ID Modifier in the NACHA FileHeaderRecord is a single alphanumeric 
    character (A–Z or 0–9) used to uniquely identify multiple files created on the same date by the same
     originator. It acts as a version indicator, ensuring that if multiple files are generated on the 
     same day, each can be differentiated by incrementing this character (e.g., A, B, C, etc.). 
     Typically, "A" is used for the first file of the day, with subsequent files using the next letter 
     or number. */
    public required string FileIdModifier = "A";  //Single character A-Z or 0-9 if more than one file is created on the same day
    /* According to NACHA specifications, each record must be 94 characters long, so recordSize is set 
    to "094". This ensures that each line in the file is padded correctly to meet the fixed-length 
    format requirements. */
    public readonly string RecordSize = "094";    
    /* Fixed blocking factor each block will have 10 records. This factor helps control data 
    organization for efficient processing, ensuring records are grouped correctly in blocks of 940 
    characters (10 records × 94 characters each). */
    public readonly string BlockingFactor = "10"; 
    public readonly string FormatCode = "1";      // Fixed format code
    public required string ImmediateDestinationName; // Destination Business Name 23 characters name of the bank
    public required string ImmediateOriginName;   // Origin Business Name 23 characters Name of your Company
    public required string ReferenceCode;  // Customer reference code for their own tracking Guid? 8 characters

    [SetsRequiredMembers]    
    public FileHeaderRecord(string immediateDestinationName, string immediateDestination, string immediateOriginName, string immediateOrigin, string referenceCode)
    {
        ImmediateDestinationName = immediateDestinationName;
        ImmediateDestination = immediateDestination;
        ImmediateOriginName = immediateOriginName;
        ImmediateOrigin = immediateOrigin;
        ReferenceCode = referenceCode;
    }

    public string GenerateRecord()
    {
        return $"{RecordTypeCode.ToStringValue()}{PriorityCode}{ImmediateDestination.PadLeft(10, ' ')}{ImmediateOrigin.PadLeft(10, ' ')}{FileCreationDate:yyMMdd}{FileCreationTime:hhmm}{FileIdModifier}{RecordSize}{BlockingFactor}{FormatCode}{ImmediateDestinationName.PadRight(23)}{ImmediateOriginName.PadRight(23)}{ReferenceCode.PadRight(8)}";
    }
}
