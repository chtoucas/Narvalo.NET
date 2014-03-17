// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    public abstract class CommandLine<TArguments>
    {
        readonly TArguments _arguments;

        protected CommandLine(TArguments arguments)
        {
            _arguments = arguments;
        }

        protected TArguments Arguments { get { return _arguments; } }

        public abstract void Run();
    }
}
