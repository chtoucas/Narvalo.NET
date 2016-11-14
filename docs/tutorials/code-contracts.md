Code Contracts
==============

Preconditions
-------------

### Preconditions that will survive in Release mode:

`Narvalo.Require`:
- Code Contract + Throws on failure.
- To be used when the condition is compulsory.

### Preconditions that will not survive in Release mode:

`Narvalo.Demand`:
- Code Contract + Debug.Assert
- **WARNING:** Never use this method to guard a public method.
- To be used with private/protected/internal methods for which the condition
  is mandatory.

`Narvalo.Expect`:
- Code Contract
- To be used with public methods for which the condition is not compulsory.


```csharp
public class MyClass {
  public void PublicMethod(string value) {
    Require.NotNull(value, nameof(value));
  }

  public void PublicMethod2(string value) {
    Expect.NotNull(value);
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

```csharp
using static System.Diagnostics.Contracts.Contract;

Ensures(Result<string>() != null);
```

Check points
------------

None of these assertions will survive in Release mode:
- `Narvalo.Check`: Code Contract + Debug.Assert

Invariants
----------

