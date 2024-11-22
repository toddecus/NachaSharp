using System.Diagnostics.CodeAnalysis;

namespace NachaSharp
{
    public class ACHRoutingNumber : DFINumber
    {
        public string CheckDigit { get;} 
        
        [SetsRequiredMembers]
        public ACHRoutingNumber(string number) : base(number.Substring(0, 8))
        {
            if (number.Length != 9)
            {
                throw new ArgumentException("ACH Routing Number must be a 9-digit number.");
            }
            if (VerifyRoutingString(number) == false)
            {
                throw new ArgumentException("ACH Routing Number must be a valid routing number.");
            }
            CheckDigit = number[8].ToString();
        }
        /*
        public static string GetValidRoutingString(string destination)
        {
            if (string.IsNullOrWhiteSpace(destination))
            {
                throw new ArgumentNullException(nameof(destination), "Can't get a padded routing string from a null or empty string");
            }
            if (destination.Length == 10 && validateDestination(destination))
            {
                return destination;
            }
            else if (destination.Length == 9 && validateDestination(destination))
            {
                return destination.PadLeft(10, ' ');
            }
            else if (destination.Length == 8 && validateDestination(destination))
            {
                destination = destination + EntryDetailRecord.CalculateCheckDigit(destination);
                return destination.PadLeft(10, ' ');
            }
            else if (destination.Length <= 7 || destination.Length > 10)
            {
                throw new ArgumentException("Routing String must be a non-empty string with at least numeric 8 characters, or 9 with checkdigit, or 10 with space prepended.");
            }
            else
            {
                throw new ArgumentException("This should be a valid routing number. : " + destination);
            }
        }
        */
        /*
        public static bool validateDestination(string destination)
        {
        if (string.IsNullOrWhiteSpace(destination) || destination.Length < 9)
        {
            return false;
        }
        else if (destination.Length == 10 && destination[0] == ' ' && validateDestination(destination.Substring(1, 8))
                    && destination[9] == DFINumber.CalculateCheckDigit(destination.Substring(1, 8))[0])
        {
            return true;
        }
        else if (destination.Length == 9 && validateDestination(destination.Substring(0, 8)) && destination[8] == EntryDetailRecord.CalculateCheckDigit(destination.Substring(0, 8))[0])
        {
            return true;
        }
        else if (destination.Length == 8)
        {
            string pattern = @"^\d{8}$"; // Example pattern: 8-digit number
            return Regex.IsMatch(destination, pattern);
        }
        else
        {
            return false;
        }
        }
        */
        public static bool VerifyRoutingString(string routingString)
        {
            if (routingString.Length != 9 || !ulong.TryParse(routingString.Substring(0, 9), out _))
            {
                return false;
            }
            else if (routingString[8] != (char)('0'+ CalculateCheckDigit(routingString.Substring(0, 8))))
            {
                return false;
            }
            return true;

        }
        public string toString()
        {
            return CHARACTERS + CheckDigit;
        }
        public string getDFI()
        {
            return CHARACTERS;
        }
    }
}