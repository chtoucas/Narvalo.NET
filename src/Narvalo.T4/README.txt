Monads
------

For types with more than one generic type, we do not (yet?) support LINQ and MonadOr.

We do not implement methods which are related to the way Haskell handles actions with side-effects:
- Monad::Forever
- Monad::Unless
- Monad::When
- Monad::Collect_
- Monad::Fold_
- Monad::InvokeWith_
- Monad::Repeat_
- Monad::SelectWith_
- Monad::ZipWith_
- Alternative::Many
- Alternative::Optional
- Alternative::Some
We also exclude methods without clear usecases:
- Monad::SelectUnzip