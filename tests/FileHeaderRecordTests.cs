using System;
using Xunit;
using NachaSharp;
namespace NachaSharp;

public class FileHeaderRecordTests
{
    [Fact]
    public void Constructor_ShouldInitializeFieldsCorrectly()
    {
        // Arrange
        string immediateDestination = "123456789";
        string immediateOrigin = "987654321";
        string immediateDestinationName = "Destination Bank";
        string immediateOriginName = "Origin Business";
        string referenceCode = "88888888";

        // Act
        var fileHeaderRecord = new FileHeaderRecord(immediateDestinationName,
                                                    immediateDestination,
                                                    immediateOriginName,
                                                    immediateOrigin,
                                                    referenceCode
                                                    );

        // Set the datetime to match the generated record
        fileHeaderRecord.FileCreationDate = new DateTime(2024, 11, 06);
        fileHeaderRecord.FileCreationTime = new TimeSpan(20, 07, 10);
        
        // Assert
        Assert.Equal(immediateDestinationName, fileHeaderRecord.ImmediateDestinationName);
        Assert.Equal(immediateDestination, fileHeaderRecord.ImmediateDestination);
        Assert.Equal(immediateOriginName, fileHeaderRecord.ImmediateOriginName);
        Assert.Equal(immediateOrigin, fileHeaderRecord.ImmediateOrigin);
        Assert.Equal(referenceCode, fileHeaderRecord.ReferenceCode);
        Assert.Equal("1", fileHeaderRecord.RecordTypeCode.ToStringValue());
        Assert.Equal("01", fileHeaderRecord.PriorityCode);
        Assert.Equal("A", fileHeaderRecord.FileIdModifier);
        Assert.Equal("094", fileHeaderRecord.RecordSize);
        Assert.Equal("10", fileHeaderRecord.BlockingFactor);
        Assert.Equal("1", fileHeaderRecord.FormatCode);
        //string s = fileHeaderRecord.GenerateRecord();
        Assert.Equal("101 123456789 9876543212411062007A094101Destination Bank       Origin Business        88888888" + Environment.NewLine,fileHeaderRecord.GenerateRecord());
        Assert.Equal(95, fileHeaderRecord.GenerateRecord().Length);
    }
}