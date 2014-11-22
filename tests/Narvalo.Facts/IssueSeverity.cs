// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    /// <summary>
    /// Defines the impact that a given issue has on the system.
    /// </summary>
    public enum IssueSeverity
    {
        /// <summary>
        /// The bug or issue affects a crucial part of a system, and must be 
        /// fixed in order for it to resume normal operation.
        /// </summary>
        High = 0,

        /// <summary>
        /// The bug or issue affects a minor part of a system, but has some 
        /// impact on its operation. This severity level is assigned when a 
        /// non-central requirement of a system is affected.
        /// </summary>
        Medium = 1,

        /// <summary>
        /// The bug or issue affects a minor part of a system, and has very 
        /// little impact on its operation. This severity level is assigned
        /// when a non-central requirement of a system (and with lower 
        /// importance) is affected.
        /// </summary>
        Low = 2,

        /// <summary>
        /// The system works correctly, but the appearance does not match the 
        /// expected one. For example: wrong colors, too much or too little 
        /// spacing between contents, incorrect font sizes, typos, etc. This 
        /// is the lowest severity issue.
        /// </summary>
        Cosmetic = 3
    }
}
