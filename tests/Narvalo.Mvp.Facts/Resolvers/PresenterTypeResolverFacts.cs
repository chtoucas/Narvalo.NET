// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using NSubstitute;
    using Xunit;

    public static partial class PresenterTypeResolverFacts
    {
        #region Ctor()

        [Fact]
        public static void Ctor_ThrowsArgumentNullException_ForNullBuildManager()
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
        public static void Ctor_ThrowsArgumentNullException_ForNullDefaultNamespaces()
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
        public static void Ctor_ThrowsArgumentNullException_ForNullViewSuffixes()
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
        public static void Ctor_ThrowsArgumentNullException_ForNullPresenterNameTemplates()
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

        #endregion
    }

    public static partial class PresenterTypeResolverFacts
    {
        public interface IMyView : IView { }

        public interface IMyView<T> : IView<T> { }

        public interface IMyAltView<T> : IView<T> { }

        public interface IMyInterface { }

        public interface IMyControl : IView { }

        public interface IMyControl<T> : IView<T> { }

        public interface IMyViewControl : IView { }

        public interface IMyViewControl<T> : IView<T> { }

        public interface IMyComplexView : IMyView, IMyAltView<Object> { }
    }

#if !NO_INTERNALS_VISIBLE_TO // White-box tests.
    public static partial class PresenterTypeResolverFacts
    {
        #region GetCandidatePrefixesFromInterfaces()

        [Fact]
        public static void GetCandidatePrefixesFromInterfaces_IgnoresIViewInterface()
        {
            // Arrange
            var viewType = Substitute.For<IView>().GetType();

            // Act
            var shortNames = PresenterTypeResolver.GetCandidatePrefixesFromInterfaces(viewType);

            // Assert
            Assert.False(shortNames.Any());
        }

        [Fact]
        public static void GetCandidatePrefixesFromInterfaces_IgnoresGenericIViewInterface()
        {
            // Arrange
            var viewType = Substitute.For<IView<Object>>().GetType();

            // Act
            var shortNames = PresenterTypeResolver.GetCandidatePrefixesFromInterfaces(viewType);

            // Assert
            Assert.False(shortNames.Any());
        }

        [Fact]
        public static void GetCandidatePrefixesFromInterfaces_IgnoresInterfaceNotInheritingIView()
        {
            // Arrange
            var viewType = Substitute.For<IMyInterface>().GetType();

            // Act
            var shortNames = PresenterTypeResolver.GetCandidatePrefixesFromInterfaces(viewType);

            // Assert
            Assert.False(shortNames.Any());
        }

        [Fact]
        public static void GetCandidatePrefixesFromInterfaces_IgnoresInterfaceNameNotEndingWithView()
        {
            // Arrange
            var viewType = Substitute.For<IMyViewControl>().GetType();

            // Act
            var shortNames = PresenterTypeResolver.GetCandidatePrefixesFromInterfaces(viewType);

            // Assert
            Assert.False(shortNames.Any());
        }

        [Fact]
        public static void GetCandidatePrefixesFromInterfaces_IgnoresGenericInterfaceNameNotEndingWithView()
        {
            // Arrange
            var viewType = Substitute.For<IMyViewControl<Object>>().GetType();

            // Act
            var shortNames = PresenterTypeResolver.GetCandidatePrefixesFromInterfaces(viewType);

            // Assert
            Assert.False(shortNames.Any());
        }

        [Fact]
        public static void GetCandidatePrefixesFromInterfaces_IgnoresInterfaceNameNotContainingView()
        {
            // Arrange
            var viewType = Substitute.For<IMyControl>().GetType();

            // Act
            var shortNames = PresenterTypeResolver.GetCandidatePrefixesFromInterfaces(viewType);

            // Assert
            Assert.False(shortNames.Any());
        }

        [Fact]
        public static void GetCandidatePrefixesFromInterfaces_IgnoresGenericInterfaceNameNotContainingView()
        {
            // Arrange
            var viewType = Substitute.For<IMyControl<Object>>().GetType();

            // Act
            var shortNames = PresenterTypeResolver.GetCandidatePrefixesFromInterfaces(viewType);

            // Assert
            Assert.False(shortNames.Any());
        }

        [Fact]
        public static void GetCandidatePrefixesFromInterfaces_TrimsPrefixIAndSuffixView_FromInterfaceName()
        {
            // Arrange
            var viewType = Substitute.For<IMyView>().GetType();

            // Act
            var shortNames = PresenterTypeResolver.GetCandidatePrefixesFromInterfaces(viewType);

            // Assert
            Assert.Equal(new[] { "My" }, shortNames);
        }

        [Fact]
        public static void GetCandidatePrefixesFromInterfaces_TrimsPrefixIAndSuffixView_ForGenericInterfaceName()
        {
            // Arrange
            var viewType = Substitute.For<IMyView<Object>>().GetType();

            // Act
            var shortNames = PresenterTypeResolver.GetCandidatePrefixesFromInterfaces(viewType);

            // Assert
            Assert.Equal(new[] { "My" }, shortNames);
        }

        [Fact]
        public static void GetCandidatePrefixesFromInterfaces_ReturnsAllCandidates_ForComplexInterfaceName()
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

        #endregion
    }
#endif
}
