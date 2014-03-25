// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System.IO;
    using Autofac.Extras.DynamicProxy2;

    [Intercept(typeof(WeavingInterceptor))]
    public interface IWeaver
    {
        string Weave(TextReader reader);
    }
}
