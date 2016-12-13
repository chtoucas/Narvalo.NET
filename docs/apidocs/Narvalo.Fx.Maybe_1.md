---
uid: Narvalo.Fx.Maybe`1
---

## The Maybe Monad ##

The `Maybe < T >` class is kind of like the `Nullable<T>` class but without any
restriction on the underlying type: *it provides a way to tell the absence or
the presence of a value*. Taken alone, it might not look that useful, we could
simply use a nullable for value types and `null` for reference types.That's
where the monad comes into play. The `Maybe<T>` satisfies a very simple grammar,
known as the monad laws, from which derives a rich vocabulary.

What I like the most about this class is that it helps to clearly express our
intent with a very clean syntax.For instance, consider the following methods

```csharp
string GetPhoneNumber() { }
Maybe<string> MayGetPhoneNumber() { }
```

I believe that the second version makes it clearer that we might actually not
know the phone number.It then makes easy to write what we do in either cases:

```csharp
MayGetPhoneNumber().OnNone(action).OnSome(action)
```

This class is sometimes referred to as the Option type.

#### Naming convention
We suggest to prefix with _May_ the methods that return a Maybe instance.

### Design of `Maybe<T>` ###

#### Class vs structure
Argument _towards_ a structure : C# then guarantees that an instance is never
null, which seems like a good thing here. Isn't it one of the reasons why in the
first place we decided to create such a class?

Argument _against_ a structure: An instance is _mutable_ if `T` is mutable.
This should always raise a big warning.

(TODO: Other things to discuss: impact on performances(boxing, size of the struct...)

#### `Maybe<T>` vs `Nullable<T>`
For value types, most of the time `T?` offers a much better alternative.We can
not discourage you enough to use a `Maybe<T>` when a nullable would make a
better fit, We can not enforce this rule with a generic constraint.For instance,
this would prevent us from being able to use `Maybe<Unit>` which must be allowed
to unleash the real power of the Maybe monad.

### Constructor ###

All constructors are made private. Having complete control over the creation of
an instance helps us to ensure that `value` is never `null` when passed to the
constructor. This is exactly what we do in the static method
`Maybe<T>.η(value)`.

To make things simpler, we provide two public factory methods:

```csharp
Maybe.Of<T>(value)
Maybe.Of<T?>(value)
```

and one static property `Maybe<T>.None` to reference a Maybe that has no value.

### `IEnumerable<T>` interface ###

To support LINQ we only need to create the appropriate methods and the C#
compiler will work its magic.Actually, this is something that we have almost
already done.Indeed, this is just a matter of using the right terminology:
- `Select` is the LINQ name for the `Map` method from monads
- `SelectMany` is the LINQ name for the `Bind` method from monads

Nevertheless, since this might look a bit too unusual we also explicitely implement the
`IEnumerable<T>` interface.

### `IEquatable<T>` and `IEquatable<Maybe<T>>` interfaces ###

#### Referential equality and structural equality
(To be revised)
We redefine the `Equals()` method to allow for structural equality for reference types that
follow value type semantics.Nevertheless, we do not change the meaning of the equality
operators(`==` and `!=`) which continue to test referential equality, behaviour expected by
the.NET framework for all reference types.I might change my mind on this and try to make
`Maybe<T>` behave more like `Nullable<T>`. As a matter of convenience, we also
implement the `IEquatable<T>` interface. Another(abandonned) possibility has
been to implement the `IStructuralEquatable` interface.

#### Sample rules
(To be revised)
```csharp
Maybe<T>.None != null
Maybe<T>.None.Equals(null)

Maybe.Of(1) != Maybe.Of(1)
Maybe.Of(1).Equals(Maybe.Of(1))
Maybe.Of(1) != 1
Maybe.Of(1).Equals(1)
```

### References ###

- [Wikipedia](http://en.wikipedia.org/wiki/Monad_(functional_programming)#The_Maybe_monad)
- [Haskell](http://hackage.haskell.org/package/base-4.6.0.1/docs/Data-Maybe.html)
- [Kinds of Immutability](http://blogs.msdn.com/b/ericlippert/archive/2007/11/13/immutability-in-c-part-one-kinds-of-immutability.aspx)

There are many alternative implementations in C#; just google it!

