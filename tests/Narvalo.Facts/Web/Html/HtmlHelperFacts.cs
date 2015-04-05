// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Html
{
    using System;
    using System.Web.Mvc;

    using Microsoft.Web.UnitTestUtil;
    using Xunit;

    public static class HtmlHelperFacts
    {
        #region LoremIpsum()

        [Fact]
        public static void LoremIpsum_DoesNotThrow_ForNullObject()
        {
            // Arrange
            HtmlHelper htmlHelper = null;

            // Act
            htmlHelper.LoremIpsum();
        }

        #endregion

        #region Image()

        [Fact]
        public static void Image_DoesNotThrow_ForNullObject()
        {
            // Arrange
            HtmlHelper htmlHelper = null;

            // Act
            htmlHelper.Image(new Uri("http://tempuri.org/assets/image.jpg"));
        }

        #endregion

        #region Link()

        [Fact]
        public static void Link_ThrowsArgumentNullException_ForNullUri()
        {
            // Arrange
            HtmlHelper htmlHelper = MvcHelper.GetHtmlHelper();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => htmlHelper.Link(null));
        }

        [Fact]
        public static void Link_DoesNotThrow_ForNullObject()
        {
            // Arrange
            HtmlHelper htmlHelper = null;

            // Act
            htmlHelper.Link(new Uri("http://tempuri.org/assets/style.css"));
        }

        [Fact]
        public static void Link_ReturnsExpectedHtml_ForAbsoluteUri()
        {
            // Arrange
            HtmlHelper htmlHelper = MvcHelper.GetHtmlHelper();

            // Act
            var result = htmlHelper.Link(new Uri("http://tempuri.org/assets/style.css"));

            // Assert
            Assert.Equal(@"<link href=""//tempuri.org/assets/style.css"" />", result.ToHtmlString());
        }

        [Fact]
        public static void Link_ReturnsExpectedHtml_ForRelativeUri()
        {
            // Arrange
            HtmlHelper htmlHelper = MvcHelper.GetHtmlHelper();

            // Act
            var result = htmlHelper.Link(new Uri("/assets/style.css", UriKind.Relative));

            // Assert
            Assert.Equal(@"<link href=""/assets/style.css"" />", result.ToHtmlString());
        }

        [Fact]
        public static void Link_ReturnsExpectedHtml_ForNullLinkType()
        {
            // Arrange
            HtmlHelper htmlHelper = MvcHelper.GetHtmlHelper();

            // Act
            var result = htmlHelper.Link(new Uri("http://tempuri.org/assets/style.css"), null);

            // Assert
            Assert.Equal(@"<link href=""//tempuri.org/assets/style.css"" />", result.ToHtmlString());
        }

        [Fact]
        public static void Link_ReturnsExpectedHtml_ForEmptyLinkType()
        {
            // Arrange
            HtmlHelper htmlHelper = MvcHelper.GetHtmlHelper();

            // Act
            var result = htmlHelper.Link(new Uri("http://tempuri.org/assets/style.css"), String.Empty);

            // Assert
            Assert.Equal(@"<link href=""//tempuri.org/assets/style.css"" />", result.ToHtmlString());
        }

        [Fact]
        public static void Link_ReturnsExpectedHtml_ForLinkType()
        {
            // Arrange
            HtmlHelper htmlHelper = MvcHelper.GetHtmlHelper();

            // Act
            var result = htmlHelper.Link(new Uri("http://tempuri.org/assets/style.css"), "text/css");

            // Assert
            Assert.Equal(@"<link href=""//tempuri.org/assets/style.css"" type=""text/css"" />", result.ToHtmlString());
        }

        [Fact]
        public static void Link_ReturnsExpectedHtml_ForNullRelation()
        {
            // Arrange
            HtmlHelper htmlHelper = MvcHelper.GetHtmlHelper();

            // Act
            var result = htmlHelper.Link(new Uri("http://tempuri.org/assets/style.css"), "text/css", null);

            // Assert
            Assert.Equal(@"<link href=""//tempuri.org/assets/style.css"" type=""text/css"" />", result.ToHtmlString());
        }

        [Fact]
        public static void Link_ReturnsExpectedHtml_ForEmptyRelation()
        {
            // Arrange
            HtmlHelper htmlHelper = MvcHelper.GetHtmlHelper();

            // Act
            var result = htmlHelper.Link(new Uri("http://tempuri.org/assets/style.css"), "text/css", String.Empty);

            // Assert
            Assert.Equal(@"<link href=""//tempuri.org/assets/style.css"" type=""text/css"" />", result.ToHtmlString());
        }

        [Fact]
        public static void Link_ReturnsExpectedHtml_ForRelation()
        {
            // Arrange
            HtmlHelper htmlHelper = MvcHelper.GetHtmlHelper();

            // Act
            var result = htmlHelper.Link(new Uri("http://tempuri.org/assets/style.css"), "text/css", "stylesheet");

            // Assert
            Assert.Equal(@"<link href=""//tempuri.org/assets/style.css"" rel=""stylesheet"" type=""text/css"" />", result.ToHtmlString());
        }

        [Fact]
        public static void Link_ReturnsExpectedHtml_ForCustomAttribute()
        {
            // Arrange
            HtmlHelper htmlHelper = MvcHelper.GetHtmlHelper();

            // Act
            var result = htmlHelper.Link(
                new Uri("http://tempuri.org/assets/style.css"),
                "text/css",
                "stylesheet",
                new { attrName = "attrValue" });

            // Assert
            Assert.Equal(@"<link attrName=""attrValue"" href=""//tempuri.org/assets/style.css"" rel=""stylesheet"" type=""text/css"" />", result.ToHtmlString());
        }

        [Fact]
        public static void Link_ReturnsExpectedHtml_ForExistingCustomAttribute()
        {
            // Arrange
            HtmlHelper htmlHelper = MvcHelper.GetHtmlHelper();

            // Act
            var result = htmlHelper.Link(
                new Uri("http://tempuri.org/assets/style.css"),
                "text/css",
                "stylesheet",
                new { rel = "relation" });

            // Assert
            Assert.Equal(@"<link href=""//tempuri.org/assets/style.css"" rel=""relation"" type=""text/css"" />", result.ToHtmlString());
        }

        #endregion

        #region Script()

        [Fact]
        public static void Script_ThrowsArgumentNullException_ForNullUri()
        {
            // Arrange
            HtmlHelper htmlHelper = MvcHelper.GetHtmlHelper();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => htmlHelper.Script(null));
        }

        [Fact]
        public static void Script_DoesNotThrow_ForNullObject()
        {
            // Arrange
            HtmlHelper htmlHelper = null;

            // Act
            htmlHelper.Script(new Uri("http://tempuri.org/assets/script.js"));
        }

        [Fact]
        public static void Script_ReturnsExpectedHtml_ForAbsoluteUri()
        {
            // Arrange
            HtmlHelper htmlHelper = MvcHelper.GetHtmlHelper();

            // Act
            var result = htmlHelper.Script(new Uri("http://tempuri.org/assets/script.js"));

            // Assert
            Assert.Equal(@"<script src=""//tempuri.org/assets/script.js""></script>", result.ToHtmlString());
        }

        [Fact]
        public static void Script_ReturnsExpectedHtml_ForRelativeUri()
        {
            // Arrange
            HtmlHelper htmlHelper = MvcHelper.GetHtmlHelper();

            // Act
            var result = htmlHelper.Script(new Uri("/assets/script.js", UriKind.Relative));

            // Assert
            Assert.Equal(@"<script src=""/assets/script.js""></script>", result.ToHtmlString());
        }

        [Fact]
        public static void Script_ReturnsExpectedHtml_ForNullScriptType()
        {
            // Arrange
            HtmlHelper htmlHelper = MvcHelper.GetHtmlHelper();

            // Act
            var result = htmlHelper.Script(new Uri("http://tempuri.org/assets/script.js"), null);

            // Assert
            Assert.Equal(@"<script src=""//tempuri.org/assets/script.js""></script>", result.ToHtmlString());
        }

        [Fact]
        public static void Script_ReturnsExpectedHtml_ForEmptyScriptType()
        {
            // Arrange
            HtmlHelper htmlHelper = MvcHelper.GetHtmlHelper();

            // Act
            var result = htmlHelper.Script(new Uri("http://tempuri.org/assets/script.js"), String.Empty);

            // Assert
            Assert.Equal(@"<script src=""//tempuri.org/assets/script.js""></script>", result.ToHtmlString());
        }

        [Fact]
        public static void Script_ReturnsExpectedHtml_ForScriptType()
        {
            // Arrange
            HtmlHelper htmlHelper = MvcHelper.GetHtmlHelper();

            // Act
            var result = htmlHelper.Script(new Uri("http://tempuri.org/assets/script.js"), "text/javascript");

            // Assert
            Assert.Equal(@"<script src=""//tempuri.org/assets/script.js"" type=""text/javascript""></script>", result.ToHtmlString());
        }

        [Fact]
        public static void Script_ReturnsExpectedHtml_ForCustomAttribute()
        {
            // Arrange
            HtmlHelper htmlHelper = MvcHelper.GetHtmlHelper();

            // Act
            var result = htmlHelper.Script(
                new Uri("http://tempuri.org/assets/script.js"),
                "text/javascript",
                new { attrName = "attrValue" });

            // Assert
            Assert.Equal(@"<script attrName=""attrValue"" src=""//tempuri.org/assets/script.js"" type=""text/javascript""></script>", result.ToHtmlString());
        }

        [Fact]
        public static void Script_ReturnsExpectedHtml_ForExistingCustomAttribute()
        {
            // Arrange
            HtmlHelper htmlHelper = MvcHelper.GetHtmlHelper();

            // Act
            var result = htmlHelper.Script(
                new Uri("http://tempuri.org/assets/script.js"),
                "text/javascript",
                new { type = "scriptType" });

            // Assert
            Assert.Equal(@"<script src=""//tempuri.org/assets/script.js"" type=""scriptType""></script>", result.ToHtmlString());
        }

        #endregion
    }
}
