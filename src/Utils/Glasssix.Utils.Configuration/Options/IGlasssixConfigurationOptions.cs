using System.Text.Json.Serialization;

namespace Glasssix.Utils.Configuration.Options
{
    /// <summary>
    /// 自动映射关系规范.
    /// 当ParentSection为Null或空字符串时，配置将装载到根节点.
    /// 当Section为Null时，配置将安装在ParentSection节点下，其节点名为类名.
    /// 如果Section是空字符串，它将直接装入ParentSection节点下
    /// </summary>
    public interface IGlasssixConfigurationOptions
    {
        /// <summary>
        ///父节的名称，如果为空，则将在SectionType下装入，否则将装入SectionType中的指定节
        /// </summary>
        [JsonIgnore]
        string ParentSection { get; }

        /// <summary>
        /// 节null表示与类名相同，否则从指定节加载
        /// </summary>
        [JsonIgnore]
        string Section { get; }

        [JsonIgnore]
        SectionTypes SectionType { get; }
    }
}