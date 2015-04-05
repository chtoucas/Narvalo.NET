// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Html
{
    using System;

    using Xunit;

    public static class LinkHelperFacts
    {
        #region Render()

        [Fact]
        public static void Render_ThrowsArgumentNullException_ForNullUri()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => LinkHelper.Render(null));
        }

        [Fact]
        public static void Render_ReturnsExpectedHtml_ForAbsoluteUri()
        {
            // Act
            var result = LinkHelper.Render(new Uri("http://tempuri.org/assets/style.css"));

            // Assert
            Assert.Equal(@"<link href=""//tempuri.org/assets/style.css"" />", result.ToHtmlString());
        }

        [Fact]
        public static void Render_ReturnsExpectedHtml_ForRelativeUri()
        {
            // Act
            var result = LinkHelper.Render(new Uri("/assets/style.css", UriKind.Relative));

            // Assert
            Assert.Equal(@"<link href=""/assets/style.css"" />", result.ToHtmlString());
        }

        [Fact]
        public static void Render_ReturnsExpectedHtml_ForNullLinkType()
        {
            // Act
            var result = LinkHelper.Render(new Uri("http://tempuri.org/assets/style.css"), null);

            // Assert
            Assert.Equal(@"<link href=""//tempuri.org/assets/style.css"" />", result.ToHtmlString());
        }

        [Fact]
        public static void Render_ReturnsExpectedHtml_ForEmptyLinkType()
        {
            // Act
            var result = LinkHelper.Render(new Uri("http://tempuri.org/assets/style.css"), String.Empty);

            // Assert
            Assert.Equal(@"<link href=""//tempuri.org/assets/style.css"" />", result.ToHtmlString());
        }

        [Fact]
        public static void Render_ReturnsExpectedHtml_ForLinkType()
        {
            // Act
            var result = LinkHelper.Render(new Uri("http://tempuri.org/assets/style.css"), "text/css");

            // Assert
            Assert.Equal(@"<link href=""//tempuri.org/assets/style.css"" type=""text/css"" />", result.ToHtmlString());
        }

        [Fact]
        public static void Render_ReturnsExpectedHtml_ForNullRelation()
        {
            // Act
            var result = LinkHelper.Render(new Uri("http://tempuri.org/assets/style.css"), "text/css", null);

            // Assert
            Assert.Equal(@"<link href=""//tempuri.org/assets/style.css"" type=""text/css"" />", result.ToHtmlString());
        }

        [Fact]
        public static void Render_ReturnsExpectedHtml_ForEmptyRelation()
        {
            // Act
            var result = LinkHelper.Render(new Uri("http://tempuri.org/assets/style.css"), "text/css", String.Empty);

            // Assert
            Assert.Equal(@"<link href=""//tempuri.org/assets/style.css"" type=""text/css"" />", result.ToHtmlString());
        }

        [Fact]
        public static void Render_ReturnsExpectedHtml_ForRelation()
        {
            // Act
            var result = LinkHelper.Render(new Uri("http://tempuri.org/assets/style.css"), "text/css", "stylesheet");

            // Assert
            Assert.Equal(@"<link href=""//tempuri.org/assets/style.css"" rel=""stylesheet"" type=""text/css"" />", result.ToHtmlString());
        }

        [Fact]
        public static void Render_ReturnsExpectedHtml_ForCustomAttribute()
        {
            // Act
            var result = LinkHelper.Render(
                new Uri("http://tempuri.org/assets/style.css"),
                "text/css",
                "stylesheet",
                new { attrName = "attrValue" });

            // Assert
            Assert.Equal(@"<link attrName=""attrValue"" href=""//tempuri.org/assets/style.css"" rel=""stylesheet"" type=""text/css"" />", result.ToHtmlString());
        }

        [Fact]
        public static void Render_ReturnsExpectedHtml_ForExistingCustomAttribute()
        {
            // Act
            var result = LinkHelper.Render(
                new Uri("http://tempuri.org/assets/style.css"),
                "text/css",
                "stylesheet",
                new { rel = "relation" });

            // Assert
            Assert.Equal(@"<link href=""//tempuri.org/assets/style.css"" rel=""relation"" type=""text/css"" />", result.ToHtmlString());
        }

        #endregion
    }
}
