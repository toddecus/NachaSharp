namespace NachaSharp;
public class FileControlRecord
{
    public RecordTypeCode RecordTypeCode = RecordTypeCode.FileControl;
    //publcipublic string RecordTypeCode = "9";  // Fixed value for File Control
    public int BatchCount { get; set; }  // Total number of Batch Header Records
    public int BlockCount { get; set; }  // Number of 940-character blocks (10 records each)
    public int EntryAndAddendumCount { get; set; }  // Total entry and entry addendum records
    public string EntryHash { get; set; }  //Hash total of all routing numbers, using the last 10 digits (10 characters).
    public decimal TotalDebitDollarAmount { get; set; }  // Total debits in the file
    public decimal TotalCreditDollarAmount { get; set; }  // Total credits in the file
    public string Reserved = "".PadRight(39);  // Reserved field (empty)

    public static string CalculateEntryHash(IEnumerable<string> routingNumbers)
    {
        long totalHash = routingNumbers
        .Select(r => long.Parse(r.Substring(0, 8))) // Get the first 8 digits of each routing number
        .Sum();

    // Convert total hash to string and get the last 10 digits
    string entryHash = totalHash.ToString();
    return entryHash.Length > 10 ? entryHash.Substring(entryHash.Length - 10) : entryHash.PadLeft(10, '0');
    }
    public string GenerateRecord()
    {
        return $"{RecordTypeCode.ToStringValue()}" +
               $"{BatchCount.ToString().PadLeft(6, '0')}" +
               $"{BlockCount.ToString().PadLeft(6, '0')}" +
               $"{EntryAndAddendumCount.ToString().PadLeft(8, '0')}" +
               $"{EntryHash.PadLeft(10, '0')}" +
               $"{((int)(TotalDebitDollarAmount * 100)).ToString().PadLeft(12, '0')}" +
               $"{((int)(TotalCreditDollarAmount * 100)).ToString().PadLeft(12, '0')}" +
               $"{Reserved}";
    }
    /* The file must be a multiple of 10 records, so this method pads the file with 9s to reach that multiple
        entryRecordCount should be EntryDetailRecordCount + EntryAddendumRecordCount */
    public static string PadFile(int entryAddendumCount)
    {
        string results = "";
        int fileRecordCount = 2; // File Header and File Control
        int batchCount = 2; // Batch Header and Batch Control
        int totalCount = entryAddendumCount + fileRecordCount + batchCount;
        int padsNeeded = 10 - (totalCount % 10);
        for (int i = 0; i < padsNeeded; i++)
        {
            results += "".PadRight(94, '9')+"\n"; 
        }
        return results;
    }
}