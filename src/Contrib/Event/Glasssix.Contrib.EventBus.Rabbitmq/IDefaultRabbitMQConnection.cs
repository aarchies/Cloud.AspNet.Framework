using RabbitMQ.Client;
using System;

namespace Glasssix.Contrib.EventBus.Rabbitmq
{
    public interface IDefaultRabbitMQConnection
        : IDisposable
    {
        bool IsConnected { get; }
        bool IsTopicConnected { get; }

        IConnectionFactory ConnectionFactory();

        IModel CreateModel();

        IModel CreateTopicModel();

        bool TryConnect();

        bool TryConnectTopic();
    }
}