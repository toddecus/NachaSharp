using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.Extensions.Logging;

namespace NachaSharp;
public class NachaFile
{
    private readonly ILogger<NachaFile> _logger;

    public required FileHeaderRecord FileHeader;
    public List<Batch> Batches { get; } = new List<Batch>(); // List of batches 1 or more
    public required FileControlRecord FileControl;


    [SetsRequiredMembers]
    public NachaFile(FileHeaderRecord fileHeaderRecord, FileControlRecord fileControlRecord, ILogger<NachaFile> logger)
    {
        FileHeader = fileHeaderRecord ?? throw new ArgumentNullException(nameof(fileHeaderRecord));
        FileControl = fileControlRecord ?? throw new ArgumentNullException(nameof(fileControlRecord));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [SetsRequiredMembers]
    public NachaFile(FileHeaderRecord fileHeaderRecord, FileControlRecord fileControlRecord)
    {
        FileHeader = fileHeaderRecord ?? throw new ArgumentNullException(nameof(fileHeaderRecord));
        FileControl = fileControlRecord ?? throw new ArgumentNullException(nameof(fileControlRecord));
        using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Warning);    
            });
        _logger = loggerFactory.CreateLogger<NachaSharp.NachaFile>();
    }

    /*
    * This method generates the NACHA file as a string with each 94-character record on a new line and file padded with lines of 9 to make
    * the file a multiple of 10 records.
    */
    public string ToStringValue()
    {
        if(FileControl.BatchCount == 0 || FileControl.BlockCount== 0)
        {
            return "ERROR: FileControl BatchCount and BlockCount must be populated before generating the NACHA file. Did you forget to call CalculateFileControl?";
        }
        if(Batches.Count == 0)
        {
            return "ERROR: Invalid Natcha File No batches to generate.";
        }
        if(Batches[0].ControlRecord.EntryAndAddendumCount == 0)
        {
            return "ERROR: No entries in the first batch. Did you forget to call CalculateControlRecord() on the Batch?"; 
        }
        StringBuilder batchesString = new StringBuilder();
        int entryAndAddendumCount = 0;
        foreach (var batch in Batches)
        {
            batchesString.Append( batch.GenerateRecord());
            entryAndAddendumCount += batch.ControlRecord.EntryAndAddendumCount;
        }
        this.CalculateFileControl();
        return string.Concat(
            FileHeader.GenerateRecord(), 
            batchesString.ToString(), 
            FileControl.GenerateRecord(),  
            GetFileNinePad(entryAndAddendumCount, Batches.Count, _logger)
        );
    }

    private void CalculateFileControl()
    {
        if (FileControl == null)
        {
            if (FileHeader == null)
            {
                throw new ArgumentNullException(nameof(FileHeader), "Can't calculate control record without a header record");
            }
            FileControl = new FileControlRecord(0, 0, 0, "", 0.00m, 0.00m);
        }
        List<string> routingNumbers = new List<string>();
        FileControl.BatchCount = Batches.Count;
        int entryCounter = 0;
        FileControl.TotalDebitDollarAmount = 0.00m;
        FileControl.TotalCreditDollarAmount = 0.00m;
        foreach (var batch in Batches)
        {
            entryCounter+= batch.ControlRecord.EntryAndAddendumCount; 
            
            FileControl.TotalDebitDollarAmount += batch.ControlRecord.TotalDebitAmount;
            FileControl.TotalCreditDollarAmount += batch.ControlRecord.TotalCreditAmount;
            foreach (var entry in batch.EntryDetailRecords)
            {
                routingNumbers.Add(entry.ReceivingDFI.ToString());
            }
        }
        FileControl.EntryAndAddendumCount += entryCounter;
        FileControl.BlockCount = FileControlRecord.CalculateBlockCount(entryCounter, FileControl.BatchCount);
        FileControl.EntryHash = FileControlRecord.CalculateEntryHash(routingNumbers);
    }

    public void GenerateTestNachaFile()
    {
        string filePath = "./outputFiles/";
        string fileName = "nacha.txt";
        string fullPath = Path.Combine(filePath, fileName);

        _logger.LogTrace($"FileHeader{FileHeader}, BatchHeader{Batches[0].HeaderRecord}, BatchControl{Batches[0].ControlRecord}," +
            $" and FileControl{FileControl} must be populated before generating the NACHA file.");
        if (FileHeader == null || Batches[0].HeaderRecord == null || Batches[0].ControlRecord == null || FileControl == null)
        {
            throw new InvalidOperationException($"FileHeader{FileHeader}, BatchHeader{Batches[0].HeaderRecord}, BatchControl{Batches[0].ControlRecord}," +
                $" and FileControl{FileControl} must be populated before generating the NACHA file.");
        }

        using (var writer = new StreamWriter(fullPath))
        {
            _logger.LogTrace("Generating NACHA file... {0}", filePath + fileName);
             int entryAndAddendumCount = 0;
            foreach (var batch in Batches)
            {
                _logger.LogTrace("Batch and Entry Records generated for BatchNumber {0}.", batch.HeaderRecord.BatchNumber);
              entryAndAddendumCount += batch.ControlRecord.EntryAndAddendumCount;
            }
            string nachaFileString = this.ToStringValue();
            if (nachaFileString.Contains("ERROR"))
            {
                throw new InvalidOperationException(nachaFileString);
            }

            writer.Write(nachaFileString);
            _logger.LogTrace("File Control Record generated.");
            writer.Write(GetFileNinePad(entryAndAddendumCount, Batches.Count, _logger)); // Pad the file to 10 records 
            _logger.LogTrace("File padded to 10 records. entryAndAddendumCount {0} batchCount {1}", entryAndAddendumCount, Batches.Count);
        }


    }
    public void PopulateTestData()
    {
        BatchHeaderRecord batchHeader = new BatchHeaderRecord
        (
            FileHeader.ImmediateOriginName,
            "01",
            FileHeader.ImmediateOrigin,
            "Payments",
            DateTime.Now.AddDays(2), // Company Descriptive Date
            DateTime.Now.AddDays(2), // Effective Entry Date
            FileHeader.ImmediateDestinationRoutingNumber, // Originating DFI
            1 // Batch number
        );
        BatchControlRecord batchControl = new BatchControlRecord
        (
            0,
            BatchControlRecord.CalculateEntryHash(new List<string> { "01100001", "01100002", "01100003" }),
            0.00m,
            0.00m,
            "",
            "",
            new DFINumber("99999999"),
            1
        );
        Batch firstBatch = new Batch(batchHeader, batchControl);
        List<EntryDetailRecord> entryDetailRecords = firstBatch.EntryDetailRecords;
        entryDetailRecords.Add(new EntryDetailRecord
        (
            TransactionCode.DebitChecking,  // Checking account debit
            new DFINumber("01100001"),
            DFINumber.CalculateCheckDigit("01100001").ToString(),
            "123456789",
            500.06m,  // $500.00
            "123456789",
            "John Doe",
            FileHeader.ImmediateDestinationRoutingNumber.CHARACTERS+"0000001"
        ));
        entryDetailRecords[0].EntryAddendumRecord = new EntryAddendumRecord
        (
            "Payment for invoice 12345",
            entryDetailRecords[0].TraceNumber.Substring(8, 7)

        );

        entryDetailRecords.Add(new EntryDetailRecord
        (
            TransactionCode.DepositChecking,  // Checking account credit
            new DFINumber("01100002"),
            DFINumber.CalculateCheckDigit("01100002").ToString(),
            "123456789",
            200.02m,  // $500.00
            "Jane Doe",
            "123456789",
            FileHeader.ImmediateDestinationRoutingNumber.CHARACTERS+"0000002"
        ));
        entryDetailRecords[1].EntryAddendumRecord = new EntryAddendumRecord
        (
            "Payment for invoice 12346",
            entryDetailRecords[1].TraceNumber.Substring(8, 7)

        );
        entryDetailRecords.Add(new EntryDetailRecord
        (
            TransactionCode.DepositChecking,  // Checking account credit
            new DFINumber("01100003"),
            DFINumber.CalculateCheckDigit("01100003").ToString(),
            "123456789",
            300.04m,  // $500.00
            "Chris Doe",
            "123456789",
            FileHeader.ImmediateDestinationRoutingNumber.CHARACTERS+"0000003"
        ));

        Batches.Add(firstBatch);
        foreach (var batch in Batches)
        {
            batch.CalculateControlRecord();
            _logger.LogTrace("Batch {0} populated with {1} EntryDetailRecords.", batch.HeaderRecord.BatchNumber, batch.EntryDetailRecords.Count);
            _logger.LogTrace("Control Records EntryAndAddendum Count {0} EntryHash {1} Credits {2} Debits {3}.", batch.ControlRecord.EntryAndAddendumCount,batch.ControlRecord.EntryHash, batch.ControlRecord.TotalCreditAmount, batch.ControlRecord.TotalDebitAmount);
        }
        CalculateFileControl();

    }
    /* The file must be a multiple of 10 records, so this method pads the file with 9s to reach that multiple
        entryRecordCount should be EntryDetailRecordCount + EntryAddendumRecordCount */
    public static string GetFileNinePad(int entryRecordCount, int batchCount, ILogger  logger)
    {
        logger.LogTrace("GetFileNinePad EntryRecordCount {0} BatchCount {1}", entryRecordCount, batchCount);
        string results = "";
        int fileRecordCount = 2; // File Header and File Control
        int batchesCount = 2 * batchCount ; // Batch Header and Batch Control
        int totalCount = entryRecordCount + fileRecordCount + batchesCount;
        int padsNeeded = 10 - (totalCount % 10);
        if (padsNeeded == 10) return results; // No padding needed
        for (int i = 1; i < padsNeeded - 1; i++)
        {
            results += "".PadRight(94, '9') + Environment.NewLine;
        }
        results += "".PadRight(94, '9');
        return results;
    }
}
