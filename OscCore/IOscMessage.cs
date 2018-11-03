// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;

namespace OscCore
{
    public interface IOscMessage
    {
        string Address { get; }

        int Count { get; }

        Uri Origin { get; }

        OscTimeTag? Timestamp { get; }
    }
}