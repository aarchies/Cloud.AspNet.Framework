using Glasssix.BuildingBlocks.Caching.Distributed.StackExchangeRedis.Control.Extensions;
using Glasssix.BuildingBlocks.Caching.Distributed.StackExchangeRedis.Control.Options;
using Glasssix.BuildingBlocks.Caching.Distributed.StackExchangeRedis.Extensions;
using Glasssix.BuildingBlocks.Caching.Distributed.StackExchangeRedis.RateLimits;
using Glasssix.BuildingBlocks.Caching.MultilevelCache.Extensions;
using Glasssix.BuildingBlocks.Caching.MultilevelCache.Options;
using Glasssix.BuildingBlocks.Data.Uow.Extensions;
using Glasssix.BuildingBlocks.DependencyInjection.Ioc;
using Glasssix.BuildingBlocks.EventCenter;
using Glasssix.BuildingBlocks.MessageCenter;
using Glasssix.Contrib.Caching.TypeAlias.Options;
using Glasssix.Contrib.Data.Orm.SqlSugar.SqlSugar;
using Glasssix.Contrib.Data.Storage;
using Glasssix.Contrib.Data.Storage.InfluxDb.InfluxDb;
using Glasssix.Contrib.EventBus.Rabbitmq.Option;
using Glasssix.Contrib.File.Storage;
using Glasssix.Contrib.File.Storage.Extensions;
using Glasssix.Contrib.Message.Emqx.Option;
using Glasssix.Contrib.Repository.Base;
using Glasssix.Contrib.ServiceDiscovery;
using Glasssix.DotNet.Framework.Const;
using Glasssix.DotNet.Framework.Extensions;
#if NET7_0
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.RateLimiting;
#endif
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using static Glasssix.BuildingBlocks.Scheduler.SchedulerClientProvider;
using Glasssix.Contrib.Message.Emqx.MultipleImplementation;
using Glasssix.Contrib.Data.Orm;

namespace Glasssix.DotNet.Framework.Extensions
{
    /// <summary>
    /// 依赖注入模块
    /// </summary>
    public static class GlasssixAspNetCoreModules
    {
        /// <summary>
        /// 服务发现选项
        /// </summary>
        public static Action<ConsulOptions>? ConsulOptions;

        /// <summary>
        /// 健康检查
        /// </summary>
        public static IHealthChecksBuilder? HealthChecksBuilder;

        /// <summary>
        /// 启用Influxdb
        /// </summary>
        /// <param name="services"></param>
        /// <param name="action">Influxdb配置选项</param>
        /// <returns></returns>
        public static IGlasssixBuilder AddAddInflux(this IGlasssixBuilder services, Action<InfluxConfig>? action)
        {
            var option = new InfluxConfig();
            action(option);
            Log.Information($"Register Framework DataStore -Influx Host:{option.Server}");
            services.Services.AddInflux(action);
            return services;
        }

        /// <summary>
        /// 启用多级缓存
        /// </summary>
        /// <param name="services"></param>
        /// <param name="action">IMultilevelCacheClient</param>
        /// <param name="multilevelCacheOptionsAction">缓存全局配置</param>
        /// <param name="typeAliasOptionsAction"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <returns></returns>
        public static IGlasssixBuilder AddCache(this IGlasssixBuilder services, Action<RedisConfigurationOptions>? action, Action<MultilevelCacheGlobalOptions>? multilevelCacheOptionsAction = null, Action<TypeAliasOptions>? typeAliasOptionsAction = null, JsonSerializerOptions? jsonSerializerOptions = null)
        {
            AppsettingConst.IsCache = true;

            var cache = new RedisConfigurationOptions();
            var options = new MultilevelCacheGlobalOptions();
            var aliasOptions = new TypeAliasOptions();
            action(cache);
            var host = string.Join(",", cache.Servers.Select(x => x.Host + ":" + x.Port));
            services.Services.AddMultilevelCache(distributedCacheOptions =>
            {
                Log.Information($"Register Framework Caching -MultilevelCache Host:{host} ");
                distributedCacheOptions.UseStackExchangeRedisCache(action, jsonSerializerOptions);
            }, multilevelCacheOptionsAction, typeAliasOptionsAction);
            HealthChecksBuilder?.AddRedis($"{host},password={cache.Password},serviceName={cache.ClientName}", name: "redis-check", tags: new string[] { "redis" });
            return services;
        }

        /// <summary>
        /// 启用服务发现
        /// </summary>
        /// <param name="services"></param>
        /// <param name="action">配置选项</param>
        /// <returns></returns>
        public static IGlasssixBuilder AddConsul(this IGlasssixBuilder services, Action<ConsulOptions>? action)
        {
            var option = new ConsulOptions();
            action(option);
            Log.Information($"Register Framework ServiceDiscovery -Consul Host:{option.Host}");
            AppsettingConst.IsConsul = true;
            ConsulOptions = action;
            services.Services.AddConsul(action);
            return services;
        }

