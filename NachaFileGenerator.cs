namespace NachaFile;
public class NachaFileGenerator
{
    public void GenerateNachaFile(string filePath)
    {
        var fileHeader = new FileHeaderRecord
        {
            ImmediateDestination = "123456789",
            ImmediateOrigin = "987654321",
            FileCreationDate = DateTime.Now,
            ImmediateDestinationName = "DEST BANK",
            ImmediateOriginName = "MY COMPANY"
        };

        var batchHeader = new BatchHeaderRecord
        {
            CompanyName = "MY COMPANY",
            CompanyId = "1234567890",
            EffectiveEntryDate = DateTime.Now.AddDays(1),
            OriginatingDFI = "12345678"
        };

        var entryDetail = new EntryDetailRecord
        {
            TransactionCode = "22",  // Checking account credit
            ReceivingDFIRoutingNumber = "01100001",
            ReceivingDFIAccountNumber = "123456789",
            Amount = 500.00m,  // $500.00
            ReceiverName = "John Doe",
            TraceNumber = "123456789000001"
        };

        var batchControl = new BatchControlRecord
        {
            EntryCount = 1,
            TotalDebitAmount = 0.00m,
            TotalCreditAmount = 500.00m,
            OriginatingDFI = "12345678"
        };

        using (var writer = new StreamWriter(filePath))
        {
            writer.WriteLine(fileHeader.GenerateRecord());
            writer.WriteLine(batchHeader.GenerateRecord());
            writer.WriteLine(entryDetail.GenerateRecord());
            writer.WriteLine(batchControl.GenerateRecord());
        }

        Console.WriteLine("NACHA file generated successfully.");
    }
}
