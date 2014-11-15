namespace Narvalo.Reliability
{
    public interface IBarrier : IGuard
    {
        bool CanExecute { get; }
    }
}
