namespace NachaSharp;
public class EntryDetailRecord
{
    public string RecordTypeCode = "6";
    public string TransactionCode;
    public string ReceivingDFIRoutingNumber; // First 8 digits of the routing number
    public string ReceivingDFIAccountNumber;
    public decimal Amount; // In cents
    public string ReceiverIdNumber; // Can be a unique ID for the receiver
    public string ReceiverName;
    public string TraceNumber; // Unique identifier

    public string GenerateRecord()
    {
        return $"{RecordTypeCode}{TransactionCode}{ReceivingDFIRoutingNumber.PadLeft(8)}{ReceivingDFIAccountNumber.PadRight(17)}{Amount.ToString("0000000000")}{ReceiverIdNumber.PadRight(15)}{ReceiverName.PadRight(22)}{TraceNumber.PadLeft(15)}";
    }
}
