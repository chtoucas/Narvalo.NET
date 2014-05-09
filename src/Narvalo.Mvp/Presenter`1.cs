// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    //using System;
    //using System.Linq;

    public abstract class Presenter<TView> : IPresenter<TView>
        where TView : class, IView
    {
        readonly TView _view;

        protected Presenter(TView view)
        {
            Require.NotNull(view, "view");

            //InitializeDefaultModel(view);

            _view = view;
        }

        public TView View { get { return _view; } }

        //protected virtual void InitializeDefaultModel(TView view)
        //{
        //    var modelType = (from t in view.GetType().GetInterfaces()
        //                     where t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IView<>)
        //                     select t.GetGenericArguments().Single()).FirstOrDefault();

        //    if (modelType == null) { return; }

        //    var defaultModel = Activator.CreateInstance(modelType);

        //    typeof(IView<>)
        //        .MakeGenericType(modelType)
        //        .GetProperty("Model")
        //        .SetValue(view, defaultModel, null);
        //}
    }
}
