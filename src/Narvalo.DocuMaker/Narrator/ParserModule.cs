// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.DocuMaker.Narrator
{
    using Autofac;
    using Narvalo;
    using Narvalo.DocuMaker.Parsing;

    public sealed class ParserModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Require.NotNull(builder, "builder");

            builder.RegisterType<SimpleParser>().As<IParser>();
        }
    }
}
