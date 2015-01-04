// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class IssueAttribute : Attribute
    {
        readonly string _closingDate;
        readonly IssueSeverity _severity;

        public IssueAttribute(IssueSeverity severity, string closingDate)
        {
            _severity = severity;
            _closingDate = closingDate;
        }

        public IssueSeverity Severity
        {
            get { return _severity; }
        }

        public string ClosingDate
        {
            get { return _closingDate; }
        }
    }
}
