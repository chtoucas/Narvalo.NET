﻿namespace Playground.WebForms.Presenters
{
    using System;
    using Narvalo.Mvp;
    using Playground.WebForms.Views.Models;

    public class HelloWorldPresenter : PresenterOf<StringModel>
    {
        public HelloWorldPresenter(IView<StringModel> view)
            : base(view)
        {
            View.Model.Message = "If you see this message, something went wrong :-(";

            View.Load += Load;
        }

        void Load(object sender, EventArgs e)
        {
            View.Model.Message = "Hello World form Presenter!";
        }
    }
}