﻿namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface VirtualHost :
        ResourceSummary
    {
        [JsonProperty("name1")]
        string Name { get; }
        
        [JsonProperty("tracing")]
        string Tracing { get; }
    }
}