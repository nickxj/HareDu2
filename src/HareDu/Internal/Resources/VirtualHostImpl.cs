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
    using Configuration;
    using Exceptions;
    using Model;

    internal class VirtualHostImpl :
        ResourceBase,
        VirtualHost
    {
        public VirtualHostImpl(HttpClient client, HareDuClientSettings settings)
            : base(client, settings)
        {
        }

        public async Task<Result<IEnumerable<VirtualHostInfo>>> GetAll(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled(LogInfo);

            string url = "api/vhosts";
            var result = await Get<IEnumerable<VirtualHostInfo>>(url, cancellationToken);

            return result;
        }

        public async Task<Result<VirtualHostInfo>> Create(Action<VirtualHostCreateAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled(LogInfo);

            var impl = new VirtualHostCreateActionImpl();
            action(impl);

            string vhost = impl.VirtualHostName.Value;

            if (string.IsNullOrWhiteSpace(vhost))
                return Result.None<VirtualHostInfo>(errors: new List<Error>{ new ErrorImpl("The name of the virtual host is missing.") });

            string sanitizedVHost = vhost.SanitizeVirtualHostName();

            string url = $"api/vhosts/{sanitizedVHost}";

            DefinedVirtualHost definition = impl.Definition.Value;

            var result = await Put<DefinedVirtualHost, VirtualHostInfo>(url, definition, cancellationToken);

            return result;
        }

        public async Task<Result<VirtualHostInfo>> Delete(Action<VirtualHostDeleteAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled(LogInfo);

            var impl = new VirtualHostDeleteActionImpl();
            action(impl);

            if (string.IsNullOrWhiteSpace(impl.VirtualHostName))
                return Result.None<VirtualHostInfo>(errors: new List<Error>{ new ErrorImpl("The name of the virtual host is missing.") });

            string sanitizedVHost = impl.VirtualHostName.SanitizeVirtualHostName();

            if (sanitizedVHost == "2%f")
                return Result.None<VirtualHostInfo>(errors: new List<Error>{ new ErrorImpl("Cannot delete the default virtual host.") });

            string url = $"api/vhosts/{sanitizedVHost}";

            var result = await Delete<VirtualHostInfo>(url, cancellationToken);

            return result;
        }

        
        class VirtualHostDeleteActionImpl :
            VirtualHostDeleteAction
        {
            public string VirtualHostName { get; private set; }
            
            public void VirtualHost(string name) => VirtualHostName = name;
        }

        
        class VirtualHostCreateActionImpl :
            VirtualHostCreateAction
        {
            static bool _tracing;
            static string _vhost;

            public Lazy<DefinedVirtualHost> Definition { get; }
            public Lazy<string> VirtualHostName { get; }
            
            public VirtualHostCreateActionImpl()
            {
                Definition = new Lazy<DefinedVirtualHost>(
                    () => new DefinedVirtualHostImpl(_tracing), LazyThreadSafetyMode.PublicationOnly);
                VirtualHostName = new Lazy<string>(() => _vhost, LazyThreadSafetyMode.PublicationOnly);
            }

            public void VirtualHost(string name) => _vhost = name;

            public void Configure(Action<VirtualHostConfiguration> configuration)
            {
                var impl = new VirtualHostConfigurationImpl();
                configuration(impl);

                _tracing = impl.Tracing;
            }

            
            class DefinedVirtualHostImpl :
                DefinedVirtualHost
            {
                public bool Tracing { get; }

                public DefinedVirtualHostImpl(bool tracing)
                {
                    Tracing = tracing;
                }
            }

            
            class VirtualHostConfigurationImpl :
                VirtualHostConfiguration
            {
                public bool Tracing { get; private set; }

                public void WithTracingEnabled() => Tracing = true;
            }
        }
    }
}