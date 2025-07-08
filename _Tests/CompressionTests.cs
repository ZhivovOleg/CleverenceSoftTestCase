namespace CleverenceSoftTestCase.Tests;

public class CompressionTests
{
    public void Compress_Simple_Success()
    {
        // Arrange
        string testString0 = null;
        string testString1 = "aaaabbb";
        string testString2 = "";
        string testString3 = "abbcccddddeeeee";


        Compressor compressor = new();

        // Act
        string result0 = compressor.Compress(testString0);
        string result1 = compressor.Compress(testString1);
        string result2 = compressor.Compress(testString2);
        string result3 = compressor.Compress(testString3);

        // Assert
        if (result0 != "")
            throw new Exception($"Сжатие не удалось: {result0}");
        if (result1 != "a4b3")
            throw new Exception($"Сжатие не удалось: {result1}");
        if (result2 != "")
            throw new Exception($"Сжатие не удалось: {result2}");
        if (result3 != "ab2c3d4e5")
            throw new Exception($"Сжатие не удалось: {result3}");
    }

    public void Decompress_Simple_Success()
    {
        // Arrange
        string testString0 = null;
        string testString1 = "";
        string testString2 = "a4b3";
        string testString3 = "ab2c3d4e5";
        Compressor compressor = new();

        // Act
        string result0 = compressor.Decompress(testString0);
        string result1 = compressor.Decompress(testString1);
        string result2 = compressor.Decompress(testString2);
        string result3 = compressor.Decompress(testString3);

        // Assert
        if (result0 != "")
            throw new Exception($"Распаковка не удалась: {result0}");
        if (result1 != "")
            throw new Exception($"Распаковка не удалась: {result1}");
        if (result2 != "aaaabbb")
            throw new Exception($"Распаковка не удалась: {result2}");
        if (result3 != "abbcccddddeeeee")
            throw new Exception($"Распаковка не удалась: {result3}");
    }
}