namespace CleverenceSoftTestCase;

public static class Server
{
    private static ReaderWriterLockSlim _lock = new();
    private static int _count;

    public static int GetCount(int timeout = 100)
    {
        if (_lock.TryEnterReadLock(timeout))
            return _count;
        else
            throw new TimeoutException("Превышено время ожидания");
    }
    public static void AddToCount(int value, int timeout = 100)
    {
        if (_lock.TryEnterWriteLock(timeout))
            _count += value;
        else
            throw new TimeoutException("Превышено время ожидания");
    }
}