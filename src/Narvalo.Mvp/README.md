
A port of "Web Forms Mvp" usable outside Web context. 

See LICENSE-WebFormsMvp.txt for license information.

Changes from the original "Web Forms Mvp"
=========================================

- removed all code related to System.Web: HttpContext, TraceContext...
- simpler API (to me at least) by preferring object methods to static methods
- no more constraints on the ViewModel; namely the class and new() constraints
- use ConcurrentDictionary instead of Dictionary for type caching (maybe I should rollback on this),
  this affects:
    AttributeBasedPresenterDiscoveryStrategy
    CompositeViewFactory
    ConventionBasedPresenterDiscoveryStrategy
    DefaultPresenterFactory
    ViewInterfacesCache
- configuration of IPresenterDiscoveryStrategy is done via PresenterDiscoveryStrategyBuilder.SetFactory()
- configuration of IPresenterFactory is done via PresenterBuilder.SetFactory()

On the way, a few things disappear that I might re-enable:
- AsyncTaskManager
- Tracing

