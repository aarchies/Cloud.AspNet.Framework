using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.IO;
using System.Net.Sockets;

namespace Glasssix.Contrib.EventBus.Rabbitmq
{
    /// <summary>
    /// RBMQ连接类
    /// </summary>
    public class DefaultRabbitMQConnection : IDefaultRabbitMQConnection
    {
        private IConnection? _topicConnection;
        private readonly IConnectionFactory? _topicConnectionFactory;
        private IConnection? _connection;
        private readonly IConnectionFactory? _connectionFactory;
        private readonly ILogger<DefaultRabbitMQConnection> _logger;
        private readonly int _retryCount;
        private string[] _clusterNodes;
        private bool _disposed;
        private object sync_root = new object();

        public DefaultRabbitMQConnection(IConnectionFactory connectionFactory, ILogger<DefaultRabbitMQConnection> logger, string[]? clusterNodes = null, int retryCount = 10, IConnectionFactory? TopicConnectionFactory = null)
        {
            if (connectionFactory != null)
                _connectionFactory = connectionFactory;
            if (TopicConnectionFactory != null)
                _topicConnectionFactory = TopicConnectionFactory;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _retryCount = retryCount;
            _clusterNodes = clusterNodes!;
        }

        public bool IsConnected
        {
            get
            {
                return _connection != null && _connection.IsOpen && !_disposed;
            }
        }

        public bool IsTopicConnected
        {
            get
            {
                return _topicConnection != null && _topicConnection.IsOpen && !_disposed;
            }
        }

        public IConnectionFactory ConnectionFactory()
        {
            return _connectionFactory!;
        }

        public IConnectionFactory ConnectionTopicFactory()
        {
            return _topicConnectionFactory!;
        }

        public IModel CreateModel()
        {
            if (!IsConnected)
                TryConnect();
            return _connection!.CreateModel();
        }

        public IModel CreateTopicModel()
        {
            if (!IsConnected)
                TryConnectTopic();
            return _topicConnection!.CreateModel();
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;

            try
            {
                _connection!.ConnectionShutdown -= OnConnectionShutdown;
                _connection.CallbackException -= OnCallbackException;
                _connection.ConnectionBlocked -= OnConnectionBlocked;
                _connection.Dispose();
                if (_topicConnection != null)
                {
                    _topicConnection.ConnectionShutdown -= OnConnectionShutdown;
                    _topicConnection.CallbackException -= OnCallbackException;
                    _topicConnection.ConnectionBlocked -= OnConnectionBlocked;
                    _topicConnection.Dispose();
                }
            }
            catch (IOException ex)
            {
                _logger.LogCritical(ex.ToString());
            }
        }

        public bool TryConnect()
        {
            _logger.LogInformation("Starting RabbitMQ.....");

            lock (sync_root)
            {
                var policy = RetryPolicy.Handle<SocketException>()
                    .Or<BrokerUnreachableException>()
                    .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                    {
                        _logger.LogWarning(ex, "RabbitMQ TimeOut {TimeOut}s ({ExceptionMessage})", $"{time.TotalSeconds:n1}", ex.Message);
                    }
                );

                policy.Execute(() =>
                {
                    if (_clusterNodes != null)
                        _connection = _connectionFactory!.CreateConnection(_clusterNodes);
                    else
                        _connection = _connectionFactory!.CreateConnection();
                });

                if (IsConnected)
                {
                    _connection!.ConnectionShutdown += OnConnectionShutdown;
                    _connection.CallbackException += OnCallbackException;
                    _connection.ConnectionBlocked += OnConnectionBlocked;
                    _logger.LogInformation($"RabbitMQ Client {_connection.Endpoint.HostName} Connection Success");
                    return true;
                }
                else
                {
                    _logger.LogCritical("RabbitMQ Client  Connection Error!");
                    return false;
                }
            }
        }

        public bool TryConnectTopic()
        {
            _logger.LogInformation("Starting TopicModel RabbitMQ.....");

            lock (sync_root)
            {
                var policy = RetryPolicy.Handle<SocketException>()
                    .Or<BrokerUnreachableException>()
                    .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                    {
                        _logger.LogWarning(ex, "RabbitMQ TimeOut {TimeOut}s ({ExceptionMessage})", $"{time.TotalSeconds:n1}", ex.Message);
                    }
                );

                policy.Execute(() =>
                {
                    if (_clusterNodes != null)
                        _topicConnection = _topicConnectionFactory!.CreateConnection(_clusterNodes);
                    else
                        _topicConnection = _topicConnectionFactory!.CreateConnection();
                });

                if (IsConnected)
                {
                    _topicConnection!.ConnectionShutdown += OnConnectionShutdown;
                    _topicConnection.CallbackException += OnCallbackException;
                    _topicConnection.ConnectionBlocked += OnConnectionBlocked;
                    _logger.LogInformation($"RabbitMQ Client {_topicConnection.Endpoint.HostName} Connection Success");
                    return true;
                }
                else
                {
                    _logger.LogCritical("RabbitMQ Client  Connection Error!");

                    return false;
                }
            }
        }

        private void OnCallbackException(object sender, CallbackExceptionEventArgs e)
        {
            if (_disposed) return;

            _logger.LogWarning("RabbitMQ发生异常. Trying to re-connect...");

            TryConnect();
        }

        private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            if (_disposed) return;

            _logger.LogWarning("RabbitMQ连接中断.正在重试....");

            TryConnect();
        }

        private void OnConnectionShutdown(object sender, ShutdownEventArgs reason)
        {
            if (_disposed) return;

            _logger.LogWarning("RabbitMQ已经停止. Trying to re-connect...");

            TryConnect();
        }
    }
}