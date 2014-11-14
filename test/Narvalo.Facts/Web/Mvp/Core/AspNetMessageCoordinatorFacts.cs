// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Mvp.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class AspNetMessageCoordinatorFacts
    {
        public static class PublishMethod
        {
            [Fact]
            public static void ThrowsInvalidOperationException_WhenCalledAfterClose()
            {
                // Arrange
                var coordinator = new AspNetMessageCoordinator();
                coordinator.Close();

                // Act & Assert
                Assert.Throws<InvalidOperationException>(() => coordinator.Publish("message"));
            }

            [Fact]
            public static void AcceptsMessageWithoutMatchingSubscribers()
            {
                // Arrange
                var coordinator = new AspNetMessageCoordinator();

                // Act & Assert
                Assert.DoesNotThrow(() => coordinator.Publish("message"));
            }

            [Fact]
            public static void AcceptsNullMessage()
            {
                // Arrange
                var coordinator = new AspNetMessageCoordinator();
                string receivedMessage = "whatever";
                coordinator.Subscribe<string>(_ => receivedMessage = _);

                // Act
                coordinator.Publish((string)null);

                // Assert
                Assert.Null(receivedMessage);
            }

            [Fact]
            public static void PushesMessageToSubscribers()
            {
                // Arrange
                var coordinator = new AspNetMessageCoordinator();
                string receivedMessage = null;
                coordinator.Subscribe<string>(_ => receivedMessage = _);

                // Act
                var publishedMessage = "hello";
                coordinator.Publish(publishedMessage);

                // Assert
                Assert.Equal(publishedMessage, receivedMessage);
            }

            [Fact]
            public static void PushesMessageToSubscribersOfBaseType()
            {
                // Arrange
                var coordinator = new AspNetMessageCoordinator();
                MyMessage receivedMessage = null;
                coordinator.Subscribe<MyMessage>(_ => receivedMessage = _);

                // Act
                var publishedMessage = new MyInheritedMessage();
                coordinator.Publish(publishedMessage);

                // Assert
                Assert.Equal(publishedMessage, receivedMessage);
            }

            [Fact]
            public static void DoesNotPushMessageToSubscribersOfInheritedTypes()
            {
                // Arrange
                var coordinator = new AspNetMessageCoordinator();
                coordinator.Subscribe<MyInheritedMessage>(
                    _ => Assert.False(true, "Callback should never have been called."));

                // Act
                coordinator.Publish(new MyMessage());

                // Assert
            }

            [Fact]
            public static void PushesToSubscribersInOrderOfSubscription()
            {
                // Arrange
                var coordinator = new AspNetMessageCoordinator();
                var subscriberCount = 20;
                var firedSubscribers = new List<int>();
                for (var i = 0; i < subscriberCount; i++) {
                    var subscriberIndex = i;
                    coordinator.Subscribe<string>(_ => firedSubscribers.Add(subscriberIndex));
                }

                // Act
                coordinator.Publish("message");

                // Assert
                Assert.Equal(Enumerable.Range(0, subscriberCount).ToArray(), firedSubscribers);
            }

            [Fact]
            public static void StopsPushingMessageAfterFirstFailingSubscriber()
            {
                // Arrange
                var coordinator = new AspNetMessageCoordinator();
                coordinator.Subscribe<string>(
                    _ => { throw new ApplicationException("Test exception"); });
                coordinator.Subscribe<string>(
                    _ => Assert.False(true, "This subscriber should not have been called."));

                // Act
                try {
                    coordinator.Publish("message");
                }
                catch (ApplicationException) {
                }

                // Assert
            }
        }

        public static class CloseMethod
        {
            [Fact]
            public static void MayBeCalledMultipleTimes()
            {
                // Arrange
                var coordinator = new AspNetMessageCoordinator();
                coordinator.Close();

                // Act & Assert
                Assert.DoesNotThrow(() => coordinator.Close());
            }
        }

        public static class SubscribeMethod
        {
            [Fact]
            public static void ThrowsInvalidOperationException_WhenCalledAfterClose()
            {
                // Arrange
                var coordinator = new AspNetMessageCoordinator();
                coordinator.Close();

                // Act & Assert
                Assert.Throws<InvalidOperationException>(() => coordinator.Subscribe<string>(_ => { }));
            }

            [Fact]
            public static void ThrowsArgumentNullException_ForNullSubscriber()
            {
                // Arrange
                var coordinator = new AspNetMessageCoordinator();

                // Act a Assert
                Assert.Throws<ArgumentNullException>(() => coordinator.Subscribe((Action<object>)null));
            }

            [Fact]
            public static void PushesPreviouslyPublishedMessagesToNewSubscribers()
            {
                // Arrange
                var coordinator = new AspNetMessageCoordinator();
                var publishedMessage = "hello";
                coordinator.Publish(publishedMessage);

                // Act
                string receivedMessage = null;
                coordinator.Subscribe<string>(_ => receivedMessage = _);

                // Assert
                Assert.Equal(publishedMessage, receivedMessage);
            }

            [Fact]
            public static void PushesPreviouslyPublishedMessagesToNewSubscribersInOrderOfPublication()
            {
                // Arrange
                var coordinator = new AspNetMessageCoordinator();
                var messageCount = 20;
                var publishedMessages = Enumerable.Range(0, messageCount).ToList();
                foreach (var message in publishedMessages) {
                    coordinator.Publish(message);
                }

                // Act
                var receivedMessages = new List<int>();
                coordinator.Subscribe<int>(_ => receivedMessages.Add(_));

                // Assert
                Assert.Equal(publishedMessages, receivedMessages);
            }
        }

        [Fact]
        public static void PushesMessages_WhenNestedInSubscriber()
        {
            // Arrange
            var coordinator = new AspNetMessageCoordinator();
            var received = false;
            coordinator.Subscribe<int>(_ => { received = _ == 123; });
            coordinator.Subscribe<string>(_ => coordinator.Publish(123));

            // Act
            coordinator.Publish("message");

            // Assert
            Assert.True(received);
        }

        [Fact]
        public static void PushesPreviouslyPublishedMessages_WhenNestedInSubscriber()
        {
            // Arrange
            var coordinator = new AspNetMessageCoordinator();
            var received = false;
            coordinator.Publish("message");

            // Act
            coordinator.Subscribe<int>(_ => { received = _ == 123; });
            coordinator.Subscribe<string>(_ => coordinator.Publish(123));

            // Assert
            Assert.True(received);
        }

        #region Helper classes

        public interface IMyMessage { }

        public class MyMessage : IMyMessage { }

        public class MyInheritedMessage : MyMessage { }

        #endregion

    }
}