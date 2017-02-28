Monads
------

For types with more than one generic type aprameter, we do not provide any kind of support for:
- the main (monadic) type parameter must be the one on the far left
- LINQ
- MonadOr, where HasZero is true
- type constraints

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

Methods specific to our .NET implementation:
- Miscs: Zip (w/o zipper)
- Resources management: Using
- Flow control: If, Coalesce
- Query Expression Pattern: SelectMany, Join and GroupJoin
- See also Narvalo.Fx.Internal for interfaces implemented by all monads
