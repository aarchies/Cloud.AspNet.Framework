namespace Glasssix.Contrib.Caller.DaprStarter;

public interface IDaprProcess : IDisposable
{
    void Start();

    void Stop();
}