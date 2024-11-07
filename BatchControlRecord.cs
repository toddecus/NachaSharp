namespace NachaSharp; 
public class BatchControlRecord
{
    public string RecordTypeCode = "8";  // Fixed value
    public string ServiceClassCode = "200";  // Mixed debits and credits
    public int EntryCount;
    public decimal TotalDebitAmount;  // In cents
    public decimal TotalCreditAmount;  // In cents
    public string OriginatingDFI;
    
    public string GenerateRecord()
    {
        return $"{RecordTypeCode}{ServiceClassCode.PadLeft(3)}{EntryCount.ToString().PadLeft(6, '0')}" +
               $"{TotalDebitAmount.ToString("0000000000").PadLeft(10)}{TotalCreditAmount.ToString("0000000000").PadLeft(10)}" +
               $"{OriginatingDFI.PadLeft(8)}".PadRight(94);
    }
}
