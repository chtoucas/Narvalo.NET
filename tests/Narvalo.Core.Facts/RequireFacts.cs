// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo {
    using System;

    using Xunit;

    using static global::My;

    public static partial class RequireFacts {
        internal sealed class factAttribute : FactAttribute_ {
            public factAttribute(string message) : base(nameof(Require), message) { }
        }

        #region State()

        [fact("State(true) does not throw if the precondition is met.")]
        public static void State_passes() {
            Require.State(true);
            Require.State(true, "My message");
        }

        [fact("State(false) throws InvalidOperationException.")]
        public static void State_throws_1()
            => Assert.Throws<InvalidOperationException>(() => Require.State(false));

        [fact("State(false, message) throws InvalidOperationException.")]
        public static void State_throws_2() {
            var message = "My message";
            Action act = () => Require.State(false, message);

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal(message, ex.Message);
        }

        #endregion

        #region True()

        [fact("True(true) does not throw if the precondition is met.")]
        public static void True_passes() {
            Require.True(true, "paramName");
            Require.True(true, "paramName", "My message");
        }

        [fact("True(false) throws ArgumentException.")]
        public static void True_throws_1() {
            var paramName = "paramName";
            Action act = () => Require.True(false, paramName);

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.NotNull(ex.Message);
            var argex = Assert.IsType<ArgumentException>(ex);
            Assert.Equal(paramName, argex.ParamName);
        }

        [fact("True(false, message) throws ArgumentException.")]
        public static void True_throws_2() {
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

        #endregion

        #region Range()

        [fact("Range(true) does not throw if the precondition is met.")]
        public static void Range_passes() {
            Require.Range(true, "paramName");
            Require.Range(true, "paramName", "My message");
        }

        [fact("Range(false) throws ArgumentOutOfRangeException.")]
        public static void Range_throws_1() {
            var paramName = "paramName";
            Action act = () => Require.Range(false, paramName);

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.NotNull(ex.Message);
            var argex = Assert.IsType<ArgumentOutOfRangeException>(ex);
            Assert.Equal(paramName, argex.ParamName);
        }

        [fact("Range(false, message) throws ArgumentOutOfRangeException.")]
        public static void Range_throws_2() {
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

        #endregion

        #region NotNull()

        [fact("NotNull() does not throw if the precondition is met.")]
        public static void NotNull_passes() => Require.NotNull(new Obj(), "paramName");

        [fact("NotNull(null) throws ArgumentNullException.")]
        public static void NotNull_throws() {
            Object obj = null;
            var paramName = "paramName";
            Action act = () => Require.NotNull(obj, paramName);

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.NotNull(ex.Message);
            var argex = Assert.IsType<ArgumentNullException>(ex);
            Assert.Equal(paramName, argex.ParamName);
        }

        #endregion

        #region NotNullUnconstrained()

        [fact("NotNullUnconstrained() does not throw if the precondition is met.")]
        public static void NotNullUnconstrained_passes() {
            Require.NotNullUnconstrained(new Val(1), "paramName");
            Require.NotNullUnconstrained(new Obj(), "paramName");
        }

        [fact("NotNullUnconstrained(null) throws ArgumentNullException.")]
        public static void NotNullUnconstrained_throws() {
            Object obj = null;
            var paramName = "paramName";
            Action act = () => Require.NotNullUnconstrained(obj, paramName);

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.NotNull(ex.Message);
            var argex = Assert.IsType<ArgumentNullException>(ex);
            Assert.Equal(paramName, argex.ParamName);
        }

        #endregion

        #region NotNullOrEmpty()

        [fact("NotNullOrEmpty() does not throw if the precondition is met.")]
        public static void NotNullOrEmpty_passes() {
            Require.NotNullOrEmpty("value", "paramName");
            Require.NotNullOrEmpty(" ", "paramName");
        }

        [fact("NotNullOrEmpty(null) throws ArgumentNullException.")]
        public static void NotNullOrEmpty_throws_1() {
            var paramName = "paramName";
            Action act = () => Require.NotNullOrEmpty(null, paramName);

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.NotNull(ex.Message);
            var argex = Assert.IsType<ArgumentNullException>(ex);
            Assert.Equal(paramName, argex.ParamName);
        }

        [fact("NotNullOrEmpty(String.Empty) throws ArgumentException.")]
        public static void NotNullOrEmpty_throws_2() {
            var paramName = "paramName";
            Action act = () => Require.NotNullOrEmpty(String.Empty, paramName);

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.NotNull(ex.Message);
            var argex = Assert.IsType<ArgumentException>(ex);
            Assert.Equal(paramName, argex.ParamName);
        }

        #endregion

        #region NotNullOrWhiteSpace()

        [fact("NotNullOrWhiteSpace() does not throw if the precondition is met.")]
        public static void NotNullOrWhiteSpace_passes() {
            Require.NotNullOrWhiteSpace("value", "paramName");
            Require.NotNullOrWhiteSpace("va lue", "paramName");
        }

        [fact("NotNullOrWhiteSpace(null) throws ArgumentNullException.")]
        public static void NotNullOrWhiteSpace_throws_1() {
            var paramName = "paramName";
            Action act = () => Require.NotNullOrWhiteSpace(null, paramName);

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.NotNull(ex.Message);
            var argex = Assert.IsType<ArgumentNullException>(ex);
            Assert.Equal(paramName, argex.ParamName);
        }

        [fact("NotNullOrWhiteSpace(String.Empty) throws ArgumentException.")]
        public static void NotNullOrWhiteSpace_throws_2() {
            var paramName = "paramName";
            Action act = () => Require.NotNullOrWhiteSpace(String.Empty, paramName);

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.NotNull(ex.Message);
            var argex = Assert.IsType<ArgumentException>(ex);
            Assert.Equal(paramName, argex.ParamName);
        }

        [fact(@"NotNullOrWhiteSpace("" "") throws ArgumentException.")]
        public static void NotNullOrWhiteSpace_throws_3() {
            var paramName = "paramName";
            Action act = () => Require.NotNullOrWhiteSpace(" ", paramName);

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.NotNull(ex.Message);
            var argex = Assert.IsType<ArgumentException>(ex);
            Assert.Equal(paramName, argex.ParamName);
        }

        #endregion
    }
}
