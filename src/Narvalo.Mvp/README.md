
A port of "Web Forms Mvp" usable outside Web context. 

See _LICENSE-WebFormsMvp.txt_ for license information.

Changes from the original "Web Forms Mvp"
-----------------------------------------

- removed all code related to System.Web: HttpContext, TraceContext...
- an API easier to follow (to me at least) by preferring object methods to static methods
- no more constraints on the ViewModel; namely the class and new() constraints
- use ConcurrentDictionary instead of Dictionary for type caching (maybe I should rollback on this)
  We expect to mostly deal with read operations and to only do very few updates. 
- configuration of IPresenterDiscoveryStrategy is done via PresenterDiscoveryStrategyBuilder.SetFactory()
- configuration of IPresenterFactory is done via PresenterBuilder.SetFactory()

### Dropped functionalities

- AsyncTaskManager
- Tracing

Planned changes
---------------

- Re-enable tracing
- Make IMessageBus available to DI
- Alternative implementation of IMessageBus (Rx)

