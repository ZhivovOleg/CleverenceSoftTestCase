namespace CleverenceSoftTestCase.Tests;

public class FormatterTests
{
    public void TestFormatting()
    {
        // Arrange
        string inputVarOne = "10.03.2025 15:14:49.523 INFORMATION Версия программы: '3.4.0.48729'";
        string inputVarTwo = "2025-03-10 15:14:51.5882| INFO|11|MobileComputer.GetDeviceId| Код устройства: '@MINDEO-M40-D-410244015546'";

        LogFormatter formatter = new LogFormatter();

        // Act
        string? result1 = formatter.FormatRecord(inputVarOne);
        string? result2 = formatter.FormatRecord(inputVarTwo);

        // Assert
        if (result1 != "10-03-2025\t15:14:49.523\tINFO\t\tВерсия программы: '3.4.0.48729'")
            throw new Exception("Неверное форматирование");
        if (result2 != "10-03-2025\t15:14:51.5882\tINFO\tMobileComputer.GetDeviceId\tКод устройства: '@MINDEO-M40-D-410244015546'")
            throw new Exception("Неверное форматирование");
    }

    public async Task TestLineByLineReading()
    {
        // Arrange
        string inputFileName = "./ReadTest.log";
        string[] lines =
        {
            "10.03.2025 15:14:49.523 INFORMATION Версия программы: '3.4.0.48729'",
            "",
            "2025-03-10 15:14:51.5882| INFO|11|MobileComputer.GetDeviceId| Код устройства: '@MINDEO-M40-D-410244015546'"
        };
        await File.WriteAllLinesAsync(inputFileName, lines);

        LogFormatter formatter = new();
        List<string?> result = new();

        // Act
        await foreach (string line in formatter.ReadFileLineByLine(inputFileName))
            result.Add(line);

        try
        {
            if (result.Count != 3)
                throw new Exception("Неверный размер итогового массива");
            for (int i = 0; i < 3; i++)
                if (result[i] != lines[i])
                    throw new Exception("Некорректное чтение");
        }
        finally
        {
            File.Delete(inputFileName);
        }
    }
}