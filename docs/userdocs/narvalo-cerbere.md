Narvalo.Cerbere
===============

Preconditions
-------------

A precondition is a condition that must be true at the beginning of the execution
of a method; argument validation is one such condition, and certainly the most
common.

We provide four static classes to write preconditions:
- `Require` uses Code Contracts preconditions and throws an exception on failure.
- `Enforce` only throws an exception on failure. It complements `Require` when
  the condition is too complicate for the Code Contracts tools.
- `Expect` only uses Code Contracts preconditions.
- `Demand` uses Code Contracts preconditions and debug assertions (`Debug.Assert`).
Among them, only `Require` and `Enforce` method calls will survive in retail
builds, that is when selecting the Release configuration and when not defining
the `CONTRACTS_FULL` symbol.

If you verify your code with the Code Contracts tool,
- for public/protected/internal methods, use `Require` or `Enforce` when
  a condition is compulsory, use `Expect` otherwise,
- for private methods, use `Demand` when a condition is compulsory, use `Expect`
  otherwise.

If you do not, you only care about mandatory preconditions, then use `Require`
or `Enforce` for public/protected/internal methods and `Demand` otherwise.
You should also use Code Analysis to check that you did not forget any precondition.

In any case, **never** use `Demand` to guard a public method.

When you are comfortable with the library, our recommendations above can be
improved. Indeed, for protected or internal methods, **when** you have
complete control of all the callers, you can certainly replace `Require` by
`Demand`. Be very careful with protected methods, if your class is not sealed
you can't know in advance if the caller will satisfy the condition.

Miscellany
----------

### Check points

None of these assertions will survive in retail buidls:
- `Check`: Code Contract + Debug.Assert

### Class Invariants

### Unreachable code

### `Format`

### `ExcludeFromCodeCoverage`

### `ValidatedNotNull`

