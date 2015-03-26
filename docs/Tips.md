Tips
====

Often obvious but worth recalling.

### The counter intuitive meaning of "protected internal" ###

This accessibility level is counter intuitive:
_Access is limited to the current assembly or types derived from the containing class._

It _does not_ mean protected _and_ internal but rather protected _or_ internal.

[MSDN](https://msdn.microsoft.com/library/ba0a1yw2.aspx)
[Stackoverflow](http://stackoverflow.com/questions/585859/what-is-the-difference-between-protected-and-protected-internal)

### Infinite sequence ###

An `IEnumerable<T>` sequence might be infinite. This is important to remember 
because some LINQ operators need to read _ALL_ data before doing their work.

In those cases, don't use `Count()` if `Any()` would do or, if you can, use `Take(1)` for instance.

[EduLinq](http://codeblog.jonskeet.uk/category/edulinq/)

### Conditional attribute and Code Contracts ###

If a compilation symbol is not set, all methods with a `Conditional` attribute for
this symbol will be erased by the compiler. 

There is still a special case where a consumer of your library can still _see_ 
the erased code. Indeed, with the `CONTRACTS_FULL` symbol, if you build a contract 
reference assembly too, your consumers will be able to discover and use all the 
contracts if they wish to.

### Multiple Conditional attributes ###

Applying more than one conditional attribute does not mean that all conditions
must be met but rather that one of the conditions must be met.

### DebuggerStepThrough vs DebuggerHidden ###
              
`DebuggerStepThrough` can be set on a class, `DebuggerHidden` can not.

`DebuggerHidden` means that the code won't appear in the call stack.

`DebuggerStepThrough` means that the code will be marked as an _external code_ in the call stack.

### CmdletBinding and scripts ###
WhatIf and Confirm parameters won't propagate from a script to a module, 
but one can manually get their values.
If `-WhatIf` is in use, the `$WhatIfPreference` variable is `$true`, `$false` otherwise. 
The `$ConfirmPreference` variable contains the value of ConfirmImpact and
one can check if `-Confirm` is in use by using the following code snippet: 
```posh
$confirm = $PSBoundParameters.ContainsKey('Confirm') `
    -and [bool] $PSBoundParameters.Item('Confirm') -eq $true
```

### Truly initialize a PowerShell string to $null ###

```posh
[string] $value  = [NullString]::Value
```

### Update all binding redirects
Inside the Package Manager Console:
```posh
Get-Project -All | Add-BindingRedirect
```

### A call to an extension method on a null instance does not automatically throw
Calling an extension method on a null reference does not automatically cause a 
`NullExceptionReference` exception to be thrown. Thereby, we can bypass
null-reference check **if** the extension method allows for them.

A more common case is an extension method that solely calls another extension
method on the same object, we may omit the null-reference check in the calling
method.

[Stackoverflow](http://stackoverflow.com/questions/847209/in-c-what-happens-when-you-call-an-extension-method-on-a-null-object)

### A foreach loop throw when the collection is null
This one is obvious since a foreach loop is just a hidden call to GetEnumerator().
Anyway, null collections should raise an alarm.
  
[Stackoverflow](http://stackoverflow.com/questions/11734380/check-for-null-in-foreach-loop)
