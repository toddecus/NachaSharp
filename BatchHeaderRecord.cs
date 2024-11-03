namespace NachaFile; 
public class BatchHeaderRecord
{
    public string RecordTypeCode = "5";  // Fixed value for batch header
    public string ServiceClassCode = "200";  // Mixed debits and credits
    public string CompanyName;
    public string CompanyDiscretionaryData = " "; // Optional
    public string CompanyId;
    public string StandardEntryClassCode = "PPD"; // Payment type
    public string CompanyEntryDescription = "PAYROLL"; // Payment description
    public DateTime EffectiveEntryDate;
    public string OriginatingDFI;

    public string GenerateRecord()
    {
        return $"{RecordTypeCode}{ServiceClassCode}{CompanyName.PadRight(16)}{CompanyDiscretionaryData.PadRight(20)}{CompanyId.PadLeft(10)}{StandardEntryClassCode.PadRight(3)}{CompanyEntryDescription.PadRight(10)}{EffectiveEntryDate:yyMMdd}  {OriginatingDFI.PadRight(8)}";
    }
}
