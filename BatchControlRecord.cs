using System.Diagnostics.CodeAnalysis;
namespace NachaSharp;
public class BatchControlRecord
{
    public RecordTypeCode RecordTypeCode = RecordTypeCode.BatchControl;  // Fixed value for batch control
    public ServiceClassCode ServiceClassCode = ServiceClassCode.MixedDebitsAndCredits; // Mixed debits and credits
    public required int EntryAndAddendumCount;
    public required string EntryHash;  //Hash total of all routing numbers, using the last 10 digits (10 characters).
    public required decimal TotalDebitAmount;  // In cents
    public required decimal TotalCreditAmount;  // In cents
    public required string CompanyIdentification; // Tax ID same as Field 4 in File Header Record = ImmediateOrigin
    public string MessageAuthenticationCode; // Optional field
    public readonly string Reserved = "".PadRight(6);  // Reserved field (empty)
    public required DFINumber OriginatingDFI; // Routing number
    public required int BatchNumber; // Batch number


    [SetsRequiredMembers]
    public BatchControlRecord(int entryAndAddendumCount, string entryHash, decimal totalDebitAmount, decimal totalCreditAmount, string companyIdentification, string messageAuthenticationCode, DFINumber originatingDFI, int batchNumber)
    {
        EntryAndAddendumCount = entryAndAddendumCount;
        EntryHash = entryHash;
        TotalDebitAmount = totalDebitAmount;
        TotalCreditAmount = totalCreditAmount;
        CompanyIdentification = companyIdentification;
        MessageAuthenticationCode = messageAuthenticationCode;
        OriginatingDFI = originatingDFI;
        BatchNumber = batchNumber;
    }


    public string GenerateRecord()
    {
        return string.Concat(
            RecordTypeCode.ToStringValue() +
            ServiceClassCode.ToStringValue() +
            EntryAndAddendumCount.ToString().PadLeft(6, '0') +
            EntryHash.PadLeft(10, '0') +
            TotalDebitAmount.ToString("F2").Replace(".", "").PadLeft(12, '0') +
            TotalCreditAmount.ToString("F2").Replace(".", "").PadLeft(12, '0') +
            CompanyIdentification.PadLeft(10, ' ') +
            MessageAuthenticationCode.PadRight(19) +
            Reserved +
            OriginatingDFI +
            BatchNumber.ToString().PadLeft(7, '0')
        );
    }

    /* todo: Consolidated this to an interface that both ControlRecords can use */
    public static string CalculateEntryHash(IEnumerable<string> routingNumbers)
    {
        long totalHash = routingNumbers
        .Select(r => long.Parse(r.Substring(0, 8))) // Get the first 8 digits of each routing number
        .Sum();

        // Convert total hash to string and get the last 10 digits
        string entryHash = totalHash.ToString();
        return entryHash.Length > 10 ? entryHash.Substring(entryHash.Length - 10) : entryHash.PadLeft(10, '0');
    }
}
