namespace CleverenceSoftTestCase.Data;

/// <summary>
/// Запись лога
/// </summary>
public class LogRecord
{
    public DateOnly Date { get; set; }

    public string Time { get; set; } = string.Empty;

    public LogLevel LogLevel { get; set; }

    public string Method { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;
}