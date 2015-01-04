// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Benchmarking.Internal
{
    using System;
    using System.Reflection;

    // FIXME: Cela ne marchera que si la méthode est statique http://msdn.microsoft.com/en-us/library/53cz7sc6(v=vs.100).aspx
    static class ActionFactory
    {
        public static Action Create(MethodInfo method)
        {
            return (Action)Delegate.CreateDelegate(typeof(Action), method);
        }

        // FIXME: ???
        public static Action<T> Create<T>(MethodInfo method)
        {
            return (Action<T>)Delegate.CreateDelegate(typeof(Action<T>), method);
        }
    }
}
