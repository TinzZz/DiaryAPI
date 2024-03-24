using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary.Producer.Interfaces
{
    internal interface IMessageProducer
    {
        void SendMessage<T>(T message, string routingKey, string? exchange = default);
    }
}
