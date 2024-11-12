using System;
using Xunit;
using NachaSharp;

namespace NachaSharp
{
    public class BatchHeaderRecordTests
    {
        [Fact]
        public void Constructor_ShouldInitializeFieldsCorrectly()
        {
            // Arrange
            string companyName = "My Company";
            string companyDiscretionaryData = "Optional Data";
            string companyIdentification = "123456789";
            string companyEntryDescription = "Payments";
            DateTime companyDescriptiveDate = new DateTime(2024, 11, 06);
            DateTime effectiveEntryDate = new DateTime(2024, 11, 06);
            string originatingDFI = "12345678";
            int batchNumber = 1;

            // Act
            var batchHeaderRecord = new BatchHeaderRecord(
                companyName,
                companyDiscretionaryData,
                companyIdentification,
                companyEntryDescription,
                companyDescriptiveDate,
                effectiveEntryDate,
                originatingDFI,
                batchNumber
            );

            // Assert
            Assert.Equal(companyName, batchHeaderRecord.CompanyName);
            Assert.Equal(companyDiscretionaryData, batchHeaderRecord.CompanyDiscretionaryData);
            Assert.Equal(companyIdentification, batchHeaderRecord.CompanyIdentification);
            Assert.Equal(companyEntryDescription, batchHeaderRecord.CompanyEntryDescription);
            Assert.Equal(companyDescriptiveDate, batchHeaderRecord.CompanyDescriptiveDate);
            Assert.Equal(effectiveEntryDate, batchHeaderRecord.EffectiveEntryDate);
            Assert.Equal(originatingDFI, batchHeaderRecord.OriginatingDFI);
            Assert.Equal(batchNumber, batchHeaderRecord.BatchNumber);
            //string s = batchHeaderRecord.GenerateRecord();
            Assert.Equal("5200My Company      Optional Data        123456789CCDPayments  241106241106   1123456780000001", batchHeaderRecord.GenerateRecord());
            Assert.Equal(94, batchHeaderRecord.GenerateRecord().Length);

        }
    }
}