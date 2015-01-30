// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace MvpCommandLine
{
    using System;
    using Narvalo.Mvp.CommandLine;

    public sealed class SampleCommand : MvpCommand, ISampleView
    {
        public void ShowLoad()
        {
            Console.WriteLine(Strings.SampleCommand_OnLoad);
        }

        public void ShowCompleted()
        {
            Console.WriteLine(Strings.SampleCommand_OnCompleted);
        }
    }
}
