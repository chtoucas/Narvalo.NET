namespace Narvalo.Presentation.Mvp.Simple
{
    using System.Collections.Generic;
    
    public interface IPresenterDiscoveryStrategy
    {
        IEnumerable<PresenterDiscoveryResult> GetBindings(IEnumerable<IView> views);
    }
}