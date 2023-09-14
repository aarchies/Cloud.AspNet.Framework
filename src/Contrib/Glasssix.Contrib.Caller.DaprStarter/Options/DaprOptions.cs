using Glasssix.Contrib.Caller.DaprStarter.Enumerates;
using Glasssix.Utils.Exceptions;

namespace Glasssix.Contrib.Caller.DaprStarter.Options;

/// <summary>
/// dapr启动配置信息
/// 当指定的属性配置为null时，参数的默认值将以当前版本的默认值dapr为准
/// </summary>
public class DaprOptions
{
    private string _appIdDelimiter = Constant.DEFAULT_APPID_DELIMITER;

    private string? _appIdSuffix;

    private ushort? _appPort;

    private ushort? _daprGrpcPort;

    private ushort? _daprHttpPort;

    private int? _daprMaxRequestSize;

    private int _heartBeatInterval = Constant.DEFAULT_HEARTBEAT_INTERVAL;

    private int? _maxConcurrency;

    private ushort? _metricsPort;

    private ushort? _profilePort;

    /// <summary>
    /// The id for your application, used for service discovery
    /// </summary>
    public string? AppId { get; set; }

    /// <summary>
    /// 用于拼接AppId和AppIdSuffix的分隔符
    /// 默认值：-，不支持AppIdDelimiter .
    /// </summary>
    public string AppIdDelimiter
    {
        get => _appIdDelimiter;
        set
        {
            GlasssixArgumentException.ThrowIfContain(value, ".", nameof(AppIdDelimiter));

            _appIdDelimiter = value;
        }
    }

    /// <summary>
    ///默认为当前MAC地址
    /// </summary>
    public string? AppIdSuffix
    {
        get => _appIdSuffix;
        set
        {
            GlasssixArgumentException.ThrowIfContain(value, ".", nameof(AppIdSuffix));

            _appIdSuffix = value;
        }
    }

    /// <summary>
    /// 应用程序正在侦听的端口
    /// 必须介于0-65535之间
    /// </summary>
    public ushort? AppPort
    {
        get => _appPort;
        set
        {
            if (value != null)
                GlasssixArgumentException.ThrowIfLessThanOrEqual(value.Value, (ushort)0, nameof(AppPort));

            _appPort = value;
        }
    }

    /// <summary>
    /// Dapr用于与应用程序对话的协议（gRPC或HTTP）。有效值为：http或grpc
    /// </summary>
    public Protocol? AppProtocol { get; set; }

    /// <summary>
    /// 组件目录的路径
    /// default:
    /// Linux & Mac: $HOME/.dapr/components
    /// Windows: %USERPROFILE%\.dapr\components
    /// </summary>
    public string? ComponentPath { get; set; }

    /// <summary>
    /// Dapr配置文件
    /// default:
    /// Linux & Mac: $HOME/.dapr/config.yaml
    /// Windows: %USERPROFILE%\.dapr\config.yaml
    /// </summary>
    public string? Config { get; set; }

    public bool CreateNoWindow { get; set; } = true;

    /// <summary>
    /// Dapr侦听的gRPC端口
    /// Must be greater than 0
    /// </summary>
    public ushort? DaprGrpcPort
    {
        get => _daprGrpcPort;
        set
        {
            if (value != null)
                GlasssixArgumentException.ThrowIfLessThanOrEqual(value.Value, (ushort)0, nameof(DaprGrpcPort));

            _daprGrpcPort = value;
        }
    }

    /// <summary>
    /// Dapr侦听的HTTP端口
    /// Must be greater than 0
    /// </summary>
    public ushort? DaprHttpPort
    {
        get => _daprHttpPort;
        set
        {
            if (value != null)
                GlasssixArgumentException.ThrowIfLessThanOrEqual(value.Value, (ushort)0, nameof(DaprHttpPort));

            _daprHttpPort = value;
        }
    }

    /// <summary>
    /// 请求正文的最大大小（MB）
    /// Must be greater than 0
    /// </summary>
    public int? DaprMaxRequestSize
    {
        get => _daprMaxRequestSize;
        set
        {
            if (value != null)
                GlasssixArgumentException.ThrowIfLessThanOrEqual(value.Value, 0, nameof(DaprMaxRequestSize));

            _daprMaxRequestSize = value;
        }
    }

    /// <summary>
    /// 是否禁用AppIdSuffix
    /// 默认值：false
    /// </summary>
    public bool DisableAppIdSuffix { get; set; }

    /// <summary>
    /// 启动心跳检查以确保dapr程序处于活动状态。
    /// 当心跳检查关闭时，dapr在异常退出后不会自动启动
    /// </summary>
    public bool EnableHeartBeat { get; set; } = true;

    /// <summary>
    /// 通过HTTP端点启用pprof分析
    /// </summary>
    public bool? EnableProfiling { get; set; }

    /// <summary>
    /// Dapr调用应用程序时启用https
    /// 默认值：null（不使用https）
    /// </summary>
    public bool? EnableSsl { get; set; }

    /// <summary>
    /// 心跳检测间隔，用于检测dapr状态
    /// default: 5000 ms
    /// Must be greater than 0
    /// </summary>
    public int HeartBeatInterval
    {
        get => _heartBeatInterval;
        set
        {
            GlasssixArgumentException.ThrowIfLessThanOrEqual(value, 0, nameof(HeartBeatInterval));

            _heartBeatInterval = value;
        }
    }

    /// <summary>
    /// 要在其中生成代码的图像。输入为：repository/image
    /// </summary>
    public string? Image { get; set; }

    /// <summary>
    /// 日志详细信息。有效值为：debug、info、warn、error、fatal或panic
    /// default: info
    /// </summary>
    public LogLevel? LogLevel { get; set; }

    /// <summary>
    /// 应用程序的并发级别，否则不受限制
    /// 必须大于0
    /// </summary>
    public int? MaxConcurrency
    {
        get => _maxConcurrency;
        set
        {
            if (value != null)
                GlasssixArgumentException.ThrowIfLessThanOrEqual(value.Value, 0, nameof(MaxConcurrency));

            _maxConcurrency = value;
        }
    }

    /// <summary>
    ///Dapr向其发送度量信息的端口
    /// Must be greater than 0
    /// </summary>
    public ushort? MetricsPort
    {
        get => _metricsPort;
        set
        {
            if (value is <= 0)
                throw new NotSupportedException($"{nameof(MetricsPort)} must be greater than 0 .");

            _metricsPort = value;
        }
    }

    /// <summary>
    /// default: localhost
    /// </summary>
    public string? PlacementHostAddress { get; set; }

    /// <summary>
    /// 配置文件服务器侦听的端口
    /// Must be greater than 0
    /// </summary>
    public ushort? ProfilePort
    {
        get => _profilePort;
        set
        {
            if (value != null)
                GlasssixArgumentException.ThrowIfLessThanOrEqual(value.Value, (ushort)0, nameof(ProfilePort));

            _profilePort = value;
        }
    }

    /// <summary>
    /// Address 哨兵CA服务地址
    /// </summary>
    public string? SentryAddress { get; set; }

    /// <summary>
    /// unix域套接字目录装载的路径。如果指定
    /// 与使用TCP端口相比，与Dapr sidecar的通信使用unix域套接字以实现更低的延迟和更大的吞吐量
    /// Windows操作系统上不可用
    /// </summary>
    public string? UnixDomainSocket { get; set; }

    public bool IsIncompleteAppId()
    {
        return !DisableAppIdSuffix && (AppIdSuffix == null || AppIdSuffix.Trim() != string.Empty);
    }
}