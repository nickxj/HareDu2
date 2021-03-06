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
namespace HareDu
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Extensions;

    public class SuccessfulListResult<T> :
        Result<IReadOnlyList<T>>
    {
        public SuccessfulListResult(IReadOnlyList<T> data, DebugInfo debugInfo)
        {
            Data = data;
            DebugInfo = debugInfo;
            Errors = new List<Error>();
            Timestamp = DateTimeOffset.UtcNow;
            HasResult = !Data.IsNull() && Data.Any();
        }

        public IReadOnlyList<T> Data { get; }
        public bool HasResult { get; }
        public DateTimeOffset Timestamp { get; }
        public DebugInfo DebugInfo { get; }
        public IReadOnlyList<Error> Errors { get; }
        public bool HasFaulted => false;
    }
}