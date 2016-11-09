Code Contracts
==============

Preconditions
-------------

Preconditions that will survive in Release mode:
- `Narvalo.Require`: Code Contracts + Throws on failure
- `Narvalo.Guard`: Throws on failure (if CC can't handle the situation) <- Remove?

Preconditions that will not survive in Release mode:
- `Narvalo.Demand`: Code Contract + Debug.Assert
- `Debug.Assert` (if CC can't handle the situation)

```csharp
public class MyClass {
  public void PublicMethod(string value) {
    Require.NotNull(value, nameof(value));

    // Code that calls at least
    // - one public/internal method from another object
    // - or one private/protected/internal method from this object
    // that requires value not to be null.
    OtherObject.PublicMethod(value);
    OtherObject.InternalMethod(value);
    InternalMethod(value);
    ProtectedMethod(value);
    PrivateMethod(value);
  }

  public void PublicMethod2(string value) {
    Require.NotNull(value != null);

    // Code that only calls public methods.
    OtherObject.PublicMethod(value);
    PublicMethod(value);
  }

  internal void InternalMethod(string value) {
    Demand.NotNull(value);
  }

  protected virtual void ProtectedVirtualMethod(string value) {
    Demand.NotNull(value);
  }

  protected void ProtectedMethod(string value) {
    Demand.NotNull(value);
  }

  private void PrivateMethod(string value) {
    Demand.NotNull(value);
  }
}
```

```csharp
public class DerivedClass : MyClass {
  protected override void ProtectedVirtualMethod(string value) {
    // Same contract as the overriden method. Nothing to do.
  }
}
```

**TODO:** Abstract classes.

Postconditions
--------------

Check points
------------

Postconditions that will survive in Release mode:
- `Narvalo.Promise`: Code Contract + Throws on failure
- `Narvalo.???`: Throws on failure (if CC can't handle the situation)

Postconditions that will not survive in Release mode:
- `Narvalo.Check`: Code Contract + Debug.Assert
- `Debug.Assert` (if CC can't handle the situation)

Invariants
----------

