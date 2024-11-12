namespace NachaSharp
{
    public enum AddendumTypeCode
    {
        TerminalLocationInformation = 2, //Used for the POS, MTE and SHR Standard Entry Class Codes
        AdditionalPaymentInformation = 5, //Used for the CCD, CTX and PPD Standard Entry Class Codes
        NotificationOfChange = 98, //Used for the Notification of Change Entries 
        ReturnEntries = 99 //Used for Return Entries
    }

    public static class AddendumTypeCodeExtensions
    {
        public static string ToStringValue(this AddendumTypeCode addendumTypeCode)
        {
            return addendumTypeCode switch
            {
                AddendumTypeCode.TerminalLocationInformation => "02",
                AddendumTypeCode.AdditionalPaymentInformation => "05",
                AddendumTypeCode.NotificationOfChange => "98",
                AddendumTypeCode.ReturnEntries => "99",
                _ => "05" // Default to Additional Payment Information
            };
        }
    }
}