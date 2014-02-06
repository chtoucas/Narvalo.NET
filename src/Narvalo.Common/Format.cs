// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Globalization;

    public static class Format
    {
        public static string CurrentCulture(string format, object arg0, params object[] args)
        {
            Require.NotNull(arg0, "arg0");

            return String.Format(CultureInfo.CurrentCulture, format, args == null ? arg0 : GetArguments_(arg0, args));
        }

        public static string InvariantCulture(string format, object arg0, params object[] args)
        {
            Require.NotNull(arg0, "arg0");

            return String.Format(CultureInfo.InvariantCulture, format, args == null ? arg0 : GetArguments_(arg0, args));
        }

        static object[] GetArguments_(object arg0, params object[] args)
        {
            DebugCheck.NotNull(args);

            var arguments = new object[1 + args.Length];
            arguments[0] = arg0;
            args.CopyTo(arguments, 1);

            return arguments;
        }
    }
}
