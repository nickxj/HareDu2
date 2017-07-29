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
namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface ExchangeInfo
    {
        [JsonProperty("name")]
        string Name { get; }
        
        [JsonProperty("vhost")]
        string VirtualHost { get; }
        
        [JsonProperty("type")]
        string RoutingType { get; }
        
        [JsonProperty("durable")]
        bool Durable { get; }
        
        [JsonProperty("auto_delete")]
        bool AutoDelete { get; }
        
        [JsonProperty("internal")]
        bool Internal { get; }
    }
}