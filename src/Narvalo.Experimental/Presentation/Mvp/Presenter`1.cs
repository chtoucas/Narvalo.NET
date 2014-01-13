namespace Narvalo.Presentation.Mvp
{
    using System;
    using System.Linq;

    public class Presenter<TView> : IPresenter<TView> where TView : class, IView
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
