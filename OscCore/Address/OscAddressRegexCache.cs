// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace OscCore.Address
{
    /// <summary>
    ///     Regex cache is an optimisation for regexs for address patterns. Caching is enabled by default.
    /// </summary>
    /// <remarks>
    ///     This mechanism assumes that the same addresses will be used multiple times
    ///     and that there will be a finite number of unique addresses parsed over the course
    ///     of the execution of the program.
    ///     If there are to be many unique addresses used of the course of the execution of
    ///     the program then it maybe desirable to disable caching.
    /// </remarks>
    public static class OscAddressRegexCache
    {
        private static readonly ConcurrentDictionary<string, Regex> Lookup = new ConcurrentDictionary<string, Regex>();

        /// <summary>
        ///     The number of cached regex(s)
        /// </summary>
        public static int Count => Lookup.Count;

        /// <summary>
        ///     Enable regex caching for the entire program (Enabled by default)
        /// </summary>
        public static bool Enabled { get; set; }

        static OscAddressRegexCache()
        {
            // enable caching by default
            Enabled = true;
        }

        /// <summary>
        ///     Acquire a regex, either by creating it if no cached one can be found or retrieving the cached one.
        /// </summary>
        /// <param name="regex">regex pattern</param>
        /// <returns>a regex created from or retrieved for the pattern</returns>
        public static Regex Aquire(string regex)
        {
            return Enabled == false
                ?
                // if caching is disabled then just return a new regex
                new Regex(regex, RegexOptions.None)
                :
                // else see if we have one cached
                Lookup.GetOrAdd(
                    regex,
                    // create a new one, we can compile it as it will probably be reused
                    func => new Regex(regex, RegexOptions.Compiled)
                );
        }

        /// <summary>
        ///     Clear the entire cache
        /// </summary>
        public static void Clear()
        {
            Lookup.Clear();
        }
    }
}