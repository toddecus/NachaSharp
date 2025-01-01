using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;

namespace NachaSharp
{
    public class Batch
    {
        int _nextTransactionNumber = 1; // keep track of transactions numbers in the batch increment when used as part of tracenumber
        public int NextTransactionNumber
        {
            get
            {
                return _nextTransactionNumber++;
            }
        }
        public required BatchHeaderRecord HeaderRecord { get; set; }
        public required BatchControlRecord ControlRecord { get; set; }
        public required List<EntryDetailRecord> EntryDetailRecords { get; set; } = new List<EntryDetailRecord>();

        [SetsRequiredMembers]
        public Batch(BatchHeaderRecord headerRecord, BatchControlRecord controlRecord)
        {
            HeaderRecord = headerRecord;
            ControlRecord = controlRecord;
        }
        public void CalculateControlRecord()
        {
            if (ControlRecord == null)
            {
                if (HeaderRecord == null)
                {
                    throw new ArgumentNullException(nameof(HeaderRecord), "Can't calculate control record without a header record");
                }
                ControlRecord = new BatchControlRecord(0, "", 0, 0, "", "", new DFINumber("99999999"), 0);
            }
            List<string> routingNumbers = new List<string>();
            int entryCounter = 0;
            int addendumCounter = 0;
            ControlRecord.TotalCreditAmount = 0.0m;
            ControlRecord.TotalDebitAmount = 0.0m;
            foreach (var entry in EntryDetailRecords)
            {
                if (entry.TransactionCode == TransactionCode.DebitChecking || entry.TransactionCode == TransactionCode.DebitSavings)
                {
                    ControlRecord.TotalDebitAmount += entry.Amount;
                }
                else if (entry.TransactionCode == TransactionCode.DepositChecking || entry.TransactionCode == TransactionCode.DepositSavings)
                {
                    ControlRecord.TotalCreditAmount += entry.Amount;
                }
                entryCounter++;
                if (entry.EntryAddendumRecord != null)
                {
                    addendumCounter++;
                }
                ControlRecord.EntryAndAddendumCount = entryCounter + addendumCounter;
                routingNumbers.Add(entry.ReceivingDFI.ToString());

            }
            ControlRecord.EntryHash = BatchControlRecord.CalculateEntryHash(routingNumbers);
            ControlRecord.OriginatingDFI = HeaderRecord.OriginatingDFI;
            ControlRecord.BatchNumber = HeaderRecord.BatchNumber;
            ControlRecord.CompanyIdentification = HeaderRecord.CompanyIdentification;

        }

        public string GenerateRecord()
        {
            if (EntryDetailRecords.Count == 0)
            {
                return "ERROR: Invalid Batch No entries to generate.";
            }
            var batchString = HeaderRecord.GenerateRecord() + Environment.NewLine;

            foreach (var entry in EntryDetailRecords)
            {
                batchString += entry.GenerateRecord() + Environment.NewLine;
                if (entry.EntryAddendumRecord != null)
                {
                    batchString += entry.EntryAddendumRecord.GenerateRecord() + Environment.NewLine;
                }
            }

            batchString += ControlRecord.GenerateRecord() + Environment.NewLine;
            return batchString;
        }
    }
}