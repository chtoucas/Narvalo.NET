// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    public static class Format
    {
        [Pure]
        public static string CurrentCulture(string format, params object[] args)
        {
            Contract.Requires(format != null);
            Contract.Requires(args != null);
            Contract.Ensures(Contract.Result<string>() != null);

            return String.Format(CultureInfo.CurrentCulture, format, args);
        }

        [Pure]
        public static string InvariantCulture(string format, params object[] args)
        {
            Contract.Requires(format != null);
            Contract.Requires(args != null);
            Contract.Ensures(Contract.Result<string>() != null);

            return String.Format(CultureInfo.InvariantCulture, format, args);
        }

        ////public static string CurrentCulture(string format, object arg0, params object[] args)
        ////{
        ////    Enforce.NotNull(arg0, "arg0");
        ////    Enforce.NotNull(format, "format");
        ////    Contract.Ensures(Contract.Result<string>() != null);

        ////    var arguments = args == null ? arg0 : GetArguments_(arg0, args);

        ////    return String.Format(CultureInfo.CurrentCulture, format, arguments);
        ////}

        ////public static string InvariantCulture(string format, object arg0, params object[] args)
        ////{
        ////    Enforce.NotNull(arg0, "arg0");
        ////    Enforce.NotNull(format, "format");
        ////    Contract.Ensures(Contract.Result<string>() != null);

        ////    var arguments = args == null ? arg0 : GetArguments_(arg0, args);

        ////    return String.Format(CultureInfo.InvariantCulture, format, arguments);
        ////}

        ////static object[] GetArguments_(object arg0, params object[] args)
        ////{
        ////    Enforce.NotNull(args, "args");
        ////    Contract.Ensures(Contract.Result<object[]>() != null);

        ////    var arguments = new object[1 + args.Length];
        ////    arguments[0] = arg0;
        ////    args.CopyTo(arguments, 1);

        ////    return arguments;
        ////}
    }
}
