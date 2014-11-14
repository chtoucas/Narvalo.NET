// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace MvpCommandLine
{
    using System;

    public static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            new SampleCommand().Execute();
            new SampleCommand().Execute();
        }
    }
}
