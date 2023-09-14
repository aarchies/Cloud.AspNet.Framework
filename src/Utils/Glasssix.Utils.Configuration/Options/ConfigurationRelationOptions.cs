using System;

namespace Glasssix.Utils.Configuration.Options
{
    public class ConfigurationRelationOptions
    {
        public string Name { get; set; }

        /// <summary>
        /// Object type of mapping node relationship
        /// </summary>
        public Type ObjectType { get; set; } = default!;

        public string ParentSection { get; set; }
        public string Section { get; set; } = default!;
        public SectionTypes SectionType { get; set; }
    }
}