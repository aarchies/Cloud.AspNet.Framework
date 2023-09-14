using SqlSugar;

namespace Glasssix.Contrib.Data.Orm.Configuration.Builders
{
    public interface ISqlSugarSdkConfigBuilder
    {
        /// <summary>
        /// 生成配置
        /// </summary>
        /// <returns></returns>
        ISqlSugarSdkConfig Build();

        /// <summary>
        /// 是否启用，默认启用
        /// </summary>
        /// <param name="enabled"></param>
        /// <returns></returns>
        ISqlSugarSdkConfigBuilder Enable(bool enabled);

        /// <summary>
        /// 设置连接字符串
        /// </summary>
        /// <param name="connentionString"></param>
        /// <returns></returns>
        ISqlSugarSdkConfigBuilder SetConnectionString(string connentionString);

        /// <summary>
        /// 设置DbType
        /// </summary>
        /// <returns></returns>
        ISqlSugarSdkConfigBuilder SetDbType(DbType dbType);

        /// <summary>
        /// 设置InitKeyType
        /// </summary>
        /// <param name="initKeyType"></param>
        /// <returns></returns>
        ISqlSugarSdkConfigBuilder SetInitKeyType(InitKeyType initKeyType);

        /// <summary>
        /// 设置IsAutoCloseConnection=true
        /// </summary>
        /// <returns></returns>
        ISqlSugarSdkConfigBuilder WithAutoCloseConnection();
    }
}