Issues
======

High Priority
-------------


Medium Priority
---------------


Low Priority
------------


Resolved Issues
===============

* When adding a NuGet package, the Code Contracts library was incorrectly added
to the project references. We just need to use the 'references' section 
in the nuspec to help NuGet identify the _true_ references (Fixed 2014/12/17).

* When building a PCL project _from the command line_, MSBuild generates output
inside a subdirectory of `$(OutDir)`. To correct this, we instruct MSBuild to
use the standard behaviour: `$(GenerateProjectSpecificOutputFolder) = false`.

* Narvalo.Facts fails when called from Make.proj and run twice in a row.
Narvalo.Core and Narvalo.Common use the default namespace (`Narvalo`) and
both define a resource named `SR.resx` (with default access modifier kept,
that is internal). I have not found the reason but, when running the tests
from the command line, the resource embedded in `Narvalo.Core` got replaced
by the one in `Narvalo.Common`. The solution was to use different names for
both resources.