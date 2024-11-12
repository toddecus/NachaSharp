namespace NachaSharp
{
    public enum StandardEntryClassCode
    {
        PrearrangedPaymentsAndDeposits = 1,
        CashConcentrationAndDisbursement = 2,
        CorporateTradeExchange = 3,
        TelephoneInitiated = 4,
        InternetInitiated = 5
    }

    public static class StandardEntryClassCodeExtensions
    {
        public static string ToStringValue(this StandardEntryClassCode entryClassCode)
        {
            return entryClassCode switch
            {
                StandardEntryClassCode.PrearrangedPaymentsAndDeposits => "PPD",
                StandardEntryClassCode.CashConcentrationAndDisbursement => "CCD",
                StandardEntryClassCode.CorporateTradeExchange => "CTX",
                StandardEntryClassCode.TelephoneInitiated => "TEL",
                StandardEntryClassCode.InternetInitiated => "WEB",
                _ => "PPD" // Default to PPD - Prearranged Payments and Deposits
            };
        }
    }
}
