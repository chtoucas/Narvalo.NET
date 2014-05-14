// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public sealed class MessageCoordinatorBlackHole : IMessageCoordinator
    {
        MessageCoordinatorBlackHole() { }

        public static MessageCoordinatorBlackHole Instance { get { return Singleton.Instance; } }

        // NB: Since we choose to use a singleton, we never close this message bus.
        public void Close() { }

        public void Publish<T>(T message)
        {
            __Tracer.Warning(typeof(MessageCoordinatorBlackHole),
                "All messages published to this message bus are dropped.");
        }

        public void Subscribe<T>(Action<T> onNext)
        {
            __Tracer.Warning(typeof(MessageCoordinatorBlackHole),
                "Even if subscription is allowed, no messages will be ever received.");
        }

        [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses",
            Justification = "Implementation of lazy initialized singleton.")]
        class Singleton
        {
            // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
            [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline",
               Justification = "Implementation of lazy initialized singleton.")]
            static Singleton() { }

            internal static readonly MessageCoordinatorBlackHole Instance
                = new MessageCoordinatorBlackHole();
        }
    }
}
