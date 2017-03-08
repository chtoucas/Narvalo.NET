// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Runtime.CompilerServices;

    // This class uses the FormattableString class which is new to .NET v4.6.
    // See https://gist.github.com/jskeet/9d297d0dc013d7a557ee (for .NET version lower than 4.6).
    // There is also a NuGet package that does exactly this: StringInterpolationBridge.
    [DebuggerStepThrough]
    public static class Formattable
    {
        [Pure]
        public static string Current(FormattableString formattable)
        {
            // The C# compiler automatically transforms a formattable string ($"") to a string with:
            //    System.Runtime.CompilerServices.FormattableStringFactory.Create()
            // See https://github.com/dotnet/coreclr/blob/master/src/mscorlib/shared/System/Runtime/CompilerServices/FormattableStringFactory.cs
            // The result of this method is NEVER null, we can't add a CC precondition
            // since the actual code is compiler generated but we can safely assume that
            // 'formattable' is not null. Nevertheless, in the unlikely scenario where we directly
            // create an object of type FormattableString, we still have to handle the possibility
            // of a null value.
            if (formattable == null)
            {
                throw new ArgumentNullException(nameof(formattable));
            }

            // NB: Behind the curtain, formattable.ToString() uses String.Format(formatProvider, format, arguments)
            // which ensures that the result is never null.
            // The default for ToString() is to use the current culture.
            // See https://github.com/dotnet/coreclr/blob/master/src/mscorlib/shared/System/FormattableString.cs
            return formattable.ToString();
        }

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Invariant(FormattableString formattable)
            => FormattableString.Invariant(formattable);
    }
}
