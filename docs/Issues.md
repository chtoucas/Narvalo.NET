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

[2] When building a PCL project _from the command line_, MSBuild generates output
    inside a subdirectory of `$(OutDir)`. To correct this, we instruct MSBuild to
    use the standard behaviour: `$(GenerateProjectSpecificOutputFolder) = false`. 

[1] Narvalo.Facts fails when called from Make.proj and run twice in a row.
    Narvalo.Core and Narvalo.Common use the default namespace (`Narvalo`) and
    both define a resource named `SR.resx` (with default access modifier kept,
    that is internal). I have not found the reason but, when running the tests
    from the command line, the resource embedded in `Narvalo.Core` got replaced
    by the one in `Narvalo.Common`. The solution was to use different names for
    both resources.