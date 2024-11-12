using System.IO;
using Microsoft.Extensions.Logging;

namespace NachaSharp;
public class NachaFileGenerator
{
    private readonly ILogger<NachaFileGenerator> _logger;

    public NachaFileGenerator(ILogger<NachaFileGenerator> logger)
    {
        _logger = logger;
    }
    public string filePath = "./outputFiles/";
    public string fileName = "nacha.txt";

    public FileHeaderRecord fileHeader;
    public BatchHeaderRecord batchHeader;
    public List<EntryDetailRecord> entryDetailRecords = new List<EntryDetailRecord>();
    public BatchControlRecord batchControl;
    public FileControlRecord fileControl;
    public void GenerateNachaFile()
    {
        string fullPath = Path.Combine(filePath, fileName);

        using (var writer = new StreamWriter(fullPath))
        {
            _logger.LogTrace("Generating NACHA file... {0}", filePath+fileName);
            writer.WriteLine(fileHeader.GenerateRecord());
            _logger.LogTrace("File Header Record generated.");
            writer.WriteLine(batchHeader.GenerateRecord());
            _logger.LogTrace("Batch Header Record generated.");
            int countEntryAddendumRecords = 0;
            foreach (var entryDetailRecord in entryDetailRecords)
            {
                writer.WriteLine(entryDetailRecord.GenerateRecord());
                _logger.LogTrace("Entry Detail Record generated.");

                if (entryDetailRecord.EntryAddendumRecord != null)
                {
                    countEntryAddendumRecords++; // addendums have their own line for the pad file calculation
                    writer.WriteLine(entryDetailRecord.EntryAddendumRecord.GenerateRecord());
                    _logger.LogTrace("Entry Addendum Record generated.");
                }
            }
            writer.WriteLine(batchControl.GenerateRecord());
            _logger.LogTrace("Batch Control Record generated.");
            writer.WriteLine(fileControl.GenerateRecord());
            _logger.LogTrace("File Control Record generated.");
            writer.Write(FileControlRecord.PadFile(entryDetailRecords.Count + countEntryAddendumRecords)); // Pad the file to 10 characters
            _logger.LogTrace("File padded to 10 characters. EntryDetailRecords.Count {0} EntryAddendumRecordsCount {1}", entryDetailRecords.Count , countEntryAddendumRecords);  
        }


    }
    public void PopulateTestData()
    {
        fileHeader = new FileHeaderRecord
        (
           "DEST BANK",
            "123456789",
            "MY COMPANY",
            "987654321",
            "88888888"

        );

        batchHeader = new BatchHeaderRecord
        (
            "MY COMPANY",
            "01",
            "987654321",
            "Payments",
            DateTime.Now.AddDays(2), // Company Descriptive Date
            DateTime.Now.AddDays(2), // Effective Entry Date
            "12345678", // Originating DFI
            1 // Batch number
        );
        entryDetailRecords = new List<EntryDetailRecord>();

        entryDetailRecords.Add(new EntryDetailRecord
        {
            TransactionCode = "27",  // Checking account debit
            ReceivingDFIRoutingNumber = "01100001",
            ReceivingDFIAccountNumber = "123456789",
            Amount = 1000.00m,  // $500.00
            ReceiverIdNumber = "123456789",
            ReceiverName = "John Doe",
            TraceNumber = "123456789000001",
            EntryAddendumRecord = new EntryAddendumRecord
            {
                AddendaSequenceNumber = 1,
                EntryDetailSequenceNumber = "1234567",
                PaymentRelatedInformation = "Payment for invoice 12345"
            }
        });
        entryDetailRecords.Add(new EntryDetailRecord
        {
            TransactionCode = "22",  // Checking account credit
            ReceivingDFIRoutingNumber = "01100002",
            ReceivingDFIAccountNumber = "123456789",
            Amount = 500.00m,  // $500.00
            ReceiverName = "John Doe",
            ReceiverIdNumber = "123456789",
            TraceNumber = "123456789000001",
            EntryAddendumRecord = new EntryAddendumRecord
            {
                AddendaSequenceNumber = 2,
                EntryDetailSequenceNumber = "1234567",
                PaymentRelatedInformation = "Payment for invoice 12345"
            }
        });
        entryDetailRecords.Add(new EntryDetailRecord
        {
            TransactionCode = "22",  // Checking account credit
            ReceivingDFIRoutingNumber = "01100003",
            ReceivingDFIAccountNumber = "123456789",
            Amount = 500.00m,  // $500.00
            ReceiverName = "John Doe",
            ReceiverIdNumber = "123456789",
            TraceNumber = "123456789000001"
        });

        batchControl = new BatchControlRecord
        (
            5,
            BatchControlRecord.CalculateEntryHash(new List<string> { "01100001", "01100002", "01100003" }),
            1000.00m,
            1000.00m,
            "987654321",
            "",
            "12345678",
            1
        );
        fileControl = new FileControlRecord(
        
            1,
            1,
            5,
            FileControlRecord.CalculateEntryHash(new List<string> { "011000011", "051000011", "061000011" }),
            1000.00m,
            1000.00m

        );
        
    }
}
