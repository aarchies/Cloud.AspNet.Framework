using System;

namespace Glasssix.Contrib.Data.Options.Internal
{
    internal class DbContextNameRelationOptions
    {
        public DbContextNameRelationOptions(string name, Type dbContextType)
        {
            Name = name;
            DbContextType = dbContextType;
        }

        public Type DbContextType { get; }
        public string Name { get; }
    }
}