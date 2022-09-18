﻿using Bot.GetByLink.Proxy.Reddit.Model.Imgur;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Bot.GetByLink.Proxy.Reddit.Model.Streamable
{
    /// <summary>
    ///     Class for streamable data.
    /// </summary>
    public sealed class StreamableData
    {
        /// <summary>
        ///     Gets streamable files.
        /// </summary>
        [JsonPropertyName("files")]
        public IDictionary<string, StreamableFile>? Files { get; init; } = null;

        /// <summary>
        ///     Gets unknow fields.
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, JsonElement>? UnknowFields { get; init; } = null;
    }
}