        /// <summary>
        /// 启用数据仓储
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="connectionString">连接字符串</param>
        /// <returns></returns>
        public static IGlasssixBuilder AddDbContext<TContext>(this IGlasssixBuilder services, string? connectionString) where TContext : DbContext
        {
            Log.Information($"Register Framework Data.Repository -DbContext {typeof(TContext)} ");
            services.Services.AddDbContext<TContext>(options =>
            options.UseMySql(connectionString, new MySqlServerVersion(new Version(5, 7, 28)), mySqlOptions =>
            {
                //去掉注入时重试策略 替换为框架自定义用户策略
                //mySqlOptions.EnableRetryOnFailure();
                //mySqlOptions.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: TimeSpan.FromSeconds(5), errorNumbersToAdd: null);
                //mySqlOptions.CommandTimeout(10);
            })).AddUnitOfWork<TContext>();
            services.Services.TryAddScoped(typeof(IRepository<,>), typeof(BaseRepository<,>));
            HealthChecksBuilder?.AddMySql(connectionString, "DbContext");
            return services;
        }

        /// <summary>
        /// 启用仓储
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="optionsBuilder"></param>
        /// <returns></returns>
        public static IGlasssixBuilder AddDbContext<TContext>(this IGlasssixBuilder services, Action<DbContextOptionsBuilder>? optionsBuilder) where TContext : DbContext
        {
            //GlasssixIocApp.SetServiceCollection(services.Services);
            Log.Information($"Register Framework Data.Repository -DbContext {typeof(TContext)} ");
            services.Services.AddDbContext<TContext>(optionsBuilder).AddUnitOfWork<TContext>();

            services.Services.TryAddScoped(typeof(IRepository<,>), typeof(BaseRepository<,>));

            var db = GlasssixIocApp.GetRequiredService<TContext>();
            HealthChecksBuilder?.AddMySql(db.Database.GetDbConnection().ConnectionString, $"DbContext-{Random.Shared.Next()}");

            return services;
        }

        /// <summary>
        /// 启用Emqx消息
        /// </summary>
        /// <param name="services"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IGlasssixBuilder AddEmqxClient(this IGlasssixBuilder services, Action<MqttOption>? action)
        {
            var option = new MqttOption();
            action(option);
            Log.Information($"Register Framework MessagesCenter -Emqx Host:{option.Node} ClientId:{option.ClientId}");
            services.Services.AddMqttClient(action);
            return services;
        }

        /// <summary>
        /// 启用Emqx消息 特性注入方式
        /// </summary>
        /// <param name="services"></param>
        /// <param name="action"></param>
        /// <param name="mapAssemblies"></param>
        /// <returns></returns>
        public static IGlasssixBuilder AddEmqxClient(this IGlasssixBuilder services, Action<MqttClientOptions>? action, params Assembly[]? mapAssemblies)
        {
            var option = new MqttClientOptions();
            action(option);
            Log.Information($"Register Framework MessagesCenter -Emqx Host:{option.Broker} ClientId:{option.ClientId}");
            services.Services.AddMqttClient(action, mapAssemblies);
            return services;
        }

        /// <summary>
        /// 启用事件总线
        /// </summary>
        /// <param name="services"></param>
        /// <param name="action">配置选项</param>
        /// <returns></returns>
        public static IGlasssixBuilder AddEventBus(this IGlasssixBuilder services, Action<EventBusOptions>? action)
        {
            AppsettingConst.IsEventBus = true;
            var option = new EventBusOptions();
            action.Invoke(option);
            Log.Information($"Register Framework EventBus -Rabbitmq Host:{option.Connection}:{option.Port}");
            services.Services.AddEventBusonRabbitmq(action);
            var host = $"amqp://{option.UserName}:{option.Password}@{option.Connection}:{option.Port}/";
            HealthChecksBuilder?.AddRabbitMQ(host, name: "eventbus-check", tags: new string[] { "rabbitmq" });
            return services;
        }

        /// <summary>
        /// 启用文件存储 Minio
        /// </summary>
        /// <param name="services"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IGlasssixBuilder AddFileStoreMinio(this IGlasssixBuilder services, Action<MinioOption>? action)
        {
            var option = new MinioOption();
            action(option);
            Log.Information($"Register Framework FileStore -Minio Host:{option.Node}");
            services.Services.AddMinio(action);
            return services;
        }

