---
uid: Narvalo.Fx.Maybe`1
---

The `Maybe<T>` class is like `Nullable<T>` class but without restriction
on the underlying type: *it provides a way to tell the absence or the presence
of a value*. For value types, most of the time `T?` offers a much better
alternative. This class is sometimes referred to as the Option type.

We suggest to prefix with _May_ the methods that return a Maybe instance.

### Haskell ###
- `catMaybes`   -> `Maybe.CollectAny()`
- `isJust`      -> `Maybe<T>.IsSome`
- `isNothing`   -> `Maybe<T>.IsNone`
- `fromMaybe`
- `fromJust`
- `maybeToList` -> `Maybe<T>.ToEnumerable()`
- `maybe`       -> `Maybe<T>.Match()`
- `listToMaybe` -> `Qperators.FirstOrNone()`
- `mapMaybe`    -> `Qperators.SelectAny()`


### References ###

- [Haskell](http://hackage.haskell.org/package/base-4.6.0.1/docs/Data-Maybe.html)
- [Kinds of Immutability](http://blogs.msdn.com/b/ericlippert/archive/2007/11/13/immutability-in-c-part-one-kinds-of-immutability.aspx)
