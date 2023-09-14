using System.Text.Json.Serialization;

namespace Glasssix.Utils.Configuration.Options
{
    public abstract class GlasssixConfigurationOptions : IGlasssixConfigurationOptions
    {
        /// <summary>
        /// 父节的名称，如果为空，则将在SectionType下装入，否则将装入SectionType中的指定节
        /// </summary>
        [JsonIgnore]
        public virtual string ParentSection => null;

        /// <summary>
        /// 节null表示与类名相同，否则从指定节加载
        /// </summary>
        [JsonIgnore]
        public virtual string Section => null;

        [JsonIgnore]
        public abstract SectionTypes SectionType { get; }
    }
}