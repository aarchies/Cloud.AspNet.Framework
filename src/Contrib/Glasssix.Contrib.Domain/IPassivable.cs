namespace Glasssix.Contrib.Domain
{
    public interface IPassivable
    {
        /// <summary>
        /// 启用 = true
        /// 禁用 = false
        /// </summary>
        bool IsActive { get; set; }

        /// <summary>
        /// 禁用
        /// </summary>
        void Disable();

        /// <summary>
        /// 启用
        /// </summary>
        void Enable();
    }
}