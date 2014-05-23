// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System;
    using NSubstitute;
    using Xunit;

    public static class CompositeViewFacts
    {
        public static class AddMethod
        {
            [Fact]
            public static void ThrowsArgumentNullException_ForNullView()
            {
                // Arrange
                var compositeView = new MyCompositeView<IView<Object>>();

                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => compositeView.Add(view: null));
            }

            [Fact]
            public static void ThrowsArgumentException_ForViewOfWrongType()
            {
                // Arrange
                var compositeView = new MyCompositeView<IView<Object>>();

                // Act & Assert
                Assert.Throws<ArgumentException>(() => compositeView.Add(Substitute.For<IView>()));
            }

            [Fact]
            public static void AddsViewsToList()
            {
                // Arrange
                var compositeView = new MyCompositeView<IView<Object>>();
                var view1 = Substitute.For<IView<Object>>();
                var view2 = Substitute.For<IView<Object>>();

                // Act
                compositeView.Add(view1);
                compositeView.Add(view2);

                // Assert
                Assert.Equal(new[] { view1, view2 }, compositeView.Views);
            }
        }

        class MyCompositeView<TView> : CompositeView<TView> where TView : IView
        {
            public override event EventHandler Load
            {
                add { throw new NotImplementedException(); }
                remove { throw new NotImplementedException(); }
            }
        }
    }
}