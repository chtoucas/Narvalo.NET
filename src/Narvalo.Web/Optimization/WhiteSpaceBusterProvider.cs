// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Optimization
{
    public sealed partial class WhiteSpaceBusterProvider
    {
        private static readonly WhiteSpaceBusterProvider s_Instance = new WhiteSpaceBusterProvider();

        private IWhiteSpaceBusterProvider _current;

        public WhiteSpaceBusterProvider() : this(null) { }

        public WhiteSpaceBusterProvider(IWhiteSpaceBusterProvider provider)
        {
            InnerSetProvider(provider ?? new DefaultWhiteSpaceBusterProvider());
        }

        public static IWhiteSpaceBusterProvider Current
        {
            get
            {
                Warrant.NotNull<IWhiteSpaceBusterProvider>();

                return s_Instance.InnerCurrent;
            }
        }

        public IWhiteSpaceBusterProvider InnerCurrent
        {
            get
            {
                Warrant.NotNull<IWhiteSpaceBusterProvider>();

                return _current;
            }
        }

        public static void SetProvider(IWhiteSpaceBusterProvider provider)
        {
            Expect.NotNull(provider);

            s_Instance.InnerSetProvider(provider);
        }

        public void InnerSetProvider(IWhiteSpaceBusterProvider provider)
        {
            Require.NotNull(provider, nameof(provider));

            _current = provider;
        }
    }
}

#if CONTRACTS_FULL

namespace Narvalo.Web.Optimization
{
    using System.Diagnostics.Contracts;

    public sealed partial class WhiteSpaceBusterProvider
    {
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_current != null);
        }
    }
}

#endif
