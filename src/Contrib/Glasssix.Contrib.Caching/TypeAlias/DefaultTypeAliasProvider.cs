using Glasssix.Contrib.Caching.TypeAlias.Options;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Glasssix.Contrib.Caching.TypeAlias
{
    public class DefaultTypeAliasProvider : ITypeAliasProvider
    {
        private readonly object _lock = new();
        private readonly TypeAliasOptions? _options;
        private ConcurrentDictionary<string, Lazy<string>>? _dicCache;
        private DateTime? _lastDateTime;

        public DefaultTypeAliasProvider(TypeAliasOptions? options)
        {
            _options = options;
        }

        public string GetAliasName(string typeName)
        {
            if (_options == null || _options.GetAllTypeAliasFunc == null)
                throw new NotImplementedException();

            if (_dicCache == null || _dicCache.IsEmpty)
            {
                RefreshTypeAlias();
            }
            var aliasName = _dicCache?.GetOrAdd(typeName, key => new Lazy<string>(() =>
            {
                RefreshTypeAlias();

                if (_dicCache.TryGetValue(key, out var alias))
                    return alias.Value;

                throw new ArgumentNullException(key, $"找不到类型别名{typeName}");
            }, LazyThreadSafetyMode.ExecutionAndPublication));
            return aliasName?.Value ?? throw new ArgumentNullException(typeName, $"找不到类型别名{typeName}");
        }

        private void RefreshTypeAlias()
        {
            if (_lastDateTime != null && (DateTime.UtcNow - _lastDateTime.Value).TotalSeconds < _options!.RefreshTypeAliasInterval)
            {
                return;
            }

            lock (_lock)
            {
                _lastDateTime = DateTime.UtcNow;
                _dicCache?.Clear();
                _dicCache ??= new ConcurrentDictionary<string, Lazy<string>>();

                var typeAliases = _options!.GetAllTypeAliasFunc!.Invoke();
                foreach (var typeAlias in typeAliases)
                {
                    _dicCache[typeAlias.Key] = new Lazy<string>(typeAlias.Value);
                }
            }
        }
    }
}