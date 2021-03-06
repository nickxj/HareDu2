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
namespace HareDu.Internal
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    internal interface DefinedQueue
    {
        [JsonProperty("node")]
        string Node { get; }
        
        [JsonProperty("durable")]
        bool Durable { get; }
        
        [JsonProperty("auto_delete")]
        bool AutoDelete { get; }
                
        [JsonProperty("arguments", Required = Required.Default)]
        IDictionary<string, object> Arguments { get; }
    }
}