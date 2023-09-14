using System;
using System.Collections.Generic;

namespace Glasssix.Utils.Configuration
{
    public abstract class AbstractConfigurationRepository : IConfigurationRepository
    {
        private readonly List<IRepositoryChangeListener> _listeners = new List<IRepositoryChangeListener>();

        public abstract SectionTypes SectionType { get; }

        public void AddChangeListener(IRepositoryChangeListener listener)
        {
            if (!_listeners.Contains(listener))
                _listeners.Add(listener);
        }

        public void FireRepositoryChange(SectionTypes sectionType, Properties newProperties)
        {
            foreach (var listener in _listeners)
            {
                try
                {
                    listener.OnRepositoryChange(sectionType, newProperties);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to invoke repository change listener {listener.GetType()}", ex);
                }
            }
        }

        public abstract Properties Load();

        public void RemoveChangeListener(IRepositoryChangeListener listener)
            => _listeners.Remove(listener);
    }
}