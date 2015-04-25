// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.FxCop
{
    using Microsoft.FxCop.Sdk;

    public abstract class IntrospectionRule : BaseIntrospectionRule
    {
        protected IntrospectionRule(string name)
            : base(name, "Narvalo.FxCop.Rules", typeof(IntrospectionRule).Assembly)
        {
        }
    }
}
