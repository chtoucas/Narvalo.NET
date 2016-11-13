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
                yield return new object[] { "http://tempuri.org/assets/path" };
                yield return new object[] { "https://tempuri.org/assets/path" };
                yield return new object[] { "//tempuri.org/assets/path" };
                yield return new object[] { "/assets/path" };
                yield return new object[] { "assets/path" };
                yield return new object[] { "./assets/path" };
                yield return new object[] { "../assets/path" };
                yield return new object[] { "~/assets/path" };
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

        [Theory]
        [MemberData(nameof(Paths), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Link_ReturnsExpectedHtml(string value)
        {
            // Arrange
            var expectedValue = @"<link href=""" + value + @""" />";

            // Act
            var result = Markup.Link(value);

            // Assert
            Assert.Equal(expectedValue, result.ToHtmlString());
        }

        [Theory]
        [MemberData(nameof(Paths), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Link_ReturnsExpectedHtml_ForNullLinkType(string value)
        {
            // Arrange
            var expectedValue = @"<link href=""" + value + @""" />";

            // Act
            var result = Markup.Link(value, null);

            // Assert
            Assert.Equal(expectedValue, result.ToHtmlString());
        }

        [Theory]
        [MemberData(nameof(Paths), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Link_ReturnsExpectedHtml_ForEmptyLinkType(string value)
        {
            // Arrange
            var expectedValue = @"<link href=""" + value + @""" />";

            // Act
            var result = Markup.Link(value, String.Empty);

            // Assert
            Assert.Equal(expectedValue, result.ToHtmlString());
        }

        [Theory]
        [MemberData(nameof(Paths), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Link_ReturnsExpectedHtml_ForNonNullOrEmptyLinkType(string value)
        {
            // Arrange
            var expectedValue = @"<link href=""" + value + @""" type=""text/css"" />";

            // Act
            var result = Markup.Link(value, "text/css");

            // Assert
            Assert.Equal(expectedValue, result.ToHtmlString());
        }

        [Theory]
        [MemberData(nameof(Paths), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Link_ReturnsExpectedHtml_ForNullRelation(string value)
        {
            // Arrange
            var expectedValue = @"<link href=""" + value + @""" type=""text/css"" />";

            // Act
            var result = Markup.Link(value, "text/css", null);

            // Assert
            Assert.Equal(expectedValue, result.ToHtmlString());
        }

        [Theory]
        [MemberData(nameof(Paths), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Link_ReturnsExpectedHtml_ForEmptyRelation(string value)
        {
            // Arrange
            var expectedValue = @"<link href=""" + value + @""" type=""text/css"" />";

            // Act
            var result = Markup.Link(value, "text/css", String.Empty);

            // Assert
            Assert.Equal(expectedValue, result.ToHtmlString());
        }

        [Theory]
        [MemberData(nameof(Paths), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Link_ReturnsExpectedHtml_ForNonNullOrEmptyRelation(string value)
        {
            // Arrange
            var expectedValue = @"<link href=""" + value + @""" rel=""stylesheet"" type=""text/css"" />";

            // Act
            var result = Markup.Link(value, "text/css", "stylesheet");

            // Assert
            Assert.Equal(expectedValue, result.ToHtmlString());
        }

        [Theory]
        [MemberData(nameof(Paths), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Link_ReturnsExpectedHtml_ForCustomAttribute(string value)
        {
            // Arrange
            var expectedValue =
                @"<link attrName=""attrValue"" href=""" + value + @""" rel=""stylesheet"" type=""text/css"" />";

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
        [MemberData(nameof(Paths), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Link_ReturnsExpectedHtml_ForOverriddenAttribute(string value)
        {
            // Arrange
            var expectedValue = @"<link href=""" + value + @""" rel=""relation"" type=""text/css"" />";

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

        [Theory]
        [MemberData(nameof(Paths), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Script_ReturnsExpectedHtml(string value)
        {
            // Arrange
            var expectedValue = @"<script src=""" + value + @"""></script>";

            // Act
            var result = Markup.Script(value);

            // Assert
            Assert.Equal(expectedValue, result.ToHtmlString());
        }

        [Theory]
        [MemberData(nameof(Paths), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Script_ReturnsExpectedHtml_ForNullScriptType(string value)
        {
            // Arrange
            var expectedValue = @"<script src=""" + value + @"""></script>";

            // Act
            var result = Markup.Script(value, null);

            // Assert
            Assert.Equal(expectedValue, result.ToHtmlString());
        }

        [Theory]
        [MemberData(nameof(Paths), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Script_ReturnsExpectedHtml_ForEmptyScriptType(string value)
        {
            // Arrange
            var expectedValue = @"<script src=""" + value + @"""></script>";

            // Act
            var result = Markup.Script(value, String.Empty);

            // Assert
            Assert.Equal(expectedValue, result.ToHtmlString());
        }

        [Theory]
        [MemberData(nameof(Paths), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Script_ReturnsExpectedHtml_ForNonNullOrEmptyScriptType(string value)
        {
            // Arrange
            var expectedValue = @"<script src=""" + value + @""" type=""text/javascript""></script>";

            // Act
            var result = Markup.Script(value, "text/javascript");

            // Assert
            Assert.Equal(expectedValue, result.ToHtmlString());
        }

        [Theory]
        [MemberData(nameof(Paths), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Script_ReturnsExpectedHtml_ForCustomAttribute(string value)
        {
            // Arrange
            var expectedValue =
                @"<script attrName=""attrValue"" src=""" + value + @""" type=""text/javascript""></script>";

            // Act
            var result = Markup.Script(
                value,
                "text/javascript",
                new { attrName = "attrValue" });

            // Assert
            Assert.Equal(expectedValue, result.ToHtmlString());
        }

        [Theory]
        [MemberData(nameof(Paths), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Script_ReturnsExpectedHtml_ForOverriddenAttribute(string value)
        {
            // Arrange
            var expectedValue = @"<script src=""" + value + @""" type=""scriptType""></script>";

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
