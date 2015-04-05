// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Html
{
    using System;

    using Xunit;

    public static class ScriptHelperFacts
    {
        #region Render()

        [Fact]
        public static void Script_ThrowsArgumentNullException_ForNullUri()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ScriptHelper.Render(null));
        }

        [Fact]
        public static void Script_ReturnsExpectedHtml_ForAbsoluteUri()
        {
            // Act
            var result = ScriptHelper.Render(new Uri("http://tempuri.org/assets/script.js"));

            // Assert
            Assert.Equal(@"<script src=""//tempuri.org/assets/script.js""></script>", result.ToHtmlString());
        }

        [Fact]
        public static void Script_ReturnsExpectedHtml_ForRelativeUri()
        {
            // Act
            var result = ScriptHelper.Render(new Uri("/assets/script.js", UriKind.Relative));

            // Assert
            Assert.Equal(@"<script src=""/assets/script.js""></script>", result.ToHtmlString());
        }

        [Fact]
        public static void Script_ReturnsExpectedHtml_ForNullScriptType()
        {
            // Act
            var result = ScriptHelper.Render(new Uri("http://tempuri.org/assets/script.js"), null);

            // Assert
            Assert.Equal(@"<script src=""//tempuri.org/assets/script.js""></script>", result.ToHtmlString());
        }

        [Fact]
        public static void Script_ReturnsExpectedHtml_ForEmptyScriptType()
        {
            // Act
            var result = ScriptHelper.Render(new Uri("http://tempuri.org/assets/script.js"), String.Empty);

            // Assert
            Assert.Equal(@"<script src=""//tempuri.org/assets/script.js""></script>", result.ToHtmlString());
        }

        [Fact]
        public static void Script_ReturnsExpectedHtml_ForScriptType()
        {
            // Act
            var result = ScriptHelper.Render(new Uri("http://tempuri.org/assets/script.js"), "text/javascript");

            // Assert
            Assert.Equal(@"<script src=""//tempuri.org/assets/script.js"" type=""text/javascript""></script>", result.ToHtmlString());
        }

        [Fact]
        public static void Script_ReturnsExpectedHtml_ForCustomAttribute()
        {
            // Act
            var result = ScriptHelper.Render(
                new Uri("http://tempuri.org/assets/script.js"),
                "text/javascript",
                new { attrName = "attrValue" });

            // Assert
            Assert.Equal(@"<script attrName=""attrValue"" src=""//tempuri.org/assets/script.js"" type=""text/javascript""></script>", result.ToHtmlString());
        }

        [Fact]
        public static void Script_ReturnsExpectedHtml_ForExistingCustomAttribute()
        {
            // Act
            var result = ScriptHelper.Render(
                new Uri("http://tempuri.org/assets/script.js"),
                "text/javascript",
                new { type = "scriptType" });

            // Assert
            Assert.Equal(@"<script src=""//tempuri.org/assets/script.js"" type=""scriptType""></script>", result.ToHtmlString());
        }

        #endregion
    }
}
