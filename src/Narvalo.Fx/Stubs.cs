// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Provides helper methods pertaining to <see cref="Action"/> instances.
    /// </summary>
    public static class Stubs
    {
        private static readonly Action s_Noop = () => { };

        /// <summary>
        /// Gets the action with an empty body.
        /// </summary>
        /// <value>The empty action.</value>
        public static Action Noop
        {
            get
            {
                Contract.Ensures(Contract.Result<Action>() != null);

                return s_Noop;
            }
        }
    }
}
