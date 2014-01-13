namespace Narvalo.Diagnostics.Log4Net
{
    using System;
    using log4net.Core;
    using MongoDB.Bson;

    public class DesktopMongoDBAppender : MongoDBAppenderBase
    {
        public DesktopMongoDBAppender() : base() { }

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
            doc["domain"] = loggingEvent.Domain;

            // Exception.
            if (loggingEvent.ExceptionObject != null) {
                doc["exceptionType"] = loggingEvent.ExceptionObject.GetType().ToString();
                doc["exception"] = ExceptionToBson(loggingEvent.ExceptionObject);
            }

            return doc;
        }
    }
}
