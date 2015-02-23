// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace MvpWebForms.Views
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Narvalo.Mvp;

    public interface IWidgetsReadOnlyView : IView<WidgetsReadOnlyModel>
    {
        event EventHandler<WidgetIdEventArgs> Finding;

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Apm")]
        event EventHandler<WidgetIdEventArgs> FindingApm;

        event EventHandler<WidgetIdEventArgs> FindingTap;
    }
}
