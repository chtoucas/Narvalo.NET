// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance {
    using System;
    using System.Reflection;

    using Xunit;
    using Xunit.Sdk;

    [DataDiscoverer("Xunit.Sdk.MemberDataDiscoverer", "xunit.core")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class IbanDataAttribute : MemberDataAttributeBase {
        public IbanDataAttribute(string memberName, params object[] parameters)
            : base(memberName, parameters) {
            MemberType = typeof(IbanData);
            DisableDiscoveryEnumeration = true;
        }

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
