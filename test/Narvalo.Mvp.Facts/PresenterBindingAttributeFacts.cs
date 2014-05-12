namespace Narvalo.Mvp
{
    using Xunit;

    public static class PresenterBindingAttributeFacts
    {
        public static class TheConstructor
        {
            [Fact]
            public static void SetsBindingModeToDefault()
            {
                // Arrange
                var presenterType = typeof(TestPresenter);
                // Act
                var attribute = new PresenterBindingAttribute(presenterType);
                // Assert
                Assert.Equal(PresenterBindingMode.Default, attribute.BindingMode);
            }

            [Fact]
            public static void SetsPresenterType()
            {
                // Arrange
                var presenterType = typeof(TestPresenter);
                // Act
                var attribute = new PresenterBindingAttribute(presenterType);
                // Assert
                Assert.Equal(typeof(TestPresenter), attribute.PresenterType);
            }

            [Fact]
            public static void SetsViewTypeToNull()
            {
                // Arrange
                var presenterType = typeof(TestPresenter);
                // Act
                var attribute = new PresenterBindingAttribute(presenterType);
                // Assert
                Assert.Null(attribute.ViewType);
            }
        }

        class TestPresenter : Presenter<IView>
        {
            public TestPresenter(IView view) : base(view) { }
        }
    }
}