// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narrative.Narrator
{
    using Autofac;
    using Narrative.Parsers;
    using Narvalo;

    public sealed class ParserModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Require.NotNull(builder, "builder");

            builder.RegisterType<SimpleParser>().As<IParser>();
        }
    }
}
