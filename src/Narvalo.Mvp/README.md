
A port of "Web Forms Mvp" usable outside Web context. 

See _LICENSE-WebFormsMvp.txt_ for license information.

Changes from the original "Web Forms Mvp"
-----------------------------------------

- Removed all code related to System.Web: HttpContext, TraceContext...
  (Support MVP for ASP.Net is available in a separate package)
- An API easier to follow (to me at least) by preferring object methods to static methods
- No more constraints on the ViewModel; namely the class and new() constraints
- Use ConcurrentDictionary instead of Dictionary for type caching
- MVP configuration is done via MvpBootstrapper

### Dropped functionalities for now

- enableAutomaticDataBinding
- AsyncTaskManager

### TODO

- Not yet satisfied by the various implementations of IPresenter

- Re-design MessageBus: if Publish occurs before Subscribe, messages are lost. The way this problem
  is solved in WebFormsMvp is by keeping the list of messages in memory. For short-lived containers
  that seems fine but I would prefer a better solution.

