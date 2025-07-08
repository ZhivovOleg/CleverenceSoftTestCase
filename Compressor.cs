using System.Globalization;
using System.Text;

namespace CleverenceSoftTestCase;

/// <summary>
/// Дана строка, содержащая n маленьких букв латинского алфавита.
/// Требуется реализовать алгоритм компрессии этой строки, замещающий группы последовательно идущих одинаковых букв формой "sc" (где "s" – символ, "с" – количество букв в группе)
/// , а также алгоритм декомпрессии, возвращающий исходную строку по сжатой.
/// Если буква в группе всего одна – количество в сжатой строке не указываем, а пишем её как есть.
/// </summary>
public class Compressor
{
    /// <summary>
    /// Сжатие
    /// </summary>
    /// <param name="input">Исходная строка</param>
    /// <returns>Сжатая строка</returns>
    public string Compress(string input)
    {
        if (string.IsNullOrEmpty(input))
            return "";

        StringBuilder sb = new();
        int i = 0;
        while (i < input.Length)
        {
            int j = 1;
            char c = input[i++];
            sb.Append(c);
            while (i < input.Length && c == input[i])
            {
                i++;
                j++;
            }
            if (j > 1)
            {
                sb.Append(j);
            }
        }
        return sb.ToString();
    }


    /// <summary>
    /// Разархивирование
    /// </summary>
    /// <param name="input">Исходная строка</param>
    /// <returns>Разархивированная строка</returns>
    public string Decompress(string input)
    {
        if (string.IsNullOrEmpty(input))
            return "";

        StringBuilder sb = new();
        int i = 0;

        while (i < input.Length)
        {
            char c = input[i];
            sb.Append(c);
            i++;
            if (i < input.Length && char.IsNumber(input[i]))
            {
                var num = char.GetNumericValue(input[i++]);
                for (int j = 1; j < num; j++)
                    sb.Append(c);
            }
        }
        return sb.ToString();
    }
}