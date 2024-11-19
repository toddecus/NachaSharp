using System;
using Xunit;
using NachaSharp;
namespace NachaSharp;

public class EntryDetailRecordTests
{
    [Fact]
    public void Constructor_ShouldInitializeFieldsCorrectly()
    {
        // Arrange update string compare below if you modify
        TransactionCode transactionCode = TransactionCode.DepositChecking;
        string receivingDFIRoutingNumber = "12345678";
        string checkDigit = "8";
        string receivingDFIAccountNumber = "12345678901234567";
        decimal amount = 100.50m;
        string individualIdentificationNumber = "ID123456";
        string individualName = "John Doe";
        string discretionaryData = "A1";
        string traceNumber = "000000987654321";

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
        Assert.Equal("622123456788123456789012345670000010050ID123456       John Doe              A10000000987654321", entryDetailRecord.GenerateRecord());
        Assert.Equal(94, entryDetailRecord.GenerateRecord().Length);
    }
    [Fact]
    public void Constructor_ShouldThrowException_WhenReceivingDFIRoutingNumberIsInvalid()
    {
        // Arrange
        TransactionCode transactionCode = TransactionCode.DepositChecking;
        string invalidReceivingDFIRoutingNumber = "123";
        string checkDigit = "8";
        string receivingDFIAccountNumber = "12345678901234567";
        decimal amount = 100.50m;
        string individualIdentificationNumber = "ID123456";
        string individualName = "John Doe";
        string traceNumber = "000000987654321";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new EntryDetailRecord(transactionCode,
                                                                     invalidReceivingDFIRoutingNumber,
                                                                     checkDigit,
                                                                     receivingDFIAccountNumber,
                                                                     amount,
                                                                     individualIdentificationNumber,
                                                                     individualName,
                                                                     traceNumber));
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenCheckDigitIsInvalid()
    {
        // Arrange
        TransactionCode transactionCode = TransactionCode.DepositChecking;
        string receivingDFIRoutingNumber = "12345678";
        string invalidCheckDigit = "A";
        string receivingDFIAccountNumber = "12345678901234567";
        decimal amount = 100.50m;
        string individualIdentificationNumber = "ID123456";
        string individualName = "John Doe";
        string traceNumber = "000000987654321";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new EntryDetailRecord(transactionCode,
                                                                     receivingDFIRoutingNumber,
                                                                     invalidCheckDigit,
                                                                     receivingDFIAccountNumber,
                                                                     amount,
                                                                     individualIdentificationNumber,
                                                                     individualName,
                                                                     traceNumber));
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenReceivingDFIAccountNumberIsInvalid()
    {
        // Arrange
        TransactionCode transactionCode = TransactionCode.DepositChecking;
        string receivingDFIRoutingNumber = "12345678";
        string checkDigit = "8";
        string invalidReceivingDFIAccountNumber = "";
        decimal amount = 100.50m;
        string individualIdentificationNumber = "ID123456";
        string individualName = "John Doe";
        string traceNumber = "000000987654321";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new EntryDetailRecord(transactionCode,
                                                                     receivingDFIRoutingNumber,
                                                                     checkDigit,
                                                                     invalidReceivingDFIAccountNumber,
                                                                     amount,
                                                                     individualIdentificationNumber,
                                                                     individualName,
                                                                     traceNumber));
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenAmountIsNegative()
    {
        // Arrange
        TransactionCode transactionCode = TransactionCode.DepositChecking;
        string receivingDFIRoutingNumber = "12345678";
        string checkDigit = "8";
        string receivingDFIAccountNumber = "12345678901234567";
        decimal invalidAmount = -100.50m;
        string individualIdentificationNumber = "ID123456";
        string individualName = "John Doe";
        string traceNumber = "000000987654321";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new EntryDetailRecord(transactionCode,
                                                                     receivingDFIRoutingNumber,
                                                                     checkDigit,
                                                                     receivingDFIAccountNumber,
                                                                     invalidAmount,
                                                                     individualIdentificationNumber,
                                                                     individualName,
                                                                     traceNumber));
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenIndividualIdentificationNumberIsInvalid()
    {
        // Arrange
        TransactionCode transactionCode = TransactionCode.DepositChecking;
        string receivingDFIRoutingNumber = "12345678";
        string checkDigit = "8";
        string receivingDFIAccountNumber = "12345678901234567";
        decimal amount = 100.50m;
        string invalidIndividualIdentificationNumber = "7777777777777777";
        string individualName = "John Doe";
        string traceNumber = "000000987654321";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new EntryDetailRecord(transactionCode,
                                                                     receivingDFIRoutingNumber,
                                                                     checkDigit,
                                                                     receivingDFIAccountNumber,
                                                                     amount,
                                                                     invalidIndividualIdentificationNumber,
                                                                     individualName,
                                                                     traceNumber));
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenIndividualNameIsInvalid()
    {
        // Arrange
        TransactionCode transactionCode = TransactionCode.DepositChecking;
        string receivingDFIRoutingNumber = "12345678";
        string checkDigit = "8";
        string receivingDFIAccountNumber = "12345678901234567";
        decimal amount = 100.50m;
        string individualIdentificationNumber = "ID123456";
        string invalidIndividualName = "John Doe John Doe John Doe";
        string traceNumber = "000000987654321";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new EntryDetailRecord(transactionCode,
                                                                     receivingDFIRoutingNumber,
                                                                     checkDigit,
                                                                     receivingDFIAccountNumber,
                                                                     amount,
                                                                     individualIdentificationNumber,
                                                                     invalidIndividualName,
                                                                     traceNumber));
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenTraceNumberIsInvalid()
    {
        // Arrange
        TransactionCode transactionCode = TransactionCode.DepositChecking;
        string receivingDFIRoutingNumber = "12345678";
        string checkDigit = "8";
        string receivingDFIAccountNumber = "12345678901234567";
        decimal amount = 100.50m;
        string individualIdentificationNumber = "ID123456";
        string individualName = "John Doe";
        string invalidTraceNumber = "1234567890123456";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new EntryDetailRecord(transactionCode,
                                                                     receivingDFIRoutingNumber,
                                                                     checkDigit,
                                                                     receivingDFIAccountNumber,
                                                                     amount,
                                                                     individualIdentificationNumber,
                                                                     individualName,
                                                                     invalidTraceNumber));
    }
}

