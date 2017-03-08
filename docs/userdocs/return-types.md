The various ways to return from a method
========================================

Typical use cases:
- To encapsulate the result of a computation with lightweight error reporting
  to the caller in the form of a string: `Outcome` and `Outcome<T>`.
- To encapsulate the result of a computation with exception capture:
  `Fallible` and `Fallible<T>`.
- In all other cases: `Result<T, TError>`.
Of course, we could have gone away with one single type, but at the expense
of complicated signatures.

Correspondence:
- `Outcome`        -> `Result<Unit, string>` or `Outcome<Unit>`
- `Outcome<T>`     -> `Result<T, string>`
- `Fallible`       -> `Result<Unit, ExceptionDispatchInfo>` or `Fallible<Unit>`
- `Fallible<T>`    -> `Result<T, ExceptionDispatchInfo>`

Remarks:
- All these types are value types, their primary usage is as a return type.
  For long-lived objects prefer `Either<T, TError>`.
- `Result<T, Exception>` should be used only in very rare situations; this is
  **not** a replacement for the standard exception mechanism in .NET.
  In any cases, `Fallible` and `Fallible<T>` offer better alternatives.
- You can see `Outcome` and `Outcome<T>` as verbose versions of `Maybe<Unit>`
  and `Maybe<T>`. By the way, **always** prefer `Outcome` over `Maybe<TError>`.
  With `Maybe<TError>` it is not obvious that the underlying type (`TError`)
  represents an error and not the "normal" return type.