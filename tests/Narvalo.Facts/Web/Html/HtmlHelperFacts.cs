// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Html
{
    using System;
    using Microsoft.Web.UnitTestUtil;
    using Xunit;

    public class HtmlHelperFacts
    {
        public static class TheScriptMethod
        {
            [Fact]
            public static void ScriptLink()
            {
                // Arrange
                var htmlHelper = MvcHelper.GetHtmlHelper();
                // Act
                var html = htmlHelper.Script(new Uri("http://localhost"), "scripttype");
                // Assert
                Assert.Equal(@"<script src=""//localhost/"" type=""scripttype""></script>", html.ToHtmlString());
            }
        }
    }
}
