namespace NachaSharp;
public class FileControlRecord
{
    public RecordTypeCode RecordTypeCode = RecordTypeCode.FileControl;
    //publcipublic string RecordTypeCode = "9";  // Fixed value for File Control
    public int BatchCount { get; set; }  // Total number of Batch Header Records
    public int BlockCount { get; set; }  // Number of 940-character blocks (10 records each)
    public int EntryAddendaCount { get; set; }  // Total entry and addenda records
    public long EntryHash { get; set; }  // Last 10 digits of sum of routing numbers
    public decimal TotalDebitDollarAmount { get; set; }  // Total debits in the file
    public decimal TotalCreditDollarAmount { get; set; }  // Total credits in the file
    public string Reserved = "".PadRight(39);  // Reserved field (empty)

    public string GenerateRecord()
    {
        return $"{RecordTypeCode.ToStringValue()}" +
               $"{BatchCount.ToString().PadLeft(6, '0')}" +
               $"{BlockCount.ToString().PadLeft(6, '0')}" +
               $"{EntryAddendaCount.ToString().PadLeft(8, '0')}" +
               $"{EntryHash.ToString().PadLeft(10, '0')}" +
               $"{((int)(TotalDebitDollarAmount * 100)).ToString().PadLeft(12, '0')}" +
               $"{((int)(TotalCreditDollarAmount * 100)).ToString().PadLeft(12, '0')}" +
               $"{Reserved}";
    }
    public string PadFile(int entryRecordCount)
    {
        string results = "";
        int fileRecordCount = 2; // File Header and File Control
        int batchCount = 2; // Batch Header and Batch Control
        int totalCount = entryRecordCount + fileRecordCount + batchCount;
        int padsNeeded = 10 - (totalCount % 10);
        for (int i = 0; i < padsNeeded; i++)
        {
            results += "".PadRight(94, '9')+"\n"; 
        }
        return results;
    }
}