namespace Narvalo.Web.Html
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    // See: http://haacked.com/archive/2008/05/03/code-based-repeater-for-asp.net-mvc.aspx
    public static class RepeaterExtensions
    {
        public static void Repeater<T>(
            this HtmlHelper html,
            IEnumerable<T> items, Action<T> render, Action<T> renderAlt)
        {

            if (items == null) {
                return;
            }

            int i = 0;

            foreach (var item in items) {
                if (i++ % 2 == 0)
                    render(item);
                else
                    renderAlt(item);
            }
        }

        public static void Repeater<T>(this HtmlHelper html, Action<T> render, Action<T> renderAlt)
        {
            var items = html.ViewContext.ViewData as IEnumerable<T>;
            html.Repeater(items, render, renderAlt);
        }

        public static void Repeater<T>(this HtmlHelper html, string viewDataKey, Action<T> render, Action<T> renderAlt)
        {
            var items = html.ViewContext.ViewData as IEnumerable<T>;
            var viewData = html.ViewContext.ViewData as IDictionary<string, object>;
            if (viewData != null) {
                items = viewData[viewDataKey] as IEnumerable<T>;
            }
            else {
                items = new ViewDataDictionary(viewData)[viewDataKey] as IEnumerable<T>;
            }
            html.Repeater(items, render, renderAlt);
        }
    }
}
