﻿using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using GestionSolicitud.HelperClasses.Interfaces;

namespace GestionSolicitud.HelperClasses
{

    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private readonly Channel<Func<CancellationToken, ValueTask>> _queue;

        public BackgroundTaskQueue(int capacity)
        {
            BoundedChannelOptions options = new BoundedChannelOptions(capacity)
            {
                FullMode = BoundedChannelFullMode.Wait
            };
            _queue = Channel.CreateBounded<Func<CancellationToken, ValueTask>>(options);
        }

        public async ValueTask QueueBackgroundWorkItemAsync(Func<CancellationToken, ValueTask> workItem)
        {
            if (workItem == null)
            {
                throw new ArgumentNullException("workItem");
            }
            await _queue.Writer.WriteAsync(workItem);
        }

        public async ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(CancellationToken cancellationToken)
        {
            return await _queue.Reader.ReadAsync(cancellationToken);
        }
    }

}
