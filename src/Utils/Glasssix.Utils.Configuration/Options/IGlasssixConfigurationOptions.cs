using System.Text.Json.Serialization;

namespace Glasssix.Utils.Configuration.Options
{
    /// <summary>
    /// �Զ�ӳ���ϵ�淶.
    /// ��ParentSectionΪNull����ַ���ʱ�����ý�װ�ص����ڵ�.
    /// ��SectionΪNullʱ�����ý���װ��ParentSection�ڵ��£���ڵ���Ϊ����.
    /// ���Section�ǿ��ַ���������ֱ��װ��ParentSection�ڵ���
    /// </summary>
    public interface IGlasssixConfigurationOptions
    {
        /// <summary>
        ///���ڵ����ƣ����Ϊ�գ�����SectionType��װ�룬����װ��SectionType�е�ָ����
        /// </summary>
        [JsonIgnore]
        string ParentSection { get; }

        /// <summary>
        /// ��null��ʾ��������ͬ�������ָ���ڼ���
        /// </summary>
        [JsonIgnore]
        string Section { get; }

        [JsonIgnore]
        SectionTypes SectionType { get; }
    }
}