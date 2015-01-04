// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace MvpCommandLine
{
    using System;
    using Narvalo.Mvp.CommandLine;

    public sealed class SampleCommand : MvpCommand, ISampleView
    {
        public static void DisplayText()
        {
            Console.WriteLine();
            Console.WriteLine(Strings.SampleCommand_DisplayText);
            Console.WriteLine();
        }

        protected override void ExecuteCore()
        {
            DisplayText();
        }
    }
}
