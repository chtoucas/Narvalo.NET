// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Xunit;

    // TODO: Add tests for debug configuration. See CheckFacts.
    public static class DemandFacts
    {
        #region State()

        [Fact]
        public static void State_DoesNothing_ForTrue()
        {
            Demand.State(true);
        }

        [Fact]
        public static void State_DoesNothing_ForFalse_NonDebugConfiguration()
        {
#if !DEBUG
            Demand.State(false);
#endif
        }

        #endregion

        #region True()

        [Fact]
        public static void True_DoesNothing_ForTrue()
        {
            Demand.True(true);
        }

        [Fact]
        public static void True_DoesNothing_ForFalse_NonDebugConfiguration()
        {
#if !DEBUG
            Demand.True(false);
#endif
        }

        #endregion

        #region Range()

        [Fact]
        public static void Range_DoesNothing_ForTrue()
        {
            Demand.Range(true);
        }

        [Fact]
        public static void Range_DoesNothing_ForFalse_NonDebugConfiguration()
        {
#if !DEBUG
            Demand.Range(false);
#endif
        }

        #endregion

        #region NotNull()

        [Fact]
        public static void NotNull_DoesNothing_ForNonNull()
        {
            Demand.NotNull(new Object());
        }

        [Fact]
        public static void NotNull_DoesNothing_ForNull_NonDebugConfiguration()
        {
#if !DEBUG
            Object obj = null;
            Demand.NotNull(obj);
#endif
        }

        #endregion

        #region NotNullOrEmpty()

        [Fact]
        public static void NotNullOrEmpty_DoesNothing_ForNonNullorEmptyString()
        {
            Demand.NotNullOrEmpty("value");
        }

        [Fact]
        public static void NotNullOrEmpty_DoesNothing_ForNullOrEmpty_NonDebugConfiguration()
        {
#if !DEBUG
            Demand.NotNullOrEmpty(null);
            Demand.NotNullOrEmpty(String.Empty);
#endif
        }

        #endregion

        #region Object()

        [Fact]
        public static void Object_DoesNothing_ForNonNull()
        {
            Demand.Object("this");
        }

        [Fact]
        public static void Object_DoesNothing_ForNull_NonDebugConfiguration()
        {
#if !DEBUG
            Object obj = null;
            Demand.Object(obj);
#endif
        }

        #endregion

        #region Property()

        [Fact]
        public static void Property_DoesNothing_ForTrue()
        {
            Demand.Property(true);
        }

        [Fact]
        public static void Property_DoesNothing_ForFalse_NonDebugConfiguration()
        {
#if !DEBUG
            Demand.Property(false);
#endif
        }

        [Fact]
        public static void Property_DoesNothing_ForNonNull()
        {
            Demand.Property(new Object());
        }

        [Fact]
        public static void Property_DoesNothing_ForNull_NonDebugConfiguration()
        {
#if !DEBUG
            Object obj = null;
            Demand.Property(obj);
#endif
        }

        #endregion

        #region PropertyNotEmpty()

        [Fact]
        public static void PropertyNotEmptyy_DoesNothing_ForNonNullorEmptyString()
        {
            Demand.PropertyNotEmpty("value");
        }

        [Fact]
        public static void PropertyNotEmpty_DoesNothing_ForNullOrEmpty_NonDebugConfiguration()
        {
#if !DEBUG
            Demand.PropertyNotEmpty(null);
            Demand.PropertyNotEmpty(String.Empty);
#endif
        }

        #endregion
    }
}
