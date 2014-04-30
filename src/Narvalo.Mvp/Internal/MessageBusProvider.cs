// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Internal
{
    using System.Diagnostics.CodeAnalysis;

    internal sealed class MessageBusProvider : ServiceProvider<IMessageBus>
    {
        static readonly MessageBusProvider Instance_ = new MessageBusProvider();

        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
        static MessageBusProvider() { }
        
        MessageBusProvider() : base(() => new MessageBus()) { }

        public static MessageBusProvider Current { get { return Instance_; } }
    }
}
