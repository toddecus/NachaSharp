using System;
using Xunit;
using NachaSharp;
namespace NachaSharp;
public class FileControlRecordTests
{
    [Fact]
    public void Constructor_ShouldInitializeFieldsCorrectly()
    {
        // Arrange
        int batchCount = 1;
        int blockCount = 1;
        int entryAndAddendumCount = 5;
        string entryHash = "234567890";
        decimal totalDebitEntryDollarAmount = 1000.00m;
        decimal totalCreditEntryDollarAmount = 1000.00m;

        // Act
        var fileControlRecord = new FileControlRecord(
            batchCount,
            blockCount,
            entryAndAddendumCount,
            entryHash,
            totalDebitEntryDollarAmount,
            totalCreditEntryDollarAmount
        );
        // Assert
        Assert.Equal(batchCount, fileControlRecord.BatchCount);
        Assert.Equal(blockCount, fileControlRecord.BlockCount);
        Assert.Equal(entryAndAddendumCount, fileControlRecord.EntryAndAddendumCount);
        Assert.Equal(entryHash, fileControlRecord.EntryHash);
        Assert.Equal(totalDebitEntryDollarAmount, fileControlRecord.TotalDebitDollarAmount);
        Assert.Equal(totalCreditEntryDollarAmount, fileControlRecord.TotalCreditDollarAmount);
        String s = fileControlRecord.GenerateRecord();
        Assert.Equal("9000001000001000000050234567890000000100000000000100000                                       ", fileControlRecord.GenerateRecord());
    }
}