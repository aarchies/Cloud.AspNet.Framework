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
        /// <param name="parentSection">���ڣ����ؽ��Ǳ������õĽڵ����ƣ�ConfigurationApi���������ڵ�Appid������</param>
        /// <param name="section">Ĭ��ֵΪ�գ���ӳ������һ��</param>
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
        /// <param name="parentSection">���ڵ����ƣ����Ϊ�գ�����SectionType��װ�룬����װ��SectionType�е�ָ����</param>
        /// <param name="section">Ĭ��ֵΪnull����ӳ���������ַ���һ�¡���������ڸ��ڵ㣬��Ϊ��</param>
        /// <param name="name"></param>
        /// <returns></returns>
        public GlasssixConfigurationRelationOptions MappingConfigurationApi<TModel>(string parentSection, string? section = null, string? name = null)
            where TModel : class
            => Mapping<TModel>(SectionTypes.ConfigurationApi, parentSection, section, name);

        /// <summary>
        /// ӳ�䱾������
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="section">Ĭ��ֵΪnull����ӳ���������ַ���һ�¡���������ڸ��ڵ㣬��Ϊ��</param>
        /// <param name="name"></param>
        /// <returns></returns>
        public GlasssixConfigurationRelationOptions MappingLocal<TModel>(string? section = null, string? name = null) where TModel : class
            => Mapping<TModel>(SectionTypes.Local, null!, section, name);
    }
}