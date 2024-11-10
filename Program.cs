namespace NachaSharp;
using System;
using System.IO;
using Microsoft.Extensions.Logging;
public class Program
{
    static void Main(string[] args)
    {
        using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Trace);
            });
        var logger = loggerFactory.CreateLogger<NachaSharp.NachaFileGenerator>();
        logger.LogTrace("Starting NACHA file generation...");

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
            var nachaFileGenerator = new NachaFileGenerator(logger);

            // Generate the NACHA file
            nachaFileGenerator.PopulateTestData();
            logger.LogTrace("Test data populated!");
        
            nachaFileGenerator.GenerateNachaFile();


            logger.LogTrace("NACHA file generated successfully, look for a nacha.txt!");
        }
        catch (Exception ex)
        {
            // Catch any errors during test file generation
            logger.LogError($"An error occurred: {ex.Message}");
        }
    }
}

