// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    [Serializable]
    public class RazorTemplateException : NarrativeException
    {
        public RazorTemplateException() : base() { ; }

        public RazorTemplateException(string message) : base(message) { ; }

        public RazorTemplateException(string message, Exception innerException)
            : base(message, innerException) { ; }

        protected RazorTemplateException(SerializationInfo info, StreamingContext context)
            : base(info, context) { ; }

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
