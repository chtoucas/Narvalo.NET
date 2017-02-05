// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Providers.Snv
{
    using System;

    public sealed class PubDateEventArgs : EventArgs
    {
        public PubDateEventArgs(DateTime date)
        {
            PubDate = date;
        }

        public DateTime PubDate { get; }
    }
}
