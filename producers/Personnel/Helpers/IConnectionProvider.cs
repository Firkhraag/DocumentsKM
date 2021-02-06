using RabbitMQ.Client;
using System;

namespace Personnel.Helpers
{
    public interface IConnectionProvider : IDisposable
    {
        IConnection GetConnection();
    }
}
