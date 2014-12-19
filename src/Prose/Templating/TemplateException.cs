// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Prose.Templating
{
    using System;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    [Serializable]
    public class TemplateException : ProseException
    {
        public TemplateException() : base() { }

        public TemplateException(string message) : base(message) { }

        public TemplateException(string message, Exception innerException)
            : base(message, innerException) { }

        protected TemplateException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        public int Column { get; set; }

        public int Line { get; set; }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("Column", Column);
            info.AddValue("Line", Line);
        }
    }
}
