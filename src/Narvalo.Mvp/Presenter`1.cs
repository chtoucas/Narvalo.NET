// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    //using System;
    //using System.Linq;

    public class Presenter<TView> : IPresenter<TView> where TView : IView
    {
        readonly TView _view;

        public Presenter(TView view)
        {
            //InitializeDefaultModel_(view);

            _view = view;
        }

        public IMessageBus Messages { get; set; }

        public TView View { get { return _view; } }

        //static void InitializeDefaultModel_(TView view)
        //{
        //    var modelType = view.GetType()
        //        .GetInterfaces()
        //        .Where(t => t.IsGenericType)
        //        .Where(t => t.GetGenericTypeDefinition() == typeof(IView<>))
        //        .Select(t => t.GetGenericArguments().Single())
        //        .FirstOrDefault();

        //    if (modelType == null) { return; }

        //    var defaultModel = Activator.CreateInstance(modelType);

        //    typeof(IView<>)
        //        .MakeGenericType(modelType)
        //        .GetProperty("Model")
        //        .SetValue(view, defaultModel, null);
        //}
    }
}
