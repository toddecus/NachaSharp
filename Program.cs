namespace NachaSharp;
using System;
using System.IO;
using Microsoft.Extensions.Logging;
public class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Starting NACHA file generation...");
        using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Information);
            });
        var logger = loggerFactory.CreateLogger<NachaSharp.NachaFileGenerator>();

        try
        {
            // Instantiate the NACHA file generator
            var nachaFileGenerator = new NachaFileGenerator(logger);

            // Generate the NACHA file
            nachaFileGenerator.PopulateTestData();
            Console.WriteLine("Test data populated!");
        
            nachaFileGenerator.GenerateNachaFile();


            Console.WriteLine("NACHA file generated successfully, look for a nacha.txt!");
        }
        catch (Exception ex)
        {
            // Catch any errors during file generation
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}

