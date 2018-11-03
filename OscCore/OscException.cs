// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;

namespace OscCore
{
    public class OscException : Exception
    {
        public readonly OscError OscError;

        public OscException(OscError oscError, string message) : base(message)
        {
            OscError = oscError;
        }

        public OscException(OscError oscError, string message, Exception innerException) : base(message, innerException)
        {
            OscError = oscError;
        }
    }
}