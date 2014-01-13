namespace Narvalo.Presentation.Mvp
{
    using System.IO;

    public interface ITermPresenter : IPresenter<ITermView>
    {
        TextReader Input { get; }
        TextWriter Output { get; }
        TextWriter Error { get; }
    }

}
