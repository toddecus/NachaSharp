using System;
using System.Collections.Generic;
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

    public FileHeaderRecord? fileHeader;
    public BatchHeaderRecord? batchHeader;
    public List<EntryDetailRecord> entryDetailRecords = new List<EntryDetailRecord>();
    public BatchControlRecord? batchControl;
    public FileControlRecord? fileControl;
    public void GenerateNachaFile()
    {
        string fullPath = Path.Combine(filePath, fileName);

        _logger.LogTrace($"FileHeader{fileHeader}, BatchHeader{batchHeader}, BatchControl{batchControl}," +
            $" and FileControl{fileControl} must be populated before generating the NACHA file.");
        if (fileHeader == null || batchHeader == null || batchControl == null || fileControl == null)
        {
            throw new InvalidOperationException($"FileHeader{fileHeader}, BatchHeader{batchHeader}, BatchControl{batchControl}," +
                $" and FileControl{fileControl} must be populated before generating the NACHA file.");
        }

        using (var writer = new StreamWriter(fullPath))
        {
            _logger.LogTrace("Generating NACHA file... {0}", filePath + fileName);
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
            _logger.LogTrace("File padded to 10 characters. EntryDetailRecords.Count {0} EntryAddendumRecordsCount {1}", entryDetailRecords.Count, countEntryAddendumRecords);
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
