// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Html
{
    using System;
    using System.Web.Mvc;

    using Microsoft.Web.UnitTestUtil;
    using Xunit;

    public static class HtmlHelperFacts
    {
        #region Script()

        [Fact]
        public static void Script_OutputsScriptTag()
        {
            // Arrange
            HtmlHelper htmlHelper = MvcHelper.GetHtmlHelper();

            // Act
            var html = htmlHelper.Script(new Uri("http://localhost"), "scripttype");

            // Assert
            Assert.Equal(@"<script src=""//localhost/"" type=""scripttype""></script>", html.ToHtmlString());
        }

        #endregion
    }
}
