// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace MvpWebForms.Views
{
    public sealed class StringModel
    {
        string _message = "If you see this message, something went wrong :-(";

        public string Message { get { return _message; } set { _message = value; } }
    }
}
