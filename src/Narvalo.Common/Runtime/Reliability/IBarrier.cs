namespace Narvalo.Runtime.Reliability
{
    public interface IBarrier : IGuard
    {
        bool CanExecute { get; }
    }
}
