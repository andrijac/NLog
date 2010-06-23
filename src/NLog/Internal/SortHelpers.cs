// 
// Copyright (c) 2004-2010 Jaroslaw Kowalski <jaak@jkowalski.net>
// 
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions 
// are met:
// 
// * Redistributions of source code must retain the above copyright notice, 
//   this list of conditions and the following disclaimer. 
// 
// * Redistributions in binary form must reproduce the above copyright notice,
//   this list of conditions and the following disclaimer in the documentation
//   and/or other materials provided with the distribution. 
// 
// * Neither the name of Jaroslaw Kowalski nor the names of its 
//   contributors may be used to endorse or promote products derived from this
//   software without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF 
// THE POSSIBILITY OF SUCH DAMAGE.
// 

namespace NLog.Internal
{
    using System;
    using System.Collections.Generic;
    using NLog.Common;

    /// <summary>
    /// Provides helpers to sort log events and associated continuations.
    /// </summary>
    internal static class SortHelpers
    {
        /// <summary>
        /// Key selector delegate.
        /// </summary>
        /// <param name="logEventInfo">Log event to extract key information from.</param>
        /// <returns>Key selected from log event.</returns>
        internal delegate string KeySelector(LogEventInfo logEventInfo);

        /// <summary>
        /// Performs bucket sort (group by) on three arrays and returns a dictionary for easy traversal of the result set.
        /// </summary>
        /// <param name="keySelector">The key selector function.</param>
        /// <param name="events">The array of events.</param>
        /// <returns>
        /// Dictonary where keys are unique input keys, and values are lists of <see cref="AsyncLogEventInfo"/>.
        /// </returns>
        public static Dictionary<string, List<AsyncLogEventInfo>> BucketSort(KeySelector keySelector, AsyncLogEventInfo[] events)
        {
            var buckets = new Dictionary<string, List<AsyncLogEventInfo>>();

            for (int i = 0; i < events.Length; ++i)
            {
                var keyValue = keySelector(events[i].LogEvent);
                List<AsyncLogEventInfo> eventsInBucket;
                if (!buckets.TryGetValue(keyValue, out eventsInBucket))
                {
                    eventsInBucket = new List<AsyncLogEventInfo>();
                    buckets.Add(keyValue, eventsInBucket);
                }

                eventsInBucket.Add(events[i]);
            }

            return buckets;
        }
    }
}