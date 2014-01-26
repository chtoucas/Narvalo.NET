namespace Narvalo.Web.Html {
    using System;
    using Microsoft.Web.UnitTestUtil;
    using Xunit;

    public class ScriptExtensionsTests {
        #region ScriptLink()

        [Fact(DisplayName = "ScriptExtensions.ScriptLink() should create a minimal script tag")]
        public void ScriptLink() {
            // Arrange
            var htmlHelper = MvcHelper.GetHtmlHelper();
            // Act
            var html = htmlHelper.Script(new Uri("http://localhost"), "scripttype");
            // Assert
            Assert.Equal(@"<script src=""//localhost/"" type=""scripttype""></script>", html.ToHtmlString());
        }

        #endregion
    }
}
