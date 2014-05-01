// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Internal
{
    using System.Diagnostics.CodeAnalysis;

    internal sealed class MessageBusContainer : Delayed<IMessageBus>
    {
        static readonly MessageBusContainer Instance_ = new MessageBusContainer();

        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
        static MessageBusContainer() { }
        
        MessageBusContainer() : base(() => new MessageBus()) { }

        public static MessageBusContainer Current { get { return Instance_; } }
    }
}
