using System.Diagnostics.CodeAnalysis;

namespace NachaSharp
{
    public class DFINumber
    {
        public required string CHARACTERS { get; set; } // 8 digits

        [SetsRequiredMembers]
        public DFINumber(string dfiNumber)
        {
            if (dfiNumber.Length != 8 || !ulong.TryParse(dfiNumber, out _))
            {
                throw new ArgumentException("DFI number must be an 8-digit number.");
            }
            CHARACTERS = dfiNumber;
        }

        [SetsRequiredMembers]
        public DFINumber(int dfiNumber)
        {
            if (dfiNumber < 0 || dfiNumber > 99999999)
            {
                throw new ArgumentException("DFI number must be an 8-digit number.");
            }
            CHARACTERS = dfiNumber.ToString().PadLeft(8, '0');
        }

        public static int CalculateCheckDigit(string dFINumberString)
        {
            if (string.IsNullOrWhiteSpace(dFINumberString) || dFINumberString.Length != 8)
            {
                throw new ArgumentException("Routing number must be 8 digits");
            }
            int[] weights = { 3, 7, 1, 3, 7, 1, 3, 7 };
            int weightedSum = 0;

            for (int i = 0; i < 8; i++)
            {
                int digit = int.Parse(dFINumberString[i].ToString());
                weightedSum += digit * weights[i];
            }

            int remainder = weightedSum % 10;
            int temp = (10 - remainder) % 10;
            return temp;
        }
        public static int CalculateCheckDigit(DFINumber dfiNumber)
        {
            return DFINumber.CalculateCheckDigit(dfiNumber.ToString());
        }


        public override string ToString()
        {
            return CHARACTERS;
        }

        public string ToRoutingString()
        {
            return CHARACTERS + CalculateCheckDigit(CHARACTERS);
        }
    }

}