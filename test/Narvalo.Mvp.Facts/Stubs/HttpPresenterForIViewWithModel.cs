// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Stubs
{
    using Narvalo.Mvp.Web;

    public class HttpPresenterForIViewWithModel : HttpPresenterOf<ViewModel>
    {
        public HttpPresenterForIViewWithModel(IView<ViewModel> view) : base(view) { }
    }
}
