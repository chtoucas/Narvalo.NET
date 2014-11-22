// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class IssueAttribute : Attribute
    {
        readonly int _id;
        readonly IssueSeverity _severity;

        public IssueAttribute(int id, IssueSeverity severity)
        {
            _id = id;
            _severity = severity;
        }

        public int Id
        {
            get { return _id; }
        }

        public IssueSeverity Severity
        {
            get { return _severity; }
        }
    }
}
