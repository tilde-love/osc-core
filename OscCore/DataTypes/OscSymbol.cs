// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

// ReSharper disable once CheckNamespace
namespace OscCore
{
    /// <summary>
    ///     Osc symbol
    /// </summary>
    public struct OscSymbol
    {
        /// <summary>
        ///     The string value of the symbol
        /// </summary>
        public readonly string Value;

        /// <summary>
        ///     Create a new symbol
        /// </summary>
        /// <param name="value">literal string value</param>
        public OscSymbol(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }

        public override bool Equals(object obj)
        {
            return obj is OscSymbol symbol
                ? Value.Equals(symbol.Value)
                : Value.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}