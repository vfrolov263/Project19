using System;
using System.Threading;
using UnityEngine;
using UniRx;

public static class UniRxAwaitableExtensions
{
    public static Awaitable<T> ToAwaitable<T>(this IObservable<T> source, CancellationToken token = default)
    {
        var completionSource = new AwaitableCompletionSource<T>();
        IDisposable subscription = null;

        if (token.IsCancellationRequested)
        {
            completionSource.SetCanceled();
            return completionSource.Awaitable;
        }

        var registration = token.Register(() =>
        {
            subscription?.Dispose();
            completionSource.SetCanceled();
        });

        subscription = source.Subscribe(
            value =>
            {
                subscription?.Dispose();
                registration.Dispose();
                completionSource.SetResult(value);
            },
            error =>
            {
                subscription?.Dispose();
                registration.Dispose();
                completionSource.SetException(error);
            },
            () =>
            {
                if (!token.IsCancellationRequested)
                {
                    subscription?.Dispose();
                    registration.Dispose();
                    completionSource.SetCanceled();
                }
            }
        );

        return completionSource.Awaitable;
    }
}
