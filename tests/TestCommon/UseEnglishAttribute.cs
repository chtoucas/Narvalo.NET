// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class UseEnglishAttribute : UseCultureAttribute
    {
        public UseEnglishAttribute() : base("en") { }
    }
}
