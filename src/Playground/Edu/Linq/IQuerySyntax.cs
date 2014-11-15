// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Playground.Edu.Linq
{
    public interface IQuerySyntax
    {
        IQuerySyntax<T> Cast<T>();
    }
}
