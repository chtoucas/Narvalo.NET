Narvalo.Fx
==========

[![NuGet](https://img.shields.io/nuget/v/Narvalo.Finance.svg)](https://www.nuget.org/packages/Narvalo.Finance/)
[![MyGet](https://img.shields.io/myget/narvalo-edge/v/Narvalo.Finance.svg)](https://www.myget.org/feed/narvalo-edge/package/nuget/Narvalo.Finance)

Features implementations of some of the usual suspects from functional
programming: option type (`Maybe<T>`), return types (`Result<T, TError>`,
`Outcome<T>` and `Fallible<T>`), simple disjoint union (`Either<T1, T2>`),
sequence generators and LINQ extensions.

### Status
- The next release should be the first one to be declared stable.

### Detailed description
This assembly encourages an applicative-style of programming (or functional if
you prefer):
- Types are **immutable**.
- Methods are **pure** - they are free of side-effects.

The main namespace is `Narvalo.Applicative`:
- `Unit`
- `Maybe<T>`, the option monad.
- `Outcome`
- `Outcome<T>`
- `Fallible`
- `Fallible<T>`
- `Result<T, TError>`, the error monad.
- `Either<T1, T2>`, the either monad.
- `Sequence`
- Stubs

The other namespace is `Narvalo.Linq` which contains LINQ extensions.

The various ways to return from a method
----------------------------------------

Typical use cases:
- To encapsulate the result of a computation with lightweight error reporting
  to the caller in the form of a string: `Outcome` and `Outcome<T>`.
- To encapsulate the result of a computation with exception capture
  (`ExceptionDispatchInfo`): `Fallible` and `Fallible<T>`.
- In all other cases: `Result<T, TError>`.

Of course, we could have gone away with one single type, but at the expense
of complicated signatures. The correspondence is as follows:

Type             | Alternatives
-----------------|-------------
`Outcome`        | `Result<Unit, string>` or `Outcome<Unit>`
`Outcome<T>`     | `Result<T, string>`
`Fallible`       | `Result<Unit, ExceptionDispatchInfo>` or `Fallible<Unit>`
`Fallible<T>`    | `Result<T, ExceptionDispatchInfo>`

Remarks:
- All these types are value types, their primary usage is as a return type.
  For long-lived objects prefer `Either<T, TError>`.
- `Result<T, Exception>` should be used only in very rare situations; this is
  **not** a replacement for the standard exception mechanism in .NET.
  In any cases, `Fallible` and `Fallible<T>` offer better alternatives.
- You can see `Outcome` and `Outcome<T>` as verbose versions of `Maybe<Unit>`
  and `Maybe<T>`.
- **Always** prefer `Outcome` over `Maybe<TError>`.
  With `Maybe<TError>` it is not obvious that the underlying type (`TError`)
  represents an error and not the "normal" return type.
