namespace Narvalo.Narrative
{
    using System;
    using System.CodeDom.Compiler;

    public sealed class CompilerErrorEventArgs : EventArgs
    {
        readonly CompilerErrorCollection _errors;

        public CompilerErrorEventArgs(CompilerErrorCollection errors)
        {
            _errors = errors;
        }

        public CompilerErrorCollection CompilerErrors
        {
            get { return _errors; }
        }
    }

}
