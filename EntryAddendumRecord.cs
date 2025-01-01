using System;
using System.Diagnostics.CodeAnalysis;

namespace NachaSharp;
public class EntryAddendumRecord
{
    public RecordTypeCode RecordTypeCode = RecordTypeCode.Addendum; // Fixed value for Addendum
    public AddendumTypeCode AddendumTypeCode = AddendumTypeCode.AdditionalPaymentInformation; // 05 for CCD, CTX, PPD entries
    private string _paymentRelatedInformation = string.Empty;
    public required string PaymentRelatedInformation
    {
        get => _paymentRelatedInformation;
        set
        {
            if (value == null)
            {
                value = string.Empty;
            }
            if (value.Length > 80)
            {
                throw new ArgumentException("Payment related information cannot exceed 80 characters.", nameof(PaymentRelatedInformation));
            }
            _paymentRelatedInformation = value;
        }
    }
    public readonly int AddendaSequenceNumber = 1; // Sequence within the entry Not supporting multiple addendum lines
    public required string EntryDetailSequenceNumber;  // Last 7 digits of Entry Detail’s trace number

    [SetsRequiredMembers]
    public EntryAddendumRecord(string paymentRelatedInformation, string entryDetailSequenceNumber)
    {
        // Validate paymentRelatedInformation
        if (paymentRelatedInformation == null)
        {
            paymentRelatedInformation = "";
        }
        if (paymentRelatedInformation.Length > 80)
        {
            throw new ArgumentException("Payment related information cannot exceed 80 characters.", nameof(paymentRelatedInformation));
        }

        // Validate entryDetailSequenceNumber
        if (string.IsNullOrWhiteSpace(entryDetailSequenceNumber))
        {
            throw new ArgumentException("Entry detail sequence number cannot be null or empty it should be the last 7 characters of the EntryDetail TraceNumber.", nameof(entryDetailSequenceNumber));
        }
        if (entryDetailSequenceNumber.Length > 7)
        {
            throw new ArgumentException("Entry detail sequence number must be 7 characters or less.", nameof(entryDetailSequenceNumber));
        }

        PaymentRelatedInformation = paymentRelatedInformation;
        EntryDetailSequenceNumber = entryDetailSequenceNumber;
    }
    public string GenerateRecord()
    {
        return string.Concat(
            RecordTypeCode.ToStringValue(),
            AddendumTypeCode.ToStringValue(),
            PaymentRelatedInformation.PadRight(80),
            AddendaSequenceNumber.ToString().PadLeft(4, '0'),
            EntryDetailSequenceNumber.PadLeft(7, '0')
        );
    }
}