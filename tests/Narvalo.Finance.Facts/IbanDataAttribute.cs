// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance {
    using System;
    using System.Reflection;

    using Xunit;
    using Xunit.Sdk;

    [DataDiscoverer("Xunit.Sdk.MemberDataDiscoverer", "xunit.core")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class IbanDataAttribute : MemberDataAttributeBase {
        /// <summary>
        /// Initializes a new instance of the <see cref="IbanDataAttribute"/> class.
        /// </summary>
        /// <param name="memberName">The name of the public static member on the test class that will provide the test data</param>
        /// <param name="parameters">The parameters for the member (only supported for methods; ignored for everything else)</param>
        public IbanDataAttribute(string memberName, params object[] parameters)
            : base(memberName, parameters) {
            MemberType = typeof(IbanData);
            DisableDiscoveryEnumeration = true;
        }

        /// <inheritdoc/>
        protected override object[] ConvertDataItem(MethodInfo testMethod, object item) {
            if (item == null) { return null; }

            var array = item as object[];
            if (array == null) {
                throw new ArgumentException($"Property {MemberName} on {MemberType ?? testMethod.DeclaringType} yielded an item that is not an object[]");
            }

            return array;
        }
    }
}
