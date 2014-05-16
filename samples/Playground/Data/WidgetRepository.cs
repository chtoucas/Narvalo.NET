namespace Playground.Data
{
    using System.Collections.Generic;
    using System.Linq;

    public class WidgetRepository : IWidgetRepository
    {
        PlaygroundDataContext _dataContext = new PlaygroundDataContext();

        public Widget Find(int id)
        {
            var q = from _ in _dataContext.Widget where _.Id == id select _;

            return q.SingleOrDefault();
        }

        public Widget FindByName(string name)
        {
            var q = from _ in _dataContext.Widget where _.Name == name select _;

            return q.SingleOrDefault();
        }

        public IEnumerable<Widget> FindAll()
        {
            return _dataContext.Widget;
        }

        public void Create(Widget widget)
        {
            _dataContext.Widget.InsertOnSubmit(widget);
            _dataContext.SubmitChanges();
        }

        public void Update(Widget widget, Widget originalWidget)
        {
            _dataContext.Widget.Attach(widget, originalWidget);
            _dataContext.SubmitChanges();
        }

        public void Delete(Widget widget)
        {
            var q = from _ in _dataContext.Widget where _.Id == widget.Id select _;
            _dataContext.Widget.DeleteOnSubmit(q.Single());
            _dataContext.SubmitChanges();
        }
    }
}