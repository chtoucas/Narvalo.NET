namespace Narvalo.Web
{
    using System;
	using System.Runtime.Serialization;

	[Serializable]
	public class HttpRequestPathException : Exception {
		public HttpRequestPathException() : base() { }

		public HttpRequestPathException(string message) : base(message) { ; }

		public HttpRequestPathException(string message, Exception innerException)
			: base(message, innerException) { ; }

		protected HttpRequestPathException(SerializationInfo info, StreamingContext context)
			: base(info, context) { ; }
	}
}
