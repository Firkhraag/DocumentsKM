using RabbitMQ.Client;
using System;

namespace DocumentsKM.Helpers
{
    public interface IConnectionProvider : IDisposable
    {
        IConnection GetConnection();
    }
}
