// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.CommandLine
{
    using System;
    using System.Text;
    using Narvalo;

    public class Terminal : ITerminal
    {
        public ConsoleColor BackgroundColor
        {
            get { return Console.BackgroundColor; }
            set { Console.BackgroundColor = value; }
        }

        public ConsoleColor ForegroundColor
        {
            get { return Console.ForegroundColor; }
            set { Console.ForegroundColor = value; }
        }

        public Encoding InputEncoding
        {
            get { return Console.InputEncoding; }
            set
            {
                Require.Property(value);

                Console.InputEncoding = value;
            }
        }

        public Encoding OutputEncoding
        {
            get { return Console.OutputEncoding; }
            set
            {
                Require.Property(value);

                Console.OutputEncoding = value;
            }
        }

        public void Clear()
        {
            Console.Clear();
        }

        public ConsoleKeyInfo ReadKey()
        {
            return Console.ReadKey();
        }

        public string ReadLine()
        {
            return Console.ReadLine();
        }

        public void ResetColor()
        {
            Console.ResetColor();
        }

        public void Write(string value)
        {
            Console.Write(value);
        }

        public void WriteLine()
        {
            Console.WriteLine();
        }

        public void WriteLine(string value)
        {
            Console.WriteLine(value);
        }
    }
}
