namespace MvpWebForms.Views
{
    public sealed class StringModel
    {
        string _message = "If you see this message, something went wrong :-(";

        public string Message { get { return _message; } set { _message = value; } }
    }
}