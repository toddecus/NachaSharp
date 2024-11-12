using System;
using Xunit;
using NachaSharp;
namespace NachaSharp;

public class EntryAddendumRecordTests
{
    [Fact]
    public void Constructor_ShouldInitializeFieldsCorrectly()
    {
        // Arrange
        string paymentRelatedInformation = "Payment Info";
        string entryDetailSequenceNumber = "0000001";

        // Act
        var entryAddendumRecord = new EntryAddendumRecord(paymentRelatedInformation,
                                                          entryDetailSequenceNumber);

        // Assert
        Assert.Equal("7", entryAddendumRecord.RecordTypeCode.ToStringValue());
        Assert.Equal(paymentRelatedInformation, entryAddendumRecord.PaymentRelatedInformation);
        Assert.Equal(entryDetailSequenceNumber, entryAddendumRecord.EntryDetailSequenceNumber);
        string s = entryAddendumRecord.GenerateRecord();
        Assert.Equal("705Payment Info                                                                    00010000001", entryAddendumRecord.GenerateRecord());
        Assert.Equal(94, entryAddendumRecord.GenerateRecord().Length);
    }
}