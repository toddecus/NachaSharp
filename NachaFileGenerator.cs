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
        var entryDetailDebitAddendum = new EntryAddendumRecord
        {
            AddendaSequenceNumber = 1,
            EntryDetailSequenceNumber = "1234567",
            PaymentRelatedInformation = "Payment for invoice 12345"
        };
        var entryDetailCredit = new EntryDetailRecord
        {
            TransactionCode = "22",  // Checking account credit
            ReceivingDFIRoutingNumber = "01100002",
            ReceivingDFIAccountNumber = "123456789",
            Amount = 500.00m,  // $500.00
            ReceiverName = "John Doe",
            TraceNumber = "123456789000001"
        };
        var entryDetailCreditAddendum = new EntryAddendumRecord
        {
            AddendaSequenceNumber = 2,
            EntryDetailSequenceNumber = "1234567",
            PaymentRelatedInformation = "Payment for invoice 12346"
        };
        var entryDetailCredit2 = new EntryDetailRecord
        {
            TransactionCode = "22",  // Checking account credit
            ReceivingDFIRoutingNumber = "01100003",
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
            EntryHash = FileControlRecord.CalculateEntryHash(new List<string> { "011000012", "011000013", "011000014", "011000015", "011000016" }),
            EntryAndAddendumCount = 3,
            TotalDebitDollarAmount = 0.00m,
            TotalCreditDollarAmount = 1500.00m
         
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
            writer.Write(FileControlRecord.PadFile(3));
        }

        
    }
}
