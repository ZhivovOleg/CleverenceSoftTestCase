using CleverenceSoftTestCase.Tests;

namespace CleverenceSoftTestCase;

internal class Program
{
    private static async Task Main(string[] args)
    {
        await DoTests();
        Console.WriteLine("All test success");
        Console.WriteLine("Please, write log filename");
        string? fileName = Console.ReadLine();

        if (string.IsNullOrEmpty(fileName))
            throw new ArgumentNullException("Empty file name is not allowed");

        LogFormatter formatter = new LogFormatter();
        string[] lines = await formatter.ReadFileAsync(fileName);

        using (FileStream fs = File.OpenWrite($"./{DateTime.UtcNow.ToString("dd-MM-yyyy_HH:mm")}.log"))
        using (StreamWriter sw = new StreamWriter(fs))
        {
            foreach (string ln in lines)
            {
                await sw.WriteLineAsync(formatter.FormatRecord(ln));
            }
        }

        Console.ReadKey();
    }

    private static async Task DoTests()
    {
        CompressionTests compressionTester = new();
        compressionTester.Compress_Simple_Success();
        compressionTester.Decompress_Simple_Success();

        FormatterTests formatterTests = new();
        formatterTests.TestFormatting();
        await formatterTests.TestLineByLineReading();
    }
}