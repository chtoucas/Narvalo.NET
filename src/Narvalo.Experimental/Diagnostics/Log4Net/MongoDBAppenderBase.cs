namespace Narvalo.Diagnostics.Log4Net
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using log4net.Appender;
    using log4net.Core;
    using MongoDB.Bson;
    using MongoDB.Driver;

    // See : https://github.com/jsk/log4mongo-net
    // See : https://github.com/g0t4/DotNetExtensions/tree/master/mongo4log4net/mongo4log4net
    public abstract class MongoDBAppenderBase : AppenderSkeleton
    {
        protected static readonly string MachineName = Environment.MachineName;

        private string _databaseName = String.Empty;
        private MongoCollection _collection;
        private string _collectionName = String.Empty;
        private string _connectionString = String.Empty;
        private MongoServer _server;

        protected MongoDBAppenderBase() : base() { }

        /// <summary>
        /// Mongo collection used for logs
        /// The main reason of exposing this is to have same log collection available for unit tests
        /// </summary>
        public MongoCollection LogCollection
        {
            get { return _collection; }
        }

        #region Configuration properties

        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        public string CollectionName
        {
            get { return _collectionName; }
            set { _collectionName = value; }
        }

        public string DatabaseName
        {
            get { return _databaseName; }
            set { _databaseName = value; }
        }

        #endregion

        protected override bool RequiresLayout
        {
            get { return false; }
        }

        /// <summary>
        /// Create BSON representation of LoggingEvent
        /// </summary>
        /// <param name="loggingEvent"></param>
        /// <returns></returns>
        protected abstract BsonDocument LoggingEventToBson(LoggingEvent loggingEvent);

        [SuppressMessage("Microsoft.Globalization", "CA1303:Ne pas passer de littéraux en paramètres localisés", MessageId = "log4net.Core.IErrorHandler.Error(System.String,System.Exception,log4net.Core.ErrorCode)"), SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public override void ActivateOptions()
        {
            base.ActivateOptions();

            try {
                _server = MongoServer.Create(ConnectionString);
                _collection = _server.GetDatabase(DatabaseName).GetCollection(CollectionName);
            }
            catch (MongoException ex) {
                ErrorHandler.Error("Exception while initializing MongoDB Appender.", ex, ErrorCode.GenericFailure);
            }
        }

        protected override void OnClose()
        {
            _collection = null;
            _server.Disconnect();
            base.OnClose();

        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            if (_collection != null && loggingEvent != null) {
                _collection.Insert(LoggingEventToBson(loggingEvent));
            }
        }

        /// <summary>
        /// Create BSON representation of Exception
        /// Inner exceptions are handled recursively
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        protected static BsonDocument ExceptionToBson(Exception ex)
        {
            if (ex == null) {
                throw new ArgumentNullException("ex");
            }

            var doc = new BsonDocument();

            doc["message"] = ex.Message;
            doc["source"] = ex.Source ?? String.Empty;
            doc["stackTrace"] = ex.StackTrace ?? String.Empty;

            if (ex.InnerException != null) {
                doc["innerException"] = ExceptionToBson(ex.InnerException);
            }

            return doc;
        }
    }
}