using System;
using System.Collections.Generic;

namespace Personnel.Services
{
    public interface IPublisherService : IDisposable
    {
        void Publish(
            string message,
            string routingKey,
            IDictionary<string, object> messageAttributes,
            string timeToLive = null);
    }
}