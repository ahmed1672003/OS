using System.Collections.Concurrent;
using System.Threading.Channels;

namespace OS;

public class ProducerConsumerWithBlockingCollection
{
    public static async Task StartAsync()
    {
        BlockingCollection<int> buffer = new BlockingCollection<int>();

        Task producer = Task.Run(() => Producer(buffer));
        Task consumer = Task.Run(() => Consumer(buffer));

        await Task.WhenAll(producer, consumer);
    }

    static void Producer(BlockingCollection<int> buffer)
    {
        for (int i = 0; i < 5; i++)
        {
            buffer.Add(i);
            Console.WriteLine("Produced: " + i);
        }
    }

    static void Consumer(BlockingCollection<int> buffer)
    {
        foreach (var item in buffer.GetConsumingEnumerable())
        {
            Console.WriteLine("Consumed: " + item);
        }
    }
}

public class CustomProducerConsumer
{
    static int messageLimit = 5;
    Channel<string> channel = Channel.CreateBounded<string>(messageLimit);

    public async Task StartAsync()
    {
        StartChannel();
    }

    public void StartChannel()
    {
        List<string> names = new List<string>();
        names.Add("John Smith");
        names.Add("Jane Smith");
        names.Add("John Doe");
        names.Add("Jane Doe");

        Task producer = Task.Factory.StartNew(() =>
        {
            foreach (var name in names)
            {
                channel.Writer.TryWrite(name);
            }
            channel.Writer.Complete();
        });

        Task[] consumer = new Task[4];
        for (int i = 0; i < consumer.Length; i++)
        {
            consumer[i] = Task.Factory.StartNew(async () =>
            {
                while (await channel.Reader.WaitToReadAsync())
                {
                    if (channel.Reader.TryRead(out var data))
                    {
                        Console.WriteLine($"Data read from Consumer No.{Task.CurrentId} is {data}");
                    }
                }
            });
        }
    }
}
