namespace NachaSharp;
public class EntryAddendumRecord
{
    public RecordTypeCode RecordTypeCode = RecordTypeCode.Addendum;
    public string AddendaTypeCode = "05"; // 05 for CCD, CTX, PPD entries
    public string PaymentRelatedInformation { get; set; } // Up to 80 characters
    public int AddendaSequenceNumber { get; set; } // Sequence within the entry
    public string EntryDetailSequenceNumber { get; set; } // Last 7 digits of Entry Detail’s trace number

    public string GenerateRecord()
    {
        return $"{RecordTypeCode.ToStringValue()}" +
               $"{AddendaTypeCode.PadLeft(2, '0')}" +
               $"{PaymentRelatedInformation.PadRight(80)}" +
               $"{AddendaSequenceNumber.ToString().PadLeft(4, '0')}" +
               $"{EntryDetailSequenceNumber.PadLeft(7, '0')}";
    }
}