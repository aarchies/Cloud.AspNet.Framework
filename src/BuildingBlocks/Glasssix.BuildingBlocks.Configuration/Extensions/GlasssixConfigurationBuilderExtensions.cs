using Glasssix.BuildingBlocks.Configuration.Options;
using Glasssix.Utils.Configuration.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Glasssix.BuildingBlocks.Configuration.Extensions
{
    public static class GlasssixConfigurationBuilderExtensions
    {
        public static void UseGlasssixOptions(this IGlasssixConfigurationBuilder builder, Action<GlasssixConfigurationRelationOptions> options)
        {
            var relation = new GlasssixConfigurationRelationOptions();
            options.Invoke(relation);
            builder.AddRelations(relation.Relations.ToArray());
        }

        internal static void AutoMapping(this GlasssixConfigurationBuilder builder, IEnumerable<Assembly> assemblies)
        {
            var optionTypeList = assemblies.Where(x => !x.GetName().Name.Contains("MySql.EntityFrameworkCore") && !x.GetName().Name.Contains("System.DirectoryServices.Protocols"));

            var optionTypes = optionTypeList.SelectMany(assembly => assembly.GetTypes())
                .Where(type => type != typeof(IGlasssixConfigurationOptions) &&
                    type != typeof(GlasssixConfigurationOptions) &&
                    !type.IsAbstract &&
                    typeof(IGlasssixConfigurationOptions).IsAssignableFrom(type))
                .ToList();
            optionTypes.ForEach(optionType =>
            {
                var constructorInfo = optionType.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault(info => info.GetParameters().Length == 0);

                if (constructorInfo == null)
                    throw new ArgumentException($"[{optionType.Name}] must have a parameterless constructor");

                var option = (IGlasssixConfigurationOptions)Activator.CreateInstance(optionType, !constructorInfo.IsPublic)!;
                var sectionName = option.Section ?? optionType.Name;
                var name = Microsoft.Extensions.Options.Options.DefaultName;
                if (builder.Relations.Any(relation => relation.SectionType == option.SectionType && relation.Section == sectionName && relation.ObjectType == optionType && relation.Name == name))
                {
                    throw new ArgumentException(
                        "节点已加载，无需重复加载，检查自动映射类之间是否存在重复节或继承");
                }
                builder.AddRelations(new ConfigurationRelationOptions()
                {
                    SectionType = option.SectionType,
                    ParentSection = option.ParentSection,
                    Section = sectionName,
                    ObjectType = optionType,
                    Name = name
                });
            });
        }
    }
}