namespace Narvalo.Presentation.Mvp
{
    using System;
    using System.IO;
    using System.Linq;

    public interface ITermPresenter : IPresenter<ITermView>
    {
        TextReader Input { get; }
        TextWriter Output { get; }
        TextWriter Error { get; }
    }

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

    public abstract class Presenter<TView> : IPresenter<TView> where TView : class, IView
    {
        readonly TView _view;

        /// <summary>
        /// Initializes a new instance of the <see cref="Presenter{TView}"/> class.
        /// </summary>
        protected Presenter(TView view)
        {
            Requires.NotNull(view, "view");

            InitializeDefaultModel_(view);

            _view = view;
        }

        #region IPresenter<TView>

        public TView View { get { return _view; } }

        #endregion

        static void InitializeDefaultModel_(TView view)
        {
            var modelType = view.GetType()
                .GetInterfaces()
                .Where(t => t.IsGenericType)
                .Where(t => t.GetGenericTypeDefinition() == typeof(IView<>))
                .Select(t => t.GetGenericArguments().Single())
                .FirstOrDefault();

            if (modelType == null) {
                return;
            }

            var defaultModel = Activator.CreateInstance(modelType);

            typeof(IView<>)
                .MakeGenericType(modelType)
                .GetProperty("Model")
                .SetValue(view, defaultModel, null /* index */);
        }
    }
}
