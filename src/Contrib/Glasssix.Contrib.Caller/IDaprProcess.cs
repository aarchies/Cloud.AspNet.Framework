using System;

namespace Glasssix.Contrib.Caller
{
    public interface IDaprProcess : IDisposable
    {
        void Start();

        void Stop();
    }
}