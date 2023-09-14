using System;

namespace Glasssix.DotNet.Framework.Session
{
    /// <summary>
    /// 用户会话
    /// </summary>
    public class Session : ISession
    {
        /// <summary>
        /// 用户会话
        /// </summary>
        public static readonly ISession Instance = new Session();

        /// <summary>
        /// 空用户会话
        /// </summary>
        public static readonly ISession Null = NullSession.Instance;

        /// <summary>
        /// 授权?
        /// </summary>
        public bool IsAuthenticated => throw new NotImplementedException();

        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId => throw new NotImplementedException();
    }
}