        /// <summary>
        /// 启用鉴权
        /// </summary>
        /// <param name="services"></param>
        /// <param name="identityUrl">认证中心地址</param>
        /// <param name="api">api资源</param>
        /// <param name="scope">作用域</param>
        /// <param name="clientSecret">客户端密钥</param>
        /// <returns></returns>
        public static IGlasssixBuilder AddIdentityClient(this IGlasssixBuilder services, string identityUrl, string api, string scope, string clientSecret)
        {
            services.Services.AddAuthentication("Bearer")
                           .AddJwtBearer("Bearer", options =>
                           {
                               options.Authority = identityUrl;//ISS主机
                               options.RequireHttpsMetadata = false;//是否验证Https
                               options.TokenValidationParameters = new TokenValidationParameters
                               {
                                   ValidateAudience = true,//是否验证受众
                                   ValidAudiences = new List<string> { api },//受众
                                   ValidateIssuer = true,//是否验证发行者
                                   ValidIssuer = identityUrl,//发行者
                                   ValidateIssuerSigningKey = true,//验证签名
                                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(clientSecret)),//客户端密钥
                                   ValidateLifetime = true,//验证过期时间
                                   RoleClaimType = ClaimTypes.Role,//角色类型
                                   NameClaimType = ClaimTypes.Name,//用户类型
                                   ClockSkew = TimeSpan.Zero//令牌延迟过期
                               };
                               options.SaveToken = true;//指示保存在服务器再进行验证
                           });

            Log.Information($"Register Framework Authentication -IdentityClient Host:{identityUrl} Api:{api} Scope:{scope} ClientSecret:{clientSecret}");
            //策略 此处使用特性即可
            services.Services.AddAuthorization(options =>
            {
                options.AddPolicy(scope!, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", scope);//策略需要scope有api
                });
            });
            AppsettingConst.IsAuthorization = true;
            return services;
        }

        /// <summary>
        /// 启用Orm Dapper
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connection"></param>
        /// <param name="IsAutoTableCreate">是否启用分表</param>
        /// <param name="OverdueDelete">是否自动清除过期表</param>
        /// <returns></returns>
        public static IGlasssixBuilder AddOrmDapper<T>(this IGlasssixBuilder services, string? connection, bool IsAutoTableCreate, bool OverdueDelete = false)
        {
            Log.Information($"Register Framework Orm -Dapper Connection:{connection} IsAutoTableCreate:{IsAutoTableCreate} OverdueDelete:{OverdueDelete}");
            services.Services.AddDapper<T>(connection, IsAutoTableCreate, OverdueDelete);
            HealthChecksBuilder?.AddMySql(connection, "mysql-dapper");
            return services;
        }

        /// <summary>
        /// 启用Orm SqlSugar
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static IGlasssixBuilder AddOrmSqlSugar(this IGlasssixBuilder services, string? connection)
        {
            Log.Information("Register Framework Orm -SqlSugar");
            services.Services.AddSqlSugar(connection);
            return services;
        }

        /// <summary>
        /// 启用令牌桶限流 (需Redis支持)
        /// </summary>
        /// <param name="services"></param>
        /// <param name="redisContion"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IGlasssixBuilder AddRateLimit(this IGlasssixBuilder services, string? redisContion, Action<RedisTokenBucketAlgorithmOptions>? action)
        {
            Log.Information($"Register Framework RateLimit redisContion:{redisContion}");
            AppsettingConst.IsRateLimit = true;
            services.Services.AddRateLimits(action, redisContion);
            return services;
        }


#if NET7_0

        /// <summary>
        /// 固定窗口限流 [EnableRateLimiting("FixedWindow")]  特性注入即可
        /// </summary>
        /// <param name="services"></param>
        /// <param name="policyName">特性名称</param>
        /// <param name="time">锁定时长</param>
        /// <param name="permitLimit">触发次数</param>
        /// <returns></returns>
        public static IGlasssixBuilder AddRateLimit(this IGlasssixBuilder services, string? policyName, TimeSpan time, int permitLimit)
        {
            AppsettingConst.IsRateLimit = true;
            services.Services.AddRateLimiter(p => p
              .AddFixedWindowLimiter(policyName: policyName, options =>
              {
                  options.PermitLimit = permitLimit;
                  options.Window = time;
              }));
            return services;
        }
#endif

        /// <summary>
        /// 启用调度中心
        /// </summary>
        /// <param name="services"></param>
        /// <param name="types">业务执行器 类型</param>
        /// <returns></returns>
        public static IGlasssixBuilder AddScheduler(this IGlasssixBuilder services, RegisterHandlerTypes? types = null)
        {
            AppsettingConst.IsScheduler = true;
            services.Services.AddxxlJobClient(services.Configuration, types!);
            Log.Information("Register Framework Scheduler -xxlJob ");
            return services;
        }

    }
}