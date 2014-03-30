// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative.Narrator
{
    using Autofac;
    using Narvalo.Narrative.Parsers;

    public sealed class ParserModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Require.NotNull(builder, "builder");

            builder.RegisterType<SimpleParser>().As<IParser>();
        }
    }
}
