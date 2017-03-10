// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Internal
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#", Justification = "[Intentionally] The method implements the Try... pattern. Only used internally.")]
    internal delegate bool TryParser<TResult>(string value, out TResult result);
}
