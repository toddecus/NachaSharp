using System.Diagnostics.CodeAnalysis;
using System.Runtime.ConstrainedExecution;

namespace NachaSharp;
public class FileControlRecord
{
    public RecordTypeCode RecordTypeCode = RecordTypeCode.FileControl;
    //publcipublic string RecordTypeCode = "9";  // Fixed value for File Control
    public required int BatchCount { get; set;}    // Total number of Batch Header Records 6 digits
    public required int BlockCount;   // Number of 940-character blocks (10 records each) 6 digits
    public required int EntryAndAddendumCount;  // Total entry and entry addendum records 8 digitsj
    public required string EntryHash;  //Hash total of all routing numbers, using the last 10 digits (10 characters).
    public required decimal TotalDebitDollarAmount;  // Total debits in the file 12 digits
    public required decimal TotalCreditDollarAmount;  // Total credits in the file 12 digits
    public readonly string Reserved = "".PadRight(39);  // Reserved field (empty)

    [SetsRequiredMembers]
    public FileControlRecord(int batchCount, int blockCount, int entryAddendumCount, string entryHash, decimal totalDebitDollarAmount, decimal totalCreditDollarAmount)
    {
        BatchCount = batchCount;
        BlockCount = blockCount;
        EntryAndAddendumCount = entryAddendumCount;
        EntryHash = entryHash;
        TotalDebitDollarAmount = totalDebitDollarAmount;
        TotalCreditDollarAmount = totalCreditDollarAmount;
    }

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
        if (padsNeeded == 10) return results; // No padding needed
        for (int i = 1; i < padsNeeded - 1; i++)
        {
            results += "".PadRight(94, '9') + "\n";
        }
        results += "".PadRight(94, '9');
        return results;
    }
    public bool CreditsEqualDebits()
    {
        return TotalCreditDollarAmount == TotalDebitDollarAmount;
    }
}