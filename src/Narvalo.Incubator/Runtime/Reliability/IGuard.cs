namespace Narvalo.Runtime.Reliability
{
    using System;

    // TODO: ajouter les variantes async : Task, Begin/End, async.
    public interface IGuard
    {
        void Execute(Action action);
    }
}
