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
namespace HareDu
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Model;

    public interface Node :
        Resource
    {
        /// <summary>
        /// Returns all channels on the current RabbitMQ node.
        /// </summary>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns>Asynchronous task of <see cref="Result{T}"/></returns>
        Task<Result<IEnumerable<ChannelInfo>>> GetChannelsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns all connections on the current RabbitMQ node.
        /// </summary>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns>Asynchronous task of <see cref="Result{T}"/></returns>
        Task<Result<IEnumerable<ConnectionInfo>>> GetConnectionsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns all consumers on the current RabbitMQ node.
        /// </summary>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns>Asynchronous task of <see cref="Result{T}"/></returns>
        Task<Result<IEnumerable<ConsumerInfo>>> GetConsumersAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns all object definitions on the current RabbitMQ node.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<ServerDefinition>> GetDefintiions(CancellationToken cancellationToken = default);
    }
}