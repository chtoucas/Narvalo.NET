
A port of "Web Forms Mvp" usable outside Web context. 

See _LICENSE-WebFormsMvp.txt_ for license information.

Changes from the original "Web Forms Mvp"
-----------------------------------------

- Removed all code related to System.Web: HttpContext, TraceContext...
- An API easier to follow (to me at least) by preferring object methods to static methods
- No more constraints on the ViewModel; namely the class and new() constraints
- Use ConcurrentDictionary instead of Dictionary for type caching
- MVP configuration is done via MvpConfiguration

### Dropped functionalities

- AsyncTaskManager
- Tracing

Planned changes
---------------

- Re-enable tracing
- Make IMessageBus more flexible
- Alternative implementation of IMessageBus (Rx)

