// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.CommandLine
{
    using System;
    using System.Text;

    public interface ITerminal
    {
        ConsoleColor BackgroundColor { get; set; }

        ConsoleColor ForegroundColor { get; set; }

        Encoding InputEncoding { get; set; }

        Encoding OutputEncoding { get; set; }

        void Clear();

        ConsoleKeyInfo ReadKey();

        string ReadLine();

        void ResetColor();

        void Write(string value);

        void WriteLine();

        void WriteLine(string value);
    }
}
