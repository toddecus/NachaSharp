using System.Diagnostics.CodeAnalysis;

namespace NachaSharp;
public class EntryAddendumRecord
{
    public RecordTypeCode RecordTypeCode = RecordTypeCode.Addendum; // Fixed value for Addendum
    public AddendumTypeCode AddendumTypeCode = AddendumTypeCode.AdditionalPaymentInformation; // 05 for CCD, CTX, PPD entries
    public required string PaymentRelatedInformation { get; set; } // Up to 80 characters
    public readonly int AddendaSequenceNumber = 1; // Sequence within the entry Not supporting multiple addendum lines
    public required string EntryDetailSequenceNumber;  // Last 7 digits of Entry Detail’s trace number

    [SetsRequiredMembers]
    public EntryAddendumRecord(string paymentRelatedInformation, string entryDetailSequenceNumber)
    {
        PaymentRelatedInformation = paymentRelatedInformation;
        EntryDetailSequenceNumber = entryDetailSequenceNumber;
    }
    public string GenerateRecord()
    {
        return string.Concat(
            RecordTypeCode.ToStringValue() +
            AddendumTypeCode.ToStringValue() +
            PaymentRelatedInformation.PadRight(80) +
            AddendaSequenceNumber.ToString().PadLeft(4, '0') +
            EntryDetailSequenceNumber.PadLeft(7, '0')
        );
    }
}