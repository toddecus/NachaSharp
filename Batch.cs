using System;
using System.Collections.Generic;

namespace NachaSharp
{
    public class Batch
    {
        public BatchHeaderRecord HeaderRecord { get; set; }
        public BatchControlRecord ControlRecord { get; set; }
        public List<EntryDetailRecord> EntryDetailRecords { get; set; } = new List<EntryDetailRecord>();

        public Batch(BatchHeaderRecord headerRecord, BatchControlRecord controlRecord, List<EntryDetailRecord> entryDetailRecords)
        {
            HeaderRecord = headerRecord;
            ControlRecord = controlRecord;
            EntryDetailRecords = entryDetailRecords;
        }

        public string GenerateRecord()
        {
            var batchString = HeaderRecord.GenerateRecord() + Environment.NewLine;

            foreach (var entry in EntryDetailRecords)
            {
                batchString += entry.GenerateRecord() + Environment.NewLine;
                if(entry.EntryAddendumRecord != null)
                {
                    batchString += entry.EntryAddendumRecord.GenerateRecord() + Environment.NewLine;
                }   
            }

            batchString += ControlRecord.GenerateRecord() + Environment.NewLine;
            return batchString;
        }
    }
}