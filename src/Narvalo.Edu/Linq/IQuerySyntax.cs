// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Linq
{
    interface IQuerySyntax
    {
        IQuerySyntax<T> Cast<T>();
    }
}
