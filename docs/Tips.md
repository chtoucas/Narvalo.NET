Tips
====


### Truly initialize a PS string to $null ###

[string] $value  = [NullString]::Value

### Update all binding redirects
In the Package Manager Console:
```
Get-Project –All | Add-BindingRedirect
```

### A call to an extension method on a null instance does not automatically throw
Calling an extension method on a null reference does not automatically cause a 
`NullExceptionReference` exception to be thrown. Thereby, we can bypass
null-reference check **if** the extension method allows for them.

A more common case is an extension method that solely calls another extension
method on the same object, we may omit the null-reference check in the calling
method.

References: [StackOverflow](http://stackoverflow.com/questions/847209/in-c-what-happens-when-you-call-an-extension-method-on-a-null-object)

### A foreach loop throw when the collection is null
This one is obvious since a foreach loop is just a hidden call to GetEnumerator().
Anyway, null collections should raise an alarm.
  
References: [StackOverflow](http://stackoverflow.com/questions/11734380/check-for-null-in-foreach-loop)
