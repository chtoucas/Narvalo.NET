// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Internal
{
    internal delegate bool TryParser<TResult>(string value, out TResult result);
}
