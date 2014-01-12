namespace Narvalo.Web.Validation
{
    using System.Collections.Generic;
    using System.Text;
    using System.Web;
    using System.Xml.Schema;

    /// <summary>
    /// HTML implementation of the rendering engine
    /// </summary>
    public class InlineHtmlRenderer : IValidationRenderer
    {
        // default style is inline
        private string _divStyle = "clear:both;padding:10px;";

        protected string DivStyle
        {
            get { return _divStyle; }
            set { _divStyle = value; }
        }

        /// <summary>
        /// Renders the report in HTML format
        /// </summary>
        /// <param name="response">The response object which maybe used to write/output the report</param>
        /// <param name="errors">The source data for the report</param>
        /// <param name="validationDuration">The time taken to generate the validation data</param>
        public void Render(HttpContext context, IList<ValidationEventArgs> errors) //, TimeSpan validationDuration)
        {
            StringBuilder body = new StringBuilder();

            body.AppendLine();
            body.AppendLine();

            body.AppendLine("<!-- START OF VALIDATOR REPORT || this html would not appear in the original document and, unfortunately, this paradoxically invalidates your source :\\ -->");

            body.AppendLine();
            body.AppendLine();

            body.Append("<div id='tjocWebValidatorModuleReport' style='");
            body.Append(_divStyle);

            if (errors.Count > 0) {
                body.Append("background-color:pink;border:2px solid red;'>");
            }
            else {
                body.Append("background-color:#cfc;border:2px solid green;'>");
            }

            //body.AppendFormat("<b><u>XHTML Report</u></b> (Validation took: {0}ms)<br />", validationDuration.TotalMilliseconds);

            if (errors.Count > 0) {
                body.Append("<ul>");
                foreach (var record in errors) {
                    body.Append("<li>");
                    body.Append(HttpUtility.HtmlEncode(record.ToString()));
                    body.Append("</li>");
                }
                body.Append("</ul>");
            }
            else {
                body.Append("<ul><li>Congratulations: No errors!</li></ul>");
            }

            body.Append("<a href=\"javascript:;\" onclick=\"document.getElementById('tjocWebValidatorModuleReport').parentNode.removeChild(document.getElementById('tjocWebValidatorModuleReport'));\">close</a>");
            body.Append("</div>");

            context.Response.Write(body.ToString());
        }
    }
}
