// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace MvpWebForms.Presenters
{
    using System;
    using System.Globalization;
    using MvpWebForms.Views;
    using Narvalo.Mvp;

    public sealed class CompositePresenter : PresenterOf<StringModel>
    {
        public CompositePresenter(IView<StringModel> view)
            : base(view)
        {
            View.Load += (sender, e) =>
                View.Model.Message = String.Format(
                    CultureInfo.InvariantCulture, 
                    @"Presenter instance: {0}",
                    Guid.NewGuid());
        }
    }
}
