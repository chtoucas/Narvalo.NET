namespace Narvalo.Web.Validation
{
    using System.Collections.Generic;
    using System.Text;
    using System.Web;
    using System.Xml.Schema;

    /// <summary>
    /// HTML comments implementation of the rendering engine
    /// </summary>
    public class InlineCommentRenderer : IValidationRenderer
    {
        /// <summary>
        /// Renders the report in HTML comments added to the source
        /// </summary>
        /// <param name="response">The response object which maybe used to write/output the report</param>
        /// <param name="errors">The source data for the report</param>
        /// <param name="validationDuration">The time taken to generate the validation data</param>
        public void Render(HttpContext context, IList<ValidationEventArgs> errors) //, TimeSpan validationDuration)
        {
            StringBuilder comments = new StringBuilder();

            comments.AppendLine();
            comments.AppendLine();

            comments.AppendLine("<!-- START OF VALIDATOR REPORT ************ ");

            comments.AppendLine();

            //comments.AppendFormat("Validation took: {0}ms", validationDuration.TotalMilliseconds);

            comments.AppendLine();
            comments.AppendLine();

            if (errors.Count > 0) {
                foreach (var record in errors) {
                    comments.Append(" - ");
                    comments.AppendLine(record.ToString());
                    comments.AppendLine();
                }
            }
            else {
                comments.AppendLine("Congratulations: No errors!");
            }

            comments.AppendLine();
            comments.AppendLine("************ END OF VALIDATOR REPORT -->");

            context.Response.Write(comments.ToString());
        }
    }
}
