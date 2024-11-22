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
            // Instantiate the NACHA file generator
            var nachaFileGenerator = new NachaFile(
                new FileHeaderRecord("La Salle Bank N.A.",new ACHRoutingNumber("071000505"), "YourCompany Name","072000805","12345678"),
                new FileControlRecord(0, 0, 0, "", 0.00m , 0.00m), 
                logger); 

            // Generate the test NACHA file
            nachaFileGenerator.PopulateTestData();
            logger.LogTrace("Test data populated!");
        
            nachaFileGenerator.GenerateTestNachaFile();
            logger.LogTrace("NACHA file generated successfully, look for a {0}", filePath + fileName);
            logger.LogTrace("The file should look like this:"+ Environment.NewLine+"{0}",nachaFileGenerator.ToStringValue());
            logger.LogTrace("vi {0}", filePath + fileName);
        }
        catch (Exception ex)
        {
            // Catch any errors during test file generation
            logger.LogError($"An error occurred: {ex.Message}");
        }
    }
}

