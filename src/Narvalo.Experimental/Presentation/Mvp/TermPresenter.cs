namespace Narvalo.Presentation.Mvp
{
    using System;
    using System.IO;

    public abstract class TermPresenter : Presenter<ITermView>, ITermPresenter
    {
        readonly TextReader _input;
        readonly TextWriter _output;
        readonly TextWriter _error;

        protected TermPresenter(ITermView view) : this(view, Console.In, Console.Out, Console.Error) { }
        protected TermPresenter(ITermView view, TextReader input, TextWriter output, TextWriter error)
            : base(view)
        {
            _input = input;
            _output = output;
            _error = error;
        }

        public TextReader Input { get { return _input; } }
        public TextWriter Output { get { return _output; } }
        public TextWriter Error { get { return _error; } }
    }
}
