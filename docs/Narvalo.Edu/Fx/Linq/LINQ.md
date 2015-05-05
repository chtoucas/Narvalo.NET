LINQ
====

The .NET Standard Query Operators
---------------------------------

### Restriction Operators

Linq                  | Monad<T>             | Monadic generalisation of list operations
--------------------- | -------------------- | ------------------------------------------
Where (*)             | Monad<T>.Where       | @Enumerable<T>.Filter

### Projection Operators

Linq                  | Monad<T>             | Monadic generalisation of list operations
--------------------- | -------------------- | ------------------------------------------
Select (*)            | Monad<T>.Select      | @Enumerable<T>.Map
SelectMany            | Monad<T>.Bind        |
(*)                   | @Monad<T>.SelectMany |

### Partitioning Operators

Linq                  | Monad<T>             | Monadic generalisation of list operations
--------------------- | -------------------- | ------------------------------------------
Take                  | -                    |
Skip                  | -                    |
TakeWhile             | -                    |
SkipWhile             | -                    |

### Join Operators

Linq                  | Monad<T>             | Monadic generalisation of list operations
--------------------- | -------------------- | ------------------------------------------
Join (*)              | @Monad<T>.Join       |
GroupJoin (*)         | @Monad<T>.GroupJoin  |

### Concatenation Operator

Linq                  | Monad<T>             | Monadic generalisation of list operations
--------------------- | -------------------- | ------------------------------------------
Concat                | @Monad<T>.Plus       |


### Ordering Operators

Linq                  | Monad<T>             | Monadic generalisation of list operations
--------------------- | -------------------- | ------------------------------------------
OrderBy (*)           | ???                  |
OrderByDescending (*) | ???                  |
ThenBy                | ???                  |
ThenByDescending      | ???                  |
Reverse               | -                    |

### Grouping Operators

Linq                  | Monad<T>             | Monadic generalisation of list operations
--------------------- | -------------------- | ------------------------------------------
GroupBy (*)           | ???                  |

### Set Operators

Linq                  | Monad<T>             | Monadic generalisation of list operations
--------------------- | -------------------- | ------------------------------------------
Distinct              | -                    |
Union                 | -                    |
Intersect             | -                    |
Except                | -                    |

### Conversion Operators

Linq                  | Monad<T>             | Monadic generalisation of list operations
--------------------- | -------------------- | ------------------------------------------
AsEnumerable          | -                    |
ToArray               | -                    |
ToList                | -                    |
ToDictionary          | -                    |
ToLookup              | -                    |
OfType                | ???                  |
Cast                  | ???                  |

### Equality Operator

Linq                  | Monad<T>             | Monadic generalisation of list operations
--------------------- | -------------------- | ------------------------------------------
SequenceEqual         | ??? Equals           |

### Element Operators

Linq                  | Monad<T>             | Monadic generalisation of list operations
--------------------- | -------------------- | ------------------------------------------
First                 | -                    | @Enumerable<T>.FirstOrZero
FirstOrDefault        | -                    | @Enumerable<T>.FirstOrZero
Last                  | -                    | @Enumerable<T>.LastOrZero
LastOrDefault         | -                    | @Enumerable<T>.LastOrZero
Single                | -                    | @Enumerable<T>.SingleOrZero
SingleOrDefault       | -                    | @Enumerable<T>.SingleOrZero
ElementAt             | -                    |
ElementAtOrDefault    | -                    |
DefaultIfEmpty        | ??? Monad w/Zero     |

### Generation Operators

Linq                  | Monad<T>             | Monadic generalisation of list operations
--------------------- | -------------------- | ------------------------------------------
Range                 | -                    |
Repeat                | @Monad<T>.Repeat     |
Empty                 | Monad<T>.Zero        |

### Quantifiers

Linq                  | Monad<T>             | Monadic generalisation of list operations
--------------------- | -------------------- | ------------------------------------------
Any                   | ???                  |
All                   | ???                  |
Contains              | ???                  |

### Aggregate Operators

Linq                  | Monad<T>             | Monadic generalisation of list operations
--------------------- | -------------------- | ------------------------------------------
Count                 | -                    |
LongCount             | -                    |
Sum                   | -                    | @Enumerable<Monad<T>>.Sum
Min                   | -                    |
Max                   | -                    |
Average               | -                    |
Aggregate             | -                    | @Enumerable<T>.Fold
                      | -                    | @Enumerable<T>.Reduce

### New Operators in .NET 4

Linq                  | Monad<T>             | Monadic generalisation of list operations
--------------------- | -------------------- | ------------------------------------------
Zip                   |                      | @Enumerable<T>.Zip

### Extensions to Linq

Linq                  | Monad<T>             | Monadic generalisation of list operations
--------------------- | -------------------- | ------------------------------------------
Append                |                      | -
Prepend               |                      | -
AggregateBack         |                      | @Enumerable<T>.FoldBack
                      |                      | @Enumerable<T>.Reduceback
Collect               |                      | @Enumerable<Monad<T>>.Collect

### References

+ LINQ (Language-Integrated Query)
  http://msdn.microsoft.com/en-us/library/bb397926(v=vs.120).aspx
+ Query Expression Pattern / C# 5.0 Language Specification. Section 7.16.3
  http://www.microsoft.com/en-us/download/details.aspx?id=7029
+ The .NET Standard Query Operators
  http://msdn.microsoft.com/en-us/library/bb394939.aspx
+ Classification of Standard Query Operators by Manner of Execution
  http://msdn.microsoft.com/en-us/library/bb882641.aspx
+ Wikipedia
  http://en.wikipedia.org/wiki/Language_Integrated_Query

### References

+ http://blogs.bartdesmet.net/blogs/bart/archive/2010/01/01/the-essence-of-linq-minlinq.aspx
+ http://comonad.com/reader/2009/recursion-schemes/
+ http://blogs.msdn.com/b/wesdyer/archive/2007/02/02/anonymous-recursion-in-c.aspx
