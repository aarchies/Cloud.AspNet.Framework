using System;

namespace Glasssix.Utils.Convert.Strings
{
    /// <summary>
    /// 列表项
    /// </summary>
    public class Item : IComparable<Item>
    {
        public Item()
        {
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="value">值</param>
        /// <param name="sortId">排序号</param>
        /// <param name="group">组</param>
        /// <param name="disabled">禁用</param>
        public Item(string text, object value, int? sortId = null, string group = null, bool? disabled = null)
        {
            Text = text;
            Value = value;
            SortId = sortId;
            Group = group;
            Disabled = disabled;
            Label = text;
        }

        /// <summary>
        /// 禁用
        /// </summary>
        public bool? Disabled { get; }

        /// <summary>
        /// 组
        /// </summary>
        public string Group { get; }

        public string Label { get; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int? SortId { get; }

        /// <summary>
        /// 文本
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// 值
        /// </summary>
        public dynamic Value { get; }

        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="other">其它列表项</param>
        public int CompareTo(Item other)
        {
            return string.Compare(Text, other.Text, StringComparison.CurrentCulture);
        }
    }
}