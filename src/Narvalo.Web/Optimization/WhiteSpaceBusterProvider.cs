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
                return s_Instance.InnerCurrent;
            }
        }

        public IWhiteSpaceBusterProvider InnerCurrent
        {
            get
            {
                return _current;
            }
        }

        public static void SetProvider(IWhiteSpaceBusterProvider provider)
        {
            s_Instance.InnerSetProvider(provider);
        }

        public void InnerSetProvider(IWhiteSpaceBusterProvider provider)
        {
            Require.NotNull(provider, nameof(provider));

            _current = provider;
        }
    }
}
