// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Playground.Commands
{
    using System;
    using Narvalo.Mvp.CommandLine;
    using Playground.Views;

    public sealed class TestCommand : MvpCommand, ITestView
    {
        protected override void ExecuteCore()
        {
            DisplayText();
        }

        public void DisplayText()
        {
            Console.WriteLine();
            Console.WriteLine("Help");
            Console.WriteLine();
        }
    }
}
