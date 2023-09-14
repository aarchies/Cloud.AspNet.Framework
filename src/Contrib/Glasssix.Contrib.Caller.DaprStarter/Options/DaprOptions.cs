using Glasssix.Contrib.Caller.DaprStarter.Enumerates;
using Glasssix.Utils.Exceptions;

namespace Glasssix.Contrib.Caller.DaprStarter.Options;

/// <summary>
/// dapr����������Ϣ
/// ��ָ������������Ϊnullʱ��������Ĭ��ֵ���Ե�ǰ�汾��Ĭ��ֵdaprΪ׼
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
    /// ����ƴ��AppId��AppIdSuffix�ķָ���
    /// Ĭ��ֵ��-����֧��AppIdDelimiter .
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
    ///Ĭ��Ϊ��ǰMAC��ַ
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
    /// Ӧ�ó������������Ķ˿�
    /// �������0-65535֮��
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
    /// Dapr������Ӧ�ó���Ի���Э�飨gRPC��HTTP������ЧֵΪ��http��grpc
    /// </summary>
    public Protocol? AppProtocol { get; set; }

    /// <summary>
    /// ���Ŀ¼��·��
    /// default:
    /// Linux & Mac: $HOME/.dapr/components
    /// Windows: %USERPROFILE%\.dapr\components
    /// </summary>
    public string? ComponentPath { get; set; }

    /// <summary>
    /// Dapr�����ļ�
    /// default:
    /// Linux & Mac: $HOME/.dapr/config.yaml
    /// Windows: %USERPROFILE%\.dapr\config.yaml
    /// </summary>
    public string? Config { get; set; }

    public bool CreateNoWindow { get; set; } = true;

    /// <summary>
    /// Dapr������gRPC�˿�
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
    /// Dapr������HTTP�˿�
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
    /// �������ĵ�����С��MB��
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
    /// �Ƿ����AppIdSuffix
    /// Ĭ��ֵ��false
    /// </summary>
    public bool DisableAppIdSuffix { get; set; }

    /// <summary>
    /// �������������ȷ��dapr�����ڻ״̬��
    /// ���������ر�ʱ��dapr���쳣�˳��󲻻��Զ�����
    /// </summary>
    public bool EnableHeartBeat { get; set; } = true;

    /// <summary>
    /// ͨ��HTTP�˵�����pprof����
    /// </summary>
    public bool? EnableProfiling { get; set; }

    /// <summary>
    /// Dapr����Ӧ�ó���ʱ����https
    /// Ĭ��ֵ��null����ʹ��https��
    /// </summary>
    public bool? EnableSsl { get; set; }

    /// <summary>
    /// ��������������ڼ��dapr״̬
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
    /// Ҫ���������ɴ����ͼ������Ϊ��repository/image
    /// </summary>
    public string? Image { get; set; }

    /// <summary>
    /// ��־��ϸ��Ϣ����ЧֵΪ��debug��info��warn��error��fatal��panic
    /// default: info
    /// </summary>
    public LogLevel? LogLevel { get; set; }

    /// <summary>
    /// Ӧ�ó���Ĳ������𣬷���������
    /// �������0
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
    ///Dapr���䷢�Ͷ�����Ϣ�Ķ˿�
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
    /// �����ļ������������Ķ˿�
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
    /// Address �ڱ�CA�����ַ
    /// </summary>
    public string? SentryAddress { get; set; }

    /// <summary>
    /// unix���׽���Ŀ¼װ�ص�·�������ָ��
    /// ��ʹ��TCP�˿���ȣ���Dapr sidecar��ͨ��ʹ��unix���׽�����ʵ�ָ��͵��ӳٺ͸����������
    /// Windows����ϵͳ�ϲ�����
    /// </summary>
    public string? UnixDomainSocket { get; set; }

    public bool IsIncompleteAppId()
    {
        return !DisableAppIdSuffix && (AppIdSuffix == null || AppIdSuffix.Trim() != string.Empty);
    }
}