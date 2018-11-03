// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;

// ReSharper disable once CheckNamespace
namespace OscCore
{
    /// <summary>
    ///     Osc Null Singleton
    /// </summary>
    public sealed class OscNull
    {
        public static readonly OscNull Value = new OscNull();

        private OscNull()
        {
        }

        public static bool IsNull(string str)
        {
            bool isTrue = false;

            isTrue |= "Null".Equals(str, StringComparison.OrdinalIgnoreCase);

            isTrue |= "Nil".Equals(str, StringComparison.OrdinalIgnoreCase);

            return isTrue;
        }

        public override string ToString()
        {
            return "null";
        }
    }
}