// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;

// ReSharper disable once CheckNamespace
namespace OscCore
{
    /// <summary>
    ///     Osc Impulse Singleton
    /// </summary>
    public sealed class OscImpulse
    {
        public static readonly OscImpulse Value = new OscImpulse();

        private OscImpulse()
        {
        }

        /// <summary>
        ///     Matches the string against "Impulse", "Bang", "Infinitum", "Inf" the comparison is
        ///     StringComparison.OrdinalIgnoreCase
        /// </summary>
        /// <param name="str">string to check</param>
        /// <returns>true if the string matches any of the recognised impulse strings else false</returns>
        public static bool IsImpulse(string str)
        {
            bool isTrue = false;

            isTrue |= "Infinitum".Equals(str, StringComparison.OrdinalIgnoreCase);

            isTrue |= "Inf".Equals(str, StringComparison.OrdinalIgnoreCase);

            isTrue |= "Bang".Equals(str, StringComparison.OrdinalIgnoreCase);

            isTrue |= "Impulse".Equals(str, StringComparison.OrdinalIgnoreCase);

            return isTrue;
        }

        public override string ToString()
        {
            return "impulse";
        }
    }
}