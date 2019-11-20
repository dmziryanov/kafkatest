using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using Confluent.Kafka;
using ProtoBuf;
using System.Text.Json;
using System.Runtime.Serialization.Formatters.Binary;
using kafka4net;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using System.Net;
using System.Linq;
using Avro;
using Confluent.Kafka.Serialization;

namespace ConsoleApp13
{

    public class User
    {
        public string Password { get; set; }
        public string UserName { get; set; }
    }

    class Program
    {
        private static AvroSerializer<User> s;
        private static AvroDeserializer<string> ds;
        private static int _connected;
        private static Producer _producer;
        private static string _topic;
        private static IDisposable _disposable;
        private static bool _disposed;
        private static string _seedAddress = "localhost:9094";

        public static void Main(string[] args)
        {
            NewMethod1<User>();
            NewMethod2<User>();
        }


        private static void NewMethod1<T>()
        {
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9094", ClientId = "kuku", LogQueue = true
            };

            Action<DeliveryReport<Null, string>> handler = r => Console.WriteLine(!r.Error.IsError ? "Success" : r.Error.Reason);

            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                byte[] stringValue;
                for (int i = 0; i < 100; ++i)
                {
                    stringValue = s.Serialize("test_topic", new User() { UserName = "Danya", Password = "Shirgazin" });
                    //producer.
                }

                producer.Flush(TimeSpan.FromSeconds(10));
            }
        }

        private static void NewMethod2<T>()
        {
        
            var conf = new ConsumerConfig
            {
                ClientId = "kuku",
                GroupId = "kuku",
                BootstrapServers = "localhost:9094",
                AutoOffsetReset = AutoOffsetReset.Latest,
                EnableAutoCommit = true,
                StatisticsIntervalMs = 5000,
                SessionTimeoutMs = 6000
            };

            //ds = new AvroDeserializer<string>();
            //s.Deserialize()

            using (var consumer = new ConsumerBuilder<Null, string>(conf).Build())
            {
                
                consumer.Subscribe("test_topic");
                CancellationTokenSource cts = new CancellationTokenSource();

                while (true)
                {
                    var msg = consumer.Consume();
                    Console.WriteLine(msg.Message.Value);
                }
            }
        }
    }
}
