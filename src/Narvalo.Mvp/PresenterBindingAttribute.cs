// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class PresenterBindingAttribute : Attribute
    {
        public PresenterBindingAttribute(Type presenterType)
        {
            PresenterType = presenterType;
            ViewType = null;
            BindingMode = PresenterBindingMode.Default;
        }

        public Type PresenterType { get; private set; }

        public PresenterBindingMode BindingMode { get; set; }

        public Type ViewType { get; set; }

        internal Type Origin { get; set; }
    }
}
