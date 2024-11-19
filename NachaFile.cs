using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;

namespace NachaSharp;
public class NachaFile
{
    private readonly ILogger<NachaFile> _logger;

    public required FileHeaderRecord FileHeader;
    public List<Batch> Batches { get; } = new List<Batch>();
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
        StringBuilder batchesString = new StringBuilder();
        int entryAndAddendumCount = 0;
        foreach (var batch in Batches)
        {
            batchesString.Append( batch.GenerateRecord());
            entryAndAddendumCount += batch.ControlRecord.EntryAndAddendumCount;
        }
        return string.Concat(
            FileHeader.GenerateRecord(), 
            batchesString.ToString(), 
            FileControl.GenerateRecord(),  
            FileControlRecord.GetFileNinePad(entryAndAddendumCount, Batches.Count)
        );
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
            writer.Write(FileHeader.GenerateRecord() + Environment.NewLine);
            _logger.LogTrace("File Header Record generated.");

             int entryAndAddendumCount = 0;
            foreach (var batch in Batches)
            {
                writer.Write(batch.GenerateRecord());
                _logger.LogTrace("Batch and Entry Records generated for BatchNumber {0}.", batch.HeaderRecord.BatchNumber);
              entryAndAddendumCount += batch.ControlRecord.EntryAndAddendumCount;
            }

            writer.Write(FileControl.GenerateRecord() + Environment.NewLine);
            _logger.LogTrace("File Control Record generated.");
            writer.Write(FileControlRecord.GetFileNinePad(entryAndAddendumCount, Batches.Count  )); // Pad the file to 10 characters
            _logger.LogTrace("File padded to 10 characters. entryAndAddendumCount {0} batchCount {1}", entryAndAddendumCount, Batches.Count);
        }


    }
    public void PopulateTestData()
    {
        FileHeader = new FileHeaderRecord
        (
           "DEST BANK",
            "123456789",
            "MY COMPANY",
            "987654321",
            "88888888"

        );

        BatchHeaderRecord batchHeader = new BatchHeaderRecord
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
        List<EntryDetailRecord> entryDetailRecords = new List<EntryDetailRecord>();

        entryDetailRecords.Add(new EntryDetailRecord
        (
            TransactionCode.DebitChecking,  // Checking account debit
            "01100001",
            "1",
            "123456789",
            500.06m,  // $500.00
            "123456789",
            "John Doe",
            "123456789000001"
        ));
        entryDetailRecords[0].EntryAddendumRecord = new EntryAddendumRecord
        (
            "Payment for invoice 12345",
            "9000001"

        );

        entryDetailRecords.Add(new EntryDetailRecord
        (
            TransactionCode.DepositChecking,  // Checking account credit
            "01100002",
            "2",
            "123456789",
            200.02m,  // $500.00
            "Jane Doe",
            "123456789",
            "123456789000002"
        ));
        entryDetailRecords[1].EntryAddendumRecord = new EntryAddendumRecord
        (
            "Payment for invoice 12346",
            "9000002"

        );
        entryDetailRecords.Add(new EntryDetailRecord
        (
            TransactionCode.DepositChecking,  // Checking account credit
            "01100003",
            "3",
            "123456789",
            300.04m,  // $500.00
            "Chris Doe",
            "123456789",
            "123456789000003"
        ));

        BatchControlRecord batchControl = new BatchControlRecord
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
        Batches.Add(new Batch(batchHeader, batchControl, entryDetailRecords));

        FileControl = new FileControlRecord(

            1,
            1,
            5,
            FileControlRecord.CalculateEntryHash(new List<string> { "011000011", "051000011", "061000011" }),
            1000.00m,
            1000.00m

        );
    }
}
