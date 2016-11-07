Object Contracts
================

Preconditions
-------------

```csharp
public class MyObject
{
  public void Method1(string value)
  {
    // CC + Exception
    Require.NotNull(value, nameof(value));

    // Inline code that fails if 'value' is null.
    if (value.Length == 1) {
      ...
    }
  }

  public void Method2(string value)
  {
    // CC + Exception
    Require.NotNull(value, nameof(value));

    // Code that calls at least one private/protected/internal method
    // that requires value not to be null.
    InternalMethod(value);
    ProtectedMethod(value);
    PrivateMethod(value);
  }

  public void Method3(string value)
  {
    // CC only
    Contract.Requires(value != null);

    // Code that only calls public methods that require value not to be null.
    Method3(value);
  }

  internal void InternalMethod(string value)
  {
    // CC + Debug check
    Promise.NotNull(value);

    // Inline code that fails if 'value' is null.
    if (value.Length == 1) ...
  }

  protected void ProtectedMethod(string value)
  {
    // CC + Debug check
    Promise.NotNull(value);

    // Inline code that fails if 'value' is null.
    if (value.Length == 1) ...
  }

  protected virtual void ProtectedVirtualMethod(string value)
  {
  }

  private void PrivateMethod(string value)
  {
    // CC + Debug check
    Promise.NotNull(value);

    // Inline code that fails if 'value' is null.
    if (value.Length == 1) ...
  }
}

public class DerivedObject : MyObject
{
  protected override void ProtectedVirtualMethod(string value)
  {
  }
}

internal class InternalObject
{
}
```

Postconditions
--------------

Check points
------------


