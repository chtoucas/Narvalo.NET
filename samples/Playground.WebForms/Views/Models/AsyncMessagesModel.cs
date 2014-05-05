namespace Playground.WebForms.Views.Models
{
    using System.Collections.Generic;

    public class AsyncMessagesModel
    {
        public IList<string> Messages { get; private set; }

        public AsyncMessagesModel()
        {
            Messages = new List<string>();
        }
    }
}