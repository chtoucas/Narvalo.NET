// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Html
{
    using System;
    using System.Collections.Generic;

    using Xunit;

    public static partial class MarkupFacts
    {
        public static IEnumerable<object[]> Paths
        {
            get
            {
                yield return new object[] { "http://tempuri.org/assets/path", @"//tempuri.org/assets/path" };
                yield return new object[] { "https://tempuri.org/assets/path", @"//tempuri.org/assets/path" };
                yield return new object[] { "/assets/path", @"/assets/path" };
                yield return new object[] { "assets/path", @"assets/path" };
                yield return new object[] { "./assets/path", @"./assets/path" };
                yield return new object[] { "../assets/path", @"../assets/path" };
                yield return new object[] { "~/assets/path", @"~/assets/path" };
            }
        }

        public static IEnumerable<object[]> Uris
        {
            get
            {
                yield return new object[] { new Uri("http://tempuri.org/assets/path"), @"//tempuri.org/assets/path" };
                yield return new object[] { new Uri("https://tempuri.org/assets/path"), @"//tempuri.org/assets/path" };
                yield return new object[] { new Uri("/assets/path", UriKind.Relative), @"/assets/path" };
                yield return new object[] { new Uri("assets/path", UriKind.Relative), @"assets/path" };
                yield return new object[] { new Uri("./assets/path", UriKind.Relative), @"./assets/path" };
                yield return new object[] { new Uri("../assets/path", UriKind.Relative), @"../assets/path" };
                yield return new object[] { new Uri("~/assets/path", UriKind.Relative), @"~/assets/path" };
            }
        }
    }

    public static partial class MarkupFacts
    {
        #region Link()

        [Fact]
        public static void Link_ThrowsArgumentNullException_ForNullPath()
        {
            // Arrange
            string path = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => Markup.Link(path));
        }

        [Fact]
        public static void Link_ThrowsArgumentNullException_ForNullUri()
        {
            // Arrange
            Uri linkUri = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => Markup.Link(linkUri));
        }

        [Theory]
        [MemberData("Paths")]
        [CLSCompliant(false)]
        public static void Link_ReturnsExpectedHtml_ForPath(string value, string link)
        {
            // Arrange
            var expectedValue = @"<link href=""" + link + @""" />";

            // Act
            var result = Markup.Link(value);

            // Assert
            Assert.Equal(expectedValue, result.ToHtmlString());
        }

        [Theory]
        [MemberData("Uris")]
        [CLSCompliant(false)]
        public static void Link_ReturnsExpectedHtml_ForUri(Uri value, string link)
        {
            // Arrange
            var expectedValue = @"<link href=""" + link + @""" />";

            // Act
            var result = Markup.Link(value);

            // Assert
            Assert.Equal(expectedValue, result.ToHtmlString());
        }

        [Theory]
        [MemberData("Paths")]
        [CLSCompliant(false)]
        public static void Link_ReturnsExpectedHtml_ForPathAndNullLinkType(string value, string link)
        {
            // Arrange
            var expectedValue = @"<link href=""" + link + @""" />";

            // Act
            var result = Markup.Link(value, null);

            // Assert
            Assert.Equal(expectedValue, result.ToHtmlString());
        }

        [Theory]
        [MemberData("Uris")]
        [CLSCompliant(false)]
        public static void Link_ReturnsExpectedHtml_ForUriAndNullLinkType(Uri value, string link)
        {
            // Arrange
            var expectedValue = @"<link href=""" + link + @""" />";

            // Act
            var result = Markup.Link(value, null);

            // Assert
            Assert.Equal(expectedValue, result.ToHtmlString());
        }

        [Theory]
        [MemberData("Paths")]
        [CLSCompliant(false)]
        public static void Link_ReturnsExpectedHtml_ForPathAndEmptyLinkType(string value, string link)
        {
            // Arrange
            var expectedValue = @"<link href=""" + link + @""" />";

            // Act
            var result = Markup.Link(value, String.Empty);

            // Assert
            Assert.Equal(expectedValue, result.ToHtmlString());
        }

        [Theory]
        [MemberData("Uris")]
        [CLSCompliant(false)]
        public static void Link_ReturnsExpectedHtml_ForUriAndEmptyLinkType(Uri value, string link)
        {
            // Arrange
            var expectedValue = @"<link href=""" + link + @""" />";

            // Act
            var result = Markup.Link(value, String.Empty);

            // Assert
            Assert.Equal(expectedValue, result.ToHtmlString());
        }

        [Theory]
        [MemberData("Paths")]
        [CLSCompliant(false)]
        public static void Link_ReturnsExpectedHtml_ForPathAndLinkType(string value, string link)
        {
            // Arrange
            var expectedValue = @"<link href=""" + link + @""" type=""text/css"" />";

            // Act
            var result = Markup.Link(value, "text/css");

            // Assert
            Assert.Equal(expectedValue, result.ToHtmlString());
        }

        [Theory]
        [MemberData("Uris")]
        [CLSCompliant(false)]
        public static void Link_ReturnsExpectedHtml_ForUriAndLinkType(Uri value, string link)
        {
            // Arrange
            var expectedValue = @"<link href=""" + link + @""" type=""text/css"" />";

            // Act
            var result = Markup.Link(value, "text/css");

            // Assert
            Assert.Equal(expectedValue, result.ToHtmlString());
        }

        [Theory]
        [MemberData("Paths")]
        [CLSCompliant(false)]
        public static void Link_ReturnsExpectedHtml_ForPathAndNullRelation(string value, string link)
        {
            // Arrange
            var expectedValue = @"<link href=""" + link + @""" type=""text/css"" />";

            // Act
            var result = Markup.Link(value, "text/css", null);

            // Assert
            Assert.Equal(expectedValue, result.ToHtmlString());
        }

        [Theory]
        [MemberData("Uris")]
        [CLSCompliant(false)]
        public static void Link_ReturnsExpectedHtml_ForUriAndNullRelation(Uri value, string link)
        {
            // Arrange
            var expectedValue = @"<link href=""" + link + @""" type=""text/css"" />";

            // Act
            var result = Markup.Link(value, "text/css", null);

            // Assert
            Assert.Equal(expectedValue, result.ToHtmlString());
        }

        [Theory]
        [MemberData("Paths")]
        [CLSCompliant(false)]
        public static void Link_ReturnsExpectedHtml_ForPathAndEmptyRelation(string value, string link)
        {
            // Arrange
            var expectedValue = @"<link href=""" + link + @""" type=""text/css"" />";

            // Act
            var result = Markup.Link(value, "text/css", String.Empty);

            // Assert
            Assert.Equal(expectedValue, result.ToHtmlString());
        }

        [Theory]
        [MemberData("Uris")]
        [CLSCompliant(false)]
        public static void Link_ReturnsExpectedHtml_ForUriAndEmptyRelation(Uri value, string link)
        {
            // Arrange
            var expectedValue = @"<link href=""" + link + @""" type=""text/css"" />";

            // Act
            var result = Markup.Link(value, "text/css", String.Empty);

            // Assert
            Assert.Equal(expectedValue, result.ToHtmlString());
        }

        [Theory]
        [MemberData("Paths")]
        [CLSCompliant(false)]
        public static void Link_ReturnsExpectedHtml_ForPathAndRelation(string value, string link)
        {
            // Arrange
            var expectedValue = @"<link href=""" + link + @""" rel=""stylesheet"" type=""text/css"" />";

            // Act
            var result = Markup.Link(value, "text/css", "stylesheet");

            // Assert
            Assert.Equal(expectedValue, result.ToHtmlString());
        }

        [Theory]
        [MemberData("Uris")]
        [CLSCompliant(false)]
        public static void Link_ReturnsExpectedHtml_ForUriAndRelation(Uri value, string link)
        {
            // Arrange
            var expectedValue = @"<link href=""" + link + @""" rel=""stylesheet"" type=""text/css"" />";

            // Act
            var result = Markup.Link(value, "text/css", "stylesheet");

            // Assert
            Assert.Equal(expectedValue, result.ToHtmlString());
        }

        [Theory]
        [MemberData("Paths")]
        [CLSCompliant(false)]
        public static void Link_ReturnsExpectedHtml_ForPathAndCustomAttribute(string value, string link)
        {
            // Arrange
            var expectedValue = @"<link attrName=""attrValue"" href=""" + link + @""" rel=""stylesheet"" type=""text/css"" />";

            // Act
            var result = Markup.Link(
                value,
                "text/css",
                "stylesheet",
                new { attrName = "attrValue" });

            // Assert
            Assert.Equal(expectedValue, result.ToHtmlString());
        }

        [Theory]
        [MemberData("Uris")]
        [CLSCompliant(false)]
        public static void Link_ReturnsExpectedHtml_ForUriAndCustomAttribute(Uri value, string link)
        {
            // Arrange
            var expectedValue = @"<link attrName=""attrValue"" href=""" + link + @""" rel=""stylesheet"" type=""text/css"" />";

            // Act
            var result = Markup.Link(
                value,
                "text/css",
                "stylesheet",
                new { attrName = "attrValue" });

            // Assert
            Assert.Equal(expectedValue, result.ToHtmlString());
        }

        [Theory]
        [MemberData("Paths")]
        [CLSCompliant(false)]
        public static void Link_ReturnsExpectedHtml_ForPathAndOverriddenAttribute(string value, string link)
        {
            // Arrange
            var expectedValue = @"<link href=""" + link + @""" rel=""relation"" type=""text/css"" />";

            // Act
            var result = Markup.Link(
                value,
                "text/css",
                "stylesheet",
                new { rel = "relation" });

            // Assert
            Assert.Equal(expectedValue, result.ToHtmlString());
        }

        [Theory]
        [MemberData("Uris")]
        [CLSCompliant(false)]
        public static void Link_ReturnsExpectedHtml_ForUriAndOverriddenAttribute(Uri value, string link)
        {
            // Arrange
            var expectedValue = @"<link href=""" + link + @""" rel=""relation"" type=""text/css"" />";

            // Act
            var result = Markup.Link(
                value,
                "text/css",
                "stylesheet",
                new { rel = "relation" });

            // Assert
            Assert.Equal(expectedValue, result.ToHtmlString());
        }

        #endregion

        #region Script()

        [Fact]
        public static void Script_ThrowsArgumentNullException_ForNullPath()
        {
            // Arrange
            string path = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => Markup.Script(path));
        }

        [Fact]
        public static void Script_ThrowsArgumentNullException_ForNullUri()
        {
            // Arrange
            Uri scriptUri = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => Markup.Script(scriptUri));
        }

        [Theory]
        [MemberData("Paths")]
        [CLSCompliant(false)]
        public static void Script_ReturnsExpectedHtml_ForPath(string value, string link)
        {
            // Arrange
            var expectedValue = @"<script src=""" + link + @"""></script>";

            // Act
            var result = Markup.Script(value);

            // Assert
            Assert.Equal(expectedValue, result.ToHtmlString());
        }

        [Theory]
        [MemberData("Uris")]
        [CLSCompliant(false)]
        public static void Script_ReturnsExpectedHtml_ForUri(Uri value, string link)
        {
            // Arrange
            var expectedValue = @"<script src=""" + link + @"""></script>";

            // Act
            var result = Markup.Script(value);

            // Assert
            Assert.Equal(expectedValue, result.ToHtmlString());
        }

        [Theory]
        [MemberData("Paths")]
        [CLSCompliant(false)]
        public static void Script_ReturnsExpectedHtml_ForPathAndNullScriptType(string value, string link)
        {
            // Arrange
            var expectedValue = @"<script src=""" + link + @"""></script>";

            // Act
            var result = Markup.Script(value, null);

            // Assert
            Assert.Equal(expectedValue, result.ToHtmlString());
        }

        [Theory]
        [MemberData("Uris")]
        [CLSCompliant(false)]
        public static void Script_ReturnsExpectedHtml_ForUriAndNullScriptType(Uri value, string link)
        {
            // Arrange
            var expectedValue = @"<script src=""" + link + @"""></script>";

            // Act
            var result = Markup.Script(value, null);

            // Assert
            Assert.Equal(expectedValue, result.ToHtmlString());
        }

        [Theory]
        [MemberData("Paths")]
        [CLSCompliant(false)]
        public static void Script_ReturnsExpectedHtml_ForPathAndEmptyScriptType(string value, string link)
        {
            // Arrange
            var expectedValue = @"<script src=""" + link + @"""></script>";

            // Act
            var result = Markup.Script(value, String.Empty);

            // Assert
            Assert.Equal(expectedValue, result.ToHtmlString());
        }

        [Theory]
        [MemberData("Uris")]
        [CLSCompliant(false)]
        public static void Script_ReturnsExpectedHtml_ForUriAndEmptyScriptType(Uri value, string link)
        {
            // Arrange
            var expectedValue = @"<script src=""" + link + @"""></script>";

            // Act
            var result = Markup.Script(value, String.Empty);

            // Assert
            Assert.Equal(expectedValue, result.ToHtmlString());
        }

        [Theory]
        [MemberData("Paths")]
        [CLSCompliant(false)]
        public static void Script_ReturnsExpectedHtml_ForPathAndScriptType(string value, string link)
        {
            // Arrange
            var expectedValue = @"<script src=""" + link + @""" type=""text/javascript""></script>";

            // Act
            var result = Markup.Script(value, "text/javascript");

            // Assert
            Assert.Equal(expectedValue, result.ToHtmlString());
        }

        [Theory]
        [MemberData("Uris")]
        [CLSCompliant(false)]
        public static void Script_ReturnsExpectedHtml_ForUriAndScriptType(Uri value, string link)
        {
            // Arrange
            var expectedValue = @"<script src=""" + link + @""" type=""text/javascript""></script>";

            // Act
            var result = Markup.Script(value, "text/javascript");

            // Assert
            Assert.Equal(expectedValue, result.ToHtmlString());
        }

        [Theory]
        [MemberData("Paths")]
        [CLSCompliant(false)]
        public static void Script_ReturnsExpectedHtml_ForPathAndCustomAttribute(string value, string link)
        {
            // Arrange
            var expectedValue = @"<script attrName=""attrValue"" src=""" + link + @""" type=""text/javascript""></script>";

            // Act
            var result = Markup.Script(
                value,
                "text/javascript",
                new { attrName = "attrValue" });

            // Assert
            Assert.Equal(expectedValue, result.ToHtmlString());
        }

        [Theory]
        [MemberData("Uris")]
        [CLSCompliant(false)]
        public static void Script_ReturnsExpectedHtml_ForUriAndCustomAttribute(Uri value, string link)
        {
            // Arrange
            var expectedValue = @"<script attrName=""attrValue"" src=""" + link + @""" type=""text/javascript""></script>";

            // Act
            var result = Markup.Script(
                value,
                "text/javascript",
                new { attrName = "attrValue" });

            // Assert
            Assert.Equal(expectedValue, result.ToHtmlString());
        }

        [Theory]
        [MemberData("Paths")]
        [CLSCompliant(false)]
        public static void Script_ReturnsExpectedHtml_ForPathAndOverriddenAttribute(string value, string link)
        {
            // Arrange
            var expectedValue = @"<script src=""" + link + @""" type=""scriptType""></script>";

            // Act
            var result = Markup.Script(
                value,
                "text/javascript",
                new { type = "scriptType" });

            // Assert
            Assert.Equal(expectedValue, result.ToHtmlString());
        }

        [Theory]
        [MemberData("Uris")]
        [CLSCompliant(false)]
        public static void Script_ReturnsExpectedHtml_ForUriAndOverriddenAttribute(Uri value, string link)
        {
            // Arrange
            var expectedValue = @"<script src=""" + link + @""" type=""scriptType""></script>";

            // Act
            var result = Markup.Script(
                value,
                "text/javascript",
                new { type = "scriptType" });

            // Assert
            Assert.Equal(expectedValue, result.ToHtmlString());
        }

        #endregion
    }
}
