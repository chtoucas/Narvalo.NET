// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NSubstitute;
    using Xunit;

    public static class PresenterTypeResolverFacts
    {
        public static class Ctor
        {
            [Fact]
            public static void ThrowsArgumentNullException_ForNullBuildManager()
            {
                // Arrange
                var defaultNamespaces = Substitute.For<IEnumerable<string>>();
                var viewSuffixes = Substitute.For<IEnumerable<string>>();
                var presenterNameTemplates = Substitute.For<IEnumerable<string>>();

                // Act & Assert
                Assert.Throws<ArgumentNullException>(
                    () => new PresenterTypeResolver(
                        null,
                        defaultNamespaces,
                        viewSuffixes,
                        presenterNameTemplates));
            }

            [Fact]
            public static void ThrowsArgumentNullException_ForNullDefaultNamespaces()
            {
                // Arrange
                var buildManager = Substitute.For<IBuildManager>();
                var viewSuffixes = Substitute.For<IEnumerable<string>>();
                var presenterNameTemplates = Substitute.For<IEnumerable<string>>();

                // Act & Assert
                Assert.Throws<ArgumentNullException>(
                    () => new PresenterTypeResolver(
                        buildManager,
                        null,
                        viewSuffixes,
                        presenterNameTemplates));
            }

            [Fact]
            public static void ThrowsArgumentNullException_ForNullViewSuffixes()
            {
                // Arrange
                var buildManager = Substitute.For<IBuildManager>();
                var defaultNamespaces = Substitute.For<IEnumerable<string>>();
                var presenterNameTemplates = Substitute.For<IEnumerable<string>>();

                // Act & Assert
                Assert.Throws<ArgumentNullException>(
                    () => new PresenterTypeResolver(
                        buildManager,
                        defaultNamespaces,
                        null,
                        presenterNameTemplates));
            }

            [Fact]
            public static void ThrowsArgumentNullException_ForNullPresenterNameTemplates()
            {
                // Arrange
                var buildManager = Substitute.For<IBuildManager>();
                var defaultNamespaces = Substitute.For<IEnumerable<string>>();
                var viewSuffixes = Substitute.For<IEnumerable<string>>();

                // Act & Assert
                Assert.Throws<ArgumentNullException>(
                    () => new PresenterTypeResolver(
                        buildManager,
                        defaultNamespaces,
                        viewSuffixes,
                        null));
            }
        }

        public static class GetCandidatePrefixesFromInterfacesMethod
        {
#if !NO_INTERNALS_VISIBLE_TO
            [Fact]
            public static void IgnoresIViewInterface()
            {
                // Arrange
                var viewType = Substitute.For<IView>().GetType();

                // Act
                var shortNames = PresenterTypeResolver.GetCandidatePrefixesFromInterfaces(viewType);

                // Assert
                Assert.False(shortNames.Any());
            }
#endif

#if !NO_INTERNALS_VISIBLE_TO
            [Fact]
            public static void IgnoresGenericIViewInterface()
            {
                // Arrange
                var viewType = Substitute.For<IView<Object>>().GetType();

                // Act
                var shortNames = PresenterTypeResolver.GetCandidatePrefixesFromInterfaces(viewType);

                // Assert
                Assert.False(shortNames.Any());
            }
#endif

#if !NO_INTERNALS_VISIBLE_TO
            [Fact]
            public static void IgnoresInterfaceNotInheritingIView()
            {
                // Arrange
                var viewType = Substitute.For<IMyInterface>().GetType();

                // Act
                var shortNames = PresenterTypeResolver.GetCandidatePrefixesFromInterfaces(viewType);

                // Assert
                Assert.False(shortNames.Any());
            }
#endif

#if !NO_INTERNALS_VISIBLE_TO
            [Fact]
            public static void IgnoresInterfaceNameNotEndingWithView()
            {
                // Arrange
                var viewType = Substitute.For<IMyViewControl>().GetType();

                // Act
                var shortNames = PresenterTypeResolver.GetCandidatePrefixesFromInterfaces(viewType);

                // Assert
                Assert.False(shortNames.Any());
            }
#endif

#if !NO_INTERNALS_VISIBLE_TO
            [Fact]
            public static void IgnoresGenericInterfaceNameNotEndingWithView()
            {
                // Arrange
                var viewType = Substitute.For<IMyViewControl<Object>>().GetType();

                // Act
                var shortNames = PresenterTypeResolver.GetCandidatePrefixesFromInterfaces(viewType);

                // Assert
                Assert.False(shortNames.Any());
            }
#endif

#if !NO_INTERNALS_VISIBLE_TO
            [Fact]
            public static void IgnoresInterfaceNameNotContainingView()
            {
                // Arrange
                var viewType = Substitute.For<IMyControl>().GetType();

                // Act
                var shortNames = PresenterTypeResolver.GetCandidatePrefixesFromInterfaces(viewType);

                // Assert
                Assert.False(shortNames.Any());
            }
#endif

#if !NO_INTERNALS_VISIBLE_TO
            [Fact]
            public static void IgnoresGenericInterfaceNameNotContainingView()
            {
                // Arrange
                var viewType = Substitute.For<IMyControl<Object>>().GetType();

                // Act
                var shortNames = PresenterTypeResolver.GetCandidatePrefixesFromInterfaces(viewType);

                // Assert
                Assert.False(shortNames.Any());
            }
#endif

#if !NO_INTERNALS_VISIBLE_TO
            [Fact]
            public static void TrimsPrefixIAndSuffixView_FromInterfaceName()
            {
                // Arrange
                var viewType = Substitute.For<IMyView>().GetType();

                // Act
                var shortNames = PresenterTypeResolver.GetCandidatePrefixesFromInterfaces(viewType);

                // Assert
                Assert.Equal(new[] { "My" }, shortNames);
            }
#endif

#if !NO_INTERNALS_VISIBLE_TO
            [Fact]
            public static void TrimsPrefixIAndSuffixView_ForGenericInterfaceName()
            {
                // Arrange
                var viewType = Substitute.For<IMyView<Object>>().GetType();

                // Act
                var shortNames = PresenterTypeResolver.GetCandidatePrefixesFromInterfaces(viewType);

                // Assert
                Assert.Equal(new[] { "My" }, shortNames);
            }
#endif

#if !NO_INTERNALS_VISIBLE_TO
            [Fact]
            public static void ReturnsAllCandidates_ForComplexInterfaceName()
            {
                // Arrange
                var viewType = Substitute.For<IMyComplexView>().GetType();

                // Act
                var shortNames = PresenterTypeResolver.GetCandidatePrefixesFromInterfaces(viewType).ToArray();

                // Assert
                Assert.Equal(3, shortNames.Length);
                Assert.Contains("My", shortNames);
                Assert.Contains("MyAlt", shortNames);
                Assert.Contains("MyComplex", shortNames);
            }
#endif
        }

        #region Helper classes

        public interface IMyView : IView { }

        public interface IMyView<T> : IView<T> { }

        public interface IMyAltView<T> : IView<T> { }

        public interface IMyInterface { }

        public interface IMyControl : IView { }

        public interface IMyControl<T> : IView<T> { }

        public interface IMyViewControl : IView { }

        public interface IMyViewControl<T> : IView<T> { }

        public interface IMyComplexView : IMyView, IMyAltView<Object> { }

        #endregion
    }
}
