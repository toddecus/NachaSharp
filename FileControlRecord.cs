using System.Diagnostics.CodeAnalysis;
using System.Runtime.ConstrainedExecution;
using System;
using System.Numerics;

namespace NachaSharp;
public class FileControlRecord
{
    public RecordTypeCode RecordTypeCode = RecordTypeCode.FileControl;
    //publcipublic string RecordTypeCode = "9";  // Fixed value for File Control
    public required int BatchCount = 0;     // Total number of Batch Header Records 6 digits
    public required int BlockCount =0;   // Number of 940-character blocks (10 records each) 6 digits
    public required int EntryAndAddendumCount = 0;  // Total entry and entry addendum records 8 digitsj
    public required string EntryHash = "";  //Hash total of all routing numbers, using the last 10 digits (10 characters).
    public required decimal TotalDebitDollarAmount = 0.00m;  // Total debits in the file 12 digits
    public required decimal TotalCreditDollarAmount = 0.00m;  // Total credits in the file 12 digits
    public readonly string Reserved = "".PadRight(39);  // Reserved field (empty)

    [SetsRequiredMembers]
    public FileControlRecord(int batchCount, int blockCount, int entryAddendumCount, string entryHash, decimal totalDebitDollarAmount, decimal totalCreditDollarAmount)
    {
        BatchCount = batchCount;
        BlockCount = blockCount;
        EntryAndAddendumCount = entryAddendumCount;
        if( entryHash == null)
        {
            entryHash = "";
        }
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
    /* todo: Consolidated this to an interface that both ControlRecords can use */
    public string GenerateRecord()
    {
        return string.Concat(
            RecordTypeCode.ToStringValue() +
            BatchCount.ToString().PadLeft(6, '0') +
            BlockCount.ToString().PadLeft(6, '0') +
            EntryAndAddendumCount.ToString().PadLeft(8, '0') +
            EntryHash.PadLeft(10, '0') +
            TotalDebitDollarAmount.ToString("F2").Replace(".", "").PadLeft(12, '0') +
            TotalCreditDollarAmount.ToString("F2").Replace(".", "").PadLeft(12, '0') +
            Reserved,
            Environment.NewLine
        );
    }
    /*
    * Cieling block count calculation
    */
    
    public static int CalculateBlockCount(int entryRecordCount, int batchCount) //entry and addendum records
    {
        int fileRecordCount =2; // File Header and File Control
        int batchesCount = 2 * batchCount; // Batch Header and Batch Control
        decimal totalCount = entryRecordCount + fileRecordCount + batchesCount;
        decimal totalBlocks = totalCount / 10;
        return (int)Math.Ceiling(totalBlocks); // Each block has 10 records
    }
    public bool IsCreditsEqualToDebits()
    {
        return TotalCreditDollarAmount == TotalDebitDollarAmount;
    }
}