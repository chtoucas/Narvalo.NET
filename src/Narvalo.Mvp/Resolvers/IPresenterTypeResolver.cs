﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;

    public partial interface IPresenterTypeResolver
    {
        Type Resolve(Type viewType);
    }
}
