﻿using Microsoft.Extensions.Hosting;
using SyncNode.Models;
using SyncNode.Settings;
using SyncNode.Utilities;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SyncNode.Services
{
    public class SyncWorkJobService : IHostedService
    {
        private readonly ConcurrentDictionary<Guid, SyncEntity> documents =
            new ConcurrentDictionary<Guid, SyncEntity>();
        private Timer _timer;
        private readonly IMenuAPISettings _settings;

        public SyncWorkJobService(IMenuAPISettings settings)
        {
            _settings = settings;
        }
        public void AddItem(SyncEntity entity)
        {
            SyncEntity document = null;
            bool isPresent = documents.TryGetValue(entity.Id, out document);
            if (!isPresent || (isPresent && entity.LastChangedAt > document.LastChangedAt))
            {
                documents[entity.Id] = entity;
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoSendWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
        private void DoSendWork(object state)
        {
            foreach (var doc in documents)
            {
                SyncEntity entity = null;
                var isPresent = documents.TryRemove(doc.Key, out entity);
                if (isPresent)
                {
                    var receivers = _settings.Hosts.Where(u => !u.Contains(entity.Origin));
                    foreach (var receiver in receivers)
                    {
                        var url = $"{receiver}/menu/sync";
                        try
                        {
                            var result = HttpClientUtility.SendJson(entity.JsonData, url, entity.SyncType);
                            if (!result.IsSuccessStatusCode)
                            {
                                // lala
                            }
                        }
                        catch (Exception e)
                        {

                        }
                    }
                }
            }
        }
    }
}
