// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    public static class Vuple
    {
        public static (T1 Value1, T2 Value2)? Gather<T1, T2>(T1? arg1, T2? arg2)
            where T1 : struct
            where T2 : struct
        {
            if (arg1 is T1 v1 && arg2 is T2 v2)
            {
                return (v1, v2);
            }

            return null;
        }

        public static (T1 Value1, T2 Value2, T3 Value3)? Gather<T1, T2, T3>(
            T1? arg1, T2? arg2, T3? arg3)
            where T1 : struct
            where T2 : struct
            where T3 : struct
        {
            if (arg1 is T1 v1 && arg2 is T2 v2 && arg3 is T3 v3)
            {
                return (v1, v2, v3);
            }

            return null;
        }

        public static (T1 Value1, T2 Value2, T3 Value3, T4 Value4)? Gather<T1, T2, T3, T4>(
            T1? arg1, T2? arg2, T3? arg3, T4? arg4)
            where T1 : struct
            where T2 : struct
            where T3 : struct
            where T4 : struct
        {
            if (arg1 is T1 v1 && arg2 is T2 v2 && arg3 is T3 v3 && arg4 is T4 v4)
            {
                return (v1, v2, v3, v4);
            }

            return null;
        }

        public static (T1 Value1, T2 Value2, T3 Value3, T4 Value4, T5 Value5)?
            Gather<T1, T2, T3, T4, T5>(
            T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5)
            where T1 : struct
            where T2 : struct
            where T3 : struct
            where T4 : struct
            where T5 : struct
        {
            if (arg1 is T1 v1 && arg2 is T2 v2 && arg3 is T3 v3 && arg4 is T4 v4
                && arg5 is T5 v5)
            {
                return (v1, v2, v3, v4, v5);
            }

            return null;
        }

        public static (T1 Value1, T2 Value2, T3 Value3, T4 Value4, T5 Value5, T6 Value6)?
            Gather<T1, T2, T3, T4, T5, T6>(
            T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6)
            where T1 : struct
            where T2 : struct
            where T3 : struct
            where T4 : struct
            where T5 : struct
            where T6 : struct
        {
            if (arg1 is T1 v1 && arg2 is T2 v2 && arg3 is T3 v3 && arg4 is T4 v4
                && arg5 is T5 v5 && arg6 is T6 v6)
            {
                return (v1, v2, v3, v4, v5, v6);
            }

            return null;
        }

        public static (T1 Value1, T2 Value2, T3 Value3, T4 Value4, T5 Value5, T6 Value6, T7 Value7)?
            Gather<T1, T2, T3, T4, T5, T6, T7>(
            T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7)
            where T1 : struct
            where T2 : struct
            where T3 : struct
            where T4 : struct
            where T5 : struct
            where T6 : struct
            where T7 : struct
        {
            if (arg1 is T1 v1 && arg2 is T2 v2 && arg3 is T3 v3 && arg4 is T4 v4
                && arg5 is T5 v5 && arg6 is T6 v6 && arg7 is T7 v7)
            {
                return (v1, v2, v3, v4, v5, v6, v7);
            }

            return null;
        }

        public static (T1 Value1, T2 Value2, T3 Value3, T4 Value4, T5 Value5, T6 Value6, T7 Value7, T8 Value8)?
            Gather<T1, T2, T3, T4, T5, T6, T7, T8>(
            T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7, T8? arg8)
            where T1 : struct
            where T2 : struct
            where T3 : struct
            where T4 : struct
            where T5 : struct
            where T6 : struct
            where T7 : struct
            where T8 : struct
        {
            if (arg1 is T1 v1 && arg2 is T2 v2 && arg3 is T3 v3 && arg4 is T4 v4
                && arg5 is T5 v5 && arg6 is T6 v6 && arg7 is T7 v7 && arg8 is T8 v8)
            {
                return (v1, v2, v3, v4, v5, v6, v7, v8);
            }

            return null;
        }
    }
}
