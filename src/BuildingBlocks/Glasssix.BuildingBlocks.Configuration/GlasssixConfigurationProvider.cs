using Glasssix.Utils.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Glasssix.BuildingBlocks.Configuration
{
    public class GlasssixConfigurationProvider : ConfigurationProvider, IRepositoryChangeListener, IDisposable
    {
        private readonly IEnumerable<IConfigurationRepository> _configurationRepositories;
        private readonly ConcurrentDictionary<SectionTypes, Properties> _data;

        public GlasssixConfigurationProvider(GlasssixConfigurationSource source)
        {
            _data = new();
            _configurationRepositories = source.Builder!.Repositories;

            foreach (var configurationRepository in _configurationRepositories)
            {
                configurationRepository.AddChangeListener(this);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            foreach (var configurationRepository in _configurationRepositories)
            {
                configurationRepository.RemoveChangeListener(this);
            }
            GC.SuppressFinalize(this);
        }

        public override void Load()
        {
            foreach (var configurationRepository in _configurationRepositories)
            {
                var properties = configurationRepository.Load();
                _data[configurationRepository.SectionType] = properties;
            }
            SetData();
        }

        public void OnRepositoryChange(SectionTypes sectionType, Properties newProperties)
        {
            if (_data[sectionType] == newProperties)
                return;

            _data[sectionType] = newProperties;

            SetData();

            OnReload();
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        private void SetData()
        {
            Dictionary<string, string> data = new(StringComparer.OrdinalIgnoreCase);

            foreach (var configurationType in _data.Keys)
            {
                var properties = _data[configurationType];
                foreach (var key in properties.GetPropertyNames())
                {
                    data[$"{configurationType}{ConfigurationPath.KeyDelimiter}{key}"] = properties.GetProperty(key)!;
                }
            }

            Data = data;
        }
    }
}