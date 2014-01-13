namespace Narvalo.Presentation.Mvp
{
    using System;
    using System.IO;
    using Narvalo.Presentation.Mvp.Simple;

    public abstract class TermView/*Impl*/ : ITermView
    {
        readonly TextReader _input;
        readonly TextWriter _output;
        readonly TextWriter _error;

        bool _throwIfNoPresenterBound = true;

        public TermView() : this(Console.In, Console.Out, Console.Error) { }

        public TermView(TextReader input, TextWriter output, TextWriter error)
            : base()
        {
            _input = Console.In;
            _output = Console.Out;
            _error = Console.Error;
        }

        #region IView

        public bool ThrowIfNoPresenterBound
        {
            get { return _throwIfNoPresenterBound; }
            set { _throwIfNoPresenterBound = value; }
        }

        public event EventHandler Loaded;

        #endregion

        #region ITermView

        public event EventHandler Ending;

        #endregion

        public void Init()
        {
            var presenterBinder = new PresenterBinder(this);
            presenterBinder.PerformBinding();

            OnLoaded_();

            presenterBinder.Release();
        }

        public void Exit()
        {
            OnEnding_();

            Console.ReadKey(true /* intercept */);
        }

        void OnEnding_()
        {
            EventHandler localHandler = Ending;
            if (localHandler != null) {
                localHandler(this, EventArgs.Empty);
            }
        }

        void OnLoaded_()
        {
            EventHandler localHandler = Loaded;
            if (localHandler != null) {
                localHandler(this, EventArgs.Empty);
            }
        }
    }
}
