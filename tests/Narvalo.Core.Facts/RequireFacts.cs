﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo {
    using System;

    using Xunit;

    using static global::My;
    using Assert = Narvalo.AssertExtended;

    public static partial class RequireFacts {
        internal sealed class tAttribute : TestCaseAttribute {
            public tAttribute(string description) : base(nameof(Require), description) { }
        }

        [t("State(true) does not throw if the precondition is met.")]
        public static void State1() {
            Action act = () => {
                Require.State(true);
                Require.State(true, "My message");
            };

            Assert.DoesNotThrow(act);
        }

        [t("State(false) throws InvalidOperationException.")]
        public static void State2()
            => Assert.Throws<InvalidOperationException>(() => Require.State(false));

        [t("State(false, message) throws InvalidOperationException.")]
        public static void State3() {
            var message = "My message";
            Action act = () => Require.State(false, message);

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal(message, ex.Message);
        }

        [t("True(true) does not throw if the precondition is met.")]
        public static void True1() {
            Action act = () => {
                Require.True(true, "paramName");
                Require.True(true, "paramName", "My message");
            };

            Assert.DoesNotThrow(act);
        }

        [t("True(false) throws ArgumentException.")]
        public static void True2() {
            var paramName = "paramName";
            Action act = () => Require.True(false, paramName);

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.NotNull(ex.Message);
            var argex = Assert.IsType<ArgumentException>(ex);
            Assert.Equal(paramName, argex.ParamName);
        }

        [t("True(false, message) throws ArgumentException.")]
        public static void True3() {
            var paramName = "paramName";
            var message = "My message";
            Action act = () => Require.True(false, paramName, message);

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            var argex = Assert.IsType<ArgumentException>(ex);
            Assert.Equal(paramName, argex.ParamName);
            // ArgumentException appends some info to our message.
            Assert.StartsWith(message, ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [t("Range(true) does not throw if the precondition is met.")]
        public static void Range1() {
            Action act = () => {
                Require.Range(true, "paramName");
                Require.Range(true, "paramName", "My message");
            };

            Assert.DoesNotThrow(act);
        }

        [t("Range(false) throws ArgumentOutOfRangeException.")]
        public static void Range2() {
            var paramName = "paramName";
            Action act = () => Require.Range(false, paramName);

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.NotNull(ex.Message);
            var argex = Assert.IsType<ArgumentOutOfRangeException>(ex);
            Assert.Equal(paramName, argex.ParamName);
        }

        [t("Range(false, message) throws ArgumentOutOfRangeException.")]
        public static void Range3() {
            var paramName = "paramName";
            var message = "My message";
            Action act = () => Require.Range(false, paramName, message);

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            var argex = Assert.IsType<ArgumentOutOfRangeException>(ex);
            Assert.Equal(paramName, argex.ParamName);
            // NB: ArgumentOutOfRangeException appends some info to our message.
            Assert.StartsWith(message, ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [t("NotNull() does not throw if the precondition is met.")]
        public static void NotNull1() {
            Assert.DoesNotThrow(() => Require.NotNull(new Obj(), "paramName"));
        }

        [t("NotNull(null) throws ArgumentNullException.")]
        public static void NotNull2() {
            Object obj = null;
            var paramName = "paramName";
            Action act = () => Require.NotNull(obj, paramName);

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.NotNull(ex.Message);
            var argex = Assert.IsType<ArgumentNullException>(ex);
            Assert.Equal(paramName, argex.ParamName);
        }

        [t("NotNullUnconstrained() does not throw if the precondition is met.")]
        public static void NotNullUnconstrained1() {
            Action act = () => {
                Require.NotNullUnconstrained(new Val(1), "paramName");
                Require.NotNullUnconstrained(new Obj(), "paramName");
            };

            Assert.DoesNotThrow(act);
        }

        [t("NotNullUnconstrained(null) throws ArgumentNullException.")]
        public static void NotNullUnconstrained2() {
            Object obj = null;
            var paramName = "paramName";
            Action act = () => Require.NotNullUnconstrained(obj, paramName);

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.NotNull(ex.Message);
            var argex = Assert.IsType<ArgumentNullException>(ex);
            Assert.Equal(paramName, argex.ParamName);
        }

        [t("NotNullOrEmpty() does not throw if the precondition is met.")]
        public static void NotNullOrEmpty1() {
            Action act = () => {
                Require.NotNullOrEmpty("value", "paramName");
                Require.NotNullOrEmpty(" ", "paramName");
            };

            Assert.DoesNotThrow(act);
        }

        [t("NotNullOrEmpty(null) throws ArgumentNullException.")]
        public static void NotNullOrEmpty2() {
            var paramName = "paramName";
            Action act = () => Require.NotNullOrEmpty(null, paramName);

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.NotNull(ex.Message);
            var argex = Assert.IsType<ArgumentNullException>(ex);
            Assert.Equal(paramName, argex.ParamName);
        }

        [t("NotNullOrEmpty(String.Empty) throws ArgumentException.")]
        public static void NotNullOrEmpty3() {
            var paramName = "paramName";
            Action act = () => Require.NotNullOrEmpty(String.Empty, paramName);

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.NotNull(ex.Message);
            var argex = Assert.IsType<ArgumentException>(ex);
            Assert.Equal(paramName, argex.ParamName);
        }

        [t("NotNullOrWhiteSpace() does not throw if the precondition is met.")]
        public static void NotNullOrWhiteSpace1() {
            Action act = () => {
                Require.NotNullOrWhiteSpace("value", "paramName");
                Require.NotNullOrWhiteSpace("va lue", "paramName");
            };

            Assert.DoesNotThrow(act);
        }

        [t("NotNullOrWhiteSpace(null) throws ArgumentNullException.")]
        public static void NotNullOrWhiteSpace2() {
            var paramName = "paramName";
            Action act = () => Require.NotNullOrWhiteSpace(null, paramName);

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.NotNull(ex.Message);
            var argex = Assert.IsType<ArgumentNullException>(ex);
            Assert.Equal(paramName, argex.ParamName);
        }

        [t("NotNullOrWhiteSpace(String.Empty) throws ArgumentException.")]
        public static void NotNullOrWhiteSpace3() {
            var paramName = "paramName";
            Action act = () => Require.NotNullOrWhiteSpace(String.Empty, paramName);

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.NotNull(ex.Message);
            var argex = Assert.IsType<ArgumentException>(ex);
            Assert.Equal(paramName, argex.ParamName);
        }

        [t(@"NotNullOrWhiteSpace("" "") throws ArgumentException.")]
        public static void NotNullOrWhiteSpace4() {
            var paramName = "paramName";
            Action act = () => Require.NotNullOrWhiteSpace(" ", paramName);

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.NotNull(ex.Message);
            var argex = Assert.IsType<ArgumentException>(ex);
            Assert.Equal(paramName, argex.ParamName);
        }
    }
}
