namespace NachaSharp 
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting NACHA file generation...");

            try
            {
                // Instantiate the NACHA file generator
                var nachaFileGenerator = new NachaFileGenerator();

                // Generate the NACHA file
                nachaFileGenerator.GenerateNachaFile();


                Console.WriteLine("NACHA file generated successfully!");
            }
            catch (Exception ex)
            {
                // Catch any errors during file generation
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            // Wait for user input to close console
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
