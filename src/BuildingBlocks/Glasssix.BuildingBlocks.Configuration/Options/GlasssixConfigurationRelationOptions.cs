using Glasssix.Utils.Configuration;
using Glasssix.Utils.Configuration.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Glasssix.BuildingBlocks.Configuration.Options
{
    public class GlasssixConfigurationRelationOptions
    {
        internal List<ConfigurationRelationOptions> Relations { get; } = new();

        /// <summary>
        /// Map Section relationship
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="sectionType"></param>
        /// <param name="parentSection">父节，本地节是本地配置的节的名称，ConfigurationApi是配置所在的Appid的名称</param>
        /// <param name="section">默认值为空，与映射类名一致</param>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public GlasssixConfigurationRelationOptions Mapping<TModel>(SectionTypes sectionType, string parentSection, string? section = null, string? name = null)
            where TModel : class
        {
            name ??= Microsoft.Extensions.Options.Options.DefaultName;
            section ??= typeof(TModel).Name;

            if (Relations.Any(relation => relation.SectionType == sectionType && relation.Section == section && relation.Name == name))
                throw new ArgumentOutOfRangeException(nameof(section), "The current section already has a configuration");

            Relations.Add(new ConfigurationRelationOptions()
            {
                SectionType = sectionType,
                ParentSection = parentSection,
                Section = section,
                ObjectType = typeof(TModel),
                Name = name
            });
            return this;
        }

        /// <summary>
        /// Map Section relationship By ConfigurationApi
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="parentSection">父节的名称，如果为空，则将在SectionType下装入，否则将装入SectionType中的指定节</param>
        /// <param name="section">默认值为null，与映射类名和字符串一致。如果不存在根节点，则为空</param>
        /// <param name="name"></param>
        /// <returns></returns>
        public GlasssixConfigurationRelationOptions MappingConfigurationApi<TModel>(string parentSection, string? section = null, string? name = null)
            where TModel : class
            => Mapping<TModel>(SectionTypes.ConfigurationApi, parentSection, section, name);

        /// <summary>
        /// 映射本地配置
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="section">默认值为null，与映射类名和字符串一致。如果不存在根节点，则为空</param>
        /// <param name="name"></param>
        /// <returns></returns>
        public GlasssixConfigurationRelationOptions MappingLocal<TModel>(string? section = null, string? name = null) where TModel : class
            => Mapping<TModel>(SectionTypes.Local, null!, section, name);
    }
}