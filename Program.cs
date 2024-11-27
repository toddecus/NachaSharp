namespace NachaSharp;
using System;
using System.IO;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
public class Program
{
    static void Main(string[] args)
    {
        using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Trace);
            });
        var logger = loggerFactory.CreateLogger<NachaSharp.NachaFile>();
        logger.LogTrace("Starting NACHA file main test file generation...");

        try
        {
            string filePath = "./outputFiles/";
            string fileName = "nacha.txt";
            string fullPath = Path.Combine(filePath, fileName);

            // Check if the file exists and delete it if it does
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                Console.WriteLine($"Deleted existing file: {fullPath}");
            }
            string YourBankName = "Your Bank Name"; //23 chars max
            string YourBankRoutingNumber = "999999992"; // DFI="99999999" CheckDigit="2" 9 digits
            string YourCompanyName = "My Company";// 23 chars max
            string BankAssignedYourCompanyACHID = "123456789"; //max 9 chars relates to your bank account number maybe TAXID
            string YourSystemReference = "1234ABCD";// 8 chars for your to reference this file in 
            // Instantiate the NACHA file generator
            //var nachaFile = new NachaFile( new FileHeaderRecord("La Salle Bank N.A.", new ACHRoutingNumber("071000505"), "YourCompany Name", "072000805", "12345678"), logger);
            var nachaFile = new NachaFile(
                new FileHeaderRecord(YourBankName,
                new ACHRoutingNumber(YourBankRoutingNumber),
                YourCompanyName,
                BankAssignedYourCompanyACHID,
                YourSystemReference),
                logger); 

            // Generate the test NACHA file entry data
            nachaFile.PopulateTestBatchAndEntryData();
            nachaFile.CalculateFileControl();
            logger.LogTrace("Test data populated and fileControl Calculated!");
        
            // Generate the NACHA file in outputFiles folder NachaSharp/outputFiles/nacha.txt
            nachaFile.GenerateNachaFile( fullPath);
            logger.LogTrace("NACHA file generated successfully, look for a {0}", filePath + fileName);
            logger.LogTrace("The file should look like this:"+ Environment.NewLine+"{0}",nachaFile.ToStringValue());
        }
        catch (Exception ex)
        {
            // Catch any errors during test file generation
            logger.LogError($"An error occurred: {ex.Message}");
        }
    }
}

