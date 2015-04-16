// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Internal
{
#if !NO_INTERNALS_VISIBLE_TO // White-box tests.

    using System;

    using Narvalo.Finance.Currencies;
    using Xunit;

    public static class CurrencyActivatorFacts
    {
        public class MyCurrency : Currency
        {
            public MyCurrency() : base("YYY") { }
        }

        public class MyCurrencyWithPrivateCtor : Currency
        {
            private MyCurrencyWithPrivateCtor() : base("YYY") { }
        }

        private class MyPrivateCurrency : Currency
        {
            private MyPrivateCurrency() : base("YYY") { }
        }

        public class MyCurrencyWithoutDefaultCtor : Currency
        {
            private MyCurrencyWithoutDefaultCtor(string value) : base("YYY") { }
        }

        #region CreateInstance()

        [Fact]
        public static void CreateInstance_DoesNotThrow_ForBuiltInCurrency()
        {
            var inst = CurrencyActivator<EUR>.CreateInstance();
        }

        [Fact]
        public static void CreateInstance_DoesNotThrow_ForUserDefinedCurrency_WhenCtorIsPublic()
        {
            var inst = CurrencyActivator<MyCurrency>.CreateInstance();
        }

        [Fact]
        public static void CreateInstance_DoesNotThrow_ForUserDefinedCurrency_WhenCtorIsPrivate()
        {
            var inst = CurrencyActivator<MyCurrencyWithPrivateCtor>.CreateInstance();
        }

        [Fact]
        public static void CreateInstance_DoesNotThrow_ForUserDefinedPrivateCurrency()
        {
            var inst = CurrencyActivator<MyPrivateCurrency>.CreateInstance();
        }

        [Fact]
        public static void CreateInstance_ThrowsMissingMemberException_ForUserDefinedCurrency_WithoutDefaultCtor()
        {
            Assert.Throws<MissingMemberException>(() => CurrencyActivator<MyCurrencyWithoutDefaultCtor>.CreateInstance());
        }

        #endregion
    }

#endif
}
