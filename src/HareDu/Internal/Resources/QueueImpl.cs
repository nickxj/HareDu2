﻿// Copyright 2013-2017 Albert L. Hives
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
namespace HareDu.Internal.Resources
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Exceptions;
    using Model;

    internal class QueueImpl :
        ResourceBase,
        Queue
    {
        public QueueImpl(HttpClient client, HareDuClientSettings settings)
            : base(client, settings)
        {
        }

        public async Task<Result<IEnumerable<QueueInfo>>> GetAll(CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            string url = $"api/queues";

            LogInfo($"Sent request to return all information on current RabbitMQ server.");

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<IEnumerable<QueueInfo>> result = await response.GetResponse<IEnumerable<QueueInfo>>();

            return result;
        }

        public async Task<Result> Create(string queue, string vhost, Action<QueueBehavior> behavior,
            CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            if (string.IsNullOrWhiteSpace(queue))
                throw new QueueMissingException("The name of the queue is missing.");

            if (string.IsNullOrWhiteSpace(vhost))
                throw new VirtualHostMissingException("The name of the virtual host is missing.");
            
            var impl = new QueueBehaviorImpl();
            behavior(impl);

            QueueSettings queueSettings = impl.Settings.Value;

            string sanitizedVHost = vhost.SanitizeVirtualHostName();

            string url = $"api/queues/{sanitizedVHost}/{queue}";

            LogInfo($"Sent request to RabbitMQ server to create queue '{queue}' in virtual host '{sanitizedVHost}'.");

            HttpResponseMessage response = await HttpPut(url, queueSettings, cancellationToken);
            Result result = response.GetResponse();

            return result;
        }

        public async Task<Result> Delete(string queue, string vhost,
            CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            if (string.IsNullOrWhiteSpace(queue))
                throw new QueueMissingException("The name of the queue is missing.");

            if (string.IsNullOrWhiteSpace(vhost))
                throw new VirtualHostMissingException("The name of the virtual host is missing.");

            string sanitizedVHost = vhost.SanitizeVirtualHostName();

            string url = $"api/queues/{sanitizedVHost}/{queue}";

            LogInfo($"Sent request to RabbitMQ server to delete queue '{queue}' from virtual host '{sanitizedVHost}'.");

            HttpResponseMessage response = await HttpDelete(url, cancellationToken);
            Result result = response.GetResponse();

            return result;
        }

        public async Task<Result> Delete(string queue, string vhost, Action<QueueDeleteCondition> condition,
            CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            if (string.IsNullOrWhiteSpace(queue))
                throw new QueueMissingException("The name of the queue is missing.");

            if (string.IsNullOrWhiteSpace(vhost))
                throw new VirtualHostMissingException("The name of the virtual host is missing.");

            var impl = new QueueDeleteConditionImpl();
            condition(impl);
            
            string sanitizedVHost = vhost.SanitizeVirtualHostName();

            string url = $"api/queues/{sanitizedVHost}/{queue}";
            string query = string.Empty;

            if (impl.DeleteIfUnused)
                query = "if-unused=true";

            if (impl.DeleteIfEmpty)
                query = !string.IsNullOrWhiteSpace(query) ? $"{query}&if-empty=true" : "if-empty=true";

            if (string.IsNullOrWhiteSpace(query))
                url = $"{url}?{query}";

            LogInfo($"Sent request to RabbitMQ server to delete queue '{queue}' from virtual host '{sanitizedVHost}'.");

            HttpResponseMessage response = await HttpDelete(url, cancellationToken);
            Result result = response.GetResponse();

            return result;
        }

        
        class QueueDeleteConditionImpl :
            QueueDeleteCondition
        {
            public bool DeleteIfUnused { get; private set; }
            public bool DeleteIfEmpty { get; private set; }

            public void IfUnused()
            {
                DeleteIfUnused = true;
            }

            public void IfEmpty()
            {
                DeleteIfEmpty = true;
            }
        }


        class QueueBehaviorImpl :
            QueueBehavior
        {
            static bool _durable;
            static bool _autoDelete;
            static string _node;
            static IDictionary<string, object> _arguments;

            public Lazy<QueueSettings> Settings { get; }

            public QueueBehaviorImpl() => Settings = new Lazy<QueueSettings>(Init, LazyThreadSafetyMode.PublicationOnly);

            QueueSettings Init() => new QueueSettingsImpl(_durable, _autoDelete, _node, _arguments);

            public void IsDurable() => _durable = true;

            public void OnNode(string node) => _node = node;

            public void WithArguments(IDictionary<string, object> arguments)
            {
                if (arguments == null)
                    return;

                _arguments = arguments;
            }

            public void AutoDeleteWhenNotInUse() => _autoDelete = true;


            class QueueSettingsImpl :
                QueueSettings
            {
                public QueueSettingsImpl(bool durable, bool autoDelete, string node, IDictionary<string, object> arguments)
                {
                    Durable = durable;
                    AutoDelete = autoDelete;
                    Node = node;
                    Arguments = arguments;
                }

                public string Node { get; }
                public bool Durable { get; }
                public bool AutoDelete { get; }
                public IDictionary<string, object> Arguments { get; }
            }
        }
    }
}