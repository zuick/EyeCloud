using UniRx;
using System;

namespace Game.Services
{
    public static class MessagesService
    {
        public static IDisposable Subscribe<T>(Action<T> action)
        {
            return MessageBroker.Default.Receive<T>().Subscribe(action);
        }

        public static IObservable<T> Receive<T>()
        {
            return MessageBroker.Default.Receive<T>();
        }

        public static void Publish<T>(T message)
        {
            MessageBroker.Default.Publish(message);
        }
    }
}