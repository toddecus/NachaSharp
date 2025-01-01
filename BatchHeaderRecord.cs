using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

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
    public required DFINumber OriginatingDFI; // DFI number
    public required int BatchNumber; // Batch number

    [SetsRequiredMembers]
    public BatchHeaderRecord(string companyName, string companyDiscretionaryData, string companyIdentification, string companyEntryDescription, DateTime companyDescriptiveDate, DateTime effectiveEntryDate, DFINumber originatingDFI, int batchNumber)
    {
        if (string.IsNullOrWhiteSpace(companyName) || companyName.Length > 16)
            throw new ArgumentException("Company name cannot be null or empty.", nameof(companyName));
        if (companyDiscretionaryData == null)
        {
            companyDiscretionaryData = "";
        }
        if (companyDiscretionaryData.Length > 20 || !Regex.IsMatch(companyDiscretionaryData, @"^[a-zA-Z0-9 ]*$"))
            throw new ArgumentException("Company identification cannot be null or empty and can contain only alpha numeric characters ^[a-zA-Z0-9 ]*$", nameof(companyIdentification));
        if (companyEntryDescription == null)
        {
            companyEntryDescription = "";
        }
        companyEntryDescription = companyEntryDescription.ToUpperInvariant();
        if (companyEntryDescription.Length > 10 || !Regex.IsMatch(companyEntryDescription, @"^[A-Z0-9 ]*$"))
            throw new ArgumentException("Company entry description cannot be null or empty and can contain only alpha numeric characters ^[A-Z0-9 ]*$", nameof(companyEntryDescription));
        if (batchNumber <= 0)
            throw new ArgumentException("Batch number must be greater than zero.", nameof(batchNumber));
        if (originatingDFI == null)
            throw new ArgumentNullException(nameof(originatingDFI), "OriginatingDFI cannot be null");

        CompanyName = companyName;
        CompanyDiscretionaryData = companyDiscretionaryData;
        CompanyIdentification = companyIdentification;
        CompanyEntryDescription = companyEntryDescription;
        CompanyDescriptiveDate = companyDescriptiveDate;
        EffectiveEntryDate = effectiveEntryDate;
        OriginatingDFI = originatingDFI;
        BatchNumber = batchNumber;
    }

    //return $"{RecordTypeCode.ToStringValue()}{ServiceClassCode.ToStringValue()}{CompanyName.PadRight(16)}{CompanyDiscretionaryData.PadRight(20)}{CompanyIdentification.PadLeft(10)}{StandardEntryClassCode.ToStringValue()}{CompanyEntryDescription.PadRight(10)}{CompanyDescriptiveDate:yyMMdd}{EffectiveEntryDate:yyMMdd}   {OriginatorStatusCode}{OriginatingDFI.PadRight(8)}{BatchNumber.ToString().PadLeft(7, '0')}";
    public string GenerateRecord()
    {
        return string.Concat(
            RecordTypeCode.ToStringValue(),
            ServiceClassCode.ToStringValue(),
            CompanyName.PadRight(16),
            CompanyDiscretionaryData.PadRight(20),
            CompanyIdentification.PadLeft(10),
            StandardEntryClassCode.ToStringValue(),
            CompanyEntryDescription.PadRight(10),
            CompanyDescriptiveDate.ToString("yyMMdd"),
            EffectiveEntryDate.ToString("yyMMdd"),
            "   ",
            OriginatorStatusCode,
            OriginatingDFI,
            BatchNumber.ToString().PadLeft(7, '0')
        );
    }
}
