using Glasssix.Utils.Configuration;
using Glasssix.Utils.Configuration.Options;
using System.Text.Json.Serialization;

namespace Glasssix.BuildingBlocks.Configuration.Options
{
    public abstract class LocalGlasssixConfigurationOptions : GlasssixConfigurationOptions
    {
        /// <summary>
        /// 本地配置不需要ParentSection
        /// </summary>
        [JsonIgnore]
        public override sealed string? ParentSection => null;

        [JsonIgnore]
        public override sealed SectionTypes SectionType => SectionTypes.Local;
    }

}