using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace CleverenceSoftTestCase;

/// <summary>
/// Сервис форматирования лог-файлов
/// </summary>
public class LogFormatter
{
    /// <summary>
    /// Асинхронное чтение содержимого файла.
    /// Недостаток - весь файл читается в память
    /// </summary>
    /// <param name="fileName">Полное имя файла</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Таск, содержащий список записей</returns>
    /// <exception cref="FileNotFoundException">Файл не найден</exception>
    /// <exception cref="OperationCanceledException">Операция отменена</exception>
    public async Task<string[]> ReadFileAsync(string fileName, CancellationToken cancellationToken = default)
    {
        if (!File.Exists(fileName))
            throw new FileNotFoundException($"Файл не найден: {fileName}");

        return await File.ReadAllLinesAsync(fileName, cancellationToken);
    }

    /// <summary>
    /// Асинхронное чтение содержимого файла по одной строке
    /// </summary>
    /// <param name="fileName">Полное имя файла</param>
    /// <param name="cancellationToken"> Токен отмены</param>
    /// <returns>Одна строка за один вызов</returns>
    /// <exception cref="FileNotFoundException">Файл не найден</exception>
    /// <exception cref="OperationCanceledException">Операция отменена</exception>
    public async IAsyncEnumerable<string?> ReadFileLineByLine(
        string fileName,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (!File.Exists(fileName))
            throw new FileNotFoundException($"Файл не найден: {fileName}");
        using (StreamReader sr = File.OpenText(fileName))
            while (!sr.EndOfStream)
                yield return await sr.ReadLineAsync(cancellationToken);
    }

    /// <summary>
    /// Форматирует входную строку в соответствии с требованиями.
    /// Сюда еще я бы добавил список разделителей во входящем форматировании, но в ТЗ этого нет, поэтому опустил
    /// </summary>
    /// <param name="record">Исходная строка лога</param>
    /// <returns>Отформатированная строка</returns>
    public string? FormatRecord(string record)
    {
        if (string.IsNullOrEmpty(record))
            return null;

        bool type1 = false; // задаем тип записи для упрощения парсинга далее
        StringBuilder sb = new();

        // Парсим даты
        string date = record[0..10];

        if (DateOnly.TryParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var d1))
        {
            sb.Append(d1.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture));
            type1 = true;
        }
        else if (DateOnly.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var d2))
            sb.Append(d2.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture));

        sb.Append('\t');

        // Парсим время

        int i = 11;
        while (!char.IsWhiteSpace(record[i]) && record[i] != '|')
            sb.Append(record[i++]);

        sb.Append('\t');

        // Парсим уровень логгирования

        while (record[i] == ' ' || record[i] == '|')
            i++;

        char logLevel = char.ToLowerInvariant(record[i]);

        switch (logLevel)
        {
            case 'i': sb.Append("INFO\t"); break;
            case 'w': sb.Append("WARN\t"); break;
            case 'e': sb.Append("ERROR\t"); break;
            case 'd': sb.Append("DEBUG\t"); break;
            default: throw new ArgumentException("Неизвестный уровень логгирования");
        }

        // Парсим вызвавший метод

        if (type1) // далее разбор разный для разных типов
        {
            while (!char.IsWhiteSpace(record[i])) // проматываем к следующему этапу
                i++;
            sb.Append('\t'); // в этом типе вызвавший метод не указывается, парсим сразу сообщение
            sb.Append(record[++i..]);
        }
        else
        {
            // Парсим вызвавший метод
            while (record[i] != '|') // проматываем к следующему этапу
                i++;
            i++;
            while (record[i] != '|') // проматываем ненужный этап
                i++;
            int endOfMethod = record.IndexOf('|', ++i);
            sb.Append(record[i..endOfMethod]);
            sb.Append('\t');

            // Парсим сообщение

            i = endOfMethod;
            while (char.IsWhiteSpace(record[i]) || record[i] == '|') // проматываем к следующему этапу, пропускаем ненужные пробелы
                i++;
            sb.Append(record[i..]);
        }

        return sb.ToString();
    }

}