
using System.Diagnostics.CodeAnalysis;
namespace NachaSharp;
public class EntryDetailRecord
{
    public RecordTypeCode RecordTypeCode = RecordTypeCode.EntryDetail; // Fixed value for Entry Detail
    public required TransactionCode TransactionCode; // 22 for credit, 27 for debit
    public required string ReceivingDFIRoutingNumber; // First 8 digits of the routing number
    public required string CheckDigit; // Last digit of the routing number
    public required string ReceivingDFIAccountNumber; // Account number
    public required decimal Amount; // in dollars
    public required string IndividualIdentificationNumber; // Can be a unique ID for the sender
    public required string IndividualName; // Name of the receiver
    public string DiscretionaryData =""; // Optional 2 characters
    public int AddendumRecordIndicator = 0; // (default)0 for no addenda, 1 for addenda mostly applicable for CCD+ and CTX (but this should support more than one Addendum)
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
        if(string.IsNullOrWhiteSpace(receivingDFIRoutingNumber)|| receivingDFIRoutingNumber.Length != 8)
        {
            throw new ArgumentException("ReceivingDFIRoutingNumber must be 8 digits");
        }
        ReceivingDFIRoutingNumber = receivingDFIRoutingNumber;
        if (string.IsNullOrWhiteSpace(checkDigit))
        {
            checkDigit = receivingDFIRoutingNumber[^1..];
        }
        else
        {
            if (checkDigit.Length > 1 || checkDigit != receivingDFIRoutingNumber[^1..])
            {
                throw new ArgumentException("CheckDigit must be the last digit of the ReceivingDFIRoutingNumber : " + checkDigit + " is not the last digit of " + receivingDFIRoutingNumber);
            }
        }
        CheckDigit = checkDigit;
        if (string.IsNullOrWhiteSpace(receivingDFIAccountNumber) || receivingDFIAccountNumber.Length > 17 )
        {
            throw new ArgumentException("ReceivingDFIAccountNumber must be 17 digits with at least 1 character");
        }
        ReceivingDFIAccountNumber = receivingDFIAccountNumber;
        if(amount < 0 || amount > 99999999.99M) // 8 digits and 2 decimal places total of 10
        {
            throw new ArgumentException($"Amount must be greater than 0 or less than $99999999.99 : " 
                                        + amount + " is invalid");
        }
        Amount = amount;
        if(individualIdentificationNumber.Length > 15)
        {
            throw new ArgumentException("IndividualIdentificationNumber must be 15 characters or less : " + individualIdentificationNumber + " is length : " + individualIdentificationNumber.Length);
        }
        IndividualIdentificationNumber = individualIdentificationNumber;
        if(individualName.Length > 22)
        {
            throw new ArgumentException("IndividualName must be 22 characters or less : " + individualName + " is length : " + individualName.Length);
        }
        IndividualName = individualName;
        if(traceNumber.Length > 15)
        {
            throw new ArgumentException("TraceNumber must be 15 digits or less : " + traceNumber + " is length : " + traceNumber.Length);
        }
        TraceNumber = traceNumber;
    }

    public string GenerateRecord()
    {
        return string.Concat( 
            RecordTypeCode.ToStringValue() +
            TransactionCode.ToStringValue() +
            ReceivingDFIRoutingNumber.PadLeft(8, '0') +
            CheckDigit +
            ReceivingDFIAccountNumber.PadRight(17) +
            Amount.ToString("F2").Replace(".", "").PadLeft(10, '0') +
            IndividualIdentificationNumber.PadRight(15) +
            IndividualName.PadRight(22) +
            DiscretionaryData.PadRight(2) +
            AddendumRecordIndicator +
            TraceNumber.PadLeft(15, '0')
        );
    }
    /*
    * TransactionCode 
    *   23 prenote credit checking, 28 prenote debit checking 
    *   33 prenote credit savings, 38 prenote debit savings
    * Routing number for the bank you are sending to
    * Account number for the bank you are sending to
    * Individual Identification Number << the ID of the business or person you are sending to in YOUR system
    * Individual Name << the name of the business or person you are sending to
    * TraceNumber << a unique identifier for this transaction Maybe the entryID in your system to check off this row for errors
    * Wait the required time (typically 3 banking days) before initiating live transactions. This allows the receiving institution to reject or validate the prenote.
    */
    public static EntryDetailRecord CreatePrenoteEntryDetailRecord(TransactionCode transactionCode,string receivingDFIRoutingNumber, string receivingDFIAccountNumber,  string individualIdentificationNumber, string individualName, string traceNumber)
    {
        decimal amount = 0.00M; // prenote amount is 0.00
        string checkDigit = receivingDFIRoutingNumber[^1..]; // last digit of the routing number
        return new EntryDetailRecord(transactionCode, receivingDFIRoutingNumber, checkDigit, receivingDFIAccountNumber, amount, individualIdentificationNumber, individualName, traceNumber);
    }
    public static EntryDetailRecord CreateCheckingCreditEntry(string receivingDFIRoutingNumber, string receivingDFIAccountNumber, decimal amount, string individualIdentificationNumber, string individualName, string traceNumber)
    {
        string checkDigit = receivingDFIRoutingNumber[^1..]; // last digit of the routing number
        return new EntryDetailRecord(TransactionCode.DepositChecking, receivingDFIRoutingNumber, checkDigit, receivingDFIAccountNumber, amount, individualIdentificationNumber, individualName, traceNumber);
    }
    public static EntryDetailRecord CreateCheckingCreditEntryWithAddendum(string receivingDFIRoutingNumber, string receivingDFIAccountNumber, decimal amount, string individualIdentificationNumber, string individualName, string traceNumber, string addendumString)
    {
        string checkDigit = receivingDFIRoutingNumber[^1..]; // last digit of the routing number
        EntryAddendumRecord addendumRecord = new EntryAddendumRecord(addendumString, traceNumber.Substring(traceNumber.Length - 7));
        addendumRecord.AddendumTypeCode = AddendumTypeCode.AdditionalPaymentInformation; // 05 for ccd
        EntryDetailRecord entryDetailRecord = new EntryDetailRecord(TransactionCode.DepositChecking, receivingDFIRoutingNumber, checkDigit, receivingDFIAccountNumber, amount, individualIdentificationNumber, individualName, traceNumber);
        entryDetailRecord.EntryAddendumRecord = addendumRecord;
        return entryDetailRecord;
    }
}
