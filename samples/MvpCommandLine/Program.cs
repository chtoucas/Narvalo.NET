// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace MvpCommandLine
{
    using System;

    internal static class Program
    {
        private static void Main()
        {
            var cmd = new SampleCommand();
            cmd.Run();
            cmd.Run();
            cmd.Run();
        }
    }
}
