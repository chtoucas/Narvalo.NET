// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace DocuMaker.Narrator
{
    using Autofac;
    using DocuMaker.Parsing;
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
