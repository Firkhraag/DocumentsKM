using System;
using System.Collections.Generic;

namespace DocumentsKM.Services
{
    public interface ISubscriber : IDisposable
    {
        void Subscribe(Func<string, IDictionary<string, object>, bool> callback);
    }
}
