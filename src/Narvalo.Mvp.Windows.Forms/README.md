
This package enhances Narvalo.Mvp to provide support for Windows Forms.

See _LICENSE-WebFormsMvp.txt_ for license information.

TODO
----

Cross-presenter communication is not functional. Thinks to work on before it might be useful:

- Right now, only controls contained in a MvpForm share the same presenter binder.
  We need something similar to what is done with ASP.NET (PageHost) but the situation
  is a little bit more complicated due to the different execution model. Controls 
  are fully loaded before we reach the CreateControl or Load event in the form container
  where we normally perform the binding.

- The message coordinator must support unsubscription (automatic or manual).