For monads, we do not (yet?) support
- LINQ and types with more than one generic type.
- MonadOr and types with more than one generic type.

We do not implement methods which are related to the way Haskell handles IO / side-effects
(see the namespace Narvalo.Fx.Internal for the .NET equivalent):
- Monad::When
- Monad::Unless
- Monad::Forever
- Alternative::Some
- Alternative::Many
- Alternative::Optional