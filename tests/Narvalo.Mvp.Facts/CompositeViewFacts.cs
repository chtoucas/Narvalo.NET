// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System;
    using NSubstitute;
    using Xunit;

    public static partial class CompositeViewFacts
    {
        public static partial class AddMethod
        {
            [Fact]
            public static void ThrowsArgumentNullException_ForNullView()
            {
                // Arrange
                var view = new MyCompositeView<IView<Object>>();

                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => view.Add(view: null));
            }

            [Fact]
            public static void ThrowsArgumentException_ForViewOfWrongType()
            {
                // Arrange
                var view = new MyCompositeView<IView<String>>();

                // Act & Assert
                Assert.Throws<ArgumentException>(() => view.Add(Substitute.For<IView>()));
                Assert.Throws<ArgumentException>(() => view.Add(Substitute.For<IView<Int32>>()));
            }
        }

        #region Helper classes

        public class MyCompositeView<TView> : CompositeView<TView> where TView : IView
        {
            public override event EventHandler Load
            {
                add { throw new NotImplementedException(); }
                remove { throw new NotImplementedException(); }
            }
        }

        #endregion
    }

#if !NO_INTERNALS_VISIBLE_TO

    public static partial class CompositeViewFacts
    {
        public static partial class AddMethod
        {
            [Fact]
            public static void AddsViewsToList()
            {
                // Arrange
                var view = new MyCompositeView<IView<Object>>();
                var view1 = Substitute.For<IView<Object>>();
                var view2 = Substitute.For<IView<Object>>();

                // Act
                view.Add(view1);
                view.Add(view2);

                // Assert
                Assert.Equal(new[] { view1, view2 }, view.Views);
            }
        }
    }

#endif
}