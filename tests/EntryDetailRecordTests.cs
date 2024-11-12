using System;
using Xunit;
using NachaSharp;
namespace NachaSharp;

public class EntryDetailRecordTests
{
    [Fact]
    public void Constructor_ShouldInitializeFieldsCorrectly()
    {
        // Arrange
        TransactionCode transactionCode = TransactionCode.DepositChecking;
        string receivingDFIRoutingNumber= "12345678";
        string checkDigit = "9";
        string receivingDFIAccountNumber = "1234567890123456";
        decimal amount = 100.50m;
        string individualIdentificationNumber = "ID123456";
        string individualName = "John Doe";
        string discretionaryData = "A1";
        string traceNumber = "987654321";

        // Act
        var entryDetailRecord = new EntryDetailRecord(transactionCode,
                                                      receivingDFIRoutingNumber,
                                                      checkDigit,
                                                      receivingDFIAccountNumber,
                                                      amount,
                                                      individualIdentificationNumber,
                                                      individualName,
                                                      traceNumber);

        entryDetailRecord.DiscretionaryData = discretionaryData;
        // Assert
        Assert.Equal("6", entryDetailRecord.RecordTypeCode.ToStringValue());
        Assert.Equal(transactionCode, entryDetailRecord.TransactionCode);
        Assert.Equal(receivingDFIRoutingNumber, entryDetailRecord.ReceivingDFIRoutingNumber);
        Assert.Equal(checkDigit, entryDetailRecord.CheckDigit);
        Assert.Equal(receivingDFIAccountNumber, entryDetailRecord.ReceivingDFIAccountNumber);
        Assert.Equal(amount, entryDetailRecord.Amount);
        Assert.Equal(individualIdentificationNumber, entryDetailRecord.IndividualIdentificationNumber);
        Assert.Equal(individualName, entryDetailRecord.IndividualName);
        Assert.Equal(0, entryDetailRecord.AddendumRecordIndicator);
        Assert.Equal(traceNumber, entryDetailRecord.TraceNumber);
        Assert.Equal(discretionaryData, entryDetailRecord.DiscretionaryData);
        string s = entryDetailRecord.GenerateRecord();
        Assert.Equal("6221234567891234567890123456 0000010050ID123456       John Doe              A10000000987654321", entryDetailRecord.GenerateRecord());
        Assert.Equal(94, entryDetailRecord.GenerateRecord().Length);
    }
}