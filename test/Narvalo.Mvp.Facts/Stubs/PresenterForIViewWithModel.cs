// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Stubs
{
    public class PresenterForIViewWithModel : PresenterOf<ViewModel>
    {
        public PresenterForIViewWithModel(IView<ViewModel> view) : base(view) { }
    }
}
