// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Stubs
{
    public class PresenterForViewWithModel : Presenter<ISimpleViewWithModel, ViewModel>
    {
        public PresenterForViewWithModel(ISimpleViewWithModel view) : base(view) { }
    }
}
