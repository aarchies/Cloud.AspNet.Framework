using System;

namespace Glasssix.Contrib.Data.DataFiltering
{
    public interface IDataFilter
    {
        IDisposable Disable<TFilter>() where TFilter : class;

        IDisposable Enable<TFilter>() where TFilter : class;

        bool IsEnabled<TFilter>() where TFilter : class;
    }
}