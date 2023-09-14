using Confluent.Kafka;
using System;

namespace Glasssix.Contrib.EventBus.Kafka
{
    public class tests
    {

        public void test()
        {
            var conf = new ProducerConfig { BootstrapServers = "http://10.168.1.47:32335" };

            Action<DeliveryReport<Null, string>> handler = r =>
                Console.WriteLine(!r.Error.IsError
                    ? $"Delivered message to {r.TopicPartitionOffset}"
                    : $"Delivery Error: {r.Error.Reason}");

            using (var p = new ProducerBuilder<Null, string>(conf).Build())
            {
                for (int i = 0; i < 100; ++i)
                {
                    p.Produce("my-topic", new Message<Null, string> { Value = i.ToString() }, handler);
                }

                // wait for up to 10 seconds for any inflight messages to be delivered.
                p.Flush(TimeSpan.FromSeconds(10));
            }
        }

    }
}
