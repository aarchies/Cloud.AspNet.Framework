using System;

namespace Glasssix.Contrib.Data.Concurrency
{
    public class DefaultConcurrencyStampProvider : IConcurrencyStampProvider
    {
        public string GetRowVersion() => Guid.NewGuid().ToString();
    }
}