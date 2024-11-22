using System;
using Xunit;
using NachaSharp;

namespace NachaSharp
{
    public class BatchControlRecordTests
    {
        [Fact]
        public void Constructor_ShouldInitializeFieldsCorrectly()
        {
            // Arrange
            int entryAndAddendumCount = 5;
            string entryHash = "23456789" ;
            decimal totalDebitAmount = 1000.50M;
            decimal totalCreditAmount = 2000.75M;
            string companyIdentification = "123456789";
            string messageAuthenticationCode = "ZZZZZZZZZZZZZZZZZZZ";
            DFINumber originatingDFI = new DFINumber("12345678");
            int batchNumber = 1;

            // Act
            var batchControlRecord = new BatchControlRecord(
                entryAndAddendumCount,
                entryHash,
                totalDebitAmount,
                totalCreditAmount,
                companyIdentification,
                messageAuthenticationCode,
                originatingDFI,
                batchNumber
            );

            // Assert
            Assert.Equal(entryAndAddendumCount, batchControlRecord.EntryAndAddendumCount);
            Assert.Equal(entryHash, batchControlRecord.EntryHash);
            Assert.Equal(totalDebitAmount, batchControlRecord.TotalDebitAmount);
            Assert.Equal(totalCreditAmount, batchControlRecord.TotalCreditAmount);
            Assert.Equal(companyIdentification, batchControlRecord.CompanyIdentification);
            Assert.Equal(originatingDFI, batchControlRecord.OriginatingDFI);
            Assert.Equal(batchNumber, batchControlRecord.BatchNumber);
            string s = batchControlRecord.GenerateRecord();
            Assert.Equal("820000000500234567890000001000500000002000750123456789ZZZZZZZZZZZZZZZZZZZ      123456780000001", batchControlRecord.GenerateRecord());
            Assert.Equal(94, batchControlRecord.GenerateRecord().Length);
        }
    }
}