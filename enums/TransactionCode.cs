namespace NachaSharp
{
    public enum TransactionCode
    {
        DepositChecking = 22,
        PrenotificationCheckingCredit = 23,
        ZeroDollarRemittanceCheckingCredit = 24,
        DebitChecking = 27,
        PrenotificationCheckingDebit = 28,
        ZeroDollarRemittanceCheckingDebit = 29,
        DepositSavings = 32,
        PrenotificationSavingsCredit = 33,
        ZeroDollarRemittanceSavingsCredit = 34,
        DebitSavings = 37,
        PrenotificationSavingsDebit = 38,
        ZeroDollarRemittanceSavingsDebit = 39
    }

    public static class TransactionCodeExtensions
    {
        public static string ToStringValue(this TransactionCode transactionCode)
        {
            return ((int)transactionCode).ToString("D2");
        }
    }
}