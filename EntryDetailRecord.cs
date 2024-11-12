
using System.Diagnostics.CodeAnalysis;
namespace NachaSharp;
public class EntryDetailRecord
{
    public RecordTypeCode RecordTypeCode = RecordTypeCode.EntryDetail; // Fixed value for Entry Detail
    public required TransactionCode TransactionCode; // 22 for credit, 27 for debit
    public required string ReceivingDFIRoutingNumber; // First 8 digits of the routing number
    public required string CheckDigit; // Last digit of the routing number
    public required string ReceivingDFIAccountNumber; // Account number
    public required decimal Amount; // In cents
    public required string IndividualIdentificationNumber; // Can be a unique ID for the sender
    public required string IndividualName; // Name of the receiver
    public string DiscretionaryData =""; // Optional 2 characters
    public int AddendumRecordIndicator = 0; // (default)0 for no addenda, 1 for addenda mostly applicable for CCD and CTX
    public required string TraceNumber; // Unique identifier

    private EntryAddendumRecord? _entryAddendumRecord;

    public EntryAddendumRecord? EntryAddendumRecord
    {
        get => _entryAddendumRecord;
        set
        {
            _entryAddendumRecord = value;
            AddendumRecordIndicator = value != null ? 1 : 0;
        }
    }

    [SetsRequiredMembers]
    public EntryDetailRecord(TransactionCode transactionCode, string receivingDFIRoutingNumber, string checkDigit, 
                            string receivingDFIAccountNumber, decimal amount, string individualIdentificationNumber,
                             string individualName, string traceNumber)
    {
        TransactionCode = transactionCode;
        ReceivingDFIRoutingNumber = receivingDFIRoutingNumber;
        CheckDigit = checkDigit;
        ReceivingDFIAccountNumber = receivingDFIAccountNumber;
        Amount = amount;
        IndividualIdentificationNumber = individualIdentificationNumber;
        IndividualName = individualName;
        TraceNumber = traceNumber;
    }

    public string GenerateRecord()
    {
        return $"{RecordTypeCode.ToStringValue()}" +
               $"{TransactionCode.ToStringValue()}" +
               $"{ReceivingDFIRoutingNumber.PadLeft(8, '0')}" +
               $"{CheckDigit}" +
               $"{ReceivingDFIAccountNumber.PadRight(17)}" +
               $"{((int)(Amount) * 100).ToString().PadLeft(10, '0')}" +
               $"{IndividualIdentificationNumber.PadRight(15)}" +
               $"{IndividualName.PadRight(22)}" +
               $"{DiscretionaryData.PadRight(2)}" +
               $"{AddendumRecordIndicator}" +
               $"{TraceNumber.PadLeft(15, '0')}";
    }
}
