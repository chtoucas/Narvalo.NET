// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace MvpCommandLine
{
    using System;

    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var cmd = new SampleCommand()) { cmd.Execute(); }
            using (var cmd = new SampleCommand()) { cmd.Execute(); }
        }
    }
}
