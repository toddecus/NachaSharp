namespace NachaSharp;
public class NachaFileGenerator
{
    public string filePath = "";
    public string fileName = "nacha.txt";
    public void GenerateNachaFile()
    {
        var fileHeader = new FileHeaderRecord
        (
           "DEST BANK",
            "123456789",
            "MY COMPANY",
            "987654321",
            "88888888"

        );

        var batchHeader = new BatchHeaderRecord
        {
            CompanyName = "MY COMPANY",
            CompanyId = "1234567890",
            EffectiveEntryDate = DateTime.Now.AddDays(1),
            OriginatingDFI = "12345678"
        };

        var entryDetailDebit = new EntryDetailRecord
        {
            TransactionCode = "27",  // Checking account debit
            ReceivingDFIRoutingNumber = "01100001",
            ReceivingDFIAccountNumber = "123456789",
            Amount = 500.00m,  // $500.00
            ReceiverName = "John Doe",
            TraceNumber = "123456789000001"
        };
        var entryDetailDebitAddendum = new EntryDetailRecord
        {
            TransactionCode = "27",  // Checking account debit
            ReceivingDFIRoutingNumber = "01100001",
            ReceivingDFIAccountNumber = "123456789",
            Amount = 500.00m,  // $500.00
            ReceiverName = "John Doe",
            TraceNumber = "123456789000001"
        };
        var entryDetailCredit = new EntryDetailRecord
        {
            TransactionCode = "22",  // Checking account credit
            ReceivingDFIRoutingNumber = "01100001",
            ReceivingDFIAccountNumber = "123456789",
            Amount = 500.00m,  // $500.00
            ReceiverName = "John Doe",
            TraceNumber = "123456789000001"
        };
        var entryDetailCreditAddendum = new EntryDetailRecord
        {
            TransactionCode = "22",  // Checking account credit
            ReceivingDFIRoutingNumber = "01100001",
            ReceivingDFIAccountNumber = "123456789",
            Amount = 500.00m,  // $500.00
            ReceiverName = "John Doe",
            TraceNumber = "123456789000001"
        };  
        var entryDetailCredit2 = new EntryDetailRecord
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
        var fileControl = new FileControlRecord
        {
            BatchCount = 1,
            BlockCount = 1,
            EntryAddendaCount = 2,
            EntryHash = 0,
            TotalDebitAmount = 0.00m,
            TotalCreditAmount = 500.00m,
            ImmediateDestination = "123456789",
            ImmediateOrigin = "987654321",
            FileCreationDate = DateTime.Now,
            FileCreationTime = DateTime.Now.TimeOfDay,
            FileIdModifier = "A"
        };

        using (var writer = new StreamWriter(filePath))
        {
            writer.WriteLine(fileHeader.GenerateRecord());
            writer.WriteLine(batchHeader.GenerateRecord());
            writer.WriteLine(entryDetailDebit.GenerateRecord());//Debit
            writer.WriteLine(entryDetailDebitAddendum.GenerateRecord());//Debit
            writer.WriteLine(entryDetailCredit.GenerateRecord());//Credit
            writer.WriteLine(entryDetailCreditAddendum.GenerateRecord());//Debit
            writer.WriteLine(entryDetailCredit2.GenerateRecord());//Credit
            writer.WriteLine(batchControl.GenerateRecord());
            writer.WriteLine(fileControl.GenerateRecord());
            writer.WriteLine(fileControl.padFile(3));
        }

        Console.WriteLine("NACHA file generated successfully.");
    }
}
