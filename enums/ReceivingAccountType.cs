namespace NachaSharp;
public enum ReceivingAccountType {
    checking = 1,
    savings = 2,
    generalledger = 3,
    loan = 4
  
}

public static class ReceivingAccountTypeExtensions
{
    public static string ToStringValue(this ReceivingAccountType receivingAccountType)
    {
        return receivingAccountType switch
        {
            ReceivingAccountType.checking => "DDA",
            ReceivingAccountType.savings => "SAV",
            ReceivingAccountType.generalledger => "GL",
            ReceivingAccountType.loan => "LOAN",
            _ => throw new ArgumentOutOfRangeException(nameof(receivingAccountType), receivingAccountType, "Somehow tyring to ToStringValue an unknown enum value on ReceivingAccountType.")
        };
    }
}