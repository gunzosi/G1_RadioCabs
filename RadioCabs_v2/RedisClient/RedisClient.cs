using StackExchange.Redis;

namespace RedisClient;

public class RedisClient
{
    private readonly IConnectionMultiplexer _redis;
    
    public RedisClient(string connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentException(nameof(connectionString),
                "Redis connection string is required.");
        }
        _redis = ConnectionMultiplexer.Connect(connectionString);
    }

    public void Publish(string channel, string message)
    {
        var publisher = _redis.GetSubscriber();
        publisher.Publish(channel, message);
    }
    
    public void Subscribe(string channel, Action<RedisChannel, RedisValue> handler)
    {
        var subscriber = _redis.GetSubscriber();
        subscriber.Subscribe(channel, handler);
    }
}