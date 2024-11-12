using System.Diagnostics.CodeAnalysis;
namespace NachaSharp; 
public class BatchHeaderRecord
{
    public RecordTypeCode RecordTypeCode = RecordTypeCode.BatchHeader;  // Fixed value for batch header
    public ServiceClassCode ServiceClassCode = ServiceClassCode.MixedDebitsAndCredits; // Mixed debits and credits
    public StandardEntryClassCode StandardEntryClassCode = StandardEntryClassCode.CashConcentrationAndDisbursement; // CCD

    public readonly string OriginatorStatusCode = "1"; // "1". This code indicates that the originator is a depository financial institution (DFI) authorized to transmit ACH entries
    public required string CompanyName; // Your Company name what shows up on their bank statement
    public required string CompanyDiscretionaryData; // Optional data to help identify the transaction 
    public required string CompanyIdentification; // Tax ID same as Field 4 in File Header Record = ImmediateOrigin 
    public required string CompanyEntryDescription; // Payment description of the transaction on the bank statement 
    public required DateTime CompanyDescriptiveDate; // The date you choose to identify the transactions. This date may be printed on the participants’ bank statement by the Receiving Financial Institution.
    public required DateTime EffectiveEntryDate; // Dates posted to the target account
    public required string OriginatingDFI; // Routing number
    public required int BatchNumber; // Batch number

    [SetsRequiredMembers]    
    public BatchHeaderRecord(string companyName, string companyDiscretionaryData, string companyIdentification, string companyEntryDescription, DateTime companyDescriptiveDate, DateTime effectiveEntryDate, string originatingDFI, int batchNumber)
    {
        CompanyName = companyName;
        CompanyDiscretionaryData = companyDiscretionaryData;
        CompanyIdentification = companyIdentification;
        CompanyEntryDescription = companyEntryDescription;
        CompanyDescriptiveDate = companyDescriptiveDate;
        EffectiveEntryDate = effectiveEntryDate;
        OriginatingDFI = originatingDFI;
        BatchNumber = batchNumber;
    }

    public string GenerateRecord()
    {
        return $"{RecordTypeCode.ToStringValue()}{ServiceClassCode.ToStringValue()}{CompanyName.PadRight(16)}{CompanyDiscretionaryData.PadRight(20)}{CompanyIdentification.PadLeft(10)}{StandardEntryClassCode.ToStringValue()}{CompanyEntryDescription.PadRight(10)}{CompanyDescriptiveDate:yyMMdd}{EffectiveEntryDate:yyMMdd}   {OriginatorStatusCode}{OriginatingDFI.PadRight(8)}{BatchNumber.ToString().PadLeft(7, '0')}";
    }
}
