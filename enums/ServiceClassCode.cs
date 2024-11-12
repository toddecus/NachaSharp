namespace NachaSharp
{
    public enum ServiceClassCode
    {
        MixedDebitsAndCredits = 200,
        CreditsOnly = 220,
        DebitsOnly = 225
    }

    public static class ServiceClassCodeExtensions
    {
        public static string ToStringValue(this ServiceClassCode serviceClassCode)
        {
            return serviceClassCode switch
            {
                ServiceClassCode.MixedDebitsAndCredits => "200",
                ServiceClassCode.CreditsOnly => "220",
                ServiceClassCode.DebitsOnly => "225",
                _ => "200" // Default to 200 - ACH Entries Mixed Debits and Credits
            };
        }
    }
}