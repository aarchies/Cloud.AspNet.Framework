using IdGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Glasssix.BuildingBlocks.Data.IdGenerator.NormalId.MateData
{
    public interface ICodeAndName
    {
        string Code { get; set; }
        string Name { get; set; }
        string PyCode { get; set; }
    }

    public interface ISort<T>
    {
        T Sort { get; set; }
    }

    public interface ITree<T>
    {
        T PId { get; set; }
    }

    public static class Extend
    {
        public static int ERPGetItem2(this List<Tuple<string, int, string>> list, string item1)
        {
            var it = list.FirstOrDefault(l => l.Item1 == item1);
            return it?.Item2 == null ? 0 : it.Item2;
        }

        public static string ERPGetItem3(this List<Tuple<string, int, string>> list, string item1)
        {
            var it = list.FirstOrDefault(l => l.Item1 == item1);
            return it?.Item3!;
        }

        public static string GetItem2(this List<Tuple<int, string, object>> list, int item1)
        {
            var it = list.FirstOrDefault(l => l.Item1 == item1);
            return it?.Item2!;
        }

        public static object GetItem3(this List<Tuple<int, string, object>> list, int item1)
        {
            var it = list.FirstOrDefault(l => l.Item1 == item1);
            return it?.Item3!;
        }

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ExtendInfo : Attribute
    {
    }

    public class IdGenManager
    {
        /// <summary>
        /// Id生成器
        /// </summary>
        public static readonly IdGen.IdGenerator IdGen = new IdGen.IdGenerator(GeneratorId, options!);

        // Let's say we take april 1st 2020 as our epoch
        private static readonly DateTime epoch = new DateTime(2020, 4, 1, 0, 0, 0, DateTimeKind.Utc);

        //private static readonly int GeneratorId = AppConfigurations.GetFromEnv().GetValue<int>("IdGen.Generator");
        private static readonly int GeneratorId = 1;

        // Prepare options
        private static readonly IdGeneratorOptions options = new IdGeneratorOptions(structure, new DefaultTimeSource(epoch));

        // CreateAsync an ID with 45 bits for timestamp, 2 for generator-id
        // and 16 for sequence
        private static readonly IdStructure structure = new IdStructure(45, 2, 16);
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class NotAbpGenerator : Attribute
    {
    }
}