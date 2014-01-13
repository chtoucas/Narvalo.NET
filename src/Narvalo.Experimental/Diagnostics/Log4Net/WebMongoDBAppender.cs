namespace Narvalo.Diagnostics.Log4Net
{
    using System;
    using System.Web;
    using log4net.Core;
    using MongoDB.Bson;

    public class WebMongoDBAppender : MongoDBAppenderBase
    {
        public WebMongoDBAppender() : base() { }

        protected override BsonDocument LoggingEventToBson(LoggingEvent loggingEvent)
        {
            if (loggingEvent == null) {
                throw new ArgumentNullException("loggingEvent");
            }

            var doc = new BsonDocument();

            doc["timestamp"] = loggingEvent.TimeStamp;
            doc["level"] = loggingEvent.Level.ToString();
            doc["message"] = loggingEvent.RenderedMessage;
            doc["logger"] = loggingEvent.LoggerName;
            doc["machine"] = MachineName;

            if (HttpContext.Current != null) {
                // peut arriver quand on utilise trySkipIisCustomErrors

                var req = HttpContext.Current.Request;

                doc["domain"] = req.Url.Host;
                doc["rawUrl"] = req.RawUrl;
                if (req.UrlReferrer != null) {
                    doc["referrer"] = req.UrlReferrer.ToString();
                }
                if (!String.IsNullOrEmpty(req.UserHostAddress)) {
                    doc["ip"] = req.UserHostAddress;
                }
                doc["ua"] = req.UserAgent;
            }

            // Exception.
            if (loggingEvent.ExceptionObject != null) {
                doc["exceptionType"] = loggingEvent.ExceptionObject.GetType().ToString();
                doc["exception"] = ExceptionToBson(loggingEvent.ExceptionObject);
            }

            return doc;
        }
    }
}
