namespace Playground.Data
{
    using System.Collections.Generic;

    public interface IWidgetRepository
    {
        Widget Find(int id);

        Widget FindByName(string name);

        IEnumerable<Widget> FindAll();

        void Create(Widget widget);

        void Update(Widget widget, Widget originalWidget);

        void Delete(Widget widget);
    }
}