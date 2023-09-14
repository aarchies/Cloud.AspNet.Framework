using System.Text.Json.Serialization;

namespace Glasssix.Utils.Configuration.Options
{
    public abstract class GlasssixConfigurationOptions : IGlasssixConfigurationOptions
    {
        /// <summary>
        /// ���ڵ����ƣ����Ϊ�գ�����SectionType��װ�룬����װ��SectionType�е�ָ����
        /// </summary>
        [JsonIgnore]
        public virtual string ParentSection => null;

        /// <summary>
        /// ��null��ʾ��������ͬ�������ָ���ڼ���
        /// </summary>
        [JsonIgnore]
        public virtual string Section => null;

        [JsonIgnore]
        public abstract SectionTypes SectionType { get; }
    }
}