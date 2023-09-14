using System;

namespace Glasssix.Contrib.Services.Extensions
{
    public class QueryFilter : Attribute
    {
        public Compare Compare { get; set; } = Compare.Equal;

        public string CompareName { get; set; }
    }
}