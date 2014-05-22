// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.PresenterBinding
{
    using System;
    using Xunit;

    public class CompositePresenterDiscoveryStrategyFacts
    {
        public static class TheConstructor
        {
            [Fact]
            public static void ThrowsArgumentNullException_ForNullStrategies()
            {
                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => new CompositePresenterDiscoveryStrategy(null));
            }

            [Fact]
            public static void ThrowsArgumentException_ForEmptyStrategies()
            {
                // Act & Assert
                Assert.Throws<ArgumentException>(
                    () => new CompositePresenterDiscoveryStrategy(new IPresenterDiscoveryStrategy[0]));
            }
        }

        //[Fact]
        //public void CompositePresenterDiscoveryStrategyTests_GetBindings_ShouldYieldFromChildStrategy()
        //{
        //    // Arrange
        //    var strategy = MockRepository.GenerateMock<IPresenterDiscoveryStrategy>();
        //    var traceContext = MockRepository.GenerateMock<ITraceContext>();
        //    var composite = new CompositePresenterDiscoveryStrategy(strategy);
        //    var hosts = new object[0];
        //    var viewInstances = new[]
        //    {
        //        MockRepository.GenerateMock<IView>()
        //    };

        //    var discoveryResult = TestDiscoverResult();
        //    strategy.Stub(s => s
        //        .GetBindings(Arg<IEnumerable<object>>.Is.Equal(hosts), Arg<IEnumerable<IView>>.Is.Anything))
        //        .Return(new[] { discoveryResult });

        //    // Act
        //    var bindings = composite.GetBindings(hosts, viewInstances);

        //    // Assert
        //    CollectionAssert.AreEquivalent(discoveryResult.Bindings.ToArray(), bindings.First().Bindings.ToArray());
        //}

        //[Fact]
        //public void CompositePresenterDiscoveryStrategyTests_GetBindings_ShouldNotPassMatchedViewsToSubsequentStrategies()
        //{
        //    // Arrange
        //    var traceContext = MockRepository.GenerateMock<ITraceContext>();

        //    var hosts = new object[0];

        //    var view1 = MockRepository.GenerateMock<IView>();
        //    var view2 = MockRepository.GenerateMock<IView>();
        //    var view3 = MockRepository.GenerateMock<IView>();
        //    var viewInstances = new[] { view1, view2, view3 };

        //    var strategy1 = MockRepository.GenerateMock<IPresenterDiscoveryStrategy>();
        //    var binding1 = TestBinding(view1, view2);
        //    strategy1.Stub(s => s
        //        .GetBindings(Arg<IEnumerable<object>>.Is.Equal(hosts), Arg<IEnumerable<IView>>.Is.Anything, Arg<ITraceContext>.Is.Equal(traceContext)))
        //        .Return(new[] { binding1 });

        //    var strategy2 = MockRepository.GenerateMock<IPresenterDiscoveryStrategy>();
        //    strategy2.Stub(s => s
        //        .GetBindings(Arg<IEnumerable<object>>.Is.Equal(hosts), Arg<IEnumerable<IView>>.Is.Anything, Arg<ITraceContext>.Is.Equal(traceContext)))
        //        .Return(new PresenterBinding[0]);

        //    var composite = new CompositePresenterDiscoveryStrategy(strategy1, strategy2);

        //    // Act
        //    composite.GetBindings(hosts, viewInstances, traceContext).ToArray();

        //    // Assert
        //    var strategy2ViewInstances = (IEnumerable<IView>)strategy2
        //        .GetArgumentsForCallsMadeOn(s => s
        //            .GetBindings(Arg<IEnumerable<object>>.Is.Equal(hosts), Arg<IEnumerable<IView>>.Is.Anything, Arg<ITraceContext>.Is.Equal(traceContext)))
        //        .Single()
        //        .ElementAt(1);
        //    CollectionAssert.AreEqual(new[] { view3 }, strategy2ViewInstances.ToArray());
        //}

        //[Fact]
        //public void CompositePresenterDiscoveryStrategyTests_GetBindings_ShouldFallThroughChildStrategiesInOrder()
        //{
        //    // Arrange
        //    var traceContext = MockRepository.GenerateMock<ITraceContext>();

        //    var hosts = new object[0];

        //    var view1 = MockRepository.GenerateMock<IView>();
        //    var view2 = MockRepository.GenerateMock<IView>();
        //    var view3 = MockRepository.GenerateMock<IView>();
        //    var viewInstances = new[] { view1, view2, view3 };

        //    var strategy1 = MockRepository.GenerateMock<IPresenterDiscoveryStrategy>();
        //    var binding1 = TestBinding(view1);
        //    strategy1.Stub(s => s
        //        .GetBindings(Arg<IEnumerable<object>>.Is.Equal(hosts), Arg<IEnumerable<IView>>.Is.Anything, Arg<ITraceContext>.Is.Equal(traceContext)))
        //        .Return(new[] { binding1 });

        //    var strategy2 = MockRepository.GenerateMock<IPresenterDiscoveryStrategy>();
        //    var binding2 = TestBinding(view2, view3);
        //    strategy2.Stub(s => s
        //        .GetBindings(Arg<IEnumerable<object>>.Is.Equal(hosts), Arg<IEnumerable<IView>>.Is.Anything, Arg<ITraceContext>.Is.Equal(traceContext)))
        //        .Return(new[] { binding2 });

        //    var strategy3 = MockRepository.GenerateMock<IPresenterDiscoveryStrategy>();

        //    var composite = new CompositePresenterDiscoveryStrategy(strategy1, strategy2, strategy3);

        //    // Act
        //    var bindings = composite.GetBindings(hosts, viewInstances, traceContext);

        //    // Assert
        //    CollectionAssert.AreEqual(new[] { binding1, binding2 }, bindings.ToArray());
        //}

        //static PresenterDiscoveryResult TestDiscoverResult(params IView[] viewInstances)
        //{
        //    var binding = new PresenterBinding(typeof(object), typeof(object), BindingMode.Default, viewInstances);
        //    return new PresenterDiscoveryResult(viewInstances, "", new[] { binding });
        //}
    }
}