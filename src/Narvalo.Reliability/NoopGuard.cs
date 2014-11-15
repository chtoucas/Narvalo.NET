namespace Narvalo.Reliability
{
    using System;

    public class NoopGuard : IGuard
    {
        public void Execute(Action action)
        {
            Require.NotNull(action, "action");

            action.Invoke();
        }
    }
}
