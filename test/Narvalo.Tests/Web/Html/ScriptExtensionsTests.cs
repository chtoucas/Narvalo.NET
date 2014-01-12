namespace Narvalo.Web.Html {
    using System;
    using System.Web.Mvc;
    using Microsoft.Web.UnitTestUtil;
    using Xunit;

    public class ScriptExtensionsTests {
        #region ScriptLink()

        [Fact(DisplayName = "ScriptExtensions.ScriptLink() should create a minimal script tag")]
        public void ScriptLink() {
            // Arrange
            HtmlHelper htmlHelper = MvcHelper.GetHtmlHelper();
            // Act
            var html = htmlHelper.Script(new Uri("http://localhost"), "scripttype");
            // Assert
            Assert.Equal(@"<script src=""newscript"" type=""scripttype""></script>", html.ToHtmlString());
        }

        #endregion
    }
}
