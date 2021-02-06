using System;
using System.Collections.Generic;

namespace DocumentsKM.Services
{
    public interface ISubscriberService : IDisposable
    {
        void Subscribe(Func<string, IDictionary<string, object>, bool> callback);
    }
}
