Code Contracts
==============

Preconditions
-------------

- `Narvalo.Require`
- `Narvalo.Demand`

```csharp
public class MyClass {
  public void PublicMethod(string value) {
    Require.NotNull(value, nameof(value));  // Code Contract + Throw ArgumentException
    
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
    Contract.Requires(value != null);       // Code Contract
  
    // Code that only calls public methods.
    OtherObject.PublicMethod(value);
    PublicMethod(value);
  }
  
  internal void InternalMethod(string value) {
    Demand.NotNull(value);                 // Code Contract + Debug.Assert
  }
  
  protected virtual void ProtectedVirtualMethod(string value) {
    Demand.NotNull(value);                 // Code Contract + Debug.Assert
  }
  
  protected void ProtectedMethod(string value) {
    Demand.NotNull(value);                 // Code Contract + Debug.Assert
  }
  
  private void PrivateMethod(string value) {
    Demand.NotNull(value);                 // Code Contract + Debug.Assert
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

- `Narvalo.Check`

Invariants
----------

