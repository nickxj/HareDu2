﻿// Copyright 2013-2018 Albert L. Hives
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
namespace HareDu.Model
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public interface ServerDefinitionInfo
    {
        [JsonProperty("rabbit_version")]
        string RabbitMqVersion { get; }
        
        [JsonProperty("users")]
        IEnumerable<UserInfo> Users { get; }
        
        [JsonProperty("vhosts")]
        IEnumerable<VirtualHostInfo> VirtualHosts { get; }
        
        [JsonProperty("permissions")]
        IEnumerable<UserPermissionsInfo> Permissions { get; }
        
        [JsonProperty("policies")]
        IEnumerable<PolicyInfo> Policies { get; }
        
        [JsonProperty("parameters")]
        IEnumerable<ScopedParameterInfo> Parameters { get; }
        
        [JsonProperty("global_parameters")]
        IEnumerable<GlobalParameterInfo> GlobalParameters { get; }
        
        [JsonProperty("queues")]
        IEnumerable<QueueInfo> Queues { get; }
        
        [JsonProperty("exchanges")]
        IEnumerable<ExchangeInfo> Exchanges { get; }
        
        [JsonProperty("bindings")]
        IEnumerable<BindingInfo> Bindings { get; }
    }
}